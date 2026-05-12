using Appliction.Services.UserService;
using Appliction.Services.UserService.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = nameof(SystemRole.Admin))]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("Create_User")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto input)
        {
            try
            {
                await _userService.CreateUser(input);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPost("Update_User")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto input)
        {
            try
            {
                await _userService.UpdateUser(id, input);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Get_All_Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUser();
            return Ok(users);
        }
        [HttpGet("Get_User_By_Id")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("Get_Users_Technicians")]
        public async Task<IActionResult> GetUsersTechnicians(Guid? categoryId)
        {
            var technicians = await _userService.GetUsersTechnicians(categoryId);
            return Ok(technicians);
        }
        [HttpGet("Get_Users_Employees")]
        public async Task<IActionResult> GetUsersEmployees()
        {
            var employees = await _userService.GetUsersEmployee();
            return Ok(employees);
        }

        [HttpPost("Delete_User")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
