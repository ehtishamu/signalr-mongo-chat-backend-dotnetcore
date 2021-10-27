using SignalRCallingSolution.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCallingSolution.Service
{
    public interface IWaitingRoomService
    {
         WaitingRoomCallDetail urlDecrypt(string url);
    }
}
