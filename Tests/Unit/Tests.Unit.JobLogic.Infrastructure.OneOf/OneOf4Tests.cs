using FluentAssertions;
using JobLogic.Infrastructure.OneOf;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.OneOf
{
    [TestClass]
    public class OneOf4Tests
    {

        [TestMethod]
        public void TestValueAtFirst()
        {
            var testObj = ValueGenerator.CreateObject<OneOfTestClass1>();
            OneOf<OneOfTestClass1, string, int, decimal> oneOf = testObj;

            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<OneOfTestClass1, string, int, decimal>>(strOneOf);

            newOneOf.Match(
                x => "Everything OK",
                x => throw new Exception(),
                x => throw new Exception(),
                x => throw new Exception());

            newOneOf.TryPick(out OneOfTestClass1 v, out _).Should().BeTrue();
            v.Should().BeEquivalentTo(testObj);

            newOneOf.TryPick(out string _, out _).Should().BeFalse();
            newOneOf.TryPick(out string _).Should().BeFalse();
            newOneOf.TryPick(out int _, out _).Should().BeFalse();
            newOneOf.TryPick(out int _).Should().BeFalse();
            newOneOf.TryPick(out decimal _, out _).Should().BeFalse();
            newOneOf.TryPick(out decimal _).Should().BeFalse();
        }

        [TestMethod]
        public void TestValueAtSecond()
        {
            OneOf<OneOfTestClass1, string, int, decimal> oneOf = "Test OneOf";

            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<OneOfTestClass1, string, int, decimal>>(strOneOf);

            newOneOf.Match(
                x => throw new Exception(),
                x => "Everything OK",
                x => throw new Exception(),
                x => throw new Exception());

            newOneOf.TryPick(out string v, out _).Should().BeTrue();
            v.Should().Be("Test OneOf");

            newOneOf.TryPick(out OneOfTestClass1 _, out _).Should().BeFalse();
            newOneOf.TryPick(out OneOfTestClass1 _).Should().BeFalse();
            newOneOf.TryPick(out int _, out _).Should().BeFalse();
            newOneOf.TryPick(out int _).Should().BeFalse();
            newOneOf.TryPick(out decimal _, out _).Should().BeFalse();
            newOneOf.TryPick(out decimal _).Should().BeFalse();
        }

        [TestMethod]
        public void TestValueAtThird()
        {
            OneOf<OneOfTestClass1, string, int, decimal> oneOf = 3;

            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<OneOfTestClass1, string, int, decimal>>(strOneOf);

            newOneOf.Match(
                x => throw new Exception(),
                x => throw new Exception(),
                x => "Everything OK",
                x => throw new Exception());

            newOneOf.TryPick(out int v, out _).Should().BeTrue();
            v.Should().Be(3);

            newOneOf.TryPick(out string _, out _).Should().BeFalse();
            newOneOf.TryPick(out string _).Should().BeFalse();
            newOneOf.TryPick(out OneOfTestClass1 _, out _).Should().BeFalse();
            newOneOf.TryPick(out OneOfTestClass1 _).Should().BeFalse();
            newOneOf.TryPick(out decimal _, out _).Should().BeFalse();
            newOneOf.TryPick(out decimal _).Should().BeFalse();
        }

        [TestMethod]
        public void TestValueAtLast()
        {
            OneOf<OneOfTestClass1, string, int, decimal> oneOf = 3.5M;

            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<OneOfTestClass1, string, int, decimal>>(strOneOf);

            newOneOf.Match(
                x => throw new Exception(),
                x => throw new Exception(),
                x => throw new Exception(),
                x => "Everything OK");

            newOneOf.TryPick(out decimal v, out _).Should().BeTrue();
            v.Should().Be(3.5M);

            newOneOf.TryPick(out string _, out _).Should().BeFalse();
            newOneOf.TryPick(out string _).Should().BeFalse();
            newOneOf.TryPick(out OneOfTestClass1 _, out _).Should().BeFalse();
            newOneOf.TryPick(out OneOfTestClass1 _).Should().BeFalse();
            newOneOf.TryPick(out int _, out _).Should().BeFalse();
            newOneOf.TryPick(out int _).Should().BeFalse();
        }

        [TestMethod]
        public void TestPrimitiveCollectionType()
        {
            var testObjs = ValueGenerator.CreateMany<OneOfTestClass1>();
            OneOf<OneOfTestClass1, int[], string, decimal> oneOf = new int[] { 3 };

            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<OneOfTestClass1, int[], string, decimal>>(strOneOf);

            newOneOf.Match(
                x => throw new Exception(),
                x => "Everything OK",
                x => throw new Exception(),
                x => throw new Exception());
        }

        [TestMethod]
        public void TestClassCollectionType()
        {
            var testObjs = ValueGenerator.CreateMany<OneOfTestClass1>();
            OneOf<string, IEnumerable<OneOfTestClass1>, int, decimal> oneOf = testObjs.ToList();

            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<string, IEnumerable<OneOfTestClass1>, int, decimal>>(strOneOf);

            newOneOf.Match(
                x => throw new Exception(),
                x => "Everything OK",
                x => throw new Exception(),
                x => throw new Exception());
        }

        [TestMethod]
        public void TestInheritanceValue_WhenAtFirst()
        {
            var testObj = ValueGenerator.CreateObject<OneOfTestClass1>();
            OneOf<BaseOneOfTestClass1, object, OneOfTestClass2> oneOf = testObj;

            oneOf.Match(
                x => "Everything OK",
                x => throw new Exception(),
                x => throw new Exception());



            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<BaseOneOfTestClass1, object, OneOfTestClass2>>(strOneOf);

            newOneOf.Match(
                x => "Everything OK",
                x => throw new Exception(),
                x => throw new Exception());
        }

        [TestMethod]
        public void TestInheritanceValue_WhenAtSecond()
        {
            var testObj = ValueGenerator.CreateObject<OneOfTestClass1>();

            OneOf<object, BaseOneOfTestClass1, OneOfTestClass2> oneOf2 = testObj;
            oneOf2.Match(
                x => throw new Exception(),
                x => "Everything OK",
                x => throw new Exception()
                );

            var strOneOf2 = JsonConvert.SerializeObject(oneOf2);
            var newOneOf2 = JsonConvert.DeserializeObject<OneOf<object, BaseOneOfTestClass1, OneOfTestClass2>>(strOneOf2);

            newOneOf2.Match(
                x => throw new Exception(),
                x => "Everything OK",
                x => throw new Exception()
                );
        }

        [TestMethod]
        public void TestInheritanceValue_WhenAtThird()
        {
            var testObj = ValueGenerator.CreateObject<OneOfTestClass1>();
            OneOf<object, OneOfTestClass2, BaseOneOfTestClass1> oneOf = testObj;

            oneOf.Match(
                x => throw new Exception(),
                x => throw new Exception(),
                x => "Everything OK");



            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<object, OneOfTestClass2, BaseOneOfTestClass1>>(strOneOf);

            newOneOf.Match(

                x => throw new Exception(),
                x => throw new Exception(),
                x => "Everything OK");
        }

        [TestMethod]
        public void TestInheritanceValue_WhenAtFourth()
        {
            var testObj = ValueGenerator.CreateObject<OneOfTestClass1>();
            OneOf<object, OneOfTestClass2,string, BaseOneOfTestClass1> oneOf = testObj;

            oneOf.Match(
                x => throw new Exception(),
                x => throw new Exception(),
                x => throw new Exception(),
                x => "Everything OK");



            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<object, OneOfTestClass2, string, BaseOneOfTestClass1>>(strOneOf);

            newOneOf.Match(

                x => throw new Exception(),
                x => throw new Exception(),
                x => throw new Exception(),
                x => "Everything OK");
        }

        class BaseOneOfTestClass1
        {
        }

        class OneOfTestClass1 : BaseOneOfTestClass1
        {
            public string ABC { get; set; }
            public string XYZ { get; set; }
        }

        class OneOfTestClass2
        {
            public string ABC { get; set; }
            public string XYZ { get; set; }
        }
    }

    
}
