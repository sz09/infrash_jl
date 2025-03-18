using JobLogic.Infrastructure.Dapper;
using JobLogic.Infrastructure.UnitTest;

namespace Tests.Unit.JobLogic.Infrastructure.Dapper
{
    [TestClass]
    public class BaseSqlStatementBuilderTests
    {
        [TestMethod]
        public void TestCreateSqlStatement_ShouldWork_WhenUseSqlStatementBuilder()
        {
            var tt = new TestSqlStatementBuilder(new List<Person> {
                ValueGenerator.BuildObject<Person>().With(x => x.PersonID, 1).Create(),
                ValueGenerator.BuildObject<Person>().With(x => x.PersonID, 2).Create(),
                ValueGenerator.BuildObject<Person>().With(x => x.PersonID, 3).Create()
            });

            var sqlStatement = tt.CreateSqlStatement();
        }

        public class Person
        {
            public int PersonID { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
        }

        public class TestSqlStatementBuilder : BaseSqlStatementBuilder
        {
            private readonly List<Person> _people;


            public TestSqlStatementBuilder(List<Person> people)
            {
                _people = people;
            }
            protected override string BuildStatement()
            {

                string sql = $@"
                    MERGE Persons AS o
                    USING (VALUES {BuildPersonParamList()} AS s(PersonID, LastName, City)
                    ON s.PersonID = o.PersonID
                    WHEN MATCHED THEN
                      UPDATE SET o.LastName = s.LastName,
                                 o.City = s.City
                    WHEN NOT MATCHED THEN
                      INSERT(PersonID,
                             LastName,
                             City)
                      VALUES(s.PersonID,
                             s.LastName,
                             s.City); ";

                return sql;
            }

            private string BuildPersonParamList()
            {
                return SqlImplodeComa((_, v) => BuildPersonItemParamList(v), _people);
            }

            private string BuildPersonItemParamList(Person v)
            {
                var n0 = Sql(getParamFunc => getParamFunc(), v.PersonID);
                var n1 = Sql(getParamFunc => getParamFunc(), v.Name);
                var n2 = Sql(getParamFunc => getParamFunc(), v.City);
                return $"({n0},{n1},{n2})";
            }




        }
    }
}