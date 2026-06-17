using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class LoginResponseDto
    {
        public string? Token { get; set; }

        public string? UserName { get; set; }
    }
}
