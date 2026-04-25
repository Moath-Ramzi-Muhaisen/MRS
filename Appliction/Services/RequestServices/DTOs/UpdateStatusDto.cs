using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.RequestServices.DTOs
{
    public class UpdateStatusDto
    {
        public RequestStatus NewStatus { get; set; }
        public string? Comment { get; set; }

    }
}
