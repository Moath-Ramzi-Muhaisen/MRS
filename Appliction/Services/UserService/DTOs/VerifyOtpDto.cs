using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.UserService.DTOs
{
    public class VerifyOtpDto
    {
        public string Email { get; set; }

        public string Code { get; set; }
    }
}
