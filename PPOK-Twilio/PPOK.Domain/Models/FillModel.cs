using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Models
{
    public class FillModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string PrescriptionName { get; set; }
        public string PrescriptionNumber { get; set; }
        public string Phone { get; set; }
        public FillModel(Event e)
        {
            Code = e.Code;
            //Name = e.Prescription.Patient.Name;
            //PrescriptionName = e.Prescription.Drug.Name;
            PrescriptionNumber = "need to add this"; //e.Prescription.Drug.
            //Phone = e.Prescription.Patient.Phone;
        }
    }
}
