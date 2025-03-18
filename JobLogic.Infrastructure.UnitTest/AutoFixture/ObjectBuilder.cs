using AutoFixture;
using JobLogic.Infrastructure.UnitTest.AutoFixture.Rules;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.UnitTest.AutoFixture
{
    public class ObjectBuilder<T> : BaseObjectComposer<ObjectBuilder<T>, T> where T : class
    {
        public ObjectBuilder(Fixture fixture, GenLaw law = null) : base(fixture, law)
        {
        }

        public T Create()
        {
            ApplyConstraintRuleToComposer();
            return _composer.Create();
        }

        public IEnumerable<T> CreateMany()
        {
            ApplyConstraintRuleToComposer();
            return _composer.CreateMany();
        }

        public IEnumerable<T> CreateMany(int count)
        {
            ApplyConstraintRuleToComposer();
            return _composer.CreateMany(count);
        }
    }
}
