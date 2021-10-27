using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;


namespace SignalRMongoChat.Model
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string group { get; set; }
        public string to { set; get; }
        public string from { set; get; }
        public string message { set; get; }
        public string time { set; get; }

        public string path { set; get; }
        public string size { set; get; }
        public string type { set; get; }
        public string filename { set; get; }
        public bool isFile { set; get; }


        public bool isDeleted { set; get; }
        public DateTime createdAt { set; get; }
        public DateTime updatedAt { set; get; }
    }
}
