﻿using System.Collections;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.Event.Publisher
{
    public class EventServiceBusOption
    {
        public EventServiceBusOption()
        {
            UserProperties = new Dictionary<string, object>();
        }
        public int? DelayMessageInMinutes { get; set; }
        public IDictionary<string,object> UserProperties { get; }
    }
}
