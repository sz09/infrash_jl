﻿namespace JobLogic.Infrastructure.UnitTest.EFCore.AutoFixture.Rules
{
    public class GenLaw
    {
        Dictionary<Type, object> _ruleSetDict = new Dictionary<Type, object>();
        public GenLaw SetRuleSet<T>(GenRuleSet<T> ruleSet)
        {
            _ruleSetDict.Add(typeof(T), ruleSet);
            return this;
        }
        public GenRuleSet<T> GetRuleSet<T>()
        {
            var t = typeof(T);
            if (_ruleSetDict.ContainsKey(t))
            {
                var ruleSet = _ruleSetDict[t] as GenRuleSet<T>;
                return ruleSet;
            }
            return new GenRuleSet<T>(); //Empty RuleSet
        }
    }
}
