using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;
using PPOK.Domain.Types;

namespace PPOK.Domain.Models
{
    public class EventsModel
    {
        public List<Prescription> Prescriptions { get; set; }

        public EventsModel()
        {

        }

        public EventsModel(List<Prescription> list)
        {
            Prescriptions = list;
        }
    }
}
