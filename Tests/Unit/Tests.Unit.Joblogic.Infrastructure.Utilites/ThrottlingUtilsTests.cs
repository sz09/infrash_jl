using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class ThrottlingUtilsTests : BaseUnitTest
    {
        [TestMethod]
        public async Task TestConcurrentExecForEachAsyncWithResult_ShouldTakeLessThan2Seconds_WhenMaxConcurrentIs10AndThereOnly5Items()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            async Task<string> DoSomeMockTaskWithResult(int i)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                return i.ToString();
            }
            var items = new int[] { 1, 2, 3, 4, 5 };
            var result = await items.ConcurrentExecForEachAsync(x => DoSomeMockTaskWithResult(x));
            sw.Elapsed.Should().BeCloseTo(TimeSpan.FromSeconds(1), 100);

            result.Should().HaveCount(items.Length);
        }

        [TestMethod]
        public async Task TestConcurrentExecForEachAsyncWithResult_ShouldTakeMoreThan2Seconds_WhenMaxConcurrentIs4AndThereOnly5Items()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            async Task<string> DoSomeMockTaskWithResult(int i)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                return i.ToString();
            }
            var items = new int[] { 1, 2, 3, 4, 5 };
            var result = await items.ConcurrentExecForEachAsync(x => DoSomeMockTaskWithResult(x), 4);
            sw.Elapsed.Should().BeCloseTo(TimeSpan.FromSeconds(2),100);

            result.Should().HaveCount(items.Length);
        }

        [TestMethod]
        public async Task TestConcurrentExecForEachAsyncWithResult_ShouldTakeMoreThan3Seconds_WhenMaxConcurrentIs2AndThereOnly5Items()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            async Task<string> DoSomeMockTaskWithResult(int i)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                return i.ToString();
            }
            var items = new int[] { 1, 2, 3, 4, 5 };
            var result = await items.ConcurrentExecForEachAsync(x => DoSomeMockTaskWithResult(x), 2);
            sw.Elapsed.Should().BeCloseTo(TimeSpan.FromSeconds(3), 100);

            result.Should().HaveCount(items.Length);
        }

        [TestMethod]
        public async Task TestConcurrentExecForEachAsync_ShouldTakeLessThan2Seconds_WhenMaxConcurrentIs10AndThereOnly5Items()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            async Task DoSomeMockTaskWithResult(int i)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            var items = new int[] { 1, 2, 3, 4, 5 };
            await items.ConcurrentExecForEachAsync(x => DoSomeMockTaskWithResult(x));
            sw.Elapsed.Should().BeCloseTo(TimeSpan.FromSeconds(1), 100);

        }

        [TestMethod]
        public async Task TestConcurrentExecForEachAsync_ShouldTakeMoreThan2Seconds_WhenMaxConcurrentIs4AndThereOnly5Items()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            async Task DoSomeMockTaskWithResult(int i)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            var items = new int[] { 1, 2, 3, 4, 5 };
            await items.ConcurrentExecForEachAsync(x => DoSomeMockTaskWithResult(x), 4);
            sw.Elapsed.Should().BeCloseTo(TimeSpan.FromSeconds(2), 100);

        }

        [TestMethod]
        public async Task TestConcurrentExecForEachAsync_ShouldTakeMoreThan3Seconds_WhenMaxConcurrentIs2AndThereOnly5Items()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            async Task DoSomeMockTaskWithResult(int i)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            var items = new int[] { 1, 2, 3, 4, 5 };
            await items.ConcurrentExecForEachAsync(x => DoSomeMockTaskWithResult(x), 2);
            sw.Elapsed.Should().BeCloseTo(TimeSpan.FromSeconds(3), 100);

        }
    }
}
