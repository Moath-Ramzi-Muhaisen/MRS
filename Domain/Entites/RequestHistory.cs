using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entites
{
    public class RequestHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RequestId { get; set; }
        [ForeignKey("RequestId")]
        public Request Request { get; set; }
        public Guid EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public User Employee { get; set; }

        public RequestStatus OldStatus { get; set; }
        public RequestStatus? NewStatus { get; set; }

        public DateTime ChangedAt { get; set; }
        public string? Comment { get; set; }


    }
}
