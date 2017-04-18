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
        private List<Patient> Convert(List<string> lines)
        {
            List<Patient> list = new List<Patient>();
            using (var patientService = new PatientService())
            {
                //start at 1 to skip columns titles
                for (int i = 1; i < lines.Count; i++)
                {
                    string[] values = lines[i].Split(',');
                    list.Add(patientService.Get(values[0]));
                }
            }
            return list;
        }
        // you'll use one of these two functions
        public List<Patient> UploadPatients(string file)
        {
            return Convert(CSVService.ReadFile(file));

        }
        

        public List<Patient> UploadPatientsFromStream(StreamReader stream)
        {
            return Convert(CSVService.Lines(stream));
        }
    }
}
