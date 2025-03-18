namespace JobLogic.Infrastructure.Dapper
{
    public interface ISqlStatementFactory
    {
        SqlStatement CreateSqlStatement(bool doShowAll = false);
    }
}
