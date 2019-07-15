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
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]")]
    public class AuthenticationController : Controller
    {
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;

        public AuthenticationController([FromServices] ProTickDatabaseContext db, [FromServices] IResourceDTOConverter converter, [FromServices] IDatabaseQueryManager dbm)
        {
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
        }

        [HttpPost, Route("Login")]
        public IActionResult Login([FromBody] LoginUserDTO loginUser)
        {
            if (loginUser == null)
                return BadRequest("Invalid client request");

            Console.WriteLine("Username:" + loginUser.Username);
            Console.WriteLine("Password:" + loginUser.Password);

            var emp = dbm.FindEmployeeByUsername(loginUser.Username);
            if (emp != null && emp.Password == loginUser.Password)
            {
                /*
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("protick19@da2019moveIT"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, emp.Username),
                    new Claim(ClaimTypes.Role, "Manager")
                };

                var tokenOptions = new JwtSecurityToken(
                    issuer: "http://localhost:8080",
                    audience: "http://localhost:8080",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
                */

                var handler = new JwtSecurityTokenHandler();

                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(emp.Username, "Token"),
                    new[] {
                        new Claim("ID", emp.LastName)
                    }
                    );

                var keyByteArray = System.Text.Encoding.UTF8.GetBytes("protick19@da2019moveIT");
                var signinKey = new SymmetricSecurityKey(keyByteArray);
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = "https://localhost:8080/",
                    Audience = "https://localhost:8080/",
                    SigningCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256),
                    Subject = identity,
                    Expires = DateTime.Now.AddMinutes(50),
                    NotBefore = DateTime.Now
                });

                return Ok(new { Token = handler.WriteToken(securityToken) });

            }

            return Unauthorized();

        }

        [HttpGet("{id}")]
        public TicketDTO GetTicketByID(int id)
        {
            return converter.TicketToDTO(dbm.FindTicketByID(id));
        }

        [HttpPost, Authorize]
        public TicketDTO PostTicket([FromBody] TicketDTO ticket)
        {
            var newTicket = db.Ticket.Add(converter.DTOToTicket(ticket));
            db.SaveChanges();
            return converter.TicketToDTO(newTicket.Entity);
        }

        [HttpPut("{id}"), Authorize]
        public TicketDTO PutTicket(int id, [FromBody] TicketDTO ticket)
        {
            var editTicket = dbm.FindTicketByID(id);

            bool changesDone = false;
            if (editTicket.Description != ticket.Description)
            {
                editTicket.Description = ticket.Description;
                changesDone = true;
            }
            if (editTicket.State.StateID != ticket.StateID)
            {
                editTicket.State = dbm.FindStateByID(ticket.StateID);
                changesDone = true;
            }
            if (editTicket.Subprocess.SubprocessID != ticket.SubprocessID)
            {
                editTicket.Subprocess = dbm.FindSubprocessByID(ticket.SubprocessID);
                changesDone = true;
            }

            if (changesDone) db.SaveChanges();
            return converter.TicketToDTO(editTicket);
        }

        [HttpDelete("{id}"), Authorize]
        public void DeleteTicket(int id)
        {
            var removeTicket = dbm.FindTicketByID(id);
            if (removeTicket != null)
            {
                db.Ticket.Remove(removeTicket);
                db.SaveChanges();
            }
        }
    }
}
