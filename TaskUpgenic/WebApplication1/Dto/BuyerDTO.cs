using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Dto
{
    public class BuyerDTO
    {
        public string AspUserId { get; set; }
        public string Seller_id { get; set; }
        public DateTime OrderDate { get; set; }
        public float Price { get; set; }
    }
}
