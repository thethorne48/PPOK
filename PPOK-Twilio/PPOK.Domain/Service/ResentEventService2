using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    public class ResendEventService : DatabaseService
    {
        public void ResendEvent(int eventid)
        {
            using(var service = new EventService())
            {
                var test = service.Get(eventid);
                test.Status = Types.EventStatus.ToSend;
                service.Update(test);
            }
        }
    }
}
