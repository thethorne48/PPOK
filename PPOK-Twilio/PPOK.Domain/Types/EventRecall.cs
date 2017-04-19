using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class EventRecall
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("Event")]
        public Event Event { get; set; }

        public EventRecall()
        {

        }

        public EventRecall(Event Event)
        {
            this.Event = Event;
        }

    }
}
