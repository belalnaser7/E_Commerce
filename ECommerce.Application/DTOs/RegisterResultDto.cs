using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class RegisterResultDto
    {
        public bool Success { get; set; }
        public ICollection<string> Errors { get; set; } = new List<string>();
    }
}
