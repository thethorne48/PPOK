using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Models
{
    public class SearchModel
    {
        public int Code { get; set; }
        public string EventType { get; set; }
        public string Name { get; set; }
        public string PrescriptionName { get; set; }
        public string PrescriptionNumber { get; set; }
        public string Phone { get; set; }
        public string SendDate { get; set; }
        public string Status { get; set; } //probably make this an enum thing

        public SearchModel(EventRefill e)
        {
            Code = e.Code;
            //EventType = e.Type.ToString();
            Name = e.Prescription.Patient.Name;
            PrescriptionName = e.Prescription.Drug.Name;
            Phone = e.Prescription.Patient.Phone;
            SendDate = DateTime.Now.ToShortDateString();//e.Prescription.Fills.FirstOrDefault().Date; //not terribly sure if this is what we want? this might be calculated
            Status = "How the heck do you get a Status";
        }
    }
}
