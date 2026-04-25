using Appliction.Repositories;
using Appliction.Services.RoleService.DTOs;
using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Appliction.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepository;

        public RoleService(IGenericRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public  List<GetRoleDto> GetAllRoles()
        {
           var roles = _roleRepository.GetAll().Select(role => new GetRoleDto
           {
               Id = role.Id,
               Name = role.Name,
               Code = role.Code
           }).ToList(); 

            return roles;
        }   
    }
}
