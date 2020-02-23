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
    public class TeamControllerTests : ProTickSetupFixture
    {

        [Fact]
        public void TestGetTeams()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new TeamController(context, converter, dbm);

                var actual = controller.GetTeams().ToList().Count;

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

                var controller = new TeamController(context, converter, dbm);

                var actual = controller.GetTeams().ToList();

                var expected = DbContextSeeder.GetSeededTeamDTOs(
                        3
                    )
                    .ToList();

                Assert.Equal(expected.Count, actual.Count);

                expected.Should().BeEquivalentTo(actual);
            }

        }

        [Fact]
        public void TestGetTeamsByUsername()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);


                var expectedUsername = "a2";
                var expectedRole = "Admin";

                var controller = new TeamController(context, converter, dbm);
                var user = new ClaimsPrincipal(new ClaimsIdentity(
                        new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, expectedUsername),
                            new Claim(ClaimTypes.Role, expectedRole)
                        }));
                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                };


                Assert.Throws<DatabaseEntryNotFoundException>(() => controller.getTeamsByUsername(expectedUsername));
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

                var expectedUsername = "a2";
                var expectedRole = "Admin";

                var controller = new TeamController(context, converter, dbm);
                var user = new ClaimsPrincipal(new ClaimsIdentity(
                        new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, expectedUsername),
                            new Claim(ClaimTypes.Role, expectedRole)
                        }));
                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                };

                // admin
                var actualAdmin = controller.getTeamsByUsername(expectedUsername).ToList();

                var expectedAdmin = DbContextSeeder.GetSeededTeamDTOs(
                    3
                    )
                    .Where(x => x.TeamID == 2)
                    .ToList();

                Assert.Equal(expectedAdmin.Count, actualAdmin.Count);

                expectedAdmin.Should().BeEquivalentTo(actualAdmin);

                // employee
                expectedUsername = "a1";
                expectedRole = "Employee";
                var user2 = new ClaimsPrincipal(new ClaimsIdentity(
                        new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, expectedUsername),
                            new Claim(ClaimTypes.Role, expectedRole)
                        }));
                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user2 }
                };

                var actualEmployee = controller.getTeamsByUsername(expectedUsername).ToList();
                var expectedEmployee = DbContextSeeder.GetSeededTeamDTOs(
                    1
                    )
                    .ToList();

                Assert.Equal(actualEmployee.Count, expectedEmployee.Count);

                actualEmployee.Should().BeEquivalentTo(expectedEmployee);
            }

        }

        [Fact]
        public void TestGetTicketsByTeamID()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new TeamController(context, converter, dbm);

                var actual = controller.GetTicketsByTeamID(0).ToList().Count;

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

                var controller = new TeamController(context, converter, dbm);

                var expected = DbContextSeeder.GetSeededTicketDTOs(
                    3,
                    DbContextSeeder.GetSeededStateDTOs(3),
                    DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3))
                    )
                    .ToList();

                expected[0].Should().BeEquivalentTo(controller.GetTicketsByTeamID(1).ToList()[0]);
                expected[1].Should().BeEquivalentTo(controller.GetTicketsByTeamID(2).ToList()[0]);
                expected[2].Should().BeEquivalentTo(controller.GetTicketsByTeamID(3).ToList()[0]);
            }

        }

        [Fact]
        public void TestGetEmployeesByTeamID()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new TeamController(context, converter, dbm);

                var actual = controller.GetEmployeesByTeamID(0).ToList().Count;

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

                var controller = new TeamController(context, converter, dbm);

                var expected = DbContextSeeder.GetSeededEmployeeDTOs(
                    3,
                    DbContextSeeder.GetSeededAddressDTOs(3),
                    DbContextSeeder.GetSeededRoleDTOs(3)
                    )
                    .ToList();

                expected[0].Should().BeEquivalentTo(controller.GetEmployeesByTeamID(1).ToList()[0]);
                expected[1].Should().BeEquivalentTo(controller.GetEmployeesByTeamID(2).ToList()[0]);
                expected[2].Should().BeEquivalentTo(controller.GetEmployeesByTeamID(3).ToList()[0]);
            }

        }

        [Fact]
        public void TestGetEmployeeTeamsByTeamID()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new TeamController(context, converter, dbm);

                var actual = controller.GetEmployeeTeamsByTeamID(0).ToList().Count;

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

                var controller = new TeamController(context, converter, dbm);

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

                expected[0].Should().BeEquivalentTo(controller.GetEmployeeTeamsByTeamID(1).ToList()[0]);
                expected[1].Should().BeEquivalentTo(controller.GetEmployeeTeamsByTeamID(2).ToList()[0]);
                expected[2].Should().BeEquivalentTo(controller.GetEmployeeTeamsByTeamID(3).ToList()[0]);
            }

        }

        [Fact]
        public void TestGetTeam()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new TeamController(context, converter, dbm);

                Assert.Throws<DatabaseEntryNotFoundException>(() => controller.GetTeam(0));
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

                var controller = new TeamController(context, converter, dbm);

                var expected = DbContextSeeder.GetSeededTeamDTOs(
                        3
                    )
                    .ToList();

                expected[0].Should().BeEquivalentTo(controller.GetTeam(1));
                expected[1].Should().BeEquivalentTo(controller.GetTeam(2));
                expected[2].Should().BeEquivalentTo(controller.GetTeam(3));
            }

        }

        [Fact]
        public void TestPostTeam()
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

                var controller = new TeamController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededTeamDTOs(4).ToList();
                var expectedTeams = DbContextSeeder.GetSeededTeams(4).ToList();
                //dtos[3].Abbreviation = "test";
                //dtos[3].Description = "test";

                var actual = controller.PostTeam(dtos[3]);

                actual.Should().BeEquivalentTo(dtos[3]);

                var actualCount = context.Team.ToList().Count;
                Assert.Equal(4, actualCount);

                context.Team.FirstOrDefault(x => x.TeamID == 4).Should().BeEquivalentTo(expectedTeams[3]);

            }


        }

        [Fact]
        public void TestPutTeam()
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

                var controller = new TeamController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededTeamDTOs(
                    3
                    )
                    .ToList();

                var expectedText = "test";
                var expectedID = 1;

                dtos[2].Abbreviation = expectedText;
                dtos[2].Description = expectedText;

                var actual = controller.PutTeam(3, dtos[2]);

                actual.Should().BeEquivalentTo(dtos[2]);

                var actualCount = context.Team.ToList().Count;
                Assert.Equal(3, actualCount);

                var team = context.Team
                    .FirstOrDefault(x => x.TeamID == 3);

                Assert.True(team != null);

                if (team != null)
                {
                    Assert.Equal(team.Description, expectedText);
                    Assert.Equal(team.Abbreviation, expectedText);
                }
            }
        }

        [Fact]
        public void TestDeleteTeam()
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

                var controller = new TeamController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededTeamDTOs(
                    3
                    )
                    .ToList();

                controller.DeleteTeam(dtos[2].TeamID);

                var actualCount = context.Team.ToList().Count;
                Assert.Equal(2, actualCount);
                Assert.True(context.Team.FirstOrDefault(x => x.TeamID == 3) == null);
                Assert.True(context.Team.FirstOrDefault(x => x.TeamID != 3) != null);

                controller.DeleteTeam(dtos[1].TeamID);

                actualCount = context.Team.ToList().Count;
                Assert.Equal(1, actualCount);
                Assert.True(context.Team.FirstOrDefault(x => x.TeamID == 2) == null);

            }


        }
    }
}
