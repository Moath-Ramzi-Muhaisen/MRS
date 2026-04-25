using Appliction.Services.RequestServices;
using Appliction.Services.RequestServices.DTOs;
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
        [HttpGet("GetAllRequests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _requestService.GetAllRequest();
            return Ok(requests);
        }
        [HttpGet("GetRequestById")]
        public async Task<IActionResult> GetRequestById(Guid id)
        {
            var request = await _requestService.GetRequestById(id);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }
        [HttpGet("GetAllRequestHistory")]
        public async Task<IActionResult> GetAllRequestHistory()
        {
            var requestHistory = await _requestService.GetAllRequestHistory();
            return Ok(requestHistory);
        }
        [HttpGet("GetRequestHistoryById")]
        public async Task<IActionResult> GetRequestHistoryById(Guid requestId)
        {
            var requestHistory = await _requestService.GetRequestHistoryById(requestId);
            if (requestHistory == null)
            {
                return NotFound();
            }
            return Ok(requestHistory);
        }
        [HttpPost("CreateRequest")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDto input)
        {
            await _requestService.CreateRequest(input);
            return Ok();
        }
        [HttpPut("UpdateRequest")]
        public async Task<IActionResult> UpdateRequest(Guid id, [FromBody] CreateRequestDto input)
        {
            await _requestService.UpdateRequest(id, input);
            return Ok();
        }
        [HttpDelete("DeleteRequest")]
        public async Task<IActionResult> DeleteRequest(Guid id)
        {
            await _requestService.DeleteRequest(id);
            return Ok();
        }
        [HttpGet("GetRequestsCurrantTechnicianOrEmployee")]
        public async Task<IActionResult> GetRequestsCurrantTechnicianOrEmployee()
        {
            var requests = await _requestService.GetRequestsCurrantTechnicianOrEmployee();
            if (requests == null)
            {
                NoContent();
            }
            return Ok(requests);
        }
        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto input)
        {
            await _requestService.UpdateStatus(id, input);
            return Ok();
        }
        [HttpPut("AssignTechnician")]
        public async Task<IActionResult> AssignTechnician(Guid requestId, Guid technicianId)
        {

            await _requestService.AssignTechnician(requestId, technicianId);
            return Ok();
        }
        [HttpPut("AddTechnicianNotes")]
        public async Task<IActionResult> AddTechnicianNotes(Guid requestId, string notes)
        {
            await _requestService.AddTechnicianNotes(requestId, notes);
            return Ok();
        }
    }
}
