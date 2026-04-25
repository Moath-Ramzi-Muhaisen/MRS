using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.AuthServices.DTOs
{
    public class ChangeUserPasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
