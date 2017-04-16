using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Models
{
    public class EventModel
    {
        public int Code { get; set; }

        public EventModel(int code)
        {
            Code = code;
        }
    }
}

