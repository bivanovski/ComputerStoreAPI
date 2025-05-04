using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Data.Entities
{
    public class Product
    {
        public int Id { get; set; } // Primary key
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Relationships
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
