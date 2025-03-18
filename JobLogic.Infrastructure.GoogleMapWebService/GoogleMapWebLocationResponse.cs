using System;
using System.Collections.Generic;
using System.Text;

namespace JobLogic.Infrastructure.GoogleMapWebService
{
    public class GoogleMapWebLocationResponse
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string PostCode { get; set; }
    }
}
