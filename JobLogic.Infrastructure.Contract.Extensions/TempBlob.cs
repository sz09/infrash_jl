using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobLogic.Infrastructure.Contract.Extensions
{
    public class Temp1DayBlob
    {
        [JsonConstructor]
        protected Temp1DayBlob(Uri sasBlobUrl)
        {
            SasBlobUrl = sasBlobUrl;
        }

        [JsonProperty]
        public Uri SasBlobUrl { get; private set; }
        public string Link => SasBlobUrl.AbsoluteUri;

        [Obsolete("This method was meant for blob client to generate it")]
        public static Temp1DayBlob Create(Uri sasBlobUrl)
        {
            return new Temp1DayBlob(sasBlobUrl);
        }
    }

    public class Temp3DayBlob : Temp1DayBlob
    {
        [JsonConstructor]
        protected Temp3DayBlob(Uri sasBlobUrl) : base(sasBlobUrl)
        {
        }

        [Obsolete("This method was meant for blob client to generate it")]
        public static new Temp3DayBlob Create(Uri sasBlobUrl)
        {
            return new Temp3DayBlob(sasBlobUrl);
        }
    }

    

    public class Temp7DayBlob : Temp3DayBlob
    {
        [JsonConstructor]
        protected Temp7DayBlob(Uri sasBlobUrl) : base(sasBlobUrl)
        {
        }

        [Obsolete("This method was meant for blob client to generate it")]
        public static new Temp7DayBlob Create(Uri sasBlobUrl)
        {
            return new Temp7DayBlob(sasBlobUrl);
        }
    }

    public class Temp30DayBlob : Temp7DayBlob
    {
        [JsonConstructor]
        protected Temp30DayBlob(Uri sasBlobUrl) : base(sasBlobUrl)
        {
        }

        [Obsolete("This method was meant for blob client to generate it")]
        public static new Temp30DayBlob Create(Uri sasBlobUrl)
        {
            return new Temp30DayBlob(sasBlobUrl);
        }
    }

    public static class TempBlobUtils
    {
        public static bool IsValid(this Temp1DayBlob blob)
        {
            return !string.IsNullOrWhiteSpace(blob?.SasBlobUrl?.AbsoluteUri);
        }
    }
}
