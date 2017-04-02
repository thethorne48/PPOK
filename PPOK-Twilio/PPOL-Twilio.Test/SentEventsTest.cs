using PPOK.Domain.Service;
using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOL_Twilio.Test
{
    public class SentEventsTest
    {
        public static void Test()
        {
            try
            {
                using (var init = new EventHistoryService())
                {
                    Console.WriteLine("Generating Events...");
                    EventStatus status = EventStatus.Sent;
                    var events = init.GetWhere(EventHistoryService.StatusCol == status);
                    Console.WriteLine("Events grabbed successfully.\nAll tests successful...\n");

                    foreach (EventHistory e in events)
                        Console.WriteLine(e.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }
    }
}
