using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entites
{
    public class Request
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public User Employee { get; set; }
        public Guid? TechnicianId { get; set; }
        [ForeignKey("TechnicianId")]
        public User Technician { get; set; }
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public DateTime CreatedAt { get; set; }

        public RequestStatus Status { get; set; }
        public RequestDetail RequestDetail { get; set; }
    }
}
