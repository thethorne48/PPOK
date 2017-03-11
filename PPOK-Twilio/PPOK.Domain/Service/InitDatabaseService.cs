using System.Collections.Generic;
using Dapper;
using System.IO;
using System.Reflection;
using System;

namespace PPOK.Domain.Service
{
    public class InitDatabaseService : DatabaseService
    {
        public void Reset()
        {
            Assembly domain = Assembly.GetExecutingAssembly();
            string resource = "PPOK.Domain.Scripts.CreateDatabase.sql";

            using (Stream stream = domain.GetManifestResourceStream(resource))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string sql = reader.ReadToEnd();
                    conn.Query(sql);
                }
            }
        }

        private void Convert(List<string> lines)
        {

            //start at 1 to skip columns titles
            for (int i = 1; i < lines.Count; i++)
            {
                string[] values = lines[i].Split(',');
                DateTime dob;
                if (values[3] == "NULL")
                    dob = DateTime.MinValue;
                else
                    dob = new DateTime(System.Convert.ToInt32(values[3].Substring(0, 4)), System.Convert.ToInt32(values[3].Substring(4, 2)), System.Convert.ToInt32(values[3].Substring(6, 2)));
                Types.Patient patient = new Types.Patient(System.Convert.ToInt32(values[0]), values[1], values[2], dob, values[4], values[5], values[6]);
                Types.Drug drug = new Types.Drug(Int64.Parse(values[11]), values[12]);
                Types.Prescription prescription = new Types.Prescription(Int32.Parse(values[8]), patient, drug, Int32.Parse(values[9]), Int32.Parse(values[10]));
                Types.Event Event = new Types.Event(prescription);

                //for each parsed patient / drug / etc, check if it is already in database
                //if so, update it
                //if not, create it
                using (var service = new PatientService())
                {
                    var test = service.Get(patient.Code);
                    //if (test != null)
                    //    service.Update(patient);
                    //else
                    //    service.Create(patient);
                }

                //using (var service = new DrugService())
                //{
                //    var test = service.Get(drug.Code);
                //    if (test != null)
                //        service.Update(drug);
                //    else
                //        service.Create(drug);
                //}

                //using (var service = new PrescriptionService())
                //{
                //    var test = service.Get(prescription.Code);
                //    if (test != null)
                //        service.Update(prescription);
                //    else
                //        service.Create(prescription);
                //}

                //using (var service = new EventService())
                //{
                //    var test = service.Get(Event.Code);
                //    if (test != null)
                //        service.Update(Event);
                //    else
                //        service.Create(Event);
                //}
            }
        }

        public void LoadFromFile(string file)
        {
            Convert(CSVService.ReadFile(file));

        }

        public void LoadFromResource(string resource)
        {
            Convert(CSVService.ReadResource(resource));
        }
    }
}
