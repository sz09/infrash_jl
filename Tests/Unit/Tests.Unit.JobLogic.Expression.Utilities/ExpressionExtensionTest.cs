namespace Tests.Unit.JobLogic.Expression.Utilities
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Linq.Expressions;
    using global::JobLogic.Expression.Utilities;
    using global::JobLogic.Infrastructure.UnitTest;

    [TestClass]
    public class ExpressionExtensionTest : BaseUnitTest
    {
        [TestMethod]
        public void Combine_Expressions_With_AndOperator_Return_Correct_Result()
        {
            // Arrange
            var models = Enumerable.Range(0, 20).Select(x => new TestModel() { Id = x, Name = string.Format("Name{0}", x) }).ToList();

            int minNumber = 2;
            int maxNumber = 10;

            Expression<Func<TestModel, bool>> idCompareWithMinNumber = m => (m.Id >= minNumber);
            Expression<Func<TestModel, bool>> idCompareWithMaxNumber = m => (m.Id <= maxNumber);

            var expression = idCompareWithMinNumber.And(idCompareWithMaxNumber);

            // Action
            var result = models.AsQueryable().Where(expression).ToList();

            // Assert
            var filter = models.Where(x => x.Id >= minNumber && x.Id <= maxNumber).ToList();
            Assert.AreEqual(filter.Count, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(filter[i].Id, result[i].Id);
                Assert.AreEqual(filter[i].Name, result[i].Name);
            }
        }

        [TestMethod]
        public void Combine_Expressions_With_OrOperator_Return_Correct_Result()
        {
            // Arrange
            var models = Enumerable.Range(0, 20).Select(x => new TestModel() { Id = x, Name = string.Format("Name{0}", x) }).ToList();

            int comparedNumber = 6;
            string comparedString = "8";

            Expression<Func<TestModel, bool>> idCompareWithNumber = m => (m.Id <= comparedNumber);
            Expression<Func<TestModel, bool>> nameCompareWithString = m => (m.Name.Contains(comparedString));

            var expression = idCompareWithNumber.Or(nameCompareWithString);

            // Action
            var result = models.AsQueryable().Where(expression).ToList();

            // Assert
            var filter = models.Where(x => x.Id <= comparedNumber || x.Name.Contains(comparedString)).ToList();
            Assert.AreEqual(filter.Count, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(filter[i].Id, result[i].Id);
                Assert.AreEqual(filter[i].Name, result[i].Name);
            }
        }
    }
}
