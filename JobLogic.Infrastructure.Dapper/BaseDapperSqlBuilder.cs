namespace JobLogic.Infrastructure.Dapper
{
    public abstract class BaseDapperSqlBuilder : ISqlStatementFactory
    {
        public class DapperSqlStatementModel
        {
            public string SQLStatementString { get; set; }
            public object SQLParams { get; set; }
        }
        protected abstract DapperSqlStatementModel BuildDapperStatement();
        public SqlStatement CreateSqlStatement(bool doShowAll = false)
        {
            var stm = BuildDapperStatement();
            return new SqlStatement(stm.SQLStatementString, stm.SQLParams, doShowAll);
        }
    }
}
