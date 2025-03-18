using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class AsyncLazyTests : BaseUnitTest
    {
        [TestMethod("AsyncLazy | Should Call One Times | When Call Multiple Times")]
        public async Task LazyTask_WhenCallMultipleTimes_ShouldCallOneTimes()
        {
            // Arrange
            var lazyTask = new AsyncLazy<Guid>(async () =>
            {
                return Guid.NewGuid();
            });

            // Action
            var date1 = await lazyTask;
            var date2 = await lazyTask;
            var date3 = await lazyTask;

            // Assert
            date1.Should().Be(date2);
            date2.Should().Be(date3);
        }

        [TestMethod("AsyncLazy | Should Same Value | When Call Multiple Times")]
        public async Task LazyTask_WhenCallMultipleTimes_WithException_ShouldCallOneTimes()
        {
            // Arrange
            int i = 0;
            var lazyTask = new AsyncLazy<DateTime>(async () =>
            {
                i++;
                if (i % 2 == 0)
                {
                    throw new Exception("BOOM");
                }
                else
                {
                    throw new Exception("BOOM 1");
                }
            });
            // Action

            var ex1 = await Assert.ThrowsExceptionAsync<Exception>(async () => await lazyTask);
            var ex2 = await Assert.ThrowsExceptionAsync<Exception>(async () => await lazyTask);
            var ex3 = await Assert.ThrowsExceptionAsync<Exception>(async () => await lazyTask);

            // Assert
            i.Should().Be(1);
            ex1.Message.Should().Be("BOOM 1");
            ex2.Message.Should().Be("BOOM 1");
            ex3.Message.Should().Be("BOOM 1");
            
        }
    }
}
