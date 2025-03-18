using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using MockQueryable.Moq;

namespace JobLogic.Infrastructure.UnitTest.EFCore
{
    public static class MockDbSetHelper
    {
        public static DbSet<T> ToMockDbSetObject<T>(this List<T> data) where T : class
        {
            return data.AsQueryable().BuildMockDbSet().Object;
        }

        public static DbSet<T> ToMockDbSetObject<T>(this T data) where T : class
        {
            return ToMockDbSetObject(new List<T> { data });
        }

        public static void SetUpDbSetWithRandom<T, TPropertyItem>(this Mock<T> mock, Expression<Func<T, DbSet<TPropertyItem>>> expression, int? count = null)
            where T : class
            where TPropertyItem : class

        {
            if (count == null) count = ValueGenerator.Int(0, 5);
            if (count == 0)
                mock.Setup(expression).Returns((new List<TPropertyItem>().ToMockDbSetObject()));
            else
                mock.Setup(expression).Returns((ValueGenerator.CreateMany<TPropertyItem>(count.Value).ToList().ToMockDbSetObject()));
        }

        public static void SetUpDbSetWithValue<T, TPropertyItem>(this Mock<T> mock, Expression<Func<T, DbSet<TPropertyItem>>> expression, params TPropertyItem[] items)
            where T : class
            where TPropertyItem : class

        {
            mock.Setup(expression).Returns((items.ToList().ToMockDbSetObject()));
        }

        public static Mock<DbSet<T>> ToMockDbSet<T>(this List<T> data) where T : class
        {
            return data.AsQueryable().BuildMockDbSet();
        }
    }
}
