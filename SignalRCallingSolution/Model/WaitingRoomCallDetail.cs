using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCallingSolution.Model
{
    public class WaitingRoomCallDetail
    {
        public string patientAccount { set; get; }
        public string patientName { set; get; }
        public string providerName { set; get; }
        public string providerTitle { set; get; }
        public string appointmentID { set; get; }
        public string appointmentDateTime { set; get; }
        public string practiceCode { set; get; }
        public string providerCode { set; get; }
        public string perspective { set; get; }
        public string participantName { set; get; }
        public bool isCareClinic { set; get; }
    }
}
