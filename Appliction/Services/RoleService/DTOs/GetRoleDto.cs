using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.RoleService.DTOs
{
    public class GetRoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SystemRole Code { get; set; }
    }
}
