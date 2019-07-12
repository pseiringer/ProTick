using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        

        [HttpGet("{id}")]
        public EmployeeDTO GetEmployee(int id)
        {
            return converter.EmployeeToDTO(dbm.FindEmployeeByID(id));

        }

        [HttpGet]
        public IEnumerable<EmployeeDTO> GetEmployees()
        {
            return dbm.FindAllEmployees(true).Select(x => converter.EmployeeToDTO(x)).ToList();
        }

        [HttpPost]
        public EmployeeDTO NewEmployee([FromBody] EmployeeDTO e)
        {
            var a = db.Employee.Add(converter.DTOToEmployee( e));

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
            if (emp.HireDate != e.HireDate)
                emp.HireDate = e.HireDate;
            if (emp.DateOfBirth != e.DateOfBirth)
                emp.DateOfBirth = e.DateOfBirth;
            if (emp.Address != e.Address)
                emp.Address = e.Address;

            db.SaveChanges();
            return converter.EmployeeToDTO(emp);
        }

        [HttpDelete("{id}")]
        public void DeleteEmployee(int id)
        {
            db.Employee.Remove(db.Employee.First(x => x.EmployeeID == id));
            db.SaveChanges();
        }
    }
}