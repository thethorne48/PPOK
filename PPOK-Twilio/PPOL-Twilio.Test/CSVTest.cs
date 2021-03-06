﻿using PPOK.Domain.Service;
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
                    Pharmacy pharm = new Pharmacy(1, "CSV Pharmacy", "19187661052", "1400 chrissartin street");
                    using (var service = new PharmacyService())
                    {
                        service.Create(pharm);
                    }
                    //create dummy patient
                    Patient patient = new Patient(1, "Chris", "Sartin", new DateTime(2000, DateTime.Today.Month, DateTime.Today.Day), "77777", "19183994836", "Chris.Sartin@eagles.oc.edu", pharm);
                    Patient patient1 = new Patient(2, "Matthew", "Miller", new DateTime(2000, DateTime.Today.Month, DateTime.Today.Day), "8675309", "19187661052", "matt.miller@eagles.oc.edu", pharm);

                    using (var service = new PatientService())
                    {
                        service.Create(patient);
                        service.Create(patient1);


                    }
                    //create dummy drug
                    Drug drug = new Drug(1, "Taco Medication");
                    using (var service = new DrugService())
                    {
                        service.Create(drug);
                    }
                    //create dummy prescription
                    Prescription prescription = new Prescription(1, patient, drug, 7, 7);
                    Prescription prescription1 = new Prescription(2, patient1, drug, 6, 6);

                    using (var service = new PrescriptionService())
                    {
                        service.Create(prescription);
                        service.Create(prescription1);
                    }
                    EventRefill RefillEvent;

                    //create dummy event
                    using (var service = new EventService())
                    {
                        //create dummy eventRefill
                        Event Event = new Event(patient, "this is a message", EventStatus.ToSend, EventType.REFILL);
                        Event Event1 = new Event(patient, "refill test event", EventStatus.Sent, EventType.REFILL);
                        Event Event2 = new Event(patient1, "this is a test", EventStatus.Fill, EventType.REFILL);
                        RefillEvent = new EventRefill(prescription, Event);
                        EventRefill RefillEvent1 = new EventRefill(prescription1, Event1);
                        EventRefill RefillEvent2 = new EventRefill(prescription1, Event2);
                        using (var service2 = new EventRefillService())
                        {
                            service.Create(Event);
                            service2.Create(RefillEvent);
                            
                            service.Create(Event1);
                            service2.Create(RefillEvent1);

                            service.Create(Event2);
                            service2.Create(RefillEvent2);
                        }

                        //create dummy birthdayevent
                        Event BirthdayEvent = new Event(patient, "this is a message", EventStatus.ToSend, EventType.BIRTHDAY);
                        service.Create(BirthdayEvent);

                        //create dummy recallevent
                        Event = new Event(patient, "this is a message", EventStatus.ToSend, EventType.REFILL);
                        EventRecall RecallEvent = new EventRecall(Event);
                        using (var service2 = new EventRecallService())
                        {
                            service.Create(Event);
                            service2.Create(RecallEvent);
                        }

                        //create dummy eventhistory
                        EventHistory history = new EventHistory(Event, EventStatus.InActive, new DateTime(2000, 7, 14));
                        using (var service2 = new EventHistoryService())
                        {
                            service2.Create(history);
                        }
                    }
                    
                    //create dummy pharmacist in the pharmacy
                    Pharmacist pharmacist = new Pharmacist("James", "Taco", "james.taco@eagles.oc.edu", "18884443333", new byte[] { 0 }, new byte[] { 0 });
                    Pharmacist pharmacist1 = new Pharmacist("Matthew", "Miller", "matt.miller@eagles.oc.edu", "19187661052", new byte[] { 0 }, new byte[] { 0 });
                    Pharmacist pharmacist2 = new Pharmacist("Luke", "Thorne", "luke.thorne@eagles.oc.edu", "14056932048", new byte[] { 0 }, new byte[] { 0 });
                    Pharmacist pharmacist3 = new Pharmacist("Emily", "Pielemeier", "emily.pielemeier@eagles.oc.edu", "13177536066", new byte[] { 0 }, new byte[] { 0 });
                    Pharmacist pharmacist4 = new Pharmacist("Tom", "Hartnett", "tom.hartnett@eagles.oc.edu", "14696671743", new byte[] { 0 }, new byte[] { 0 });

                    using (var service = new PharmacistService())
                    {
                        service.Create(pharmacist);
                        service.Create(pharmacist1);
                        service.Create(pharmacist2);
                        service.Create(pharmacist3);
                        service.Create(pharmacist4);
                    }


                    //create dummy fillhistory
                    FillHistory fill = new FillHistory(RefillEvent, pharmacist, new DateTime(2000, 7, 14));
                    using (var service = new FillHistoryService())
                    {
                        service.Create(fill);
                    }

                    //create dummy sysadmins (us)
                    SystemAdmin admin = new SystemAdmin("testing", "the stuff", "matt.miller@eagles.oc.edu", "19184661052", new byte[] { 0 }, new byte[] { 0 });
                    SystemAdmin admin1 = new SystemAdmin("Tom", "Hartnett", "tom1.hartnett@eagles.oc.edu", "14696671743", new byte[] { 0 }, new byte[] { 0 });
                    SystemAdmin admin2 = new SystemAdmin("Luke", "Thorne", "luke.thorne@eagles.oc.edu", "14056932048", new byte[] { 0 }, new byte[] { 0 });
                    SystemAdmin admin3 = new SystemAdmin("Jon", "Hartnett", "jonathan.hartnett@eagles.oc.edu", "14696671064", new byte[] { 0 }, new byte[] { 0 });
                    SystemAdmin admin4 = new SystemAdmin("Emily", "Pielemeier", "emily.pielemeier@eagles.oc.edu", "13177536066", new byte[] { 0 }, new byte[] { 0 });
                    using (var service = new SystemAdminService())
                    {
                        service.Create(admin);
                        service.Create(admin1);
                        service.Create(admin2);
                        service.Create(admin3);
                        service.Create(admin4);
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
                        Job j4 = new Job(pharm, pharmacist4, true, true);
                        service.Create(j4);
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
