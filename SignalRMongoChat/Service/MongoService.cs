
using MongoDB.Driver;
using SignalRMongoChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRMongoChat.Service
{
    public class MongoService
    {
        private readonly IMongoCollection<Message> _messages;

        public MongoService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _messages = database.GetCollection<Message>(settings.CollectionName);
        }
        public List<Message> GetGroupMessages(string group)
        {
            return _messages.Find(message => message.group == group).ToList();
        }
        public Message InsertMessages(Message message)
        {
            message.Id = null;
            message.createdAt = DateTime.UtcNow;
            message.updatedAt = DateTime.UtcNow;
            _messages.InsertOne(message);
            return message;
        } 
        public void UpdateMessage(Message msg)
        {
            msg.updatedAt = DateTime.UtcNow;
            _messages.ReplaceOne(message => message.Id == msg.Id, msg);       
        }
        
        public List<Message> Get() =>
            _messages.Find(message => true).ToList();

    }
}
