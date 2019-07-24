using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProTick.ResourceDTOs;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;
using ProTick.Singletons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.Configuration;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]")]
    public class AuthenticationController : Controller
    {
        private IConfiguration configuration;
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;
        private IHasher hasher;

        public AuthenticationController(IConfiguration conf, 
            [FromServices] ProTickDatabaseContext db,
            [FromServices] IResourceDTOConverter converter,
            [FromServices] IDatabaseQueryManager dbm,
            [FromServices] IHasher hasher)
        {
            this.configuration = conf;
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
            this.hasher = hasher;
        }

        [HttpPost, Route("Login")]
        public IActionResult Login([FromBody] LoginUserDTO loginUser)
        {
            if (loginUser == null)
                return BadRequest("Invalid client request");
            
            var emp = dbm.FindEmployeeByUsername(loginUser.Username);

            var pass = hasher.HashPassword(loginUser.Password);
            
            if (emp != null && emp.Password == pass)
            {

                var jwtAuthentication = configuration.GetSection("JwtAuthentication");

                var handler = new JwtSecurityTokenHandler();

                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(emp.Username, "Token"),
                    new[] {
                        new Claim("Role", "Manager")
                    }
                    );

                var keyByteArray = System.Text.Encoding.UTF8.GetBytes(jwtAuthentication.GetValue<string>("SecurityKey"));
                var signinKey = new SymmetricSecurityKey(keyByteArray);
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = jwtAuthentication.GetValue<string>("ValidIssuer"),
                    Audience = jwtAuthentication.GetValue<string>("ValidAudience"),
                    SigningCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256),
                    Subject = identity,
                    Expires = DateTime.Now.AddMinutes(50),
                    NotBefore = DateTime.Now
                });

                return Ok(new { Token = handler.WriteToken(securityToken) });

            }

            return Unauthorized();

        }
    }
}
