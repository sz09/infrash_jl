using System.Collections.Generic;
using System.Data;

namespace JobLogic.DatabaseManager
{
    public class SqlQuery
    {
        public string QueryString { get; set; }
        public IList<IDbDataParameter> QueryParams { get; set; }
    }
}
