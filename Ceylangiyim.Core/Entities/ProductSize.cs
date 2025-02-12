using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceylangiyim.Core.Entities
{
    public class ProductSize
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string? SizeName { get; set; } // Örn: S, M, L

    }
}
