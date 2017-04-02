using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class FillHistory
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("EventRefill")]
        public EventRefill EventRefill { get; set; }
        [ForeignKey("Pharmacist")]
        public Pharmacist Pharmacist { get; set; }
        public DateTime Date { get; set; }

        public FillHistory()
        {

        }

        public FillHistory(EventRefill refill, Pharmacist pharmacist, DateTime date)
        {
            EventRefill = refill;
            Pharmacist = pharmacist;
            Date = date;
        }
    }
}
