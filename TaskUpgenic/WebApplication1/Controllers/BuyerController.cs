using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        UpDbContext Db = new UpDbContext();
        [HttpPost]
        [Route("createorder")]
        [Obsolete]
        public async Task<IActionResult> CreateOrder([FromBody] BuyerDTO buyer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors.FirstOrDefault().ErrorMessage));
            }
           
                SqlParameter AspUserId = new SqlParameter("@AspUserId", buyer.AspUserId);
            SqlParameter Seller_id = new SqlParameter("@Seller_id", buyer.Seller_id);
            SqlParameter Price = new SqlParameter("@Price", buyer.Price);
            var Orders = Db.Database.ExecuteSqlCommand
     ("Sp_Create_Buyer_Orders @AspUserId,@Seller_id,@Price", AspUserId, Seller_id, Price);
            return  Ok();

        }
    }
}