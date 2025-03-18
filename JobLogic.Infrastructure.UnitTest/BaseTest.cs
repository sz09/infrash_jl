using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace JobLogic.Infrastructure.UnitTest
{
    public abstract class BaseTest
    {
        protected Random random;

        [TestInitialize]
        public void BaseTestInitialize()
        {
            random = new Random();
        }

        protected string GenerateUniqueString()
        {
            return Guid.NewGuid().ToString();
        }

        protected Guid GenerateUniqueId()
        {
            return Guid.NewGuid();
        }

        protected DateTime GenerateDateTime()
        {
            return DateTime.Now;
        }

        protected bool GenerateBoolean()
        {
            return random.Next(100) % 2 == 0;
        }

        protected string GenerateEmail()
        {
            return string.Format("{0}@gmail.com", GenerateUniqueString());
        }

        protected int GenerateNumber(int minValue = 1, int maxValue = Int32.MaxValue)
        {
            return random.Next(1 * minValue, maxValue);
        }
    }
}
