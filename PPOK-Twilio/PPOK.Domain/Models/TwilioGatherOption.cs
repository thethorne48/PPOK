using System;

namespace PPOK.Domain.Models
{
    public class TwilioGatherOption
    {
        public string Digits;
        public string Description;
        public Func<string, string, object> Func;
    }
}