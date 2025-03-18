using JobLogic.Infrastructure.ServiceResponders;
using System;
using System.ComponentModel;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace JobLogic.Infrastructure.RMGQubeServices
{
    public enum RMGQubeJobStatus
    {
        Update = 0,
        ClosedJob = 1,
        [Description("DUP")]
        CancelledDuplicatePO = 2,
        [Description("ACCESS")]
        CancelledAccessIssues = 3,
        [Description("COST")]
        CancelledCostUnauthorised = 4,
        [Description("OUTREM")]
        CancelledOutsideRemit = 5,
        [Description("INACC")]
        CancelledInaccuratePO = 6,
        [Description("UNDER")]
        CancelledUnderResourced = 7,
        [Description("PMINS")]
        CancelledPMInstruction = 8
    }

    public enum RMGQubeServerStatus
    {
        [Description("Success")]
        Success = 00,
        [Description("Bad XML")]
        BadXML = 01,
        [Description("Multiple")]
        Multiple = 02,
        [Description("Invalid Ref")]
        InvalidRef = 03,
        [Description("Not be used by JL")]
        NotUsedByJl = 04,
        [Description("Cannot Find")]
        CannotFind = 05
    }

    public interface IRMGQubeService
    {
        Task<Response<RMGQubicServiceResponse>> CallQubeProcess(string endPointAddress, string reference, string group, RMGQubeJobStatus status, string comments, string completion);
    }

    public class RMGQubeService : IRMGQubeService
    {
        #region header
        private const string _qubeProcessName = "rmgweb:closejob";
        private const string _qubeUserName = "rmgweb.servacc";
        private const string _qubePassword = "p4ssw0rd";
        //private const string _qubeGroup = "RMG Test";
        private const string _qubeApplication = "QGS Helpdesk";
        #endregion

        #region xsdNamespace
        private const string _qubeXNamespaceForXmlns = "http://www.w3.org/2001/XMLSchema-instance";
        private const string _qubeXNamespaceForxsi = "http://www.w3.org/2001/XMLSchema";
        #endregion

        #region parameters
        private const string _qubeBodyParameterReference = "reference";
        private const string _qubeBodyParameterStatus = "status";
        private const string _qubeBodyParameterComments = "comments";
        private const string _qubeBodyParameterCompletion = "completion";
        #endregion

        #region xsdSchema
        private const string _qubeBodyXsiPreFix = "xsi";
        private const string _qubeBodyXsdPreFix = "xsd";
        private const string _qubeBodyXmlnsPreFix = "xmlns";
        #endregion

        private const string _preFixQubeErrorMsg = "Qube Error Code:";

        private const RMGQubeReference.BatchActionOnAbort _qubeActionOnAbort = RMGQubeReference.BatchActionOnAbort.ContinueThenGenerateException;

        public async Task<Response<RMGQubicServiceResponse>> CallQubeProcess(string endPointAddress, string reference, string group, RMGQubeJobStatus status, string comments, string completion)
        {
            try
            {
                var response = new RMGQubicServiceResponse();
                HttpBindingBase binding = new BasicHttpBinding();
                if (endPointAddress.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    binding = new BasicHttpsBinding();
                }
                using (var rmgQubeService = new RMGQubeReference.QubeWSSoapClient(binding, new EndpointAddress(endPointAddress)))
                {
                    var parameters = constructQubeProcessParameters(reference, status.ToString("D"), comments, completion);


                    var qubeProcessresponse = await rmgQubeService.QubeProcess1Async(_qubeProcessName, parameters, _qubeActionOnAbort,
                                                               _qubeUserName, _qubePassword, group, _qubeApplication);

                    QubicDataResponse qubicDataResponse;
                    var serializer = new XmlSerializer(typeof(QubicDataResponse));

                    using (TextReader reader = new StringReader(qubeProcessresponse.QubeProcess3aResult.ToString()))
                        qubicDataResponse = (QubicDataResponse)serializer.Deserialize(reader);

                    var rmgQubeServerStatus = (RMGQubeServerStatus)int.Parse(qubicDataResponse.Closureresult.Code);
                    if (rmgQubeServerStatus != RMGQubeServerStatus.Success) return constructErrorForOutbound(rmgQubeServerStatus);

                    response = new RMGQubicServiceResponse
                    {
                        ResponseStatus = rmgQubeServerStatus,
                        Desc = qubicDataResponse.Closureresult.Desc,
                        Version = qubicDataResponse.Closureresult.Version
                    };
                }

                return ResponseFactory.Return(response);
            }
            catch (Exception ex)
            {
                return ResponseFactory.ReturnWithException<RMGQubicServiceResponse>(ex);
            }
        }

        private Response<RMGQubicServiceResponse> constructErrorForOutbound(RMGQubeServerStatus serverStatus)
        {
            return ResponseFactory.ReturnWithError<RMGQubicServiceResponse>($"{_preFixQubeErrorMsg} {((int)serverStatus)}");
        }

        private XElement constructQubeProcessParameters(string reference, string status, string comments, string completion)
        {
            XNamespace xmlns = XNamespace.Get(_qubeXNamespaceForXmlns);
            XNamespace xsi = XNamespace.Get(_qubeXNamespaceForxsi);

            XElement qubeProcessParameters = new XElement("parameters",
                                      new XElement(_qubeBodyParameterReference, reference),
                                      new XElement(_qubeBodyParameterStatus, status),
                                      new XElement(_qubeBodyParameterComments, comments),
                                      new XElement(_qubeBodyParameterCompletion, completion),
                                      new XAttribute(XNamespace.Xmlns + _qubeBodyXsiPreFix, xmlns),
                                      new XAttribute(XNamespace.Xmlns + _qubeBodyXsdPreFix, xsi),
                                      new XAttribute(_qubeBodyXmlnsPreFix, ""));

            return qubeProcessParameters;
        }
    }
}
