using Appliction.Services.RoleService.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.RoleService
{
    public interface IRoleService
    {
        Task<List<GetRoleDto>> GetAllRoles();
    }
}
