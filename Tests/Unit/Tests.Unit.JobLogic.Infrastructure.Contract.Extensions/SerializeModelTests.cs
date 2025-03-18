using FluentAssertions;
using JobLogic.Infrastructure.Contract.Extensions;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Tests.Unit.JobLogic.Infrastructure.Contract.Extensions
{
    [TestClass]
    public class SerializeModelTests
    {

        [TestMethod]
        public void TestSerialzieDeserializeContract_OK()
        {
            void TestAndCompareSerializeAndDeserialize<T>(T model)
            {
                var modelStr = JsonConvert.SerializeObject(model);
                var modelDeserializedObj = JsonConvert.DeserializeObject<T>(modelStr);
                model.Should().BeEquivalentTo(modelDeserializedObj);
            }

            TestAndCompareSerializeAndDeserialize(Temp1DayBlob.Create(new System.Uri($"https://{ValueGenerator.String()}.com/{ValueGenerator.String()}?xx={ValueGenerator.String()}")));
            TestAndCompareSerializeAndDeserialize(Temp3DayBlob.Create(new System.Uri($"https://{ValueGenerator.String()}.com/{ValueGenerator.String()}?xx={ValueGenerator.String()}")));
            TestAndCompareSerializeAndDeserialize(Temp7DayBlob.Create(new System.Uri($"https://{ValueGenerator.String()}.com/{ValueGenerator.String()}?xx={ValueGenerator.String()}")));

            TestAndCompareSerializeAndDeserialize(new Success());
            TestAndCompareSerializeAndDeserialize(new Success(ValueGenerator.String()));
            TestAndCompareSerializeAndDeserialize(new Success(ValueGenerator.String(), ValueGenerator.String()));

            TestAndCompareSerializeAndDeserialize(new Error());
            TestAndCompareSerializeAndDeserialize(new Error(ValueGenerator.String()));
            TestAndCompareSerializeAndDeserialize(new Error(ValueGenerator.String(), ValueGenerator.String()));

        }
    }
}
