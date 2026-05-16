using Appliction.Services.CatagoryServices;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]

    [Route("api/[controller]")]
    [ApiController]
    public class CatagoryController : ControllerBase
    {
        private readonly ICatagoryService _catagoryService;
        public CatagoryController(ICatagoryService catagoryService)
        {
            _catagoryService = catagoryService;
        }
        [HttpGet("Get_All_Catagory")]
        public async Task<IActionResult> GetAllCatagory()
        {
            var catagories = await _catagoryService.GetAllCatagory();
            return Ok(catagories);
        }
    }
}
