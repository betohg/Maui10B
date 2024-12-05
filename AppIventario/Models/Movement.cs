using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppIventario.Models;
using Newtonsoft.Json;
namespace AppIventario.Models
{
    public class Movement
    {
        public int MovementId { get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public string MovementTypeName { get; set; }
        public int Quantity { get; set; }
        public DateTime MovementDate { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }

        [JsonProperty("movement_type")] 

        public TypeMovement MovementType { get; set; }  

    }
}
