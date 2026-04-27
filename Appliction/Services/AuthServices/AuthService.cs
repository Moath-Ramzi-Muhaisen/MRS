using Application.Services.CurrentUserServices;
using Appliction.Repositories;
using Appliction.Services.AuthServices.DTOs;
using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Appliction.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<Token> _refershTokenRepository;
        private readonly ICurrentUserService _currentUserService;
        public AuthService(IGenericRepository<User> userRepository, IConfiguration configuration, IGenericRepository<Token> refershTokenRepository, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _refershTokenRepository = refershTokenRepository;
            _currentUserService = currentUserService;
        }

        public async Task ChangeUserPassword(ChangeUserPasswordDto input)
        {
            var userId = _currentUserService.UserId;
            var user = await _userRepository.GetByIdAsync(userId.Value);

            var passwordHasher = new PasswordHasher<User>();
            var passwordStutes = passwordHasher.VerifyHashedPassword(user, user.Password, input.CurrentPassword);

            if (passwordStutes == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid current password.");
            }
            if (input.NewPassword != input.ConfirmPassword)
            {
                throw new Exception("Confirm Password not matches.");
            }

            user.Password = passwordHasher.HashPassword(user, input.NewPassword);
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<LoginResponsDto> Login(LoginRequestDto input)
        {
            var user = _userRepository.GetAll().Include(u => u.Role).FirstOrDefault(u => u.PhoneNumber == input.UserName.ToLower().Trim() || u.Email == input.UserName.ToLower().Trim());
            if (user == null)
            {
                throw new Exception("Invalid UserName or Password");
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, input.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid UserName or Password");
            }

            var accessToken = await GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();


            await _refershTokenRepository.InsertAsync(new Token
            {
                UserId = user.Id,
                TokenStr = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
            });

            await _refershTokenRepository.SaveChangesAsync();


            var response = new LoginResponsDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleName = user.Role.Name,
                RoleCode = user.Role.Code,
                Location = user.Location,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return response;

        }

        public async Task Logout()
        {
            var UserId = _currentUserService.UserId;
            var refreshToken = _refershTokenRepository.GetAll().FirstOrDefault(rt => rt.UserId == UserId);
            if (refreshToken != null)
            {
                _refershTokenRepository.Delete(refreshToken);
                await _refershTokenRepository.SaveChangesAsync();
            }

        }

        public async Task<string> RefreshToken(RefreshTokenDto input)
        {

            var refreshToken = await _refershTokenRepository.GetAll().FirstOrDefaultAsync(rt => rt.TokenStr == input.Token && rt.ExpiryDate < DateTime.UtcNow && rt.Id == _currentUserService.UserId);

            if (refreshToken != null)
            {
                var user = await _userRepository.GetAll().Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId);
                var accessToken = await GenerateAccessToken(user);

                refreshToken.TokenStr = GenerateRefreshToken();
                refreshToken.ExpiryDate = DateTime.UtcNow.AddDays(7);

                _refershTokenRepository.Update(refreshToken);
                await _refershTokenRepository.SaveChangesAsync();

                return accessToken;
            }
            return null;
        }
        private async Task<string> GenerateAccessToken(User user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Role, user.Role.Name),
            };


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = jwtSection["Issuer"],
                Audience = jwtSection["Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);

        }

        private string GenerateRefreshToken()
        {
            var random = new byte[64];
            RandomNumberGenerator.Fill(random);
            return Convert.ToBase64String(random);
        }
    }
}
