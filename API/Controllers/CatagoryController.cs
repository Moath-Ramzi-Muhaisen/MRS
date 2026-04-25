using Appliction.Services.CatagoryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatagoryController : ControllerBase
    {
        private readonly ICatagoryService _catagoryService;
        public CatagoryController(ICatagoryService catagoryService)
        {
            _catagoryService = catagoryService;
        }
        [HttpGet("GetAllCatagory")]
        public async Task<IActionResult> GetAllCatagory()
        {
            var catagories = await _catagoryService.GetAllCatagory();
            return Ok(catagories);
        }
    }
}
