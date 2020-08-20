using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        UpDbContext Db = new UpDbContext();
        [HttpPost]
        [Route("createservice")]
        [Obsolete]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> CreateService([System.Web.Http.FromBody] ServiceDTO seller)
        {
            SqlParameter AspUserId = new SqlParameter("@AspUserId", seller.AspUserId);
            SqlParameter Discription = new SqlParameter("@Discription", seller.Discription);
            SqlParameter ImageUrl = new SqlParameter("@ImageUrl", seller.ImageUrl);
            SqlParameter Price = new SqlParameter("@Price", seller.Price);
           
            var Orders = Db.Database.ExecuteSqlCommand
     ("EXEC @ReturnValue Sp_Create_Service @AspUserId,@Discription,@ImageUrl,@Price", AspUserId, Discription, ImageUrl, Price);
            return Ok();
        }
        [HttpGet]
        [Obsolete]
        public async Task<IActionResult> GetById(int id)
        {
            SqlParameter Id = new SqlParameter("@Id", id);
            var data = Db.Database.ExecuteSqlCommand("Sp_Select_Service_By_Id @Id", Id);
            
            return Ok() ;
        }
        [HttpPost]
        [Obsolete]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Update([System.Web.Http.FromBody] ServiceDTO seller)
        {

            SqlParameter AspUserId = new SqlParameter("@AspUserId", seller.AspUserId);
            SqlParameter Discription = new SqlParameter("@Discription", seller.Discription);
            SqlParameter ImageUrl = new SqlParameter("@ImageUrl", seller.ImageUrl);
            SqlParameter Price = new SqlParameter("@Price", seller.Price);

            int Orders = Db.Database.ExecuteSqlCommand
     ("Sp_update_Sevice @AspUserId,@Discription,@ImageUrl,@Price", AspUserId, Discription, ImageUrl, Price);
            return Ok();
        }
    }
}