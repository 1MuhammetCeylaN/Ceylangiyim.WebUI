using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceylangiyim.Core.Entities
{
    public class ProductColor
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ColorName { get; set; }
        public string? ImageUrl { get; set; }
        public string? RedirectUrl { get; set; }

        public Product? Product { get; set; }
    }
}
