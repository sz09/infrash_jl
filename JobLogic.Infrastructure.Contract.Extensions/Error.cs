using System.Collections.Generic;
using System.Linq;

namespace JobLogic.Infrastructure.Contract.Extensions
{
    public class Error
    {
        public Error(params string[] errMsgs)
        {
            Messages =  new List<string>(errMsgs ?? Enumerable.Empty<string>());
        }

        public IEnumerable<string> Messages { get; private set; }

        public string ToStringWithSeparator(string separator = "\r\n") => string.Join(separator, Messages);
    }
}
