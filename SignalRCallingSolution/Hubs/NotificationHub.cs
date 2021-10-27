using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

using System.Threading.Tasks;

namespace SignalRCallingSolution.Hubs
{
    public class NotificationHub : Hub
    {
        [EnableCors("AllowOrigins")]
        public async Task RoomsUpdated(bool flag)
            => await Clients.Others.SendAsync("RoomsUpdated", flag);
    }
}
