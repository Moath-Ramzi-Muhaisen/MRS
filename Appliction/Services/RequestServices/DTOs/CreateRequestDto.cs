using Domain.Entites;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Appliction.Services.RequestServices.DTOs
{
    public class CreateRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }


        public CreateRequestDetailDto RequestDetail { get; set; }

    }
    public class CreateRequestDetailDto
    {
        public string Location { get; set; }
        public string EmployeeNotes { get; set; }


    }
}
