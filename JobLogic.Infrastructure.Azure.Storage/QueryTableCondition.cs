namespace JobLogic.Infrastructure.Azure.Storage
{
    public class QueryTableCondition
    {
        public string ColumnName { get; set; }
        public ConditionComparison Operation { get; set; }
        public string Value { get; set; }
    }
}
