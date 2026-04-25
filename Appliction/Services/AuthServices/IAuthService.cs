using Appliction.Services.AuthServices.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.AuthServices
{
    public interface IAuthService
    {
        Task<LoginResponsDto> Login(LoginRequestDto input);
        Task ChangeUserPassword(ChangeUserPasswordDto input);
        Task<string> RefreshToken(RefreshTokenDto input);
        Task Logout(Guid UserId);
    }
}
