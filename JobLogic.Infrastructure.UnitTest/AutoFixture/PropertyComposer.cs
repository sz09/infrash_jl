using AutoFixture;
using JobLogic.Infrastructure.UnitTest.AutoFixture.Rules;
using System;
using System.Linq.Expressions;

namespace JobLogic.Infrastructure.UnitTest.AutoFixture
{
    public class PropertyComposer<TOwner, T, TOwnerComposer> : BaseObjectComposer<PropertyComposer<TOwner, T, TOwnerComposer>, T>
        where TOwnerComposer : BaseObjectComposer<TOwnerComposer, TOwner>
    {
        internal Expression<Func<TOwner, T>> _expression;
        internal TOwnerComposer _ownerBuilder;
        public PropertyComposer(TOwnerComposer builder, Expression<Func<TOwner, T>> expression,
            GenLaw law)
            : base(builder._fixture, law)
        {
            _expression = expression;
            _ownerBuilder = builder;
        }

        public TOwnerComposer Done(OptionWhenRootCreateMany opt = OptionWhenRootCreateMany.ShareInstance)
        {
            ApplyConstraintRuleToComposer();
            if (opt == OptionWhenRootCreateMany.SeparateInstance)
                _ownerBuilder.With(_expression, () => _composer.Create());
            else
                _ownerBuilder.With(_expression, _composer.Create());
            return _ownerBuilder;
        }
    }
}
