using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    class LoginModel : DatabaseService
    {
        public List<Event> getEvents(EventStatus status)
        {
            using (var service = new EventService())
            {
                var events = service.GetWhere(EventService.StatusCol == status);
                return events;
            }
        }
    }
}
