using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalRCallingSolution.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCallingSolution.Controllers
{
    [EnableCors("AllowOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class WaitingRoomController : ControllerBase
    {
        IWaitingRoomService waitingRoom;

        public WaitingRoomController(IWaitingRoomService waitingRoom)
        {
            this.waitingRoom = waitingRoom;
        }
        [HttpGet]
        //[Route("api/Calling/DecryptUrl")]
        public async Task<IActionResult> decryptUrl(string url)
        {

            if (url != "")
            {

                var Result = waitingRoom.urlDecrypt(url);
                if (Result != null)
                {
                    return Ok(Result);
                }
                else
                    return NotFound();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
