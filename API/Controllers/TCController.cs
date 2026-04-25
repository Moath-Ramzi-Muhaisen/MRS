using Appliction.Services.TCServices;
using Appliction.Services.TCServices.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TCController : ControllerBase
    {
        private readonly ITCService _tcService;

        public TCController(ITCService tcService)
        {
            _tcService = tcService;
        }
        [HttpGet("GetAllTC")]
        public async Task<IActionResult> GetAllTC()
        {
            var tcList = await _tcService.GetAllTC();
            return Ok(tcList);
        }
        [HttpGet("GetTCById")]
        public async Task<IActionResult> GetTCById(Guid id)
        {
            var tc = await _tcService.GetTCById(id);
            if (tc == null)
            {
                return NotFound();
            }
            return Ok(tc);
        }
        [HttpPost("CreateTC")]
        public async Task<IActionResult> CreateTC([FromBody] CreateTCDto input)
        {
            await _tcService.CreateTC(input);
            return Ok();
        }
        [HttpPut("UpdateTC")]
        public IActionResult UpdateTC(Guid id, [FromBody] UpdateTCDto input)
        {
            _tcService.UpdateTC(id, input);
            return Ok();
        }
        [HttpDelete("DeleteTC")]
        public IActionResult DeleteTC(Guid id)
        {
            _tcService.DeleteTC(id);
            return Ok();
        }
    }
}
