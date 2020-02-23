using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProTick.Controllers;
using ProTick.Exceptions;
using ProTick.ResourceDTOs;
using ProTick.Singletons;
using ProTickDatabase;
using ProTickTest.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace ProTickTest
{
    public class SubprocessControllerTests : ProTickSetupFixture
    {

        [Fact]
        public void TestGetSubprocesses()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new SubprocessController(context, converter, dbm);

                var actual = controller.GetSubprocesses().ToList().Count;

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

                var controller = new SubprocessController(context, converter, dbm);

                var actual = controller.GetSubprocesses().ToList();

                var expected = DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                    )
                    .ToList();

                Assert.Equal(expected.Count, actual.Count);

                expected.Should().BeEquivalentTo(actual);
            }

        }

        [Fact]
        public void TestGetSubprocess()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new SubprocessController(context, converter, dbm);

                Assert.Throws<DatabaseEntryNotFoundException>(() => controller.GetSubprocess(0));
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

                var controller = new SubprocessController(context, converter, dbm);

                var expected = DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                    )
                    .ToList();

                expected[0].Should().BeEquivalentTo(controller.GetSubprocess(1));
                expected[1].Should().BeEquivalentTo(controller.GetSubprocess(2));
                expected[2].Should().BeEquivalentTo(controller.GetSubprocess(3));
            }

        }

        

        [Fact]
        public void TestGetChildrenOfSubprocess()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new SubprocessController(context, converter, dbm);

                var actual = controller.GetChildrenOfSubprocess(1).ToList().Count;

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

                var controller = new SubprocessController(context, converter, dbm);
                
                var expected = DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                    )
                    .ToList();

                expected[1].Should().BeEquivalentTo(controller.GetChildrenOfSubprocess(1).ToList()[0]);
                expected[2].Should().BeEquivalentTo(controller.GetChildrenOfSubprocess(2).ToList()[0]);
                (new List<SubprocessDTO>() { null }).Should().BeEquivalentTo(controller.GetChildrenOfSubprocess(3).ToList());
            }

        }


        [Fact]
        public void TestGetTicketsOfSubprocess()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new SubprocessController(context, converter, dbm);

                var actual = controller.GetTicketsOfSubprocess(1).ToList().Count;

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

                var controller = new SubprocessController(context, converter, dbm);

                var expected = DbContextSeeder.GetSeededTicketDTOs(
                        3,
                        DbContextSeeder.GetSeededStateDTOs(3),
                        DbContextSeeder.GetSeededSubprocessDTOs(
                            3,
                            DbContextSeeder.GetSeededProcessDTOs(3),
                            DbContextSeeder.GetSeededTeamDTOs(3)
                        )
                    )
                    .ToList();

                expected[0].Should().BeEquivalentTo(controller.GetTicketsOfSubprocess(1).ToList()[0]);
                expected[1].Should().BeEquivalentTo(controller.GetTicketsOfSubprocess(2).ToList()[0]);
                expected[2].Should().BeEquivalentTo(controller.GetTicketsOfSubprocess(3).ToList()[0]);
            }

        }

        [Fact]
        public void TestPostSubprocess()
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

                var controller = new SubprocessController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededSubprocessDTOs(
                        4,
                        DbContextSeeder.GetSeededProcessDTOs(4),
                        DbContextSeeder.GetSeededTeamDTOs(4)
                    )
                    .ToList();
                var expected = DbContextSeeder.GetSeededSubprocesses(
                        4,
                        DbContextSeeder.GetSeededProcesses(4),
                        DbContextSeeder.GetSeededTeams(4)
                    )
                    .ToList();
                dtos[3].ProcessID = 1;
                dtos[3].TeamID = 1;
                expected[3].Process = DbContextSeeder.GetSeededProcesses(1)[0];
                expected[3].Team = DbContextSeeder.GetSeededTeams(1)[0];

                var actual = controller.PostSubprocess(dtos[3]);

                actual.Should().BeEquivalentTo(dtos[3]);

                var actualCount = context.Subprocess.ToList().Count;
                Assert.Equal(4, actualCount);

                context.Subprocess.FirstOrDefault(x => x.SubprocessID == 4).Should().BeEquivalentTo(expected[3]);

            }


        }

        [Fact]
        public void TestPutSubprocess()
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

                var controller = new SubprocessController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                    )
                    .ToList();

                var expectedText = "test";
                var expectedID = 1;

                dtos[2].TeamID = expectedID;
                dtos[2].ProcessID = expectedID;
                dtos[2].Description = expectedText;

                var actual = controller.PutSubprocess(3, dtos[2]);

                actual.Should().BeEquivalentTo(dtos[2]);

                var actualCount = context.Subprocess.ToList().Count;
                Assert.Equal(3, actualCount);

                var actualInDb = context.Subprocess
                    .Include(x => x.Team)
                    .Include(x => x.Process)
                    .FirstOrDefault(x => x.SubprocessID == 3);

                Assert.True(actualInDb != null);

                if (actualInDb != null)
                {
                    Assert.Equal(actualInDb.Team.TeamID, expectedID);
                    Assert.Equal(actualInDb.Process.ProcessID, expectedID);
                    Assert.Equal(actualInDb.Description, expectedText);
                }
            }
        }

        [Fact]
        public void TestDeleteSubprocess()
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

                var controller = new SubprocessController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                    )
                    .ToList();

                controller.DeleteSubprocess(dtos[2].SubprocessID);

                var actualCount = context.Subprocess.ToList().Count;
                Assert.Equal(2, actualCount);
                Assert.True(context.Subprocess.FirstOrDefault(x => x.SubprocessID == 3) == null);
                Assert.True(context.Subprocess.FirstOrDefault(x => x.SubprocessID != 3) != null);

                controller.DeleteSubprocess(dtos[1].SubprocessID);

                actualCount = context.Subprocess.ToList().Count;
                Assert.Equal(1, actualCount);
                Assert.True(context.Subprocess.FirstOrDefault(x => x.SubprocessID == 2) == null);

            }


        }
    }
}
