using System.Collections.Generic;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class Event
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        public string Message { get; set; }
        [ForeignMultiKey("EventHistory")]
        public IEnumerable<EventHistory> History { get; set; }

        public Event()
        {

        }

        //public Event(Prescription prescription)
        //{
        //    //Prescription = prescription;
        //}

        public Event(string message)
        {
            //Prescription = prescription;
            Message = message;
        }

        public override string ToString()
        {
            return $"[{Code}, {Message}]";
        }
    }
}
