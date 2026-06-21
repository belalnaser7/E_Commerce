using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs
{
    public class UpdateCategotyDto
    {
        [Display(Name ="Category Name")]
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
