using Appliction.Services.UserService.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.UserService
{
    public interface IUserService
    {
        List<GetUsersDto> GetAllUser();
        Task<GetUsersDto> GetUserById(Guid id);
        Task CreateUser(CreateUserDto input);
        Task UpdateUser(Guid id, UpdateUserDto input);
        Task DeleteUser(Guid id);
        Task<List<GetUsersDto>> GetUsersTechnicians();

    }
}
