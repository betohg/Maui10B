using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppIventario.Models
{
    public class Supplier
    {
        [JsonProperty("supplier_id")]

        public int Supplier_Id { get; set; }  // O el nombre que corresponda en tu base de datos


        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
