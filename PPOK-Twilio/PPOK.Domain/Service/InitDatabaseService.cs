using System.Collections.Generic;
using Dapper;
using System.IO;
using System.Reflection;
using System;
using PPOK.Domain.Types;

// Here's a sample of how each function works. It's pretty simple.
// call reset to reset the database, and then either loadfromfile, or loadfromresource to import the csv
// make sure to pass the current pharmacy as well
//            try
//            {
//                InitDatabaseService init = new InitDatabaseService();
//                init.Reset();
//                Types.Pharmacy pharm = new Types.Pharmacy(1, "test1", "test2", "test3");
//                using (var service = new PharmacyService())
//                {
//                     service.Create(pharm);
//                }
//                init.LoadFromFile(@"..\..\App_Data\Scrubbed_Data.xlsx - Sheet1.csv", pharm);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex);
//                Console.ReadKey();
//            }


namespace PPOK.Domain.Service
{
    public class InitDatabaseService : DatabaseService
    {
        //here's to reset
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

        private void Convert(List<string> lines, Pharmacy pharm)
        {
            using (var patientService = new PatientService())
            using (var drugService = new DrugService())
            using (var prescriptionService = new PrescriptionService())
            using (var eventService = new EventService())
            using (var eventRefillService = new EventRefillService())
            using (var birthdayeventService = new EventBirthdayService())
            {
                //start at 1 to skip columns titles
                for (int i = 1; i < lines.Count; i++)
                {
                    string[] values = lines[i].Split(',');
                    DateTime dob;
                    if (values[3] == "NULL")
                        dob = DateTime.Now;
                    else
                        dob = new DateTime(System.Convert.ToInt32(values[3].Substring(0, 4)), System.Convert.ToInt32(values[3].Substring(4, 2)), System.Convert.ToInt32(values[3].Substring(6, 2)));
                    Patient patient = new Patient(System.Convert.ToInt32(values[0]), values[1], values[2], dob, values[4], values[5], values[6], pharm);
                    Drug drug = new Drug(Int64.Parse(values[11]), values[12]);
                    Prescription prescription = new Prescription(Int32.Parse(values[8]), patient, drug, Int32.Parse(values[9]), Int32.Parse(values[10]));
                    Event _event = new Event();
                    //if (patient.DOB.Month == DateTime.Today.Month && patient.DOB.Day == DateTime.Today.Day)
                    //{
                    //    EventBirthday test = new EventBirthday(patient, new Event("happy birthday", EventStatus.ToSend));
                    //    var birthdaytest = birthdayeventService.Get(test);
                    //    if (birthdaytest != null)
                    //        birthdayeventService.Update(birthdaytest);
                    //    else
                    //        birthdayeventService.Create(birthdaytest);
                    //}

                    EventRefill refillEvent = new EventRefill(prescription, _event);

                    //for each parsed patient / drug / etc, check if it is already in database
                    //if so, update it
                    //if not, create it
                    var test1 = patientService.Get(patient.Code);
                    if (test1 != null)
                        patientService.Update(patient);
                    else
                        patientService.Create(patient);

                    var test2 = drugService.Get(drug.Code);
                    if (test2 != null)
                        drugService.Update(drug);
                    else
                        drugService.Create(drug);

                    var test3 = prescriptionService.Get(prescription.Code);
                    if (test3 != null)
                        prescriptionService.Update(prescription);
                    else
                        prescriptionService.Create(prescription);

                    var test4 = eventService.Get(_event.Code);
                    if (test4 != null)
                        eventService.Update(_event);
                    else
                        eventService.Create(_event);

                    var test5 = eventRefillService.Get(refillEvent.Code);
                    if (test5 != null)
                        eventRefillService.Update(refillEvent);
                    else
                        eventRefillService.Create(refillEvent);
                }
            }
        }
        // you'll use one of these two functions
        public void LoadFromFile(string file, Pharmacy pharm)
        {
            Convert(CSVService.ReadFile(file), pharm);

        }

        public void LoadFromResource(string resource, Pharmacy pharm)
        {
            Convert(CSVService.ReadResource(resource), pharm);
        }

        public void LoadFromMemoryStream(MemoryStream stream, Pharmacy pharm)
        {
            Convert(CSVService.ReadResource(stream), pharm);
        }
    }
}
