using Domain.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Appliction.Services.TCServices.DTOs
{
    public class GetTCDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid TechnicianId { get; set; }
        public string TechnicianName { get; set; }
    }
}
