using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProTick.ResourceDTOs;
using ProTick.Singletons;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]"), Authorize]
    public class EmployeeController : Controller
    {
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;
        private IHasher hasher;

        public EmployeeController([FromServices] ProTickDatabaseContext db, [FromServices] IResourceDTOConverter converter, [FromServices] IDatabaseQueryManager dbm, [FromServices] IHasher hasher)
        {
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
            this.hasher = hasher;
        }

        [HttpGet]
        public IEnumerable<EmployeeDTO> GetEmployees()
        {
            return dbm.FindAllEmployees(true).Select(x => converter.EmployeeToDTO(x)).ToList();
        }

        [HttpGet("{id}")]
        public EmployeeDTO GetEmployee(int id)
        {
            return converter.EmployeeToDTO(dbm.FindEmployeeByID(id));
        }

        [HttpGet("{id}/Teams")]
        public IEnumerable<TeamDTO> GetTeamsByEmployeeID(int id)
        {
            return dbm.FindAllEmployeeTeams(true).Where(x => x.Employee.EmployeeID == id).SelectMany(x => dbm.FindAllTeams(true).Where(y => x.Team.TeamID == y.TeamID)).Distinct().Select(x => converter.TeamToDTO(x)).ToList();
        }

        [HttpPost, Authorize(Roles = StaticRoles.Admin)]
        public EmployeeDTO PostEmployee([FromBody] EmployeeDTO e)
        {
            Console.WriteLine(e.AddressID);
            Console.WriteLine(e.FirstName);
            Console.WriteLine(e.LastName);
            var newEmp = converter.DTOToEmployee(e);
            var passwordString = (e.FirstName.Substring(0, 1));
            if (e.LastName.Length >= 15)
                passwordString += e.LastName.Substring(0, 15);
            else
                passwordString += e.LastName;

            newEmp.Password = hasher.HashPassword(passwordString.ToLower());
            var a = db.Employee.Add(newEmp);

            db.SaveChanges();

            return converter.EmployeeToDTO(a.Entity);
        }

        [HttpPut("{id}"), Authorize(Roles = StaticRoles.Admin)]
        public EmployeeDTO PutEmployee(int id, [FromBody] EmployeeDTO changedE)
        {
            var emp = db.Employee.FirstOrDefault(x => x.EmployeeID == changedE.EmployeeID);
            var e = converter.DTOToEmployee(changedE);

            if (emp.FirstName != e.FirstName)
                emp.FirstName = e.FirstName;
            if (emp.LastName != e.LastName)
                emp.LastName = e.LastName;
            if (emp.Email != e.Email)
                emp.Email = e.Email;
            if (emp.PhoneNumber != e.PhoneNumber)
                emp.PhoneNumber = e.PhoneNumber;
            if (emp.HireDate != e.HireDate)
                emp.HireDate = e.HireDate;
            if (emp.DateOfBirth != e.DateOfBirth)
                emp.DateOfBirth = e.DateOfBirth;
            if (emp.Username != e.Username)
                emp.Username = e.Username;
            if (emp.Password != e.Password)
                emp.Password = e.Password;
            if (emp.Address != e.Address)
                emp.Address = e.Address;
            if (emp.Role != e.Role)
                emp.Role = e.Role;

            db.SaveChanges();
            Console.WriteLine(emp == null);
            return converter.EmployeeToDTO(emp);
        }

        [HttpDelete("{id}"), Authorize(Roles = StaticRoles.Admin)]
        public void DeleteEmployee(int id)
        {
            var emp = db.Employee.Include(x => x.Address).First(x => x.EmployeeID == id);
            var et = dbm.FindEmployeeTeamsByEmployeeID(id);
            for (int i = 0; i < et.Count; i++)
            {
                db.EmployeeTeam.Remove(et[i]);
            }

            var add = dbm.FindAddressByID(emp.Address.AddressID);

            db.Employee.Remove(emp);

            //db.SaveChanges();
            
            if(db.Employee.Include(x => x.Address).Where(x => x.Address.AddressID == add.AddressID).ToList().Count() <= 1)
            {
                db.Address.Remove(add);
            }

            db.SaveChanges();


        }
    }
}