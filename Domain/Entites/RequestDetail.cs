using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entites
{
    public class RequestDetail
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Location { get; set; }
        public string EmployeeNotes { get; set; }
        public string? TechnicianNotes { get; set; }
        public Guid RequestId { get; set; }
        [ForeignKey("RequestId")]
        public Request Request { get; set; }
        public string? ImageUrl { get; set; }
    }
}