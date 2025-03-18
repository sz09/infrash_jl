using FluentAssertions;
using JobLogic.Infrastructure.OData.Client;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.OData.Client
{
    [TestClass]
    public class ODataQueryBuilderTests
    {
        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenSelectManyNullableRelatedIdModel()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<NullableRelatedIdModel>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(NullableRelatedIdModel)}?").And.NotBeNullOrEmpty();
                return Task.FromResult(ValueGenerator.CreateMany<NullableRelatedIdModel>().ToArray());
            });
            var query = ODataQueryBuilder<NullableRelatedIdModel>.Create(mock.Object);
            int[] validRelatedIds = new int[] { 1, 2, };
            var result = await query.QueryAsync(q => q.Where(x => validRelatedIds.Contains(x.NullableRelatedId.Value)));
            result.Should().HaveCountGreaterThan(0);
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenSelectMany()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.NotBeNullOrEmpty();
                return Task.FromResult(ValueGenerator.CreateMany<Model>().ToArray());
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync();
            result.Should().HaveCountGreaterThan(0);
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenSelectManyWithKeyEqual()
        {
            int idValue = 3;
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain(idValue.ToString());
                return Task.FromResult(ValueGenerator.CreateMany<Model>().ToArray());
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.Id == idValue));
            result.Should().HaveCountGreaterThan(0);
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenUseContainCondition()
        {
            int[] idValues = new int[] { 3, 4 };
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.ContainAll(idValues.Select(x => x.ToString()));
                return Task.FromResult(ValueGenerator.CreateMany<Model>().ToArray());
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => idValues.Contains(x.Id)));
            result.Should().HaveCountGreaterThan(0);
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenSelectCustomManyWithKeyEqual()
        {
            int idValue = 3;
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain(idValue.ToString()).And.Contain(nameof(Model.Content));
                return Task.FromResult(ValueGenerator.CreateMany<Model>().ToArray());
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.Id == idValue).Select(x => new { x.Content}));
            result.Should().HaveCountGreaterThan(0);
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenSelectManyCustom()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?");
                return Task.FromResult(ValueGenerator.CreateMany<Model>().ToArray());
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Select(x => new
            {
                x.Content
            }));
            result.Should().HaveCountGreaterThan(0);
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenSelectManyWithWhereCondition()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?");
                return Task.FromResult(ValueGenerator.CreateMany<Model>().ToArray());
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.Id > 3));
            result.Should().HaveCountGreaterThan(0);
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenSelectCustomManyWithWhereCondition()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?");
                return Task.FromResult(ValueGenerator.CreateMany<Model>().ToArray());
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.Id > 3)
                .Select(x => new
                {
                    x.Content
                }));
            result.Should().HaveCountGreaterThan(0);
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenSelectFirst()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1");
                return Task.FromResult(new Model[] { ValueGenerator.CreateObject<Model>() });
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.FirstOrDefault());
            result.Should().NotBeNull();

            result = await query.QueryAsync(q => q.First());
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenSelectFirstWithOrderBy()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1");
                return Task.FromResult(new Model[] { ValueGenerator.CreateObject<Model>() });
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.OrderBy(x => x.Id).FirstOrDefault());
            result.Should().NotBeNull();
            result = await query.QueryAsync(q => q.OrderBy(x => x.Id).First());
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenSelectFirstWithOrderByAndWhere()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1");
                return Task.FromResult(new Model[] { ValueGenerator.CreateObject<Model>() });
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.OrderBy(x => x.Id).Where(x => x.Content.Length > 0).FirstOrDefault());
            result.Should().NotBeNull();
            result = await query.QueryAsync(q => q.OrderBy(x => x.Id).Where(x => x.Content.Length > 0).First());
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenSelectCustomFirst()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1");
                return Task.FromResult(new Model[] { ValueGenerator.CreateObject<Model>() });
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Select(x => new { x.Content }).FirstOrDefault());
            result.Should().NotBeNull();
            result = await query.QueryAsync(q => q.Select(x => new { x.Content }).First());
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenSelectCustomFirstWithWhere()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1");
                return Task.FromResult(new Model[] { ValueGenerator.CreateObject<Model>() });
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).Select(x => new { x.Content }).FirstOrDefault());
            result.Should().NotBeNull();
            result = await query.QueryAsync(q => q.Where(x => x.Id > 3).Select(x => new { x.Content }).First());
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHasCount_WhenSelectCustomSingleWithWhere()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=2");
                return Task.FromResult(new Model[] { ValueGenerator.CreateObject<Model>() });
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.Id == 3).Select(x => new { x.Content }).SingleOrDefault());
            result.Should().NotBeNull();
            result = await query.QueryAsync(q => q.Where(x => x.Id == 3).Select(x => new { x.Content }).Single());
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void TestQueryAsync_ShouldRaiseException_WhenSelectFirstWithoutData()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1");
                return Task.FromResult((Model[])null);
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            Func<Task> act = async () =>
            {
                var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).Select(x => new { x.Content }).First());
            };
            act.Should().Throw<InvalidOperationException>();
            act = async () =>
            {
                var result = await query.QueryAsync(q => q.Select(x => new { x.Content }).First());
            };
            act.Should().Throw<InvalidOperationException>();
            act = async () =>
            {
                var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).First());
            };
            act.Should().Throw<InvalidOperationException>();
            act = async () =>
            {
                var result = await query.QueryAsync(q => q.First());
            };
            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldNoException_WhenSelectFirstOrDefaultWithoutData()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1");
                return Task.FromResult((Model[])null);
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).FirstOrDefault());
            result.Should().BeNull();
            result = await query.QueryAsync(q => q.FirstOrDefault());
            result.Should().BeNull();
            var result2 = await query.QueryAsync(q => q.Where(x => x.Id > 3).Select(x => new { x.Content }).FirstOrDefault());
            result2.Should().BeNull();
            result2 = await query.QueryAsync(q => q.Select(x => new { x.Content }).FirstOrDefault());
            result2.Should().BeNull();
        }



        [TestMethod]
        public void TestQueryAsync_ShouldRaiseException_WhenSelectSingleWithoutData()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=2");
                return Task.FromResult((Model[])null);
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            Func<Task> act = async () =>
            {
                var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).Select(x => new { x.Content }).Single());
            };
            act.Should().Throw<InvalidOperationException>();
            act = async () =>
            {
                var result = await query.QueryAsync(q => q.Select(x => new { x.Content }).Single());
            };
            act.Should().Throw<InvalidOperationException>();
            act = async () =>
            {
                var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).Single());
            };
            act.Should().Throw<InvalidOperationException>();
            act = async () =>
            {
                var result = await query.QueryAsync(q => q.Single());
            };
            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void TestQueryAsync_ShouldRaiseException_WhenSelectSingleWithManyData()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=2");
                return Task.FromResult(ValueGenerator.CreateMany<Model>().ToArray());
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            Func<Task> act = async () =>
            {
                var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).Select(x => new { x.Content }).Single());
            };
            act.Should().Throw<InvalidOperationException>();
            act = async () =>
            {
                var result = await query.QueryAsync(q => q.Select(x => new { x.Content }).Single());
            };
            act.Should().Throw<InvalidOperationException>();
            act = async () =>
            {
                var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).Single());
            };
            act.Should().Throw<InvalidOperationException>();
            act = async () =>
            {
                var result = await query.QueryAsync(q => q.Single());
            };
            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldNoException_WhenSelectSingleOrDefaultWithoutData()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=2");
                return Task.FromResult((Model[])null);
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).SingleOrDefault());
            result.Should().BeNull();
            result = await query.QueryAsync(q => q.SingleOrDefault());
            result.Should().BeNull();
            var result2 = await query.QueryAsync(q => q.Where(x => x.Id > 3).Select(x => new { x.Content }).SingleOrDefault());
            result2.Should().BeNull();
            result2 = await query.QueryAsync(q => q.Select(x => new { x.Content }).SingleOrDefault());
            result2.Should().BeNull();
        }

        [TestMethod]
        public void TestQueryAsync_ShouldRaiseException_WhenSelectSingleOrDefaultWithManyData()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=2");
                return Task.FromResult(ValueGenerator.CreateMany<Model>().ToArray());
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            Func<Task> act = async () =>
            {
                var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).Select(x => new { x.Content }).SingleOrDefault());
            };
            act.Should().Throw<InvalidOperationException>();
            act = async () =>
            {
                var result = await query.QueryAsync(q => q.Select(x => new { x.Content }).SingleOrDefault());
            };
            act.Should().Throw<InvalidOperationException>();
            act = async () =>
            {
                var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).SingleOrDefault());
            };
            act.Should().Throw<InvalidOperationException>();
            act = async () =>
            {
                var result = await query.QueryAsync(q => q.SingleOrDefault());
            };
            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldWork_WhenSelectCount()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchCount<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?");
                return Task.FromResult((long)1);
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).OrderBy(x => x.Content).LongCount());
            result = await query.QueryAsync(q => q.Where(x => x.Id == 3).OrderBy(x => x.Content).LongCount());
            result = await query.QueryAsync(q => q.Where(x => x.Id == 3).OrderBy(x => x.Content).Select(x => new { x.Content }).LongCount());
            result = await query.QueryAsync(q => q.LongCount());


            var result2 = await query.QueryAsync(q => q.Where(x => x.Id > 3).OrderBy(x => x.Content).Count());
            result2 = await query.QueryAsync(q => q.Where(x => x.Id == 3).OrderBy(x => x.Content).Count());
            result2 = await query.QueryAsync(q => q.Where(x => x.Id == 3).OrderBy(x => x.Content).Select(x => new { x.Content }).Count());
            result2 = await query.QueryAsync(q => q.Count());
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldWork_WhenSelectAny()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1"); ;
                return Task.FromResult(new Model[] {new Model() });
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).OrderBy(x => x.Content).Any());
            result.Should().BeTrue();
            result = await query.QueryAsync(q => q.Where(x => x.Id == 3).OrderBy(x => x.Content).Any());
            result.Should().BeTrue();
            result = await query.QueryAsync(q => q.Where(x => x.Id == 3).OrderBy(x => x.Content).Select(x => new { x.Content }).Any());
            result.Should().BeTrue();
            result = await query.QueryAsync(q => q.Any());
            result.Should().BeTrue();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldWork_WhenSelectAnyFalse()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1");
                return Task.FromResult(new Model[] {  });
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.Id > 3).OrderBy(x => x.Content).Any());
            result.Should().BeFalse();
            result = await query.QueryAsync(q => q.Where(x => x.Id == 3).OrderBy(x => x.Content).Any());
            result.Should().BeFalse();
            result = await query.QueryAsync(q => q.Where(x => x.Id == 3).OrderBy(x => x.Content).Select(x => new { x.Content }).Any());
            result.Should().BeFalse();
            result = await query.QueryAsync(q => q.Any());
            result.Should().BeFalse();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldWork_WhenSelectOrderByWithSinglePropertyCondition()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?");
                return Task.FromResult(ValueGenerator.CreateMany<Model>().ToArray());
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.ModelSingleProperty.Id == 3).OrderBy(x => x.Content));
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldWork_WhenSelectOrderByWithListPropertyCondition()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?");
                return Task.FromResult(ValueGenerator.CreateMany<Model>().ToArray());
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.ModelListProperties.Any(x => x.Id == 3)).OrderBy(x => x.Content)
                    .Select(x => new { xx = x.ModelListProperties }));
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldWork_WithExpandAndWithWhereBefore()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().Contain($"expand={nameof(ModelSingleProperty)}");
                return Task.FromResult(ValueGenerator.CreateMany<Model>().ToArray());
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Where(x => x.Id == ValueGenerator.Int(1,100)).Expand(x => x.ModelSingleProperty));
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldWork_WithExpandAndWithWhereAfter()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().Contain($"expand={nameof(Model.ModelListProperties)}");
                return Task.FromResult(ValueGenerator.CreateMany<Model>().ToArray());
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Expand(x => x.ModelListProperties).Where(x => x.Id == ValueGenerator.Int(1, 100)));
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldWork_WhenSelectAnyWithConditionReturnTrue()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1"); ;
                return Task.FromResult(new Model[] { new Model() });
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Any(x => x.Id > 3));
            result.Should().BeTrue();
            result = await query.QueryAsync(q => q.Any(x => x.Id == 3));
            result.Should().BeTrue();
            result = await query.QueryAsync(q => q.Where(x => x.Id < 2).Any(x => x.Id == 3));
            result.Should().BeTrue();
            result = await query.QueryAsync(q => q.Any());
            result.Should().BeTrue();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldWork_WhenSelectAnyWithConditionReturnFalse()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1");
                return Task.FromResult(new Model[] { });
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.Any(x => x.Id > 3));
            result.Should().BeFalse();
            result = await query.QueryAsync(q => q.Any(x => x.Id == 3));
            result.Should().BeFalse();
            result = await query.QueryAsync(q => q.Where(x => x.Id < 2).Any(x => x.Id == 3));
            result.Should().BeFalse();
            result = await query.QueryAsync(q => q.Any());
            result.Should().BeFalse();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHaveData_WhenSelectFirstWithCondition()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1");
                return Task.FromResult(new Model[] { ValueGenerator.CreateObject<Model>() });
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.FirstOrDefault(x => x.Content.Length > 0));
            result.Should().NotBeNull();

            result = await query.QueryAsync(q => q.Where(x => x.Id < 2).FirstOrDefault(x => x.Content.Length > 0));
            result.Should().NotBeNull();

            result = await query.QueryAsync(q => q.First(x => x.Content.Length > 0));
            result.Should().NotBeNull();

            result = await query.QueryAsync(q => q.Where(x => x.Id < 2).First(x => x.Content.Length > 0));
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHaveData_WhenSelectFirstWithConditionAndOrderBy()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1");
                return Task.FromResult(new Model[] { ValueGenerator.CreateObject<Model>() });
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.OrderBy(x => x.Id).FirstOrDefault(x => x.Content.Length > 0));
            result.Should().NotBeNull();
            result = await query.QueryAsync(q => q.OrderBy(x => x.Id).Where(x => x.Content == "d").FirstOrDefault(x => x.Content.Length > 0));
            result.Should().NotBeNull();
            result = await query.QueryAsync(q => q.OrderBy(x => x.Id).First(x => x.Content.Length > 0));
            result.Should().NotBeNull();
            result = await query.QueryAsync(q => q.OrderBy(x => x.Id).Where(x => x.Content == "d").First(x => x.Content.Length > 0));
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void TestQueryAsync_ShouldRaiseException_WhenSelectFirstWithConditionWithoutData()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1");
                return Task.FromResult((Model[])null);
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            Func<Task> act = async () =>
            {
                var result = await query.QueryAsync(q => q.First(x => x.Id > 3));
            };
            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldNoException_WhenSelectFirstOrDefaultWithConditionWithoutData()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=1");
                return Task.FromResult((Model[])null);
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.FirstOrDefault(x => x.Id > 3));
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldHaveData_WhenSelectSingleWithCondition()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=2");
                return Task.FromResult(new Model[] { ValueGenerator.CreateObject<Model>() });
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            var result = await query.QueryAsync(q => q.SingleOrDefault(x => x.Id == 3));
            result.Should().NotBeNull();
            result = await query.QueryAsync(q => q.Where(x => x.Content == "d").SingleOrDefault(x => x.Id == 3));
            result.Should().NotBeNull();
            result = await query.QueryAsync(q => q.Single(x => x.Id == 3));
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void TestQueryAsync_ShouldRaiseException_WhenSelectSingleWithConditionWithoutData()
        {
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("$top=2");
                return Task.FromResult((Model[])null);
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            Func<Task> act = async () =>
            {
                var result = await query.QueryAsync(q => q.Single(x => x.Id > 3));
            };
            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public async Task TestQueryAsync_ShouldEscapeSpecialChar_WhenUseContains()
        {
            var clause = new[] { "a & b" };
            var mock = new Mock<IODataFetcher>();
            mock.Setup(x => x.FetchData<Model>(It.IsAny<string>())).Returns<string>(x =>
            {
                x.Should().StartWith($"/{nameof(Model)}?").And.Contain("'a%20%26%20b'");
                return Task.FromResult((Model[])null);
            });
            var query = ODataQueryBuilder<Model>.Create(mock.Object);
            Func<Task> act = async () =>
            {
                var result = await query.QueryAsync(q => q.Where(x => clause.Contains(x.Content)));
            };
            await act.Should().NotThrowAsync();
        }
    }

    public class Model
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public ModelSingleProperty ModelSingleProperty { get; set; }
        public ICollection<ModelListProperty> ModelListProperties { get; set; }
    }

    public class NullableRelatedIdModel
    {
        [Key]
        public int Id { get; set; }
        public int? NullableRelatedId { get; set; }
    }

    public class ModelSingleProperty
    {
        [Key]
        public int Id { get; set; }
        public string SingleProp1 { get; set; }
    }

    public class ModelListProperty
    {
        [Key]
        public int Id { get; set; }
        public string ListProp1 { get; set; }
    }
}
