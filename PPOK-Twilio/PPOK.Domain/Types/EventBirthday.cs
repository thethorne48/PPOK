using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class EventBirthday
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("Patient")]
        public Patient Patient { get; set; }
        [ForeignKey("Event")]
        public Event Event { get; set; }

        public EventBirthday()
        {

        }

        public EventBirthday(Patient patient, Event Event)
        {
            Patient = patient;
            this.Event = Event;
        }

    }
}
