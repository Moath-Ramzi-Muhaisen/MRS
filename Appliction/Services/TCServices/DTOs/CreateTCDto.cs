using Domain.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Appliction.Services.TCServices.DTOs
{
    public class CreateTCDto
    {
        public Guid CategoryId { get; set; }
        public Guid TechnicianId { get; set; }

    }
}
