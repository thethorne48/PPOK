using PPOK.Domain.Service;
using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOL_Twilio.Test
{
    class CSVTest
    {
        public static void Test()
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
                    Pharmacy pharm = new Pharmacy(1, "CSV Pharmacy", "999-888-7777", "1400 chrissartin street");
                    using (var service = new PharmacyService())
                    {
                        service.Create(pharm);
                    }
                    //create dummy patient
                    Patient patient = new Patient(1, "Chris", "Sartin", new DateTime(2000, DateTime.Today.Month, DateTime.Today.Day), "77777", "918-399-4836", "matt.miller@eagles.oc.edu", pharm);
                    using (var service = new PatientService())
                    {
                        service.Create(patient);
                    }
                    //create dummy drug
                    Drug drug = new Drug(1, "Taco Medication");
                    using (var service = new DrugService())
                    {
                        service.Create(drug);
                    }
                    //create dummy prescription
                    Prescription prescription = new Prescription(1, patient, drug, 7, 7);
                    using (var service = new PrescriptionService())
                    {
                        service.Create(prescription);
                    }
                    //create dummy event
                    Event Event = new Event("this is a message", EventStatus.ToSend);
                    using (var service = new EventService())
                    {
                        service.Create(Event);
                    }
                    //create dummy eventRefill
                    EventRefill RefillEvent = new EventRefill(prescription, Event);
                    using (var service = new EventRefillService())
                    {
                        service.Create(RefillEvent);
                    }
                    //create dummy birthdayevent
                    EventBirthday BirthdayEvent = new EventBirthday(patient, Event);
                    using (var service = new EventBirthdayService())
                    {
                        service.Create(BirthdayEvent);
                    }
                    //create dummy recallevent
                    EventRecall RecallEvent = new EventRecall(patient, drug, Event);
                    using (var service = new EventRecallService())
                    {
                        service.Create(RecallEvent);
                    }                        
                    //create dummy eventhistory
                    EventHistory history = new EventHistory(Event, EventStatus.InActive, new DateTime(2000, 7, 14));
                    using (var service = new EventHistoryService())
                    {
                        service.Create(history);
                    }
                    //create dummy pharmacist in the pharmacy
                    Pharmacist pharmacist = new Pharmacist("James", "Taco", "james.taco@eagles.oc.edu", "888-444-3333", new byte[] { 0 }, new byte[] { 0 });
                    Pharmacist pharmacist1 = new Pharmacist("Matthew", "Miller", "matt.miller@eagles.oc.edu", "888-444-3333", new byte[] { 0 }, new byte[] { 0 });
                    Pharmacist pharmacist2 = new Pharmacist("Luke", "Thorne", "luke.thorne@eagles.oc.edu", "888-444-3333", new byte[] { 0 }, new byte[] { 0 });
                    Pharmacist pharmacist3 = new Pharmacist("Emily", "Pielemeier", "emily.pielemeier@eagles.oc.edu", "888-444-3333", new byte[] { 0 }, new byte[] { 0 });

                    using (var service = new PharmacistService())
                    {
                        service.Create(pharmacist);
                        service.Create(pharmacist1);
                        service.Create(pharmacist2);
                        service.Create(pharmacist3);
                    }


                    //create dummy fillhistory
                    FillHistory fill = new FillHistory(RefillEvent, pharmacist, new DateTime(2000, 7, 14));
                    using (var service = new FillHistoryService())
                    {
                        service.Create(fill);
                    }

                    //create dummy sysadmins (us)
                    SystemAdmin admin = new SystemAdmin("testing", "the stuff", "luke.thorne@eagles.oc.edu", new byte[] { 0 }, new byte[] { 0 });
                    using (var service = new SystemAdminService())
                    {
                        service.Create(admin);
                    }
                    //create dummy message template
                    MessageTemplate temp = new MessageTemplate(MessageTemplateType.REFILL, MessageTemplateMedia.EMAIL, "this is the dummy Refill template");
                    MessageTemplate temp1 = new MessageTemplate(MessageTemplateType.HAPPYBIRTHDAY, MessageTemplateMedia.EMAIL, "this is the Happy Birthday template");

                    using (var service = new MessageTemplateService())
                    {
                        service.Create(temp);
                        service.Create(temp1);

                    }
                    //create dummy job
                    Job job = new Job(pharm, pharmacist, true, false);
                    using (var service = new JobService())
                    {
                        service.Create(job);
                        Job j1 = new Job(pharm, pharmacist1, true, true);
                        service.Create(j1);
                        Job j2 = new Job(pharm, pharmacist2, true, true);
                        service.Create(j2);
                        Job j3 = new Job(pharm, pharmacist3, true, true);
                        service.Create(j3);
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
