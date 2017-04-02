using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class EventRefill
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("Prescription")]
        public Prescription Prescription { get; set; }
        [ForeignKey("Event")]
        public Event Event { get; set; }
        [ForeignMultiKey("FillHistory")]
        public SubQuery<FillHistory> History { get; set; }

        public EventRefill()
        {

        }

        public EventRefill(Prescription prescription)
        {
            Prescription = prescription;
        }

        public EventRefill(Prescription prescription, Event Event)
        {
            Prescription = prescription;
            this.Event = Event;
        }

    }
}
