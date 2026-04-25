using Domain.Entites;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Appliction.Services.AuthServices.DTOs
{
    public class LoginResponsDto

    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public SystemRole RoleCode { get; set; }
        public string? Location { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
