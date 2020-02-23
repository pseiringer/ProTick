using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProTick.Controllers;
using ProTick.Exceptions;
using ProTick.Singletons;
using ProTickDatabase;
using ProTickTest.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace ProTickTest
{
    public class EmployeeTeamControllerTests : ProTickSetupFixture
    {

        [Fact]
        public void TestGetEmployeeTeams()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new EmployeeTeamController(context, converter, dbm);

                var actual = controller.GetEmployeeTeams().ToList().Count;

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

                var controller = new EmployeeTeamController(context, converter, dbm);

                var actual = controller.GetEmployeeTeams().ToList();

                var expected = DbContextSeeder.GetSeededEmployeeTeamDTOs(
                        3,
                        DbContextSeeder.GetSeededEmployeeDTOs(
                            3,
                            DbContextSeeder.GetSeededAddressDTOs(3),
                            DbContextSeeder.GetSeededRoleDTOs(3)
                        ),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                        )
                    .ToList();

                Assert.Equal(expected.Count, actual.Count);

                expected.Should().BeEquivalentTo(actual);
            }

        }

        [Fact]
        public void TestGetEmployeeTeam()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new EmployeeTeamController(context, converter, dbm);

                Assert.Throws<DatabaseEntryNotFoundException>(() => controller.GetEmployeeTeam(0));
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

                var controller = new EmployeeTeamController(context, converter, dbm);

                var expected = DbContextSeeder.GetSeededEmployeeTeamDTOs(
                        3,
                        DbContextSeeder.GetSeededEmployeeDTOs(
                            3,
                            DbContextSeeder.GetSeededAddressDTOs(3),
                            DbContextSeeder.GetSeededRoleDTOs(3)
                        ),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                        )
                    .ToList();

                expected[0].Should().BeEquivalentTo(controller.GetEmployeeTeam(1));
                expected[1].Should().BeEquivalentTo(controller.GetEmployeeTeam(2));
                expected[2].Should().BeEquivalentTo(controller.GetEmployeeTeam(3));
            }

        }

        [Fact]
        public void TestPostEmployeeTeam()
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

                var controller = new EmployeeTeamController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededEmployeeTeamDTOs(
                        4,
                        DbContextSeeder.GetSeededEmployeeDTOs(
                            4,
                            DbContextSeeder.GetSeededAddressDTOs(4),
                            DbContextSeeder.GetSeededRoleDTOs(4)
                        ),
                        DbContextSeeder.GetSeededTeamDTOs(4)
                        )
                    .ToList();
                var expected = DbContextSeeder.GetSeededEmployeeTeams(
                        4,
                        DbContextSeeder.GetSeededEmployees(
                            4,
                            DbContextSeeder.GetSeededAddresses(4),
                            DbContextSeeder.GetSeededRoles(4)
                        ),
                        DbContextSeeder.GetSeededTeams(4)
                        )
                    .ToList();

                var expectedID = 1;
                dtos[3].TeamID = expectedID;
                dtos[3].EmployeeID = expectedID;
                expected[3].Team = DbContextSeeder.GetSeededTeams(1)[0];
                expected[3].Employee = DbContextSeeder.GetSeededEmployees(
                        1,
                        DbContextSeeder.GetSeededAddresses(1),
                        DbContextSeeder.GetSeededRoles(1)
                    )[0];

                var actual = controller.PostEmployeeTeam(dtos[3]);

                actual.Should().BeEquivalentTo(dtos[3]);

                var actualCount = context.EmployeeTeam.ToList().Count;
                Assert.Equal(4, actualCount);

                context.EmployeeTeam.FirstOrDefault(x => x.EmployeeTeamID == 4).Should().BeEquivalentTo(expected[3]);

            }


        }

        [Fact]
        public void TestPutEmployeeTeam()
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

                var controller = new EmployeeTeamController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededEmployeeTeamDTOs(
                        3,
                        DbContextSeeder.GetSeededEmployeeDTOs(
                            3,
                            DbContextSeeder.GetSeededAddressDTOs(3),
                            DbContextSeeder.GetSeededRoleDTOs(3)
                        ),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                        )
                    .ToList();

                var expectedText = "test";
                var expectedID = 1;

                dtos[2].TeamID = expectedID;
                dtos[2].EmployeeID = expectedID;

                var actual = controller.PutEmployeeTeam(3, dtos[2]);

                actual.Should().BeEquivalentTo(dtos[2]);

                var actualCount = context.EmployeeTeam.ToList().Count;
                Assert.Equal(3, actualCount);

                var actualInDb = context.EmployeeTeam
                    .Include(x => x.Team)
                    .Include(x => x.Employee)
                    .FirstOrDefault(x => x.EmployeeTeamID == 3);

                Assert.True(actualInDb != null);

                if (actualInDb != null)
                {
                    Assert.Equal(actualInDb.Team.TeamID, expectedID);
                    Assert.Equal(actualInDb.Employee.EmployeeID, expectedID);
                }
            }
        }

        [Fact]
        public void TestDeleteEmployeeTeam()
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

                var controller = new EmployeeTeamController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededEmployeeTeamDTOs(
                        3,
                        DbContextSeeder.GetSeededEmployeeDTOs(
                            3,
                            DbContextSeeder.GetSeededAddressDTOs(3),
                            DbContextSeeder.GetSeededRoleDTOs(3)
                        ),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                        )
                    .ToList();

                controller.DeleteEmployeeTeam(dtos[2].EmployeeTeamID);

                var actualCount = context.EmployeeTeam.ToList().Count;
                Assert.Equal(2, actualCount);
                Assert.True(context.EmployeeTeam.FirstOrDefault(x => x.EmployeeTeamID == 3) == null);
                Assert.True(context.EmployeeTeam.FirstOrDefault(x => x.EmployeeTeamID != 3) != null);

                controller.DeleteEmployeeTeam(dtos[1].EmployeeTeamID);

                actualCount = context.EmployeeTeam.ToList().Count;
                Assert.Equal(1, actualCount);
                Assert.True(context.EmployeeTeam.FirstOrDefault(x => x.EmployeeTeamID == 2) == null);

            }
        }


        [Fact]
        public void TestDeleteEmployeeTeamByTeamAndEmpId()
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

                var controller = new EmployeeTeamController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededEmployeeTeamDTOs(
                        3,
                        DbContextSeeder.GetSeededEmployeeDTOs(
                            3,
                            DbContextSeeder.GetSeededAddressDTOs(3),
                            DbContextSeeder.GetSeededRoleDTOs(3)
                        ),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                        )
                    .ToList();

                controller.DeleteEmployeeTeamByTeamAndEmpId(dtos[2].TeamID, dtos[2].EmployeeID);

                var actualCount = context.EmployeeTeam.ToList().Count;
                Assert.Equal(2, actualCount);
                Assert.True(context.EmployeeTeam.FirstOrDefault(x => x.EmployeeTeamID == 3) == null);
                Assert.True(context.EmployeeTeam.FirstOrDefault(x => x.EmployeeTeamID != 3) != null);

                controller.DeleteEmployeeTeamByTeamAndEmpId(dtos[2].TeamID, dtos[2].EmployeeID);

                actualCount = context.EmployeeTeam.ToList().Count;
                Assert.Equal(2, actualCount);
                Assert.True(context.EmployeeTeam.FirstOrDefault(x => x.EmployeeTeamID == 3) == null);
                Assert.True(context.EmployeeTeam.FirstOrDefault(x => x.EmployeeTeamID != 3) != null);

                controller.DeleteEmployeeTeamByTeamAndEmpId(dtos[1].TeamID, dtos[1].EmployeeID);

                actualCount = context.EmployeeTeam.ToList().Count;
                Assert.Equal(1, actualCount);
                Assert.True(context.EmployeeTeam.FirstOrDefault(x => x.EmployeeTeamID == 2) == null);
                Assert.True(context.EmployeeTeam.FirstOrDefault(x => x.EmployeeTeamID != 2) != null);


            }
        }
    }
}
