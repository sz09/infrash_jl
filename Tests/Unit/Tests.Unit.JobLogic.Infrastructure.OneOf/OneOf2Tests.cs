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
    public class OneOf2Tests
    {
        [TestMethod]
        public async Task TestPrimitiveType()
        {
            OneOf<int, string> oneOf = 3;
            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<int, string>>(strOneOf);

            newOneOf.Match(
                x => "Everything OK",
                x => throw new Exception());

            newOneOf.TryPick(out int v, out _).Should().BeTrue();
            v.Should().Be(3);

            newOneOf.TryPick(out string _, out _).Should().BeFalse();
            newOneOf.TryPick(out string _).Should().BeFalse();
        }

        [TestMethod]
        public async Task TestClassType()
        {
            var testObj = ValueGenerator.CreateObject<OneOfTestClass1>();
            OneOf<OneOfTestClass1, string> oneOf = testObj;

            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<OneOfTestClass1, string>>(strOneOf);

            newOneOf.Match(
                x => "Everything OK",
                x => throw new Exception());

            newOneOf.TryPick(out OneOfTestClass1 v, out _).Should().BeTrue();
            v.Should().BeEquivalentTo(testObj);

            newOneOf.TryPick(out string _, out _).Should().BeFalse();
            newOneOf.TryPick(out string _).Should().BeFalse();
        }

        [TestMethod]
        public void TestPrimitiveType_CorrectDeserializedData()
        {
            OneOf<int, string> oneOf = 3;
            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<int, string>>(strOneOf);

            var data = newOneOf.Match(
                x => x,
                x => throw new Exception());

            data.Should().Be(3);
        }

        [TestMethod]
        public void TestClassType_CorrectDeserializedData()
        {
            var testObj = ValueGenerator.CreateObject<OneOfTestClass1>();
            OneOf<OneOfTestClass1, string> oneOf = testObj;

            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<OneOfTestClass1, string>>(strOneOf);

            var data = newOneOf.Match(
                x => x,
                x => throw new Exception());

            data.ABC.Should().Be(testObj.ABC);
            data.XYZ.Should().Be(testObj.XYZ);
        }

        [TestMethod]
        public void TestPrimitiveCollectionType()
        {
            OneOf<IEnumerable<int>, string> oneOf = new List<int> { 3 };
            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<IEnumerable<int>, string>>(strOneOf);

            newOneOf.Match(
                x => "Everything OK",
                x => throw new Exception());
        }

        [TestMethod]
        public void TestClassCollectionType()
        {
            var testObjs = ValueGenerator.CreateMany<OneOfTestClass1>();
            OneOf<IEnumerable<OneOfTestClass1>, string> oneOf = testObjs.ToList();

            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<IEnumerable<OneOfTestClass1>, string>>(strOneOf);

            newOneOf.Match(
                x => "Everything OK",
                x => throw new Exception());
        }

        [TestMethod]
        public void TestInheritanceValue_WhenHasObjectOnly()
        {
            var testObj = ValueGenerator.CreateObject<OneOfTestClass2>();
            OneOf<BaseOneOfTestClass1, object> oneOf = testObj;

            oneOf.Match(
                x => throw new Exception(),
                x => "Everything OK"
                );



            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<BaseOneOfTestClass1, object>>(strOneOf);

            newOneOf.Match(
                x => throw new Exception(),
                x => "Everything OK");
        }

        [TestMethod]
        public void TestInheritanceValue_WhenAtFirst()
        {
            var testObj = ValueGenerator.CreateObject<OneOfTestClass1>();
            OneOf<BaseOneOfTestClass1, object> oneOf = testObj;

            oneOf.Match(
                x => "Everything OK",
                x => throw new Exception());

            

            var strOneOf = JsonConvert.SerializeObject(oneOf);
            var newOneOf = JsonConvert.DeserializeObject<OneOf<BaseOneOfTestClass1, object>>(strOneOf);

            newOneOf.Match(
                x => "Everything OK",
                x => throw new Exception());
        }

        [TestMethod]
        public void TestInheritanceValue_WhenAtSecond()
        {
            var testObj = ValueGenerator.CreateObject<OneOfTestClass1>();

            OneOf<object, BaseOneOfTestClass1> oneOf2 = testObj;
            oneOf2.Match(
                x => throw new Exception(),
                x => "Everything OK"
                );

            var strOneOf2 = JsonConvert.SerializeObject(oneOf2);
            var newOneOf2 = JsonConvert.DeserializeObject<OneOf<object, BaseOneOfTestClass1>>(strOneOf2);

            newOneOf2.Match(
                x => throw new Exception(),
                x => "Everything OK"
                );
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
