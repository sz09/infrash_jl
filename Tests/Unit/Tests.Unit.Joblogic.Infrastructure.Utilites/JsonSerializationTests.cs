using System;
using System.Runtime.InteropServices.ComTypes;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class JsonSerializationTests
    {
        [TestMethod]
        public void TestIgnoreJsonProperties()
        {
            var person = new Person()
            {
                FirstName = "Chris",
                LastName = "Phan"
            };

            var serializedString = JsonSerialization.Serialize(person, true);
            Assert.IsTrue(serializedString.Contains("FirstName"));
            Assert.IsTrue(serializedString.Contains("LastName"));

            var personAfter = JsonSerialization.Deserialize<Person>(serializedString, true);

            Assert.AreEqual(person.FirstName, personAfter.FirstName);
            Assert.AreEqual(person.LastName, personAfter.LastName);
        }

        [TestMethod]
        public void TestKeepJsonProperties()
        {
            var person = new Person()
            {
                FirstName = "Chris",
                LastName = "Phan"
            };

            var serializedString = JsonSerialization.Serialize(person);
            Assert.IsTrue(serializedString.Contains("first-name"));
            Assert.IsTrue(serializedString.Contains("last-name"));

            var personAfter = JsonSerialization.Deserialize<Person>(serializedString);
            var wrongPerson = JsonSerialization.Deserialize<Person>(serializedString, true);

            Assert.AreEqual(person.FirstName, personAfter.FirstName);
            Assert.AreEqual(person.LastName, personAfter.LastName);

            Assert.IsNull(wrongPerson.FirstName);
            Assert.IsNull(wrongPerson.LastName);
        }

    }

    public class Person
    {
        [JsonProperty("first-name")]
        public string FirstName { get; set; }
        
        [JsonProperty("last-name")]
        public string LastName { get; set; }
    }
}
