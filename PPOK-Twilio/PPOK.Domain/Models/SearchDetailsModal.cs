using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Models
{
    public class SearchDetailsModal
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CurrPref { get; set; }
        public string SentType { get; set; }
        public string PrescriptionName { get; set; }
        public string PrescriptionNumber { get; set; }
        public string Status { get; set; }
        public string SendDate { get; set; }
        public string FillDate { get; set; }
        public string FillPharmacist { get; set; }
        public string RejectedBy { get; set; }
        public string RejectedDate { get; set; }
        public SearchDetailsModal(EventRefill e)
        {
            Code = e.Code;
            Name = e.Prescription.Patient.Name;
            Phone = e.Prescription.Patient.Phone;
            Email = e.Prescription.Patient.Email;

            //make a call to get the currPreference of the Patient
            CurrPref = "Needs Implementation";
            //SentType = e.Type.ToString(); // no longer a thing. Changed the DB to have a table to each message type
            PrescriptionName = e.Prescription.Drug.Name;
            PrescriptionNumber = e.Prescription.Code.ToString(); //what is this really
            Status = "Needs Implementation";
            FillDate = e.Prescription.Fills.FirstOrDefault().Date.ToString(); //not terribly sure if this is what we want? this might be calculated
            FillPharmacist = e.Prescription.Fills.FirstOrDefault().Pharmacist.FirstName;
            RejectedBy = "needs Imp.";
            RejectedDate = "needs imp.";
            Status = "How the heck do you get a Status";
        }
    }
}
