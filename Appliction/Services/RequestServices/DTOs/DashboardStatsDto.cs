using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.RequestServices.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalRequests { get; set; }
        public int NewRequests { get; set; }
        public int InProgressRequests { get; set; }
        public int ResolvedRequests { get; set; }
        public int AssignedRequests { get; set; }
        public int DoneRequests { get; set; }

    }
}
