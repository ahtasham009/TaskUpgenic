using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Buyer
    {
        [Key]
        public int Id { get; set; }
        public string AspUserId { get; set; }
        public string Seller_id { get; set; }
        public DateTime OrderDate { get; set; }
        public float Price { get; set; }
    }
}
