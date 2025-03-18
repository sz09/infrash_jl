using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class ExceptionUtilsTests : BaseUnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryInvoke_WhenOperationIsNull_ShouldThrow_ArgumentNullException()
        {
            // Arrange
            Action operation = null;

            // Action
            operation.TryInvoke();

            // Assert
            // Used ExpectedException attribute
        }

        [TestMethod]
        public void TryInvoke_WhenExceptionIsThrown_ShouldReturn_False()
        {
            // Arrange
            Action operation = () => throw new NullReferenceException();

            // Action
            var result = operation.TryInvoke();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void TryInvoke_WhenNoExceptionIsThrown_ShouldReturn_True()
        {
            // Arrange
            Action operation = () => Console.WriteLine("Doing something");

            // Action
            var result = operation.TryInvoke();

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryInvoke_T_WhenOperationIsNull_ShouldThrow_ArgumentNullException()
        {
            // Arrange
            Func<Guid> operation = null;

            // Action
            operation.TryInvoke(out _);

            // Assert
            // Used ExpectedException attribute
        }

        [TestMethod]
        public void TryInvoke_T_WhenExceptionIsThrown_ShouldReturn_False()
        {
            // Arrange
            Func<Guid> operation = () => throw new NullReferenceException();

            // Action
            var result = operation.TryInvoke(out _);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void TryInvoke_T_WhenNoExceptionIsThrown_ShouldReturn_True()
        {
            // Arrange
            Func<Guid> operation = () => Guid.Empty;

            // Action
            var result = operation.TryInvoke(out _);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public async Task TryInvoke_ForAyncAction_WhenNoExceptionIsThrown_ShouldReturn_True()
        {
            var x = 0;
            // Arrange
            Action operation =async () =>
            {
                await Task.Delay(30);
                Console.WriteLine("Doing something");
                System.Diagnostics.Debug.WriteLine("Another Thing");
                x = 1;
            };


            // Action
            var result = operation.TryInvoke();
            await Task.Delay(1000);
            // Assert
            //result.Should().BeTrue();
            Assert.AreEqual(1,x);
            result.Should().BeTrue();
        }
    }
}
