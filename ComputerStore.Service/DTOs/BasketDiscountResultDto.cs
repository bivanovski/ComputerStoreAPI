using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Service.DTOs
{
    public class BasketDiscountResultDto
    {
        public decimal OriginalTotal { get; set; }
        public decimal DiscountedTotal { get; set; }
        public List<string> AppliedDiscounts { get; set; } = new();
    }
}