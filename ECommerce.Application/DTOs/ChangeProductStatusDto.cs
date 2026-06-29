using ECommerce.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
  public  class ChangeProductStatusDto
    {
        public ProductStatus StatusProduct { get; set; }
    }
}

