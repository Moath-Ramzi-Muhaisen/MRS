using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.UserService.DTOs
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Location { get; set; }
        public Guid RoleId { get; set; }
        public List<Guid>? CatagoryIds { get; set; }
    }
}
