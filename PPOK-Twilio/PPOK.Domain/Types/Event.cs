using System.Collections.Generic;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class Event
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        public string Message { get; set; }
        public EventStatus Status { get; set; }
        [ForeignMultiKey("EventHistory")]
        public SubQuery<EventHistory> History { get; set; }

        [ForeignMultiKey("EventBirthday")]
        public SubQuery<EventBirthday> Birthdays { get; set; }

        [ForeignMultiKey("EventRecall")]
        public SubQuery<EventRecall> Recalls { get; set; }

        [ForeignMultiKey("EventRefill")]
        public SubQuery<EventRefill> Refills { get; set; }

        public Event()
        {

        }

        //public Event(Prescription prescription)
        //{
        //    //Prescription = prescription;
        //}

        public Event(string message, EventStatus status)
        {
            //Prescription = prescription;
            Message = message;
            Status = status;
        }

        public override string ToString()
        {
            return $"[{Code}, {Message}]";
        }
    }
}
