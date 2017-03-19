using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    class RecallService
    {
        private void Convert(List<string> lines, Pharmacy pharm, MessageTemplate template, Drug drug)
        {
            using (var patientService = new PatientService())
            {
                //start at 1 to skip columns titles
                for (int i = 1; i < lines.Count; i++)
                {
                    string[] values = lines[i].Split(',');

                    var patient = patientService.Get(values[0]);
                    ContactPreference type = patient.ContactPreference;

                    if(type == ContactPreference.EMAIL)
                    {
                        //send an email here
                        //sendEmail(patient.Email, template, drug);
                    } else if(type == ContactPreference.PHONE || type == ContactPreference.NONE)
                    {
                        //send phone call here
                        //sendRecallPhoneCall(patient.Phone, drug);
                    } else if(type == ContactPreference.TEXT)
                    {
                        //send text here
                        //sendRecallTest(patient.Phone, drug);
                    }
                }
            }
        }
        // you'll use one of these two functions
        public void SendRecalls(string file, Pharmacy pharm, MessageTemplate template, Drug drug)
        {
            Convert(CSVService.ReadFile(file), pharm, template, drug);

        }

        public void SendRecallsResource(string resource, Pharmacy pharm, MessageTemplate template, Drug drug)
        {
            Convert(CSVService.ReadResource(resource), pharm, template, drug);
        }
    }
}
