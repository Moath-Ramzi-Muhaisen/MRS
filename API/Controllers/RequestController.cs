using Appliction.Services.RequestServices;
using Appliction.Services.RequestServices.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }
        [Authorize(Roles = nameof(SystemRole.Employee))]
        [Consumes("multipart/form-data")]
        [HttpPost("Create_Request")]
        public async Task<IActionResult> CreateRequest([FromForm] CreateRequestDto input)
        {
            await _requestService.CreateRequest(input);
            return Ok();
        }
        [Authorize(Roles = nameof(SystemRole.Employee) + "," + nameof(SystemRole.Admin))]
        [Consumes("multipart/form-data")]
        [HttpPost("Update_Request")]
        public async Task<IActionResult> UpdateRequest(Guid id, [FromForm] CreateRequestDto input)
        {
            await _requestService.UpdateRequest(id, input);
            return Ok();
        }
        [Authorize(Roles = nameof(SystemRole.Technician) + "," + nameof(SystemRole.Admin))]
        [HttpPost("Update_Status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto input)
        {
            await _requestService.UpdateStatus(id, input);
            return Ok();
        }
        [HttpPost("Update_Image")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = nameof(SystemRole.Employee) + "," + nameof(SystemRole.Admin))]
        public async Task<IActionResult> UpdateImage(Guid id, IFormFile image)
        {
            await _requestService.UpdateImage(id, image);
            return Ok();
        }
        [Authorize(Roles = nameof(SystemRole.Admin))]
        [HttpPost("Assign_Technician")]
        public async Task<IActionResult> AssignTechnician(Guid requestId, Guid technicianId)
        {

            await _requestService.AssignTechnician(requestId, technicianId);
            return Ok();
        }
        [Authorize(Roles = nameof(SystemRole.Technician))]
        [HttpPost("Add_Technician_Notes")]
        public async Task<IActionResult> AddTechnicianNotes(Guid requestId, string notes)
        {
            await _requestService.AddTechnicianNotes(requestId, notes);
            return Ok();
        }
        [Authorize(Roles = nameof(SystemRole.Admin))]
        [HttpGet("Get_All_Requests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _requestService.GetAllRequest();
            return Ok(requests);
        }
        [Authorize(Roles = nameof(SystemRole.Employee) + "," + nameof(SystemRole.Admin) + "," + nameof(SystemRole.Admin))]
        [HttpGet("Get_Request_By_Id")]
        public async Task<IActionResult> GetRequestById(Guid id)
        {
            var request = await _requestService.GetRequestById(id);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }
        [Authorize(Roles = nameof(SystemRole.Admin))]
        [HttpGet("Get_All_Request_History")]
        public async Task<IActionResult> GetAllRequestHistory()
        {
            var requestHistory = await _requestService.GetAllRequestHistory();
            return Ok(requestHistory);
        }
        [Authorize(Roles = nameof(SystemRole.Admin) + "," + nameof(SystemRole.Employee))]
        [HttpGet("Get_Request_History_By_Id")]
        public async Task<IActionResult> GetRequestHistoryById(Guid requestId)
        {
            var requestHistory = await _requestService.GetRequestHistoryById(requestId);
            if (requestHistory == null)
            {
                return NotFound();
            }
            return Ok(requestHistory);
        }
        [Authorize(Roles = nameof(SystemRole.Technician) + "," + nameof(SystemRole.Employee))]
        [HttpGet("Get_Requests_Currant_Technician_Or_Employee")]
        public async Task<IActionResult> GetRequestsCurrantTechnicianOrEmployee()
        {
            var requests = await _requestService.GetRequestsCurrantTechnicianOrEmployee();
            if (requests == null)
            {
                NoContent();
            }
            return Ok(requests);
        }
        [Authorize(Roles = nameof(SystemRole.Admin) + "," + nameof(SystemRole.Employee))]
        [HttpPost("Delete_Request")]
        public async Task<IActionResult> DeleteRequest(Guid id)
        {
            await _requestService.DeleteRequest(id);
            return Ok();
        }
        [Authorize(Roles = nameof(SystemRole.Admin) + "," + nameof(SystemRole.Employee) + "," + nameof(SystemRole.Technician))]
        [HttpGet("dashboard-stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var result = await _requestService.GetDashboardStats();

            return Ok(result);
        }
        [Authorize(Roles = nameof(SystemRole.Employee) + "," + nameof(SystemRole.Technician))]

        [HttpGet("dashboard-stats-by-user")]
        public async Task<IActionResult> GetDashboardStatsByUserId()
        {
            var result = await _requestService
                .GetDashboardStatsByUserId();

            return Ok(result);
        }
    }
}
