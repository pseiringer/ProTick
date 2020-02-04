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
    public class TestTicketController : ProTickSetupFixture
    {
        [Fact]
        public void TestGetAll()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new TicketController(context, converter, dbm);

                var actual = controller.GetAllTickets().ToList().Count;

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

                var controller = new TicketController(context, converter, dbm);

                var actual = controller.GetAllTickets().ToList();

                var expected = DbContextSeeder.GetSeededTicketDTOs(
                    3,
                    DbContextSeeder.GetSeededStateDTOs(3),
                    DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3))
                    )
                    .ToList();

                Assert.Equal(expected.Count, actual.Count);

                expected.Should().BeEquivalentTo(actual);
            }

        }

        [Fact]
        public void TestGetTicketsByUsername()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);


                var expectedUsername = "a2";
                var expectedRole = "Admin";

                var controller = new TicketController(context, converter, dbm);
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

                
                Assert.Throws<DatabaseEntryNotFoundException>(() => controller.GetTicketsByUsername(expectedUsername));

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

                var controller = new TicketController(context, converter, dbm);
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
                var actualAdmin = controller.GetTicketsByUsername(expectedUsername).ToList();

                var expectedAdmin = DbContextSeeder.GetSeededTicketDTOs(
                    3,
                    DbContextSeeder.GetSeededStateDTOs(3),
                    DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3))
                    )
                    .Where(x => x.TicketID == 2)
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

                var actualEmployee = controller.GetTicketsByUsername(expectedUsername).ToList();
                var expectedEmployee = DbContextSeeder.GetSeededTicketDTOs(
                    1,
                    DbContextSeeder.GetSeededStateDTOs(1),
                    DbContextSeeder.GetSeededSubprocessDTOs(
                        1,
                        DbContextSeeder.GetSeededProcessDTOs(1),
                        DbContextSeeder.GetSeededTeamDTOs(1))
                    )
                    .ToList();

                Assert.Equal(actualEmployee.Count, expectedEmployee.Count);

                actualEmployee.Should().BeEquivalentTo(expectedEmployee);
            }

        }

        [Fact]
        public void TestGetTicketByID()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new TicketController(context, converter, dbm);

                Assert.Throws<DatabaseEntryNotFoundException>(() => controller.GetTicketByID(0));
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

                var controller = new TicketController(context, converter, dbm);

                var expected = DbContextSeeder.GetSeededTicketDTOs(
                    3,
                    DbContextSeeder.GetSeededStateDTOs(3),
                    DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3))
                    )
                    .ToList();

                expected[0].Should().BeEquivalentTo(controller.GetTicketByID(1));
                expected[1].Should().BeEquivalentTo(controller.GetTicketByID(2));
                expected[2].Should().BeEquivalentTo(controller.GetTicketByID(3));
            }

        }

        [Fact]
        public void TestPostTicket()
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

                var controller = new TicketController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededTicketDTOs(
                    4,
                    DbContextSeeder.GetSeededStateDTOs(4),
                    DbContextSeeder.GetSeededSubprocessDTOs(
                        4,
                        DbContextSeeder.GetSeededProcessDTOs(4),
                        DbContextSeeder.GetSeededTeamDTOs(4))
                    )
                    .ToList();
                dtos[3].StateID = 1;
                dtos[3].SubprocessID = 1;

                var actual = controller.PostTicket(dtos[3]);

                actual.Should().BeEquivalentTo(dtos[3]);

                var actualCount = context.Ticket.ToList().Count;
                Assert.Equal(4, actualCount);

                Assert.True(context.Ticket.FirstOrDefault(x => x.TicketID == 4) != null);
            }


        }


        [Fact]
        public void TestPutTicket()
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

                var controller = new TicketController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededTicketDTOs(
                    3,
                    DbContextSeeder.GetSeededStateDTOs(3),
                    DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3))
                    )
                    .ToList();

                var expectedText = "test";
                var expectedID = 1;

                dtos[2].StateID = expectedID;
                dtos[2].SubprocessID = expectedID;
                dtos[2].Description = expectedText;
                dtos[2].Note = expectedText;

                var actual = controller.PutTicket(3, dtos[2]);

                actual.Should().BeEquivalentTo(dtos[2]);

                var actualCount = context.Ticket.ToList().Count;
                Assert.Equal(3, actualCount);

                var ticket = context.Ticket
                    .Include(x => x.State)
                    .Include(x => x.Subprocess)
                    .FirstOrDefault(x => x.TicketID == 3);

                Assert.True(ticket != null);

                if (ticket != null)
                {
                    Assert.Equal(ticket.Note, expectedText);
                    Assert.Equal(ticket.Description, expectedText);
                    Assert.Equal(ticket.State.StateID, expectedID);
                    Assert.Equal(ticket.Subprocess.SubprocessID, expectedID);
                }
            }
        }

        [Fact]
        public void TestDeleteTicket()
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

                var controller = new TicketController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededTicketDTOs(
                    3,
                    DbContextSeeder.GetSeededStateDTOs(3),
                    DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3))
                    )
                    .ToList();

                controller.DeleteTicket(dtos[2].TicketID);

                var actualCount = context.Ticket.ToList().Count;
                Assert.Equal(2, actualCount);
                Assert.True(context.Ticket.FirstOrDefault(x => x.TicketID == 3) == null);

                controller.DeleteTicket(dtos[1].TicketID);

                actualCount = context.Ticket.ToList().Count;
                Assert.Equal(1, actualCount);
                Assert.True(context.Ticket.FirstOrDefault(x => x.TicketID == 2) == null);

            }


        }
    }
}
