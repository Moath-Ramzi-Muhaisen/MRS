using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.CurrentUserServices
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Name { get; }
        string? Email { get; }
        string? MobilePhone { get; }
        string? Role { get; }
    }
}
