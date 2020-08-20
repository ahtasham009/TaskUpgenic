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
    public class RoleController : ControllerBase
    {

        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IdentityRole>), 200)]
        [Route("get")]
        public IActionResult Index() => Ok(
            _roleManager.Roles
            .Select(role => new
            {
                role.Id,
                role.Name
            }));
        [HttpPost]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        [Route("insert")]
        public async Task<IActionResult> Create([FromBody]RoleViewModel model)
        {
            if (model == null)
                return BadRequest(new string[] { "No data in model!" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(x => x.Errors.FirstOrDefault().ErrorMessage));

            IdentityRole identityRole = new IdentityRole { Name = model.Name };

            IdentityResult result = await _roleManager.CreateAsync(identityRole).ConfigureAwait(false);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    identityRole.Id,
                    identityRole.Name
                });
            }
            return BadRequest(result.Errors.Select(x => x.Description));
        }

        
        [HttpPut]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        [Route("update/{Id}")]
        public async Task<IActionResult> Update(string Id, [FromBody]RoleViewModel model)
        {
            if (model == null)
                return BadRequest(new string[] { "No data in model!" });

            IdentityRole identityRole = await _roleManager.FindByIdAsync(Id).ConfigureAwait(false);

            identityRole.Name = model.Name;

            IdentityResult result = await _roleManager.UpdateAsync(identityRole).ConfigureAwait(false);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors.Select(x => x.Description));
        }
    }
}