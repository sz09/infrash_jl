using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.SignalR
{
    public class NotificationHub : Hub
    {
        public override Task OnConnected()
        {
            //each user can have multiple connections: multiple devices, tabs => create a group for each user            
            AddToGroup("userId");
            AddToGroup("tenantId");
            return base.OnConnected();
        }

        private void AddToGroup(string key)
        {
            var value = Context.QueryString[key];
            if (!string.IsNullOrEmpty(value))
            {
                Groups.Add(Context.ConnectionId, value.ToLower());
            }
        }
    }
}
