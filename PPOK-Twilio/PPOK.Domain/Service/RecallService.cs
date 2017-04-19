using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    public class RecallService
    {
        private List<Patient> Convert(List<string> lines, Pharmacy pharm)
        {
            List<Patient> list = new List<Patient>();
            using (var patientService = new PatientService())
            {
                //start at 1 to skip columns titles
                for (int i = 1; i < lines.Count; i++)
                {
                    string[] values = lines[i].Split(',');
                    var test = patientService.Get(values[0]);
                    if(test == null)
                    {
                        DateTime dob;
                        if (values[3] == "NULL")
                            dob = DateTime.Now;
                        else
                            dob = new DateTime(System.Convert.ToInt32(values[3].Substring(0, 4)), System.Convert.ToInt32(values[3].Substring(4, 2)), System.Convert.ToInt32(values[3].Substring(6, 2)));
                        Patient patient = new Patient(System.Convert.ToInt32(values[0]), values[1], values[2], dob, values[4], values[5], values[6], pharm);
                        patientService.Create(patient);
                        test = patient;
                    }
                    list.Add(test);
                }
            }
            return list;
        }
        // you'll use one of these two functions
        public List<Patient> UploadPatients(string file, Pharmacy pharm)
        {
            return Convert(CSVService.ReadFile(file), pharm);

        }
        

        public List<Patient> UploadPatientsFromStream(StreamReader stream, Pharmacy pharm)
        {
            return Convert(CSVService.Lines(stream), pharm);
        }
    }
}
