using Appliction.Services.CatagoryServices.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.CatagoryServices
{
    public interface ICatagoryService
    {
        Task<List<GetCatagoryDto>> GetAllCatagory();
    }
}
