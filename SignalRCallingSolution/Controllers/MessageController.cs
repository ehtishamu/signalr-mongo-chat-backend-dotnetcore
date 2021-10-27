using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SignalRMongoChat.Service;
using SignalRMongoChat.Model;

namespace SignalRCallingSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MongoService _messageService;

        public MessageController(MongoService MessageService)
        {
            _messageService = MessageService;
        }
        [HttpGet]
        public ActionResult<List<Message>> GetGroupMessages(string group) =>
           _messageService.GetGroupMessages(group);

        [HttpPost]
        public IActionResult InsertMessage(Message Message)
        {
            
            _messageService.InsertMessages(Message);

          return Ok("save");
        }



    }
}
