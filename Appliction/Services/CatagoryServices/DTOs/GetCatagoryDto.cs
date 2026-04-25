using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.CatagoryServices.DTOs
{
    public class GetCatagoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public TypeCatagory Type { get; set; }
    }
}
