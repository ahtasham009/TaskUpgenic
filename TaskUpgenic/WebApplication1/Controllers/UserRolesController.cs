using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [Route("get/{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(Id).ConfigureAwait(false);
            return Ok(await _userManager.GetRolesAsync(user).ConfigureAwait(false));
        }

       
        [HttpPost]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        [Route("add")]
        public async Task<IActionResult> Post([FromBody]UserViewModel model)
        {
            if (model == null)
                return BadRequest(new string[] { "No data in model!" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(x => x.Errors.FirstOrDefault().ErrorMessage));

            IdentityUser user = await _userManager.FindByIdAsync(model.Id).ConfigureAwait(false);
            if (user == null)
                return BadRequest(new string[] { "Could not find user!" });

            IdentityRole role = await _roleManager.FindByIdAsync(model.RoleId).ConfigureAwait(false);
            if (role == null)
                return BadRequest(new string[] { "Could not find role!" });

            IdentityResult result = await _userManager.AddToRoleAsync(user, role.Name).ConfigureAwait(false);
            if (result.Succeeded)
            {
                return Ok(role.Name);
            }
            return BadRequest(result.Errors.Select(x => x.Description));
        }

    }
}