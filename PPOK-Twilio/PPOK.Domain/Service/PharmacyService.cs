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
                MessageTemplate template = new MessageTemplate { Pharmacy = pharmacy };
                Action<MessageTemplateType, MessageTemplateMedia, string> addTemplate = (type, media, content) =>
                {
                    template.Type = type;
                    template.Media = media;
                    template.Content = content;
                    service.Create(template);
                };

                addTemplate(REFILL, PHONE, "Refill by phone.");
                addTemplate(REFILL, EMAIL, "Refill by email.");
                addTemplate(REFILL, TEXT, "Refill by text.");

                addTemplate(REFILL_RESPONSE, PHONE, "Refill response by phone.");
                addTemplate(REFILL_RESPONSE, EMAIL, "Refill response by email.");
                addTemplate(REFILL_RESPONSE, TEXT, "Refill response by text.");
                
                addTemplate(REFILL_PICKUP, PHONE, "Refill pickup by phone.");
                addTemplate(REFILL_PICKUP, EMAIL, "Refill pickup by email.");
                addTemplate(REFILL_PICKUP, TEXT, "Refill pickup by text.");

                addTemplate(RECALL, PHONE, "Recall by phone.");
                addTemplate(RECALL, EMAIL, "Recall by email.");
                addTemplate(RECALL, TEXT, "Recall by text.");

                addTemplate(HAPPYBIRTHDAY, PHONE, "Happy birthday to you! Happy birthday to you! Happy birthday dear {Patient.FirstName}! Happy birthday to you!");
                addTemplate(HAPPYBIRTHDAY, EMAIL, "Happy birthday {Patient.FirstName} {Patient.LastName}!");
                addTemplate(HAPPYBIRTHDAY, TEXT, "Happy birthday {Patient.FirstName} {Patient.LastName}!");
            }
        }
    }
}
