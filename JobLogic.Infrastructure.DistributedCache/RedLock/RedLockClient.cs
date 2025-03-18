using RedLockNet;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.DistributedCache
{
    public class RedLockClient : IDistributedLockFactory
    {
        private readonly string _lockConnectionString;
        public RedLockClient(string lockConnectionString)
        {
            _lockConnectionString = lockConnectionString;
        }

        public async Task<IRedLock> CreateLockAsync(string resource, TimeSpan expiryTime)
        {
            var factory = await RedLockFactoryUtils.GetRedLockFactoryAsync(_lockConnectionString);
            return await factory.CreateLockAsync(resource, expiryTime);
        }

        public async Task<IRedLock> CreateLockAsync(string resource, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null)
        {
            var factory = await RedLockFactoryUtils.GetRedLockFactoryAsync(_lockConnectionString);
            return await factory.CreateLockAsync(resource, expiryTime, waitTime, retryTime, cancellationToken);
        }

        public IRedLock CreateLock(string resource, TimeSpan expiryTime)
        {
            var factory = RedLockFactoryUtils.GetRedLockFactory(_lockConnectionString);
            return factory.CreateLock(resource, expiryTime);
        }

        public IRedLock CreateLock(string resource, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null)
        {
            var factory = RedLockFactoryUtils.GetRedLockFactory(_lockConnectionString);
            return factory.CreateLock(resource, expiryTime, waitTime, retryTime, cancellationToken);
        }
    }
}