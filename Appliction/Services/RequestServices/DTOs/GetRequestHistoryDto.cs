using Domain.Entites;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Appliction.Services.RequestServices.DTOs
{
    public class GetRequestHistoryDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RequestId { get; set; }


        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public RequestStatus OldStatus { get; set; }
        public string OldStatusName { get { return OldStatus.ToString(); } }
        public RequestStatus? NewStatus { get; set; }
        public string? NewStatusName { get { return NewStatus?.ToString(); } }

        public DateTime ChangedAt { get; set; }
        public string? Comment { get; set; }
    }
}
