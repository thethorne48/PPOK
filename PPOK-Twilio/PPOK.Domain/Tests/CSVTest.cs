using PPOK.Domain.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain
{
    class CSVTest
    {
        public static void Main()
        {
            try
            {
                Console.WriteLine("Connecting to database...");
                using (var init = new InitDatabaseService())
                {
                    Console.WriteLine("Connection successful.\nResetting the database...");
                    init.Reset();
                    Console.WriteLine("Reset successful.\nLoading data...");

                    //create dummy pharmacy
                    Types.Pharmacy pharm = new Types.Pharmacy(1, "CSV Pharmacy", "999-888-7777", "1400 chrissartin street");
                    using (var service = new PharmacyService())
                    {
                        service.Create(pharm);
                    }
                    //create dummy patient
                    Types.Patient patient = new Types.Patient(1, "Chris", "Sartin", new DateTime(2000, 7, 14), "77777", "909-333-2000", "chris.sartin@eagles.oc.edu", pharm);
                    using (var service = new PatientService())
                    {
                        service.Create(patient);
                    }
                    //create dummy drug
                    Types.Drug drug = new Types.Drug(1, "Taco Medication");
                    using (var service = new DrugService())
                    {
                        service.Create(drug);
                    }
                    //create dummy prescription
                    Types.Prescription prescription = new Types.Prescription(1, patient, drug, 7, 7);
                    using (var service = new PrescriptionService())
                    {
                        service.Create(prescription);
                    }
                    //create dummy event
                    Types.Event Event = new Types.Event("this is a message");
                    using (var service = new EventService())
                    {
                        service.Create(Event);
                    }
                    //create dummy eventRefill
                    Types.EventRefill RefillEvent = new Types.EventRefill(prescription, Event);
                    using (var service = new EventRefillService())
                    {
                        service.Create(RefillEvent);
                    }
                    //create dummy birthdayevent
                    Types.EventBirthday BirthdayEvent = new Types.EventBirthday(patient, Event);
                    using (var service = new EventBirthdayService())
                    {
                        service.Create(BirthdayEvent);
                    }
                    //create dummy recallevent
                    Types.EventRecall RecallEvent = new Types.EventRecall(patient, drug, Event);
                    using (var service = new EventRecallService())
                    {
                        service.Create(RecallEvent);
                    }                        
                    //create dummy eventhistory
                    Types.EventHistory history = new Types.EventHistory(Event, Types.EventStatus.InActive, new DateTime(2000, 7, 14));
                    using (var service = new EventHistoryService())
                    {
                        service.Create(history);
                    }
                    //create dummy pharmacist in the pharmacy
                    Types.Pharmacist pharmacist = new Types.Pharmacist("James", "Taco", "james.taco@eagles.oc.edu", "888-444-3333", new byte[] { 0 }, new byte[] { 0 });
                    using (var service = new PharmacistService())
                    {
                        service.Create(pharmacist);
                    }
                    //create dummy fillhistory
                    Types.FillHistory fill = new Types.FillHistory(prescription, pharmacist, new DateTime(2000, 7, 14));
                    using (var service = new FillHistoryService())
                    {
                        service.Create(fill);
                    }
                    //create dummy sysadmins (us)
                    Types.SystemAdmin admin = new Types.SystemAdmin("testing", "the stuff", "testing.thestuff@eagles.oc.edu", new byte[] { 0 }, new byte[] { 0 });
                    using (var service = new SystemAdminService())
                    {
                        service.Create(admin);
                    }
                    //create dummy message template
                    Types.MessageTemplate temp = new Types.MessageTemplate(Types.MessageTemplateType.REFILL, Types.MessageTemplateMedia.EMAIL, "this is the dummy template");
                    using (var service = new MessageTemplateService())
                    {
                        service.Create(temp);
                    }
                    //create dummy job
                    Types.Job job = new Types.Job(pharm, pharmacist, true, false);
                    using (var service = new JobService())
                    {
                        service.Create(job);
                    }

                    init.LoadFromFile(@"..\..\App_Data\Scrubbed_Data.xlsx - Sheet1.csv", pharm);
                    Console.WriteLine("Loading data successful.\nAll tests successful...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }
    }
}
