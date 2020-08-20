using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class ServiceProvider
    {
        public int Id    { get; set; }
        public string AspUserId { get; set; }
        public string ImageUrl { get; set; }
        public string Discription { get; set; }
        public float Price { get; set; }
    }           
}
