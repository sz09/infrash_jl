using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JobLogic.Infrastructure.EmailClient
{
    public sealed class TemplatedEmail : BaseEmailInfo
    {
        public string TemplateId { get; set; }
        public IReadOnlyDictionary<string, object> TemplateData { get; set; }
    }
}
