using FluentAssertions;
using JobLogic.Infrastructure.DistributedCache;

namespace Tests.Integration.JobLogic.Infrastructure.DistributedCache.Tests
{
    [TestClass]
    public class RedisCacheClientTest
    {
        private IDistributedCacheClient _distributedCacheClient;
        private int _maxConcurrentNumber = 100;

        [TestInitialize]
        public void RedLockTestInitialize()
        {
            _distributedCacheClient = new RedisCacheClient(AppConfig.CacheConnection);
        }
        #region Test Increment


        [TestMethod]
        public async Task TestIncrement_ConcurrentThreads_FinalValue_ShouldBeCorrect()
        {
            var testKey = "RedisCacheClientIntegrationTest_Increment";
            var originalVal = "0";

            var setOriginalValueResult = await _distributedCacheClient.SetValueAsync(testKey, originalVal);
            Assert.IsTrue(setOriginalValueResult);

            var getOriginalValueFromCache= await _distributedCacheClient.GetValueAsync(testKey);
            Assert.AreEqual(originalVal, getOriginalValueFromCache);

            var concurrentOperationList = Enumerable.Range(0, _maxConcurrentNumber);
            await ConcurrentExecForEachAsync(concurrentOperationList, x => _distributedCacheClient.IncreaseValueAsync(testKey));

            var finalValue = await _distributedCacheClient.GetValueAsync(testKey);
            Assert.AreEqual(_maxConcurrentNumber.ToString(), finalValue);
        }


        [TestMethod]
        public async Task TestIncrement_ConcurrentThreads_ValueEachOperationShouldBeDifferent_AndFinalValueShouldBeCorrect()
        {
            var testKey = "RedisCacheClientIntegrationTest_Increment";
            var originalVal = "0";

            var setOriginalValueResult = await _distributedCacheClient.SetValueAsync(testKey, originalVal);
            Assert.IsTrue(setOriginalValueResult);

            var getOriginalValueFromCache = await _distributedCacheClient.GetValueAsync(testKey);
            Assert.AreEqual(originalVal, getOriginalValueFromCache);

            var concurrentOperationList = Enumerable.Range(0, _maxConcurrentNumber);
            var result = await ConcurrentExecForEachAsync(concurrentOperationList, x => _distributedCacheClient.IncreaseValueAsync(testKey));

            Assert.IsTrue(result.GroupBy(x => x).All(x => x.Count() == 1));

            var finalValue = await _distributedCacheClient.GetValueAsync(testKey);
            Assert.AreEqual(_maxConcurrentNumber.ToString(), finalValue);
        }
        #endregion

        #region Test Decrement
        [TestMethod]
        public async Task TestDecrement_ConcurrentThreads_FinalValue_ShouldBeCorrect()
        {
            var testKey = "RedisCacheClientIntegrationTest_Decrement";
            var originalVal = _maxConcurrentNumber.ToString();

            var setOriginalValueResult = await _distributedCacheClient.SetValueAsync(testKey, originalVal);
            Assert.IsTrue(setOriginalValueResult);

            var getOriginalValueFromCache = await _distributedCacheClient.GetValueAsync(testKey);
            Assert.AreEqual(originalVal, getOriginalValueFromCache);

            var concurrentOperationList = Enumerable.Range(0, _maxConcurrentNumber);
            await ConcurrentExecForEachAsync(concurrentOperationList, x => _distributedCacheClient.DecreaseValueAsync(testKey));

            var finalValue = await _distributedCacheClient.GetValueAsync(testKey);
            Assert.AreEqual("0", finalValue);
        }


        [TestMethod]
        public async Task TestDecrement_ConcurrentThreads_ValueEachOperation_ShouldBeDifferent()
        {
            var testKey = "RedisCacheClientIntegrationTest_Decrement";
            var originalVal = _maxConcurrentNumber.ToString();

            var setOriginalValueResult = await _distributedCacheClient.SetValueAsync(testKey, originalVal);
            Assert.IsTrue(setOriginalValueResult);

            var getOriginalValueFromCache = await _distributedCacheClient.GetValueAsync(testKey);
            Assert.AreEqual(originalVal, getOriginalValueFromCache);

            var concurrentOperationList = Enumerable.Range(0, _maxConcurrentNumber);
            var result = await ConcurrentExecForEachAsync(concurrentOperationList, x => _distributedCacheClient.DecreaseValueAsync(testKey));

            Assert.IsTrue(result.GroupBy(x => x).All(x => x.Count() == 1));

            var finalValue = await _distributedCacheClient.GetValueAsync(testKey);
            Assert.AreEqual("0", finalValue);
        }

        #endregion

        private async Task<IEnumerable<R>> ConcurrentExecForEachAsync<T, R>(IEnumerable<T> inputList, Func<T, Task<R>> taskExecFunc, int maxConcurrent = 10)
        {
            SemaphoreSlim throttler = new SemaphoreSlim(maxConcurrent);
            List<Task<R>> allTasks = new List<Task<R>>();
            foreach (T i in inputList)
            {
                await throttler.WaitAsync();
                Task<R> item = Task.Run(async delegate
                {
                    try
                    {
                        return await taskExecFunc(i);
                    }
                    finally
                    {
                        throttler.Release();
                    }
                });
                allTasks.Add(item);
            }

            return await Task.WhenAll(allTasks);
        }

        [TestMethod]
        public async Task TestTryMarkIdempotentOperationAsHandledAsync_ShouldWork()
        {
            var idempotentId = Guid.NewGuid();
            string handlerName = "testHandler";

            var result = await _distributedCacheClient.TryMarkIdempotentOperationAsHandledAsync(handlerName, idempotentId);
            Assert.IsTrue(result);

            result = await _distributedCacheClient.TryMarkIdempotentOperationAsHandledAsync(handlerName, idempotentId);
            Assert.IsFalse(result);
        }
        [TestMethod]
        public async Task TestTryMarkIdempotentOperationAsHandledAsync_ShouldNotWork_WhenGuidEmpty()
        {
            var idempotentId = Guid.Empty;
            string handlerName = "testHandler";

            Func<Task> act = async () => { var result = await _distributedCacheClient.TryMarkIdempotentOperationAsHandledAsync(handlerName, idempotentId); };
            await act.Should().ThrowAsync<Exception>();
        }
        [TestMethod]
        public async Task TestTryMarkIdempotentOperationAsHandledAsync_ShouldNotWork_WhenHandlerNameEmpty()
        {
            var idempotentId = Guid.NewGuid();
            string handlerName = "";

            Func<Task> act = async () => { var result = await _distributedCacheClient.TryMarkIdempotentOperationAsHandledAsync(handlerName, idempotentId); };
            await act.Should().ThrowAsync<Exception>();
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task TestTryMarkIdempotentOperationAsHandledAsync_ShouldNotWork_WhenIdempotentIdInvalid(string idempotentId)
        {
            string handlerName = "testHandler";

            Func<Task> act = async () => { var result = await _distributedCacheClient.TryMarkIdempotentOperationAsHandledAsync(handlerName, idempotentId); };
            await act.Should().ThrowAsync<Exception>();
        }
    }
}
