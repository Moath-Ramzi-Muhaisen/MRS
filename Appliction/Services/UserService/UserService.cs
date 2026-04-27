using Appliction.Repositories;
using Appliction.Services.UserService.DTOs;
using Domain.Entites;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userService;
        public UserService(IGenericRepository<User> userService)
        {
            _userService = userService;
        }

        public async Task CreateUser(CreateUserDto input)
        {
            if (_userService.GetAll().Any(u => u.Email == input.Email.ToLower().Trim()))
            {
                throw new Exception("User with this email already exists.");

            }
            if (_userService.GetAll().Any(u => u.PhoneNumber == input.PhoneNumber.Trim()))
            {
                throw new Exception("User with this phone number already exists.");

            }
            var user = new User()
            {
                Name = input.Name,
                Email = input.Email.ToLower().Trim(),
                PhoneNumber = input.PhoneNumber.Trim(),
                Location = input.Location,
                RoleId = input.RoleId
            };

            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, input.Password);


            await _userService.InsertAsync(user);
            await _userService.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid id)
        {
            var user = _userService.GetById(id);
            _userService.Delete(user);
            _userService.SaveChanges();
        }

        public List<GetUsersDto> GetAllUser()
        {
            var users = _userService.GetAll().Include(u => u.Role).Select(u => new GetUsersDto()
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Location = u.Location,
                RoleId = u.RoleId,
                RoleName = u.Role.Name

            }).ToList();
            return users;
        }

        public async Task<GetUsersDto> GetUserById(Guid id)
        {
            var user = await _userService.GetAll().Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

            var result = new GetUsersDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Location = user.Location,
                RoleId = user.RoleId,
                RoleName = user.Role.Name
            };
            return result;
        }

        public Task<List<GetUsersDto>> GetUsersTechnicians()
        {
            var technicians = _userService.GetAll().Include(u => u.Role).Where(u => u.Role.Name == SystemRole.Technician.ToString()).Select(u => new GetUsersDto()
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Location = u.Location,
                RoleId = u.RoleId,
                RoleName = u.Role.Name
            }).ToListAsync();
            return technicians;
        }

        public async Task UpdateUser(Guid id, UpdateUserDto input)
        {
            if (_userService.GetAll().Any(u => u.Email == input.Email.ToLower().Trim() && u.Id != id))
            {
                throw new Exception("User with this email already exists.");

            }
            if (_userService.GetAll().Any(u => u.PhoneNumber == input.PhoneNumber.Trim() && u.Id != id))
            {
                throw new Exception("User with this phone number already exists.");

            }
            var user = _userService.GetById(id);

            user.Name = input.Name;
            user.Email = input.Email.ToLower().Trim();
            user.PhoneNumber = input.PhoneNumber.Trim();
            user.Location = input.Location;
            user.RoleId = input.RoleId;

            _userService.Update(user);
            _userService.SaveChanges();

        }
    }
}
