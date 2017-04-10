using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class EventRecall
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("Event")]
        public Event Event { get; set; }
        [ForeignKey("Drug")]
        public Drug Drug { get; set; }

        public EventRecall()
        {

        }

        public EventRecall(Drug drug, Event Event)
        {
            Drug = drug;
            this.Event = Event;
        }

    }
}
