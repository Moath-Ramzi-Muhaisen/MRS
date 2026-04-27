using Domain.Entites;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Appliction.Services.RequestServices.DTOs
{
    public class GetRequestDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid EmployeeId { get; set; }

        public string EmployeeName { get; set; }
        public Guid? TechnicianId { get; set; }

        public string? TechnicianName { get; set; }
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CreatedAt { get; set; }

        public RequestStatus Status { get; set; }
        public string StatusName { get { return Status.ToString(); } }
        public GetRequestDetailDto RequestDetail { get; set; }
    }
    public class GetRequestDetailDto
    {
        public string Location { get; set; }
        public string EmployeeNotes { get; set; }
        public string? TechnicianNotes { get; set; }
    }
}
