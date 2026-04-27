using Appliction.Services.TCServices.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.TCServices
{
    public interface ITCService
    {
        Task CreateTC(CreateTCDto input);
        Task<List<GetTCDto>> GetAllTC();
        Task<GetTCDto> GetTCById(Guid id);
        Task UpdateTC(Guid id, UpdateTCDto input);
        Task DeleteTC(Guid id);
    }
}
