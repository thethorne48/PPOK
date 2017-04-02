using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    class LoginModel
    {
        public List<Event> getSentEvents(EventStatus status)
        {
            List<Event> list = new List<Event>();
            using (var service = new EventHistoryService())
            {
                var events = service.GetWhere(EventHistoryService.StatusCol == status);
                foreach (EventHistory h in events)
                    list.Add(h.Event);
            }
            return list;
        }
    }
}
