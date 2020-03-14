using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProTick.Controllers;
using ProTick.Exceptions;
using ProTick.Singletons;
using ProTickDatabase;
using ProTickTest.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace ProTickTest
{
    public class EmployeeControllerTests : ProTickSetupFixture
    {

        [Fact]
        public void TestGetEmployees()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);
                var hasher = new Hasher();

                var controller = new EmployeeController(context, converter, dbm, hasher);

                var actual = controller.GetEmployees().ToList().Count;

                int expected = 0;

                Assert.Equal(expected, actual);
            }

            // seeding DB
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                DbContextSeeder.SeedFull(context);
            }

            // db is Test-Ready
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);
                var hasher = new Hasher();

                var controller = new EmployeeController(context, converter, dbm, hasher);

                var actual = controller.GetEmployees().ToList();

                var expected = DbContextSeeder.GetSeededEmployeeDTOs(
                        3,
                        DbContextSeeder.GetSeededAddressDTOs(3),
                        DbContextSeeder.GetSeededRoleDTOs(3)
                    )
                    .ToList();

                Assert.Equal(expected.Count, actual.Count);

                expected.Should().BeEquivalentTo(actual);
            }

        }

        [Fact]
        public void TestGetEmployee()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);
                var hasher = new Hasher();

                var controller = new EmployeeController(context, converter, dbm, hasher);

                Assert.Throws<DatabaseEntryNotFoundException>(() => controller.GetEmployee(0));
            }

            // seeding DB
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                DbContextSeeder.SeedFull(context);
            }

            // db is Test-Ready
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);
                var hasher = new Hasher();

                var controller = new EmployeeController(context, converter, dbm, hasher);

                var expected = DbContextSeeder.GetSeededEmployeeDTOs(
                        3,
                        DbContextSeeder.GetSeededAddressDTOs(3),
                        DbContextSeeder.GetSeededRoleDTOs(3)
                    )
                    .ToList();

                expected[0].Should().BeEquivalentTo(controller.GetEmployee(1));
                expected[1].Should().BeEquivalentTo(controller.GetEmployee(2));
                expected[2].Should().BeEquivalentTo(controller.GetEmployee(3));
            }

        }

        [Fact]
        public void TestGetTeamsByEmployeeID()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);
                var hasher = new Hasher();

                var controller = new EmployeeController(context, converter, dbm, hasher);

                var actual = controller.GetTeamsByEmployeeID(1).ToList().Count;

                int expected = 0;

                Assert.Equal(expected, actual);
            }

            // seeding DB
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                DbContextSeeder.SeedFull(context);
            }

            // db is Test-Ready
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);
                var hasher = new Hasher();

                var controller = new EmployeeController(context, converter, dbm, hasher);

                var expected = DbContextSeeder.GetSeededTeamDTOs(
                    3
                    )
                    .ToList();

                expected[0].Should().BeEquivalentTo(
                    controller.GetTeamsByEmployeeID(1).ToList()[0]);
                expected[1].Should().BeEquivalentTo(
                    controller.GetTeamsByEmployeeID(2).ToList()[0]);
                expected[2].Should().BeEquivalentTo(
                    controller.GetTeamsByEmployeeID(3).ToList()[0]);
            }

        }

        [Fact]
        public void TestPostEmployee()
        {
            // seeding DB
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                DbContextSeeder.SeedFull(context);
            }

            // db is Test-Ready
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);
                var hasher = new Hasher();

                var controller = new EmployeeController(context, converter, dbm, hasher);

                var dtos = DbContextSeeder.GetSeededEmployeeDTOs(
                        4,
                        DbContextSeeder.GetSeededAddressDTOs(4),
                        DbContextSeeder.GetSeededRoleDTOs(4)
                    )
                    .ToList();
                var expected = DbContextSeeder.GetSeededEmployees(
                        4,
                        DbContextSeeder.GetSeededAddresses(4),
                        DbContextSeeder.GetSeededRoles(4)
                    )
                    .ToList();

                dtos[3].AddressID = 1;
                dtos[3].RoleID = 1;

                expected[3].Address = DbContextSeeder.GetSeededAddresses(1)[0];
                expected[3].Role = DbContextSeeder.GetSeededRoles(1)[0];
                expected[3].Password = "67d10c8324b1e0ae4f97cba25b3bc43a9a9104b0f945750b4cf6edc14669e009";
                

                var actual = controller.PostEmployee(dtos[3]);

                actual.Should().BeEquivalentTo(dtos[3]);

                var actualCount = context.Employee.ToList().Count;
                Assert.Equal(4, actualCount);

                context.Employee.FirstOrDefault(x => x.EmployeeID == 4).Should().BeEquivalentTo(expected[3]);

            }


        }

        [Fact]
        public void TestPutEmployee()
        {
            // seeding DB
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                DbContextSeeder.SeedFull(context);
            }

            // db is Test-Ready
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);
                var hasher = new Hasher();

                var controller = new EmployeeController(context, converter, dbm, hasher);


                var dtos = DbContextSeeder.GetSeededEmployeeDTOs(
                        3,
                        DbContextSeeder.GetSeededAddressDTOs(3),
                        DbContextSeeder.GetSeededRoleDTOs(3)
                    )
                    .ToList();

                var expectedText = "test";
                var expectedID = 1;
                var expectedDate = DateTime.Parse("01/01/2020");

                dtos[2].AddressID = expectedID;
                dtos[2].DateOfBirth = expectedDate.ToShortDateString();
                dtos[2].Email = expectedText;
                dtos[2].FirstName = expectedText;
                dtos[2].HireDate = expectedDate.ToShortDateString();
                dtos[2].LastName = expectedText;
                dtos[2].PhoneNumber = expectedText;
                dtos[2].RoleID = expectedID;
                dtos[2].Username = expectedText;

                var actual = controller.PutEmployee(3, dtos[2]);

                actual.Should().BeEquivalentTo(dtos[2]);

                var actualCount = context.Employee.ToList().Count;
                Assert.Equal(3, actualCount);

                var actualInDb = context.Employee
                    .Include(x => x.Address)
                    .Include(x => x.Role)
                    .FirstOrDefault(x => x.EmployeeID == 3);

                Assert.True(actualInDb != null);

                if (actualInDb != null)
                {
                    Assert.Equal(actualInDb.Address.AddressID, expectedID);
                    Assert.Equal(actualInDb.DateOfBirth, expectedDate);
                    Assert.Equal(actualInDb.Email, expectedText);
                    Assert.Equal(actualInDb.FirstName, expectedText);
                    Assert.Equal(actualInDb.HireDate, expectedDate);
                    Assert.Equal(actualInDb.LastName, expectedText);
                    Assert.Equal(actualInDb.PhoneNumber, expectedText);
                    Assert.Equal(actualInDb.Role.RoleID, expectedID);
                    Assert.Equal(actualInDb.Username, expectedText);
                }
            }
        }

        [Fact]
        public void TestDeleteEmployee()
        {
            // seeding DB
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                DbContextSeeder.SeedFull(context);
            }

            // db is Test-Ready
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);
                var hasher = new Hasher();

                var controller = new EmployeeController(context, converter, dbm, hasher);

                var dtos = DbContextSeeder.GetSeededEmployeeDTOs(
                        3,
                        DbContextSeeder.GetSeededAddressDTOs(3),
                        DbContextSeeder.GetSeededRoleDTOs(3)
                    )
                    .ToList();

                controller.DeleteEmployee(dtos[2].EmployeeID);

                var actualCount = context.Employee.ToList().Count;
                Assert.Equal(2, actualCount);
                Assert.True(context.Employee.FirstOrDefault(x => x.EmployeeID == 3) == null);
                Assert.True(context.Employee.FirstOrDefault(x => x.EmployeeID != 3) != null);

                controller.DeleteEmployee(dtos[1].EmployeeID);

                actualCount = context.Employee.ToList().Count;
                Assert.Equal(1, actualCount);
                Assert.True(context.Employee.FirstOrDefault(x => x.EmployeeID == 2) == null);

            }


        }
    }
}
