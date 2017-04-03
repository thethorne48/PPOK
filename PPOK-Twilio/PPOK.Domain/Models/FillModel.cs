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
        public FillModel(EventRefill q)
        {
            Code = q.Code;
            Name = q.Prescription.Patient.Name;
            PrescriptionName = q.Prescription.Drug.Name;
            PrescriptionNumber = q.Prescription.Drug.Code.ToString();
            Phone = q.Prescription.Patient.Phone;

        }
    }
}
