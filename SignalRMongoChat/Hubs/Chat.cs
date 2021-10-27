
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRMongoChat.Model;
using SignalRMongoChat.Service;

namespace SignalRMongoChat.Hubs
{

    public class Chat : Hub
    {

        private readonly MongoService _messageService;
        public Chat(MongoService MessageService)
        {
            _messageService = MessageService;

        }

        public Task JoinRoom(string group) {

            string jsonString = JsonSerializer.Serialize(group);
            var messages= _messageService.GetGroupMessages(group);
            Groups.AddToGroupAsync(Context.ConnectionId, group);
            return Clients.Client(Context.ConnectionId).SendAsync("AllMessage", messages);

        }
        public Task SendMessage( Message message)
        {
            _messageService.InsertMessages(message);
            string jsonString = JsonSerializer.Serialize(message);
            return Clients.Group(message.group).SendAsync("OnMessage", message);
        }
        public  Task UpdateMessage(Message message) {
             _messageService.UpdateMessage(message);
            var messages = _messageService.GetGroupMessages(message.group);
            return Clients.Group(message.group).SendAsync("AllMessage", messages);
        }
        public Task Online(string group,string name)
        {
            UserDetail u = new UserDetail();
            u.id = Context.ConnectionId;
            u.name = name;
            return Clients.OthersInGroup(group).SendAsync("Online", u);
        }
        public Task Offline(string group, string name)
        {
            UserDetail u = new UserDetail();
            u.id = Context.ConnectionId;
            u.name = name;
            return Clients.OthersInGroup(group).SendAsync("Offline", u);
        }
        public override Task OnDisconnectedAsync(Exception exception) {
            return Clients.All.SendAsync("Disconnected", Context.ConnectionId);
        }
        public override Task OnConnectedAsync()
        {
            string jsonString = JsonSerializer.Serialize(Context.ConnectionId);
            //InterServiceLog.WriteLog("Room Connected", jsonString);
            return Clients.All.SendAsync("Connected", Context.ConnectionId);
        }






    }

    public class UserDetail { 
        public string id { set; get; }
        public string name { set; get; }
    }
}
