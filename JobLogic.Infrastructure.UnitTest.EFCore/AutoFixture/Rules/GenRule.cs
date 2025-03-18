using AutoFixture;
using AutoFixture.Dsl;
using System.Linq.Expressions;

namespace JobLogic.Infrastructure.UnitTest.EFCore.AutoFixture.Rules
{
    public enum GenRuleType
    {
        Default,
        Constraint
    }
    public abstract class BaseGenRule<T>
    {
        public abstract IPostprocessComposer<T> ApplyRule(IPostprocessComposer<T> composer);
        public abstract GenRuleType RuleType { get; }
    }

    public abstract class BaseDefaultRule<T> : BaseGenRule<T>
    {
        public override GenRuleType RuleType => GenRuleType.Default;
    }

    public abstract class BaseConstraintRule<T> : BaseGenRule<T>
    {
        public override GenRuleType RuleType => GenRuleType.Constraint;
    }

    public class StringMaxLenConstraint<T> : BaseConstraintRule<T>
    {
        Expression<Func<T, string>> _propertyPicker;
        int _maxLength;
        public StringMaxLenConstraint(Expression<Func<T, string>> propertyPicker, int maxLength)
        {
            _propertyPicker = propertyPicker;
            _maxLength = maxLength;
        }

        public override IPostprocessComposer<T> ApplyRule(IPostprocessComposer<T> composer)
        {
            var obj = composer.Create<T>();
            var val = _propertyPicker.Compile()(obj);
            if (val.Length > _maxLength)
            {
                composer = composer.With(_propertyPicker, () => ValueGenerator.String(_maxLength));
            }
            return composer;
        }
    }

    public class DefaultObject<T, TProp> : BaseDefaultRule<T> where TProp : class
    {
        Expression<Func<T, TProp>> _propertyPicker;
        GenLaw _ruleSets;
        Fixture _fixture;
        OptionWhenRootCreateMany _opt;
        public DefaultObject(Expression<Func<T, TProp>> propertyPicker, GenLaw ruleSets, Fixture fixture, OptionWhenRootCreateMany opt)
        {
            _propertyPicker = propertyPicker;
            _ruleSets = ruleSets;
            _fixture = fixture;
            _opt = opt;
        }

        public override IPostprocessComposer<T> ApplyRule(IPostprocessComposer<T> composer)
        {
            var propComposer = new ObjectBuilder<TProp>(_fixture, _ruleSets);
            if(_opt == OptionWhenRootCreateMany.SeparateInstance)
                composer = composer.With(_propertyPicker, () => propComposer.Create());
            else if (_opt == OptionWhenRootCreateMany.ShareInstance)
                composer = composer.With(_propertyPicker, propComposer.Create());
            return composer;
        }
    }

    public class DefaultValue<T, TProp>: BaseDefaultRule<T>
    {
        Expression<Func<T, TProp>> _propertyPicker;
        Func<TProp> _valueFactory;
        TProp _value;

        public DefaultValue(Expression<Func<T, TProp>> propertyPicker, Func<TProp> valFactory)
        {
            _propertyPicker = propertyPicker;
            _valueFactory = valFactory;
        }

        public DefaultValue(Expression<Func<T, TProp>> propertyPicker, TProp val)
        {
            _propertyPicker = propertyPicker;
            _value = val;
        }

        public override IPostprocessComposer<T> ApplyRule(IPostprocessComposer<T> composer)
        {
            if(_valueFactory == null)
                composer = composer.With(_propertyPicker, _value);
            else
                composer = composer.With(_propertyPicker, _valueFactory);
            return composer;
        }
    }

    public class DefaultWithout<T, TProp> : BaseDefaultRule<T>
    {
        Expression<Func<T, TProp>> _propertyPicker;
        public DefaultWithout(Expression<Func<T, TProp>> propertyPicker)
        {
            _propertyPicker = propertyPicker;
        }

        public override IPostprocessComposer<T> ApplyRule(IPostprocessComposer<T> composer)
        {
            composer = composer.Without(_propertyPicker);
            return composer;
        }
    }

    public class SameValueConstraint<T, TProp> : BaseConstraintRule<T>
    {
        Func<T, TProp> _expected;
        Expression<Func<T, TProp>> _propertyPicker;
        public SameValueConstraint(Expression<Func<T, TProp>> propertyPicker, Func<T, TProp> expected)
        {
            _expected = expected;
            _propertyPicker = propertyPicker;
        }

        public override IPostprocessComposer<T> ApplyRule(IPostprocessComposer<T> composer)
        {
            var obj = composer.Create();
            TProp exp;
            try
            {
                exp = _expected(obj);
            }
            catch (NullReferenceException)
            {
                exp = default(TProp);
            }
            composer = composer.With(_propertyPicker, () => exp);
            return composer;
        }
    }
}
