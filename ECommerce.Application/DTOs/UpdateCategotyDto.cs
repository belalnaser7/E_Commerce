using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class UpdateCategotyDto
    {
        [Display(Name ="Category Name")]
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
