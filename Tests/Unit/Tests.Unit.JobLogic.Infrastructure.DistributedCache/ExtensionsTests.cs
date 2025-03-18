using FluentAssertions;
using JobLogic.Infrastructure.DistributedCache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.DistributedCache
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod("GetObjectAsync | Should OK | When T Is Boolean and cached value is NULL")]
        public async Task TestGetObjectAsync1()
        {
            var mockCache = new Mock<IDistributedCacheClient>();
            mockCache.Setup(x => x.GetValueAsync(It.IsAny<string>())).Returns(Task.FromResult<string>(null));
            var cache = mockCache.Object;
            var rs = await cache.GetObjectAsync<bool>(It.IsAny<string>());
            rs.State.Should().Be(FindCacheResultState.NotFound);
        }

        [DataRow("false")]
        [DataRow("0")]
        [TestMethod("GetObjectAsync | Should OK | When T Is Boolean and cached value equivalent to false")]
        public async Task TestGetObjectAsync2(string cachedVal)
        {
            var mockCache = new Mock<IDistributedCacheClient>();
            mockCache.Setup(x => x.GetValueAsync(It.IsAny<string>())).Returns(Task.FromResult<string>(cachedVal));
            var cache = mockCache.Object;
            var rs = await cache.GetObjectAsync<bool>(It.IsAny<string>());
            rs.State.Should().Be(FindCacheResultState.Found);
            rs.GetValueOnlyWhenStateIsFound().Should().Be(false);
        }

        [DataRow("true")]
        [DataRow("1")]
        [DataRow("18")]
        [TestMethod("GetObjectAsync | Should OK | When T Is Boolean and cached value equivalent to true")]
        public async Task TestGetObjectAsync2a(string cachedVal)
        {
            var mockCache = new Mock<IDistributedCacheClient>();
            mockCache.Setup(x => x.GetValueAsync(It.IsAny<string>())).Returns(Task.FromResult<string>(cachedVal));
            var cache = mockCache.Object;
            var rs = await cache.GetObjectAsync<bool>(It.IsAny<string>());
            rs.State.Should().Be(FindCacheResultState.Found);
            rs.GetValueOnlyWhenStateIsFound().Should().Be(true);
        }

        [DataRow("False")]
        [DataRow("z10")]
        [DataRow("True")]
        [ExpectedException(typeof(Exception), AllowDerivedTypes =true)]
        [TestMethod("GetObjectAsync | Should throw exception | When T Is Boolean and cached value is Invalue")]
        public async Task TestGetObjectAsync3(string cachedVal)
        {
            var mockCache = new Mock<IDistributedCacheClient>();
            mockCache.Setup(x => x.GetValueAsync(It.IsAny<string>())).Returns(Task.FromResult<string>(cachedVal));
            var cache = mockCache.Object;
            var rs = await cache.GetObjectAsync<bool>(It.IsAny<string>());
        }


        [TestMethod("GetObjectAsync | Should OK | When T Is Int and cached value is NULL")]
        public async Task TestGetObjectAsync_Int()
        {
            var mockCache = new Mock<IDistributedCacheClient>();
            mockCache.Setup(x => x.GetValueAsync(It.IsAny<string>())).Returns(Task.FromResult<string>(null));
            var cache = mockCache.Object;
            var rs = await cache.GetObjectAsync<int>(It.IsAny<string>());
            rs.State.Should().Be(FindCacheResultState.NotFound);
        }

        [DataRow("1")]
        [DataRow("18")]
        [TestMethod("GetObjectAsync | Should OK | When T Is Int and cached value is valid int")]
        public async Task TestGetObjectAsync_Int2(string cachedVal)
        {
            var mockCache = new Mock<IDistributedCacheClient>();
            mockCache.Setup(x => x.GetValueAsync(It.IsAny<string>())).Returns(Task.FromResult<string>(cachedVal));
            var cache = mockCache.Object;
            var rs = await cache.GetObjectAsync<int>(It.IsAny<string>());
            rs.State.Should().Be(FindCacheResultState.Found);
            rs.GetValueOnlyWhenStateIsFound().Should().Be(int.Parse(cachedVal));
        }


        [DataRow(null)]
        [TestMethod("GetObjectAsync | Should OK | When T Is string and cached value is null")]
        public async Task TestGetObjectAsync_String(string cachedVal)
        {
            var mockCache = new Mock<IDistributedCacheClient>();
            mockCache.Setup(x => x.GetValueAsync(It.IsAny<string>())).Returns(Task.FromResult<string>(cachedVal));
            var cache = mockCache.Object;
            var rs = await cache.GetObjectAsync<string>(It.IsAny<string>());
            rs.State.Should().Be(FindCacheResultState.NotFound);
        }

        [DataRow("1")]
        [DataRow("{'a': '1'}")]
        [DataRow("18")]
        [TestMethod("GetObjectAsync | Should OK | When T Is string and cached value is valid string")]
        public async Task TestGetObjectAsync_String2(string cachedVal)
        {
            var mockCache = new Mock<IDistributedCacheClient>();
            mockCache.Setup(x => x.GetValueAsync(It.IsAny<string>())).Returns(Task.FromResult<string>(cachedVal));
            var cache = mockCache.Object;
            var rs = await cache.GetObjectAsync<string>(It.IsAny<string>());
            rs.State.Should().Be(FindCacheResultState.Found);
            rs.GetValueOnlyWhenStateIsFound().Should().Be(cachedVal);
        }
    }
}
