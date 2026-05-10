using Appliction.Services.RequestServices.DTOs;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace Appliction.Services.RequestServices
{
    public interface IRequestService
    {
        Task CreateRequest(CreateRequestDto input);
        Task<List<GetRequestDto>> GetAllRequest();
        Task<GetRequestDto> GetRequestById(Guid id);
        Task UpdateRequest(Guid id, CreateRequestDto input);
        Task DeleteRequest(Guid id);
        Task UpdateStatus(Guid id, UpdateStatusDto input);
        Task AssignTechnician(Guid requestId, Guid technicianId);
        Task AddTechnicianNotes(Guid requestId, string notes);
        Task<List<GetRequestDto>> GetRequestsCurrantTechnicianOrEmployee();
        Task<List<GetRequestHistoryDto>> GetAllRequestHistory();
        Task<GetRequestHistoryDto> GetRequestHistoryById(Guid requestId);
        Task UpdateImage(Guid requestId, IFormFile image);
        Task<DashboardStatsDto> GetDashboardStats();
        Task<DashboardStatsDto> GetDashboardStatsByUserId();
    }
}
