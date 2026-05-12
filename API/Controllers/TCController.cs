using Appliction.Services.TCServices;
using Appliction.Services.TCServices.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = nameof(SystemRole.Admin))]
    [Route("api/[controller]")]
    [ApiController]
    public class TCController : ControllerBase
    {
        private readonly ITCService _tcService;

        public TCController(ITCService tcService)
        {
            _tcService = tcService;
        }
        [HttpPost("Create_Technician_Category")]
        public async Task<IActionResult> CreateTC([FromBody] CreateTCDto input)
        {
            await _tcService.CreateTC(input);
            return Ok();
        }
        [HttpPost("Update_Technician_Category")]
        public async Task<IActionResult> UpdateTC(Guid id, [FromBody] UpdateTCDto input)
        {
            await _tcService.UpdateTC(id, input);
            return Ok();
        }
        [HttpPost("UpdateTCByTechnicianId")]
        public async Task<IActionResult> UpdateTCByTechnicianId(Guid TechnicianId, [FromBody] List<Guid> CategoryIds)
        {
            await _tcService.UpdateTCByTechnicianId(TechnicianId, CategoryIds);
            return Ok();
        }
        [HttpGet("Get_All_Technician_Categories")]
        public async Task<IActionResult> GetAllTC()
        {
            var tcList = await _tcService.GetAllTC();
            return Ok(tcList);
        }
        [HttpGet("Get_Technician_Category_By_Id")]
        public async Task<IActionResult> GetTCById(Guid id)
        {
            var tc = await _tcService.GetTCById(id);
            if (tc == null)
            {
                return NotFound();
            }
            return Ok(tc);
        }

        [HttpPost("Delete_Technician_Category")]
        public async Task<IActionResult> DeleteTC(Guid id)
        {
            await _tcService.DeleteTC(id);
            return Ok();
        }
    }
}
