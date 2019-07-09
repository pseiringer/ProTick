﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProTick.ResourceDTOs;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]")]
    public class EmployeeController : Controller
    {
        private ResourceDTOConverter converter = new ResourceDTOConverter(null);

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public EmployeeDTO GetEmployee([FromServices] ProTickDatabaseContext db, int id)
        {
            return converter.EmployeeToDTO(db.Employee.First(x => x.EmployeeID == id));
        }

        [HttpGet]
        public IEnumerable<EmployeeDTO> GetEmployees([FromServices] ProTickDatabaseContext db)
        {
            return db.Employee.Select(x => converter.EmployeeToDTO(x)).ToList();
        }

        [HttpPost("{e}")]
        public EmployeeDTO NewEmployee([FromServices] ProTickDatabaseContext db, Employee e)
        {
            var a = db.Employee.Add(e);

            db.SaveChanges();

            return converter.EmployeeToDTO(a.Entity);
        }

        [HttpPut("{id}")]
        public EmployeeDTO EditEmployee([FromServices] ProTickDatabaseContext db, int id, [FromBody] Employee e)
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
        public void DeleteEmployee([FromServices] ProTickDatabaseContext db, int id)
        {
            db.Employee.Remove(db.Employee.First(x => x.EmployeeID == id));
            db.SaveChanges();
        }
    }
}