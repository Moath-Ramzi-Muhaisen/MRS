using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum RequestStatus
    {
        New = 1,
        Assigned = 2,
        InProgress = 3,
        Resolved = 4,
        Done = 5
    }
}
