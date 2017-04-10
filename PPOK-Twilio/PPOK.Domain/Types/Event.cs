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
        public EventType Type { get; set; }
        [ForeignKey("Patient")]
        public Patient Patient { get; set; }
        [ForeignMultiKey("EventHistory")]
        public SubQuery<EventHistory> History { get; set; }

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

        public Event(Patient patient, string message, EventStatus status, EventType type)
        {
            Patient = patient;
            Message = message;
            Status = status;
            Type = type;
        }

        public override string ToString()
        {
            return $"[{Code}, {Message}]";
        }
    }
}
