using AutoFixture;
using AutoFixture.Kernel;
using Fare;
using JobLogic.Infrastructure.UnitTest.EFCore.AutoFixture;
using JobLogic.Infrastructure.UnitTest.EFCore.AutoFixture.Rules;
using System.Reflection;

namespace JobLogic.Infrastructure.UnitTest.EFCore
{
    public static class ValueGenerator
    {
        private readonly static Fixture _fixture;
        private readonly static Random _random = new Random();
        static ValueGenerator()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new IgnoreVirtualMembersAndDefaultCollectionEmpty());
        }
        public static ObjectBuilder<T> BuildObject<T>(GenLaw law = null) where T : class
        {
            return new ObjectBuilder<T>(_fixture, law);
        }
        public static T CreateObject<T>() where T:class
        {
            return _fixture.Create<T>();
        }
        public static T GenerateExcept<T>(T val) where T:struct
        {
            return _fixture.Create<Generator<T>>().First(x => !x.Equals(val));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxCharacters"></param>
        /// <returns></returns>
        public static string String(int maxCharacters = 256)
        {
            var val = _fixture.Create<string>();
            if(val.Length > maxCharacters)
                val = val.Substring(0, maxCharacters);
            return val;
        }

        public static string StringWithPattern(string pattern)
        {
            var xeger = new Xeger(pattern);
            xeger.Generate();
            return xeger.Generate();
        }

        public static string StringEmail()
        {
            return StringWithPattern(@"[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]+");
        }

        public static string StringNotEmail()
        {
            return RandomFromList(new string[] {
                StringWithPattern(@"\w+[^@]+"), //without @
                StringWithPattern(@"(.*@.*){2,}"), //more than 2 @
                StringWithPattern(@"\w+[^\.]+"), //without .
                StringWithPattern(@"[@\.]+"), //start with @ or .
                StringWithPattern(@".*@\..*"), //@ stay next to . each other
                StringWithPattern(@"\w+[^@\.]+"), //without both @ and .
                });
        }

        public static T RandomFromList<T>(params T[] elements)
        {
            return RandomFromList(elements.AsEnumerable());
        }

        public static T RandomFromList<T>(IEnumerable<T> elements)
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(
                new ElementsBuilder<T>(
                    elements));

            var actual = fixture.Create<T>();
            return actual;
        }

        public static T RandomFromEnum<T>() where T : struct
        {
            return RandomFromList((T[])Enum.GetValues(typeof(T)));
        }

        public static int Int(int min = 0, int max = int.MaxValue)
        {
            return _random.Next(min,max);
        }
        public static bool Bool()
        {
            return _random.Next(2) == 0;
        }

        public static DateTime DateTime()
        {
            return _fixture.Create<DateTime>();
        }

        public static string DateFormat()
        {
            return RandomFromList("dd/MM/yyyy", "MM/dd/yyyy");
        }

        public static DateTime DateTimeGreaterThan(DateTime date, TimeSpan leastTimeSpan)
        {
            var timespan = _fixture.Create<TimeSpan>();
            if (timespan < leastTimeSpan) timespan = leastTimeSpan;
            return date.Add(timespan);
        }

        public static DateTime DateGreaterThan(DateTime date)
        {
            return DateTimeGreaterThan(date, TimeSpan.FromDays(1));
        }

        public static DateTime DateGreaterThanOrEqual(DateTime date, int equalityHappen1OutOf = 5)
        {
            if (_random.Next(equalityHappen1OutOf) == 0) return date;
            return DateTimeGreaterThan(date, TimeSpan.FromDays(1));
        }

        

        public static DateTime DateTimeLessThan(DateTime date, TimeSpan leastTimeSpan)
        {
            var timespan = _fixture.Create<TimeSpan>();
            if (timespan < leastTimeSpan) timespan = leastTimeSpan;
            return date.Subtract(timespan);
        }

        public static DateTime DateLessThan(DateTime date)
        {
            return DateTimeLessThan(date, TimeSpan.FromDays(1));
        }

        public static DateTime DateLessThanOrEqual(DateTime date, int equalityHappen1OutOf = 5)
        {
            if (_random.Next(equalityHappen1OutOf) == 0) return date;
            return DateTimeLessThan(date, TimeSpan.FromDays(1));
        }

        public static IEnumerable<T> CreateMany<T>(int count = 3)
        {
            return _fixture.CreateMany<T>(count);
        }

        public static IEnumerable<T> CreateMany<T>(Func<T> valueFactory, int count = 3)
        {
            var res = new List<T>();
            while (count-- > 0)
            {
                res.Add(valueFactory());
            }
            return res;
        }

        public static Guid Guid()
        {
            return _fixture.Create<Guid>();
        }
    }

    /// <summary>
    /// Customisation to ignore the virtual members in the class - helps ignoring the navigational 
    /// properties and makes it quicker to generate objects when you don't care about these
    /// </summary>
    internal class IgnoreVirtualMembersAndDefaultCollectionEmpty : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var pi = request as PropertyInfo;
            if (pi == null || !pi.GetGetMethod().IsVirtual)
            {
                return new NoSpecimen();
            }
            if(pi != null)
            {
                var pipt = pi.PropertyType;
                if(pipt.IsGenericType && pipt.GetGenericTypeDefinition().IsAssignableFrom(typeof(ICollection<>)))
                {
                    Type itemType = pipt.GetGenericArguments()[0];
                    Type listGenericType = typeof(List<>);
                    Type list = listGenericType.MakeGenericType(itemType);
                    ConstructorInfo ci = list.GetConstructor(new Type[] { });
                    return ci.Invoke(new object[] { });
                }
            }
            return null;
        }
    }
}
