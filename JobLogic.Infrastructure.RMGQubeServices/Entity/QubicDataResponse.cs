using System.Xml.Serialization;

namespace JobLogic.Infrastructure.RMGQubeServices
{
    [XmlRoot(ElementName = "closure-result")]
    public class QubicClosureresult
    {
        [XmlElement(ElementName = "code")]
        public string Code { get; set; }
        [XmlElement(ElementName = "desc")]
        public string Desc { get; set; }
        [XmlElement(ElementName = "version")]
        public string Version { get; set; }
    }

    [XmlRoot(ElementName = "data")]
    public class QubicDataResponse
    {
        [XmlElement(ElementName = "closure-result")]
        public QubicClosureresult Closureresult { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    public class RMGQubicServiceResponse
    {
        public RMGQubeServerStatus ResponseStatus { get; set; }
        public string Desc { get; set; }
        public string Version { get; set; }
    }
}
