using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.IdentityModel;
using WebApplication1.IdentityModel.Settings;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSecurityTokenSettings _jwt;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JwtSecurityTokenSettings> jwt,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            this._configuration = configuration;
        }
       
        [HttpPost]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]UserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(x => x.Errors.FirstOrDefault().ErrorMessage));

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            
            if (user != null) {
                IdentityRole role = await _roleManager.FindByIdAsync(model.RoleId).ConfigureAwait(false);
                if (role == null)
                    return BadRequest(new string[] { "Could not find role!" });
                IdentityUser user1 = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = model.EmailConfirmed,
                    PhoneNumber = model.PhoneNumber
                };
                IdentityResult result1 = await _userManager.CreateAsync(user1, model.Password).ConfigureAwait(false);
                if (result1.Succeeded)
                {
                    IdentityResult result2 = await _userManager.AddToRoleAsync(user1, role.Name).ConfigureAwait(false);
                    if (result2.Succeeded)
                    {
                        return Ok(new
                        {
                            user.Id,
                            user.Email,
                            user.PhoneNumber,
                            user.EmailConfirmed,
                            user.LockoutEnabled,
                            user.TwoFactorEnabled
                        });
                    }
                }
            }
            
                
            

            return BadRequest(new string[] { "Some thing wrong in the entry." });
        }


        [HttpPost]
        [ProducesResponseType(typeof(TokenModel), 200)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
            if (user == null)
                return BadRequest(new string[] { "Invalid credentials." });

            // Used as user lock
            if (user.LockoutEnabled)
                return BadRequest(new string[] { "This account has been locked." });

            if (await _userManager.CheckPasswordAsync(user, model.Password).ConfigureAwait(false))
            {
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user).ConfigureAwait(false);
                var tokenModel = new TokenModel();
                tokenModel.TFAEnabled = false;
                tokenModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                return Ok(tokenModel);
            }
            return BadRequest(new string[] { "Invalid login attempt." });
        }
        private async Task<JwtSecurityToken> CreateJwtToken(IdentityUser user)
        {
            //var userClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
            //var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

           // var roleClaims = new List<Claim>();

            //for (int i = 0; i < roles.Count; i++)
            //{
            //    roleClaims.Add(new Claim("roles", roles[i]));
            //}



            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)

            };


            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}