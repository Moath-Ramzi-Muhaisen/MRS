using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.TCServices.DTOs
{
    public class UpdateTCDto
    {
        public int CategoryId { get; set; }
        public Guid TechnicianId { get; set; }
    }
}
