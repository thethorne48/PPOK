using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class EventRecall
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("Prescription")]
        public Patient Patient { get; set; }
        [ForeignKey("Drug")]
        public Drug Drug { get; set; }
        [ForeignKey("Event")]
        public Event Event { get; set; }

        public EventRecall()
        {

        }

        public EventRecall(Patient patient, Drug drug, Event Event)
        {
            Patient = patient;
            Drug = drug;
            this.Event = Event;
        }

    }
}
