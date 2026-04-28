using Application.Services.CurrentUserServices;
using Appliction.Helper;
using Appliction.Repositories;
using Appliction.Services.RequestServices.DTOs;
using Domain.Entites;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Appliction.Services.RequestServices
{
    public class RequestService : IRequestService
    {
        private readonly IGenericRepository<Request> _requestRepository;
        private readonly IGenericRepository<RequestDetail> _requestDetailRepository;
        private readonly IGenericRepository<RequestHistory> _requestHistoryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RequestService(IHttpContextAccessor httpContextAccessor, IGenericRepository<Request> requestRepository, IGenericRepository<RequestDetail> requestDetailRepository, IGenericRepository<RequestHistory> requestHistoryRepository, ICurrentUserService currentUserService, IConfiguration configuration)
        {
            _requestRepository = requestRepository;
            _requestDetailRepository = requestDetailRepository;
            _requestHistoryRepository = requestHistoryRepository;
            _currentUserService = currentUserService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task CreateRequest(CreateRequestDto input)
        {
            var employeeId = _currentUserService.UserId.Value;
            var request = new Request
            {
                Title = input.Title,
                Description = input.Description,
                EmployeeId = employeeId,
                CategoryId = input.CategoryId,
                CreatedAt = DateTime.UtcNow,
                Status = RequestStatus.New
            };
            await _requestRepository.InsertAsync(request);
            await _requestRepository.SaveChangesAsync();

            string? imageUrl = null;
            if (input.RequestDetail.Image != null)
            {
                imageUrl = await UploadImage(input.RequestDetail.Image);
            }

            var requestDetail = new RequestDetail
            {
                RequestId = request.Id,
                Location = input.RequestDetail.Location,
                EmployeeNotes = input.RequestDetail.EmployeeNotes,
                ImageUrl = imageUrl
            };

            await _requestDetailRepository.InsertAsync(requestDetail);
            await _requestDetailRepository.SaveChangesAsync();

            var requestHistory = new RequestHistory
            {
                RequestId = request.Id,
                EmployeeId = employeeId,
                OldStatus = RequestStatus.New,
                ChangedAt = DateTime.UtcNow
            };
            await _requestHistoryRepository.InsertAsync(requestHistory);

            await _requestHistoryRepository.SaveChangesAsync();
        }

        public async Task DeleteRequest(Guid id)
        {
            var request = _requestRepository.GetById(id);
            var requestDetail = _requestDetailRepository.GetAll().FirstOrDefault(rd => rd.RequestId == id);
            var requestHistory = _requestHistoryRepository.GetAll().FirstOrDefault(rh => rh.RequestId == id);

            DeleteImage(requestDetail?.ImageUrl);

            _requestHistoryRepository.Delete(requestHistory);
            _requestDetailRepository.Delete(requestDetail);
            _requestRepository.Delete(request);

            _requestHistoryRepository.SaveChanges();
            _requestDetailRepository.SaveChanges();
            _requestRepository.SaveChanges();
        }

        public async Task<List<GetRequestDto>> GetAllRequest()
        {
            var requests = _requestRepository.GetAll().
                Include(r => r.Employee).Include(r => r.Technician)
                .Include(r => r.Category)
                .Select(r => new GetRequestDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    EmployeeId = r.EmployeeId,
                    EmployeeName = r.Employee.Name,
                    TechnicianId = r.TechnicianId,
                    TechnicianName = r.Technician.Name,
                    CategoryId = r.CategoryId,
                    CategoryName = r.Category.Name,
                    CreatedAt = DateHelper.Format(r.CreatedAt),
                    Status = r.Status,
                    RequestDetail = new GetRequestDetailDto
                    {
                        Location = r.RequestDetail.Location,
                        EmployeeNotes = r.RequestDetail.EmployeeNotes,
                        TechnicianNotes = r.RequestDetail.TechnicianNotes,
                        ImageUrl = r.RequestDetail.ImageUrl

                    }

                }).ToList();

            return requests;
        }

        public async Task<GetRequestDto> GetRequestById(Guid id)
        {
            var request = _requestRepository.GetAll().
                Include(r => r.Employee).Include(r => r.Technician)
                .Include(r => r.Category)
                .FirstOrDefault(r => r.Id == id);

            var requestDto = new GetRequestDto
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                EmployeeId = request.EmployeeId,
                EmployeeName = request.Employee.Name,
                TechnicianId = request.TechnicianId,
                TechnicianName = request.Technician.Name,
                CategoryId = request.CategoryId,
                CategoryName = request.Category.Name,
                CreatedAt = DateHelper.Format(request.CreatedAt),
                Status = request.Status,
                RequestDetail = new GetRequestDetailDto
                {
                    Location = request.RequestDetail.Location,
                    EmployeeNotes = request.RequestDetail.EmployeeNotes,
                    TechnicianNotes = request.RequestDetail.TechnicianNotes,
                    ImageUrl = request.RequestDetail.ImageUrl

                }

            };
            return requestDto;
        }

        public async Task UpdateRequest(Guid id, CreateRequestDto input)
        {
            var request = _requestRepository.GetById(id);
            var requestDetail = _requestDetailRepository.GetAll().FirstOrDefault(rd => rd.RequestId == id);
            if (request.Status != RequestStatus.New)
            {
                throw new Exception("It cannot be modified because it is under processing.");
            }

            request.Title = input.Title;
            request.Description = input.Description;
            request.CategoryId = input.CategoryId;
            requestDetail.Location = input.RequestDetail.Location;
            requestDetail.EmployeeNotes = input.RequestDetail.EmployeeNotes;


            if (input.RequestDetail.Image != null)
            {
                DeleteImage(requestDetail.ImageUrl);
                requestDetail.ImageUrl = await UploadImage(input.RequestDetail.Image);
            }


            _requestRepository.Update(request);
            _requestDetailRepository.Update(requestDetail);

            _requestRepository.SaveChanges();
            _requestDetailRepository.SaveChanges();
        }
        public async Task UpdateImage(Guid requestId, IFormFile image)
        {
            var requestDetail = _requestDetailRepository.GetAll().FirstOrDefault(rd => rd.RequestId == requestId);
            if (requestDetail == null)
            {
                throw new Exception("Request not found.");
            }
            DeleteImage(requestDetail.ImageUrl);
            requestDetail.ImageUrl = await UploadImage(image);

            _requestDetailRepository.Update(requestDetail);
            _requestDetailRepository.SaveChanges();
        }

        public async Task UpdateStatus(Guid id, UpdateStatusDto input)
        {
            var rh = _requestHistoryRepository.GetAll().FirstOrDefault(rh => rh.RequestId == id);
            var r = _requestRepository.GetAll().FirstOrDefault(r => r.Id == id);

            if (rh.NewStatus == input.NewStatus)
            {
                throw new Exception("Status is already the same");
            }

            r.Status = input.NewStatus;
            _requestRepository.Update(r);
            _requestRepository.SaveChanges();


            rh.OldStatus = rh.NewStatus ?? RequestStatus.New;
            rh.NewStatus = input.NewStatus;
            rh.ChangedAt = DateTime.UtcNow;
            rh.Comment = input.Comment;

            _requestHistoryRepository.Update(rh);
            _requestHistoryRepository.SaveChanges();

        }
        public async Task AssignTechnician(Guid requestId, Guid technicianId)
        {
            var request = _requestRepository.GetById(requestId);
            if (request == null)
            {
                throw new Exception("Request not found.");
            }
            request.TechnicianId = technicianId;
            request.Status = RequestStatus.Assigned;
            _requestRepository.Update(request);
            _requestRepository.SaveChanges();

        }
        public async Task AddTechnicianNotes(Guid requestId, string notes)
        {
            var requestDetail = _requestDetailRepository.GetAll().FirstOrDefault(rd => rd.RequestId == requestId);
            if (requestDetail == null)
            {
                throw new Exception("Request not found.");
            }
            requestDetail.TechnicianNotes = notes;
            _requestDetailRepository.Update(requestDetail);
            _requestDetailRepository.SaveChanges();

        }
        public async Task<List<GetRequestDto>> GetRequestsCurrantTechnicianOrEmployee()
        {
            var requests = _requestRepository.GetAll().
                Include(r => r.Employee).Include(r => r.Technician)
                .Include(r => r.Category)
                .Where(r => r.TechnicianId == _currentUserService.UserId || r.EmployeeId == _currentUserService.UserId);




            var requestsList = requests.Select(r => new GetRequestDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                EmployeeId = r.EmployeeId,
                EmployeeName = r.Employee.Name,
                TechnicianId = r.TechnicianId,
                TechnicianName = r.Technician.Name,
                CategoryId = r.CategoryId,
                CategoryName = r.Category.Name,
                CreatedAt = DateHelper.Format(r.CreatedAt),
                Status = r.Status,
                RequestDetail = new GetRequestDetailDto
                {
                    Location = r.RequestDetail.Location,
                    EmployeeNotes = r.RequestDetail.EmployeeNotes,
                    TechnicianNotes = r.RequestDetail.TechnicianNotes,
                    ImageUrl = r.RequestDetail.ImageUrl

                }
            }).ToList();

            return requestsList;
        }
        public async Task<List<GetRequestHistoryDto>> GetAllRequestHistory()
        {
            var requestHistory = _requestHistoryRepository.GetAll().Include(rh => rh.Employee);
            var requestHistoryList = requestHistory.Select(rh => new GetRequestHistoryDto
            {
                Id = rh.Id,
                RequestId = rh.RequestId,
                EmployeeId = rh.EmployeeId,
                EmployeeName = rh.Employee.Name,
                OldStatus = rh.OldStatus,
                NewStatus = rh.NewStatus,
                ChangedAt = DateHelper.Format(rh.ChangedAt),
                Comment = rh.Comment
            }).ToList();
            return requestHistoryList;
        }
        public async Task<GetRequestHistoryDto> GetRequestHistoryById(Guid requestId)
        {
            var rh = _requestHistoryRepository.GetAll().Include(rh => rh.Employee).FirstOrDefault(rh => rh.RequestId == requestId);
            if (rh == null)
            {
                throw new Exception("Request not found.");
            }
            var rhDto = new GetRequestHistoryDto
            {
                Id = rh.Id,
                RequestId = rh.RequestId,
                EmployeeId = rh.EmployeeId,
                EmployeeName = rh.Employee.Name,
                OldStatus = rh.OldStatus,
                NewStatus = rh.NewStatus,
                ChangedAt = DateHelper.Format(rh.ChangedAt),
                Comment = rh.Comment
            };
            return rhDto;
        }
        private async Task<string> UploadImage(IFormFile file)
        {
            var baseUplodedPath = _configuration["FileStorage:UploadPath"];
            var uploadsfolder = Path.Combine(baseUplodedPath, "Requests");

            if (!Directory.Exists(uploadsfolder))
            {
                Directory.CreateDirectory(uploadsfolder);
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsfolder, fileName);
            using (var filestream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(filestream);
            }

            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            return $"{baseUrl}/External/Requests/{fileName}";
        }
        private void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            try
            {
                var fileName = Path.GetFileName(imageUrl);
                var baseUplodedPath = _configuration["FileStorage:UploadPath"];
                var filePath = Path.Combine(baseUplodedPath, "Requests", fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error deleting image: {ex.Message}");
            }
        }
    }
}
