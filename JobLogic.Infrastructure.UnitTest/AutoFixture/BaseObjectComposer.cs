using AutoFixture.Dsl;
using AutoFixture;
using System;
using System.Linq.Expressions;
using JobLogic.Infrastructure.UnitTest.AutoFixture.Rules;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.UnitTest.AutoFixture
{
    public abstract class BaseObjectComposer<TComposer, TObject>
        where TComposer : BaseObjectComposer<TComposer, TObject>
    {
        protected IPostprocessComposer<TObject> _composer;
        internal readonly Fixture _fixture;
        protected GenLaw _law;
        protected BaseObjectComposer(Fixture fixture, GenLaw law)
        {
            _fixture = fixture;
            _law = law;
            _composer = fixture.Build<TObject>();
            _composer = ApplyDefaultLawToComposer(_composer);
        }

        protected void ApplyConstraintRuleToComposer()
        {
            _composer = ApplyConstraintRuleToComposer(_composer);
        }

        protected IPostprocessComposer<T> ApplyConstraintRuleToComposer<T>(IPostprocessComposer<T> composer)
        {
            if (_law != null)
            {
                var rules = _law.GetRuleSet<T>();
                composer = rules.ApplyConstraint(composer, _law, _fixture);
            }
            return composer;
        }

        protected IPostprocessComposer<T> ApplyDefaultLawToComposer<T>(IPostprocessComposer<T> composer)
        {
            if (_law != null)
            {
                var rules = _law.GetRuleSet<T>();
                composer = rules.ApplyDefault(composer, _law, _fixture);
            }
            return composer;
        }

        public TComposer With<TProperty>(Expression<Func<TObject, TProperty>> propertyPicker, Func<TProperty> valueFactory)
        {
            _composer = _composer.With(propertyPicker, valueFactory);
            return this as TComposer;
        }

        public TComposer Without<TProperty>(Expression<Func<TObject, TProperty>> propertyPicker)
        {
            _composer = _composer.Without(propertyPicker);
            return this as TComposer;
        }

        public TComposer With<TProperty>(Expression<Func<TObject, TProperty>> propertyPicker, TProperty val)
        {
            _composer = _composer.With(propertyPicker, val);
            return this as TComposer;
        }

        public TComposer WithObject<TProperty>(Expression<Func<TObject, TProperty>> propertyPicker, OptionWhenRootCreateMany opt = OptionWhenRootCreateMany.ShareInstance) where TProperty : class
        {
            var objectBuilder = new ObjectBuilder<TProperty>(_fixture, _law);
            if (opt == OptionWhenRootCreateMany.SeparateInstance)
            {
                _composer = _composer.With(propertyPicker, () => objectBuilder.Create());
            }
            else
            {
                _composer = _composer.With(propertyPicker, objectBuilder.Create());
            }
            return this as TComposer;
        }

        public TComposer WithAnyExcept<TProperty>(Expression<Func<TObject, TProperty>> expression, TProperty val) where TProperty : struct
        {
            _composer = _composer.With(expression, () => ValueGenerator.GenerateExcept(val));
            return this as TComposer;
        }

        public PropertyComposer<TObject, TProperty, TComposer> BuildProperty<TProperty>(Expression<Func<TObject, TProperty>> expression) where TProperty : new()
        {
            return new PropertyComposer<TObject, TProperty, TComposer>(this as TComposer, expression, _law);
        }

        public CollectionItemPropertyComposer<TObject, TProperty, TComposer> BuildItemForCollection<TProperty>(Expression<Func<TObject, IEnumerable<TProperty>>> expression) where TProperty : class
        {
            return new CollectionItemPropertyComposer<TObject, TProperty, TComposer>(this as TComposer, expression, _law);
        }
    }
}
