using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;
using static PPOK.Domain.Types.MessageTemplateType;
using static PPOK.Domain.Types.MessageTemplateMedia;

namespace PPOK.Domain.Service
{
    public class PharmacyService : CRUDService<Pharmacy>
    {
        public class MTKey : Tuple<MessageTemplateType, MessageTemplateMedia>
        {
            public MTKey(MessageTemplateType item1, MessageTemplateMedia item2) : base(item1, item2){}

            public MessageTemplateType Type { get { return this.Item1; } }
            public MessageTemplateMedia Media { get { return this.Item2; } }
        }
        public static Dictionary<MTKey, string> defaultMessageTemplates = new Dictionary<MTKey, string>()
        {
            { new MTKey(REFILL,             PHONE), "Refill by phone" },
            { new MTKey(REFILL,             EMAIL), "Refill by email." },
            { new MTKey(REFILL,             TEXT ), "Refill by text." },

            { new MTKey(REFILL_RESPONSE,    PHONE), "Refill response by phone." },
            { new MTKey(REFILL_RESPONSE,    EMAIL), "Refill response by email." },
            { new MTKey(REFILL_RESPONSE,    TEXT ), "Refill response by text." },

            { new MTKey(REFILL_PICKUP,      PHONE), "Refill pickup by phone." },
            { new MTKey(REFILL_PICKUP,      EMAIL), "Refill pickup by email." },
            { new MTKey(REFILL_PICKUP,      TEXT ), "Refill pickup by text." },

            { new MTKey(RECALL,             PHONE), "Recall by phone." },
            { new MTKey(RECALL,             EMAIL), "Recall by email." },
            { new MTKey(RECALL,             TEXT ), "Recall by text." },

            { new MTKey(HAPPYBIRTHDAY,      PHONE), "Happy birthday to you! Happy birthday to you! Happy birthday dear {Patient.FirstName}! Happy birthday to you!" },
            { new MTKey(HAPPYBIRTHDAY,      EMAIL), "Happy birthday {Patient.FirstName} {Patient.LastName}!" },
            { new MTKey(HAPPYBIRTHDAY,      TEXT ), "Happy birthday {Patient.FirstName} {Patient.LastName}!" }
        };

        public static List<MessageResponseOption> defaultMessageResponseOptions = new List<MessageResponseOption>() {
                        new MessageResponseOption() { Type = MessageTemplateType.REFILL, CallbackFunction = "FillPrescription", Verb = "yes", ShortDescription = "fill", LongDescription = "fill your prescription" },
                        new MessageResponseOption() { Type = MessageTemplateType.REFILL, CallbackFunction = "BridgeToPharmacist", Verb = null, ShortDescription = null, LongDescription = "talk to a pharmacist" },
                        new MessageResponseOption() { Type = MessageTemplateType.REFILL, CallbackFunction = "Unsubscribe", Verb = "stop", ShortDescription = "unsubscribe", LongDescription = "unsubscribe from communications" }

                    };

        public const string TABLE = "Pharmacy";
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column NameCol = $"[{TABLE}].[Name]";
        public static readonly Column PhoneCol = $"[{TABLE}].[Phone]";
        public static readonly Column AddressCol = $"[{TABLE}].[Address]";

        public PharmacyService() : base(TABLE)
        {

        }

        public override void Create(Pharmacy pharmacy)
        {
            base.Create(pharmacy);
            //automatically insert the default message suite for the new pharmacy
            using(var service = new MessageTemplateService())
            {
                List<MessageTemplate> templates = new List<MessageTemplate>();
                foreach(var entry in defaultMessageTemplates)
                {
                    templates.Add(
                        new MessageTemplate()
                        {
                            Pharmacy = pharmacy,
                            Type = entry.Key.Type,
                            Media = entry.Key.Media,
                            Content = entry.Value
                        }
                    );
                }
                service.Create(templates);
            }
            //automatically insert the response options linked to the defautl message suite
            
            using (var service = new MessageResponseOptionService())
            {
                foreach (var opt in defaultMessageResponseOptions)
                {
                    service.Create(opt);
                }

            }
        }
    }
}
