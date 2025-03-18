using AutoFixture;
using JobLogic.Infrastructure.UnitTest.AutoFixture.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.UnitTest.AutoFixture
{
    public class CollectionItemPropertyComposer<TOwner, TProperty, TOwnerComposer> : BaseObjectComposer<CollectionItemPropertyComposer<TOwner, TProperty, TOwnerComposer>, TProperty>
        where TOwnerComposer : BaseObjectComposer<TOwnerComposer, TOwner>
        where TProperty : class
    {
        TOwnerComposer _ownerComposer;
        readonly IList<Func<IEnumerable<TProperty>>> _pendingAddedItemsFactory = new List<Func<IEnumerable<TProperty>>>();
        readonly Expression<Func<TOwner, IEnumerable<TProperty>>> _expression;
        public CollectionItemPropertyComposer(TOwnerComposer ownerComposer, Expression<Func<TOwner, IEnumerable<TProperty>>> expression,
            GenLaw law) : base(ownerComposer._fixture, law)
        {
            _ownerComposer = ownerComposer;
            _expression = expression;
        }

        private IEnumerable<TProperty> ResolveItemsValue(IEnumerable<TProperty> items, Func<IEnumerable<TProperty>>[] factories)
        {
            List<TProperty> res = new List<TProperty>();
            res.AddRange(items);
            foreach (var factory in factories)
                res.AddRange(factory());
            return res;
        }

        private Func<IEnumerable<TProperty>> CreateItemFactory(int count)
        {
            var pendingFactories = _pendingAddedItemsFactory.ToArray();
            var pendingComposer = _composer;
            Func<IEnumerable<TProperty>> itemsFactory = () =>
            {
                pendingComposer = ApplyConstraintRuleToComposer(pendingComposer);
                return ResolveItemsValue(pendingComposer.CreateMany(count), pendingFactories);
            };

            return itemsFactory;
        }

        public TOwnerComposer AddOneThenDone(OptionWhenRootCreateMany opt = OptionWhenRootCreateMany.ShareInstance)
        {
            return AddManyThenDone(1, opt);
        }

        public TOwnerComposer AddManyThenDone(int count = 3, OptionWhenRootCreateMany opt = OptionWhenRootCreateMany.ShareInstance)
        {
            if (opt == OptionWhenRootCreateMany.ShareInstance)
                _ownerComposer.With(_expression, CreateItemFactory(count));
            else _ownerComposer.With(_expression, CreateItemFactory(count).Invoke());
            return _ownerComposer;
        }

        public CollectionItemPropertyComposer<TOwner, TProperty, TOwnerComposer> AddManyThenBuildNew(int count = 3)
        {
            _pendingAddedItemsFactory.Add(CreateItemFactory(count));
            _composer = _fixture.Build<TProperty>();
            _composer = ApplyDefaultLawToComposer(_composer);
            return this;
        }

        public CollectionItemPropertyComposer<TOwner, TProperty, TOwnerComposer> AddOneThenBuildNew()
        {
            return AddManyThenBuildNew(1);
        }

        public CollectionItemPropertyComposer<TOwner, TProperty, TOwnerComposer> AddManyThenResumeBuild(int count = 3)
        {
            _pendingAddedItemsFactory.Add(CreateItemFactory(count));
            return this;
        }

        public CollectionItemPropertyComposer<TOwner, TProperty, TOwnerComposer> AddOneThenResumeBuild()
        {
            return AddManyThenResumeBuild(1);
        }
    }
}
