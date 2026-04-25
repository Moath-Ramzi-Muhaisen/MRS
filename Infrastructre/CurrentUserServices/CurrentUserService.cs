using Application.Services.CurrentUserServices;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructre.Services.CurrentUserService
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid? UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return userId != null ? Guid.Parse(userId) : (Guid?)null;
            }
        }

        public string? Name
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
            }
        }

        public string? Email
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            }
        }

        public string? MobilePhone
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.MobilePhone)?.Value;
            }
        }

        public string? Role
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            }
        }
    }
}