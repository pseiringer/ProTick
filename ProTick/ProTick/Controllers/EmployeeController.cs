using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProTick.ResourceDTOs;
using ProTick.Singletons;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]")]
    public class EmployeeController : Controller
    {
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;

        public EmployeeController([FromServices] ProTickDatabaseContext db, [FromServices] IResourceDTOConverter converter, [FromServices] IDatabaseQueryManager dbm)
        {
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
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


        [HttpPost]
        public EmployeeDTO NewEmployee([FromBody] EmployeeDTO e)
        {
            var a = db.Employee.Add(converter.DTOToEmployee(e));

            db.SaveChanges();

            return converter.EmployeeToDTO(a.Entity);
        }

        [HttpPut("{id}")]
        public EmployeeDTO EditEmployee(int id, [FromBody] Employee e)
        {
            var emp = db.Employee.FirstOrDefault(x => x.EmployeeID == e.EmployeeID);

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

            db.SaveChanges();
            return converter.EmployeeToDTO(emp);
        }

        [HttpDelete("{id}")]
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