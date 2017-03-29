using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Types
{
    public enum EventStatus
    {
        //here's how the statuses work
        // when created, starts at ToSend
        // from ToSend, can go to Sent (if sent), or InActive (if rejected)
        // Sent can go to InActive (if rejected), or Fill (if accepted)
        // Fill can go InActive (if rejected), or Complete (when filled)
        ToSend,
        Sent,
        InActive,
        Fill,
        Complete
    }
}
