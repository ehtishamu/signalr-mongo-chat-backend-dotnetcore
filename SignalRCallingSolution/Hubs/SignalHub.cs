using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SignalRCallingSolution.Hubs
{
    public class SignalHub:Hub
    {
        public static IDictionary<string, Obj> clients = new Dictionary<string, Obj>();
       
        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            var connectionId = Context.ConnectionId;
            Obj obj = clients[connectionId];
            obj.is_online = false;
            await sendToOther(obj);
            await Clients.All.SendAsync("disconnect"+obj.provider_code, obj);
            clients.Remove(connectionId);      
            await base.OnDisconnectedAsync(exception);
        }

        public async Task New(Obj obj)
        {
            clients.Add(Context.ConnectionId, obj);
            await sendToOther(obj);
        }

        public async Task Update(Obj obj)
        {
            var connectionId = Context.ConnectionId;
            var chk=clients.ContainsKey(connectionId);
            if (chk == true)
            {
                clients[connectionId] = obj;
                await sendToOther(obj);
            }
            else {
               await New(obj);
            }
           
        }
       
        private async Task sendToAll(Obj obj)
        {
            await Clients.All.SendAsync(obj.provider_code, JsonSerializer.Serialize(obj));
        }
        private async Task sendToOther(Obj obj)
        {
            await Clients.Others.SendAsync(obj.provider_code, JsonSerializer.Serialize(obj));
        }

    }
    public class Obj {
        public string practiceCode { set; get; }
        public string provider_code { set; get; }
        public string appointment_id { set; get;}
        public string perspective { set; get; }
        public string patientId { set; get; }
        public string type { set; get; }
        public string startTime { set; get; }
        public string endTime { set; get; }
        public bool is_online { set; get; }


    }
}
