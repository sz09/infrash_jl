namespace JobLogic.Infrastructure.Dapper
{
    public class SqlStatement
    {
        internal SqlStatement(string sQLStatementString, object sqlParams, bool doShowAll)
        {
            SQLStatementString = sQLStatementString;
            SQLParams = sqlParams;
            DoShowAll = doShowAll;
        }

        public string SQLStatementString { get;  }
        public object SQLParams { get;  }
        public bool DoShowAll { get; }
    }
}
