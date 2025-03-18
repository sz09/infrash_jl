using FluentAssertions;
using JobLogic.Infrastructure.Dapper;
using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration.JobLogic.Infrastructure.Dapper
{
    [TestClass]
    public class TestDbExecutionDapper : BaseTenancyTests
    {
        public TestDbExecutionDapper()
        {
        }

        public class SelectTagSqlBuilder : BaseDapperSqlBuilder
        {
            protected override DapperSqlStatementModel BuildDapperStatement()
            {
                return new DapperSqlStatementModel()
                {
                    SQLStatementString = "select top 10 * from Tag"
                };
            }
        }

        public class InsertTagSqlBuilder : BaseDapperSqlBuilder
        {
            private readonly Param _param;

            public InsertTagSqlBuilder(Param param)
            {
                _param = param;
            }
            public class Param
            {
                public string Title { get; set; }
            }
            protected override DapperSqlStatementModel BuildDapperStatement()
            {
                return new DapperSqlStatementModel()
                {
                    SQLStatementString = $"insert into Tag (Title) values (@{nameof(Param.Title)})",
                    SQLParams = _param
                };
            }
        }

        public class SoftDeleteTagSqlBuilder : BaseDapperSqlBuilder
        {
            private readonly Guid _uniqueId;

            public SoftDeleteTagSqlBuilder(Guid uniqueId)
            {
                _uniqueId = uniqueId;
            }

            protected override DapperSqlStatementModel BuildDapperStatement()
            {
                return new DapperSqlStatementModel()
                {
                    SQLStatementString = $"update Tag set Deleted = 1 where UniqueId = @UniqueId",
                    SQLParams = new
                    {
                        UniqueId = _uniqueId
                    }
                };
            }
        }

        public class TagModel
        {
            public Guid TenantId { get; set; }
            public string Title { get; set; }
            public Guid UniqueId { get; set; }
        }

        [TestMethod]
        public async Task TestExecuteAsync_ShouldWork_WhenInsert()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();
            var opInfo = GetService<ITenancyOperationInfo>();
            var p = new InsertTagSqlBuilder.Param
            {
                Title = ValueGenerator.String()
            };

            //Action
            await dbDapper.ExecuteAsync(new InsertTagSqlBuilder(p).CreateSqlStatement());

            //Assert
            var rs = await dbDapper.QueryAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());
            rs.Should().HaveCount(1);
            var rsItem = rs.ElementAt(0);
            rsItem.Title.Should().Be(p.Title);
            rsItem.UniqueId.Should().NotBeEmpty();
            rsItem.TenantId.Should().Be(opInfo.TenantId);
        }

        [TestMethod]
        public async Task TestExecuteAsync_ShouldWork_WhenPerformSoftDelete()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();
            var opInfo = GetService<ITenancyOperationInfo>();
            var p = new InsertTagSqlBuilder.Param
            {
                Title = ValueGenerator.String()
            };
            await dbDapper.ExecuteAsync(new InsertTagSqlBuilder(p).CreateSqlStatement());
            var rs = await dbDapper.QueryAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());
            rs.Should().HaveCount(1);

            //Action
            await dbDapper.ExecuteAsync(new SoftDeleteTagSqlBuilder(rs.Single().UniqueId).CreateSqlStatement());

            //Assert
            rs = await dbDapper.QueryAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());
            rs.Should().HaveCount(0);

            rs = await dbDapper.QueryAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement(true));
            rs.Should().HaveCount(1);
            rs.Single().Title.Should().Be(p.Title);
            rs.Single().UniqueId.Should().NotBeEmpty();
            rs.Single().TenantId.Should().Be(opInfo.TenantId);

        }


        [TestMethod]
        public async Task TestQueryAsync_ShouldWork_WithManyItems()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();
            var seedData = ValueGenerator.CreateMany<InsertTagSqlBuilder.Param>();
            foreach (var item in seedData)
            {
                await dbDapper.ExecuteAsync(new InsertTagSqlBuilder(item).CreateSqlStatement());
            }
            //Action
            var rs = await dbDapper.QueryAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());

            //Assert
            rs.Should().HaveCount(seedData.Count()).And.HaveCountGreaterOrEqualTo(1);
            rs.Select(x => x.Title).Should().BeEquivalentTo(seedData.Select(x => x.Title));
        }


        [TestMethod]
        public async Task TestExecuteReaderAsync_ShouldWork_WithManyItems()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();
            var seedData = ValueGenerator.CreateMany<InsertTagSqlBuilder.Param>();
            foreach (var item in seedData)
            {
                await dbDapper.ExecuteAsync(new InsertTagSqlBuilder(item).CreateSqlStatement());
            }
            //Action
            var rs = await dbDapper.ExecuteReaderAsync(
                new SelectTagSqlBuilder().CreateSqlStatement(),
                async reader =>
                {
                    List<TagModel> tagModels = new List<TagModel>();
                    while (await reader.ReadAsync())
                    {
                        tagModels.Add(new TagModel
                        {
                            TenantId = Guid.Parse(reader["TenantId"].ToString()),
                            Title = reader["Title"].ToString(),
                            UniqueId = Guid.Parse(reader["UniqueId"].ToString()),
                        });
                    }
                    return tagModels;
                });
            //Assert
            rs.Should().HaveCount(seedData.Count()).And.HaveCountGreaterOrEqualTo(1);
            rs.Select(x => x.Title).Should().BeEquivalentTo(seedData.Select(x => x.Title));
        }

        [TestMethod]
        public async Task TestQueryFirstAsync_ShouldWork_WithManyItems()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();
            var seedData = ValueGenerator.CreateMany<InsertTagSqlBuilder.Param>();
            foreach (var item in seedData)
            {
                await dbDapper.ExecuteAsync(new InsertTagSqlBuilder(item).CreateSqlStatement());
            }
            //Action
            var rs = await dbDapper.QueryFirstAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());

            //Assert
            rs.Should().NotBeNull();
            seedData.Select(x => x.Title).Should().Contain(rs.Title);
        }

        [TestMethod]
        public async Task TestQueryFirstAsync_ShouldWork_With1Item()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();
            var seedData = ValueGenerator.CreateMany<InsertTagSqlBuilder.Param>(1);
            foreach (var item in seedData)
            {
                await dbDapper.ExecuteAsync(new InsertTagSqlBuilder(item).CreateSqlStatement());
            }
            //Action
            var rs = await dbDapper.QueryFirstAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());

            //Assert
            rs.Should().NotBeNull();
            seedData.Select(x => x.Title).Should().Contain(rs.Title);
        }

        [TestMethod]
        public async Task TestQueryFirstAsync_ShouldThrow_With0Item()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();
            //Action
            var rs = new Func<Task>(async() => 
                await dbDapper.QueryFirstAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement())
                );

            //Assert
            var tt = await rs.Should().ThrowExactlyAsync<InvalidOperationException>();
            tt.And.Message.Should().Be("Sequence contains no elements");
        }

        [TestMethod]
        public async Task TestQueryFirstOrDefaultAsync_ShouldReturnNULL_With0Item()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();

            //Action
            var rs = await dbDapper.QueryFirstOrDefaultAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());

            //Assert
            rs.Should().BeNull();
        }

        [TestMethod]
        public async Task TestQueryFirstOrDefaultAsync_ShouldWork_With1Item()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();
            var seedData = ValueGenerator.CreateMany<InsertTagSqlBuilder.Param>(1);
            foreach (var item in seedData)
            {
                await dbDapper.ExecuteAsync(new InsertTagSqlBuilder(item).CreateSqlStatement());
            }

            //Action
            var rs = await dbDapper.QueryFirstOrDefaultAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());

            //Assert
            rs.Should().NotBeNull();
            seedData.Select(x => x.Title).Should().Contain(rs.Title);
        }

        [TestMethod]
        public async Task TestQueryFirstOrDefaultAsync_ShouldWork_WithManyItem()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();
            var seedData = ValueGenerator.CreateMany<InsertTagSqlBuilder.Param>();
            foreach (var item in seedData)
            {
                await dbDapper.ExecuteAsync(new InsertTagSqlBuilder(item).CreateSqlStatement());
            }

            //Action
            var rs = await dbDapper.QueryFirstOrDefaultAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());

            //Assert
            rs.Should().NotBeNull();
            seedData.Select(x => x.Title).Should().Contain(rs.Title);
        }

        [TestMethod]
        public async Task TestQuerySingleAsync_ShouldThrow_WithManyItem()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();
            var seedData = ValueGenerator.CreateMany<InsertTagSqlBuilder.Param>();
            foreach (var item in seedData)
            {
                await dbDapper.ExecuteAsync(new InsertTagSqlBuilder(item).CreateSqlStatement());
            }

            //Action
            var rs = () => dbDapper.QuerySingleAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());

            //Assert
            var tt = await rs.Should().ThrowExactlyAsync<InvalidOperationException>();
            tt.And.Message.Should().Be("Sequence contains more than one element");
        }

        [TestMethod]
        public async Task TestQuerySingleAsync_ShouldThrow_With0Item()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();

            //Action
            var rs = () => dbDapper.QuerySingleAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());

            //Assert
            var tt = await rs.Should().ThrowExactlyAsync<InvalidOperationException>();
            tt.And.Message.Should().Be("Sequence contains no elements");
        }

        [TestMethod]
        public async Task TestQueryQuerySingleAsync_ShouldWork_With1Item()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();
            var seedData = ValueGenerator.CreateMany<InsertTagSqlBuilder.Param>(1);
            foreach (var item in seedData)
            {
                await dbDapper.ExecuteAsync(new InsertTagSqlBuilder(item).CreateSqlStatement());
            }

            //Action
            var rs = await dbDapper.QuerySingleAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());

            //Assert
            rs.Should().NotBeNull();
            seedData.Select(x => x.Title).Should().Contain(rs.Title);
        }


        [TestMethod]
        public async Task TestQuerySingleOrDefaultAsync_ShouldThrow_WithManyItem()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();
            var seedData = ValueGenerator.CreateMany<InsertTagSqlBuilder.Param>();
            foreach (var item in seedData)
            {
                await dbDapper.ExecuteAsync(new InsertTagSqlBuilder(item).CreateSqlStatement());
            }

            //Action
            var rs = () => dbDapper.QuerySingleOrDefaultAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());

            //Assert
            var tt = await rs.Should().ThrowExactlyAsync<InvalidOperationException>();
            tt.And.Message.Should().Be("Sequence contains more than one element");
        }

        [TestMethod]
        public async Task TestQuerySingleOrDefaultAsync_ShouldReturnNULL_With0Item()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();

            //Action
            var rs = await dbDapper.QuerySingleOrDefaultAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());

            //Assert
            rs.Should().BeNull();
        }

        [TestMethod]
        public async Task TestQuerySingleOrDefaultAsync_ShouldWork_With1Item()
        {
            //Arrange
            var dbDapper = GetService<TenancyDapper>();
            var seedData = ValueGenerator.CreateMany<InsertTagSqlBuilder.Param>(1);
            foreach (var item in seedData)
            {
                await dbDapper.ExecuteAsync(new InsertTagSqlBuilder(item).CreateSqlStatement());
            }

            //Action
            var rs = await dbDapper.QuerySingleOrDefaultAsync<TagModel>(new SelectTagSqlBuilder().CreateSqlStatement());

            //Assert
            rs.Should().NotBeNull();
            seedData.Select(x => x.Title).Should().Contain(rs.Title);
        }
    }
}