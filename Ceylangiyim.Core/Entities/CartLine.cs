using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceylangiyim.Core.Entities
{
    public class CartLine
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public string UniqueKey => $"{Product.Id}-{Product.Name}";
        public string SizeName { get; set; }
        public string? ProductName { get; set; }

    }
}
