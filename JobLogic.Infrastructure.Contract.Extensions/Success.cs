using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobLogic.Infrastructure.Contract.Extensions
{

    public class Success
    {
        public Success(params string[] errMsgs)
        {
            Messages = new List<string>(errMsgs ?? Enumerable.Empty<string>());
        }

        public IEnumerable<string> Messages { get; private set; }

        public string ToStringWithSeparator(string separator = "\r\n") => string.Join(separator, Messages);
    }
}
