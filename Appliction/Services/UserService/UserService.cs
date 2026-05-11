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
        private readonly IGenericRepository<Role> _roleService;
        private readonly IGenericRepository<TechnicianCategory> _technicianCategoryService;
        public UserService(IGenericRepository<User> userService, IGenericRepository<Role> roleService, IGenericRepository<TechnicianCategory> technicianCategoryService)
        {
            _userService = userService;
            _roleService = roleService;
            _technicianCategoryService = technicianCategoryService;
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

            var role = await _roleService.GetByIdAsync(input.RoleId);
            if (role != null && role.Name == SystemRole.Technician.ToString() && input.CatagoryIds != null)
            {

                foreach (var catagoryId in input.CatagoryIds)
                {
                    await _technicianCategoryService.InsertAsync(new TechnicianCategory
                    {
                        TechnicianId = user.Id,
                        CategoryId = catagoryId

                    });
                }
                await _technicianCategoryService.SaveChangesAsync();
            }



        }

        public async Task DeleteUser(Guid id)
        {
            var user = _userService.GetById(id);

            if (user == null) throw new Exception("User Not Found");

            user.IsActived = false;

            _userService.Update(user);
            _userService.SaveChanges();
        }

        public async Task<List<GetUsersDto>> GetAllUser()
        {
            var users = await _userService.GetAll()
                .Where(u => u.IsActived)
                .Include(u => u.Role)
                .ToListAsync();

            var userdto = new List<GetUsersDto>();
            foreach (var user in users)
            {
                var dto = MapToDto(user);
                if (user.Role?.Name == SystemRole.Technician.ToString())
                {
                    var cataegories = await _technicianCategoryService.GetAll()
                        .Include(u => u.Category)
                        .Where(tc => tc.TechnicianId == user.Id)
                        .ToListAsync();

                    dto.Categories = cataegories.Select(c => new GetUserTCDto
                    {
                        Id = c.CategoryId,
                        Name = c.Category.Name,
                    }).ToList();
                }
                userdto.Add(dto);
            }
            return userdto;
        }

        public async Task<GetUsersDto> GetUserById(Guid id)
        {
            var user = await _userService.GetAll().Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null || user.IsActived == false) throw new Exception("user is not found");

            var dto = MapToDto(user);

            if (user.Role?.Name == SystemRole.Technician.ToString())
            {
                var cataegories = await _technicianCategoryService.GetAll()
                    .Include(u => u.Category)
                    .Where(tc => tc.TechnicianId == user.Id)
                    .ToListAsync();

                dto.Categories = cataegories.Select(c => new GetUserTCDto
                {
                    Id = c.CategoryId,
                    Name = c.Category.Name,
                }).ToList();
            }
            return dto;
        }

        public async Task<List<GetUsersDto>> GetUsersTechnicians(Guid? categoryId = null)
        {
            var techniciansUser = _userService.GetAll()
                .Include(u => u.Role)
                .Where(u => u.IsActived)
                .Where(u => u.Role.Name == SystemRole.Technician.ToString());

            if (categoryId.HasValue)
            {
                var tcId = _technicianCategoryService.GetAll()
                    .Where(tc => tc.CategoryId == categoryId.Value)
                    .Select(tc => tc.TechnicianId);

                techniciansUser = techniciansUser.Where(u => tcId.Contains(u.Id));
            }

            var tc = await techniciansUser.ToListAsync();
            var techniciansDto = new List<GetUsersDto>();

            foreach (var tech in tc)
            {
                var dto = MapToDto(tech);

                var catagories = await _technicianCategoryService.GetAll()
                    .Include(x => x.Category)
                    .Where(x => x.TechnicianId == tech.Id)
                    .ToListAsync();

                dto.Categories = catagories.Select(c => new GetUserTCDto
                {
                    Id = c.CategoryId,
                    Name = c.Category.Name,
                }).ToList();
                techniciansDto.Add(dto);
            }
            return techniciansDto;
        }
        public async Task<List<GetUsersDto>> GetUsersEmployee()
        {
            var Employee = _userService.GetAll()
                .Include(u => u.Role)
                .Where(u => u.IsActived)
                .Where(u => u.Role.Name == SystemRole.Employee.ToString());


            var tc = await Employee.ToListAsync();
            var EmployeeDto = new List<GetUsersDto>();

            foreach (var tech in tc)
            {
                var dto = MapToDto(tech);
                EmployeeDto.Add(dto);
            }
            return EmployeeDto;
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

            var role = await _roleService.GetByIdAsync(input.RoleId);
            if (role != null && role.Name == SystemRole.Technician.ToString())
            {
                var exititingCatgories = await _technicianCategoryService.GetAll()
                    .Where(tc => tc.TechnicianId == id)
                    .ToListAsync();
                foreach (var item in exititingCatgories)
                {
                    _technicianCategoryService.Delete(item);
                }
                await _technicianCategoryService.SaveChangesAsync();

                if (input.CatagoryIds != null)
                {



                    foreach (var catagoryId in input.CatagoryIds)
                    {
                        await _technicianCategoryService.InsertAsync(new TechnicianCategory
                        {
                            TechnicianId = user.Id,
                            CategoryId = catagoryId

                        });
                    }
                    await _technicianCategoryService.SaveChangesAsync();
                }
            }
            else
            {
                var exititingCatgories2 = await _technicianCategoryService.GetAll()
                .Where(tc => tc.TechnicianId == id)
                .ToListAsync();

                if (exititingCatgories2.Any())
                {

                    foreach (var item in exititingCatgories2)
                    {
                        _technicianCategoryService.Delete(item);
                    }
                    await _technicianCategoryService.SaveChangesAsync();
                }
            }
        }



        private static GetUsersDto MapToDto(User user)
        {
            return new GetUsersDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Location = user.Location,
                RoleId = user.RoleId,
                RoleName = user.Role.Name ?? "Unkowen"
            };
        }
    }
}
