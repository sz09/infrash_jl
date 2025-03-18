namespace JobLogic.Infrastructure.Utilities
{
    public static class RegularExpressionUtils
    {
        public const string MatchSingleEmailExpression = @"^[-!#$%&'*+\/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+\/0-9=?A-Z^_a-z{|}~‘`])*@[a-zA-Z0-9](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$";
        public const string MatchMultipleEmailExpression = @"^(([-!#$%&'*+\/0-9=?A-Z^_a-z{|}~.`‘]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;,.]\s*(([-!#$%&'*+\/0-9=?A-Z^_a-z{|}~.`‘]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*;?\s*$";
    }
}
