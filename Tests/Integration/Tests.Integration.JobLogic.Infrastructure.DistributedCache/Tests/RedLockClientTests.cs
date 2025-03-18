using FluentAssertions;
using JobLogic.Infrastructure.DistributedCache;
using RedLockNet;

namespace Tests.Integration.JobLogic.Infrastructure.DistributedCache
{
    [TestClass]
    public class RedLockClientTests
    {
        private IDistributedLockFactory _distributedLockFactory;

        [TestInitialize]
        public void RedLockTestInitialize()
        {

            _distributedLockFactory = new RedLockClient(AppConfig.CacheConnection);
        }

        [TestMethod]
        public void TestCreateLock_ShouldWork_WhenLockWaitConcurentHappen()
        {
            int numberOfRequest = 4;
            var lockAcquire = 0;
            var resource = $"INTEGRATION_TEST_REDLOCK_KEY_{Guid.NewGuid()}";
            var taskList = new List<Task>();
            var timeToSleep = 4;
            var timeWait = timeToSleep * numberOfRequest;

            for (var i = 0; i < numberOfRequest; i++)
            {
                var task = Task.Run(() =>
                {

                    using (var redLock = _distributedLockFactory.CreateLock(
                            resource,
                            TimeSpan.FromSeconds(30),
                            TimeSpan.FromSeconds(timeWait),
                            TimeSpan.FromSeconds(3)))
                    {
                        if (redLock.IsAcquired)
                        {
                            Thread.Sleep(timeToSleep * 1000); // 10 seconds
                            Interlocked.Increment(ref lockAcquire);
                        }

                    }

                });
                taskList.Add(task);
            }

            var from = DateTime.UtcNow;
            Task.WaitAll(taskList.ToArray());
            var to = DateTime.UtcNow;
            var timeSpend = (to - from).TotalSeconds;

            Assert.IsTrue(lockAcquire == numberOfRequest && timeSpend >= (numberOfRequest * timeToSleep));
        }

        [TestMethod]
        public void TestCreateLock_ShouldWork_WhenLockSkipConcurentHappen()
        {
            int numberOfRequest = 4;
            var lockAcquire = 0;
            int lockNotAcquire = 0;
            var resource = $"INTEGRATION_TEST_REDLOCK_KEY_{Guid.NewGuid()}";
            var taskList = new List<Task>();
            var timeToSleep = 4;
            var timeWait = timeToSleep * numberOfRequest;

            for (var i = 0; i < numberOfRequest; i++)
            {
                var task = Task.Run(() =>
                {

                    using (var redLock = _distributedLockFactory.CreateLock(
                            resource,
                            TimeSpan.FromSeconds(30)))
                    {
                        if (redLock.IsAcquired)
                        {
                            Thread.Sleep(timeToSleep * 1000); // 10 seconds
                            Interlocked.Increment(ref lockAcquire);
                        }
                        else
                        {
                            Interlocked.Increment(ref lockNotAcquire);
                        }

                    }

                });
                taskList.Add(task);
            }

            var from = DateTime.UtcNow;
            Task.WaitAll(taskList.ToArray());
            var to = DateTime.UtcNow;
            var timeSpend = (to - from).TotalSeconds;

            Assert.IsTrue(lockAcquire == 1);
            timeSpend.Should().BeLessThan(2 * timeToSleep);
            lockNotAcquire.Should().Be(3);
        }

        [TestMethod]
        public async Task TestCreateLockAsync_ShouldWork_WhenLockWaitConcurentHappen()
        {
            int numberOfRequest = 4;
            var lockAcquire = 0;
            var resource = $"INTEGRATION_TEST_REDLOCK_KEY_{Guid.NewGuid()}";
            var taskList = new List<Task>();
            var timeToSleep = 4;
            var timeWait = timeToSleep * numberOfRequest;

            for (var i = 0; i < numberOfRequest; i++)
            {
                var task = Task.Run(async () =>
                {

                    using (var redLock = await _distributedLockFactory.CreateLockAsync(
                            resource,
                            TimeSpan.FromSeconds(30),
                            TimeSpan.FromSeconds(timeWait),
                            TimeSpan.FromSeconds(3)))
                    {
                        if (redLock.IsAcquired)
                        {
                            Thread.Sleep(timeToSleep * 1000); // 10 seconds
                            Interlocked.Increment(ref lockAcquire);
                        }

                    }

                });
                taskList.Add(task);
            }

            var from = DateTime.UtcNow;
            Task.WaitAll(taskList.ToArray());
            var to = DateTime.UtcNow;
            var timeSpend = (to - from).TotalSeconds;

            Assert.IsTrue(lockAcquire == numberOfRequest && timeSpend >= (numberOfRequest * timeToSleep));
        }

        [TestMethod]
        public async Task TestCreateLockAsync_ShouldWork_WhenLockSkipConcurentHappen()
        {
            int numberOfRequest = 4;
            var lockAcquire = 0;
            int lockNotAcquire = 0;
            var resource = $"INTEGRATION_TEST_REDLOCK_KEY_{Guid.NewGuid()}";
            var taskList = new List<Task>();
            var timeToSleep = 4;
            var timeWait = timeToSleep * numberOfRequest;

            for (var i = 0; i < numberOfRequest; i++)
            {
                var task = Task.Run(async () =>
                {

                    using (var redLock = await _distributedLockFactory.CreateLockAsync(
                            resource,
                            TimeSpan.FromSeconds(30)))
                    {
                        if (redLock.IsAcquired)
                        {
                            Thread.Sleep(timeToSleep * 1000); // 10 seconds
                            Interlocked.Increment(ref lockAcquire);
                        }
                        else
                        {
                            Interlocked.Increment(ref lockNotAcquire);
                        }

                    }

                });
                taskList.Add(task);
            }

            var from = DateTime.UtcNow;
            Task.WaitAll(taskList.ToArray());
            var to = DateTime.UtcNow;
            var timeSpend = (to - from).TotalSeconds;

            Assert.IsTrue(lockAcquire == 1);
            timeSpend.Should().BeLessThan(2 * timeToSleep);
            lockNotAcquire.Should().Be(3);
        }
    }
}