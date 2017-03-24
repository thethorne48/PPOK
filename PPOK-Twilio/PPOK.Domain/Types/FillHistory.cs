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
        [ForeignKey("Prescription")]
        public Prescription Prescription { get; set; }
        [ForeignKey("Pharmacist")]
        public Pharmacist Pharmacist { get; set; }
        public DateTime Date { get; set; }

        public FillHistory()
        {

        }

        public FillHistory(Prescription prescription, Pharmacist pharmacist, DateTime date)
        {
            Prescription = prescription;
            Pharmacist = pharmacist;
            Date = date;
        }
    }
}
