using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.CatagoryServices.DTOs
{
    public class GetCatagoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public TypeCatagory Type { get; set; }
        public string TypeName { get { return Type.ToString(); } }
    }
}
