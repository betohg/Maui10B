using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppIventario.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int Quantity { get; set; }
        public string Image_Path { get; set; }

        public string ImageUrl => $"http://127.0.0.1:8000/storage/{Image_Path}";

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int SupplierId { get; set; }  // O el nombre que corresponda en tu base de datos


    }
}
