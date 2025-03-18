using Microsoft.VisualStudio.TestTools.UnitTesting;
using JobLogic.Infrastructure.Utilities;
using JobLogic.Infrastructure.UnitTest;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    public class TempClass
    {
        public string FirstName { get; set; }
    }

    [TestClass]
    public class PropertyUtilsTest: BaseUnitTest
    {
        [TestMethod]
        public void TestGetName_ReturnCorrectly_IfPropertyInVariableIsProvided()
        {
            // Arrange
            var tempClass = new TempClass();

            // Action
            var name = PropertyUtils.Instance.GetName(() => tempClass.FirstName);

            // Assert
            Assert.AreEqual("FirstName", name);
        }

        [TestMethod]
        public void TestGetName_ReturnCorrectly_IfVariableIsProvided()
        {
            // Arrange
            TempClass tempClass = null;

            // Action
            var name = PropertyUtils.Instance.GetName(() => tempClass);

            // Assert
            Assert.AreEqual("tempClass", name);
        }
    }
}
