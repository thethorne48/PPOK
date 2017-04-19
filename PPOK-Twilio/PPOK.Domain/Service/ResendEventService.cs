using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    public class ResendEventService : DatabaseService
    {
        public void ResendEvent(int eventid, int pharmCode)
        {
            using(var eventService = new EventService())
            using(var service = new EventRefillService())
            {
                var test = service.GetWhere(EventRefillService.PrescriptionCodeCol == eventid).FirstOrDefault();
                var testEvent = test.Event;
                EventProcessingService.SendEvent(testEvent, pharmCode);
            }
        }
    }
}
