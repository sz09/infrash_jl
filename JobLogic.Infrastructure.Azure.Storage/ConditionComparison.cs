using Microsoft.WindowsAzure.Storage.Table;
using System.ComponentModel;

namespace JobLogic.Infrastructure.Azure.Storage
{
    public enum ConditionComparison
    {
        [Description(QueryComparisons.Equal)]
        Equal,

        [Description(QueryComparisons.NotEqual)]
        NotEqual,

        [Description(QueryComparisons.GreaterThan)]
        GreaterThan,

        [Description(QueryComparisons.GreaterThanOrEqual)]
        GreaterThanOrEqual,

        [Description(QueryComparisons.LessThan)]
        LessThan,

        [Description(QueryComparisons.LessThanOrEqual)]
        LessThanOrEqual
    }
}
