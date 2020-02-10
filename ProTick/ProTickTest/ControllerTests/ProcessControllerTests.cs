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
    public class ProcessControllerTests : ProTickSetupFixture
    {

        [Fact]
        public void TestGetProcesss()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new ProcessController(context, converter, dbm);

                var actual = controller.GetProcesses().ToList().Count;

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

                var controller = new ProcessController(context, converter, dbm);

                var actual = controller.GetProcesses().ToList();

                var expected = DbContextSeeder.GetSeededProcessDTOs(
                        3
                    )
                    .ToList();

                Assert.Equal(expected.Count, actual.Count);

                expected.Should().BeEquivalentTo(actual);
            }

        }

        [Fact]
        public void TestGetProcess()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new ProcessController(context, converter, dbm);

                Assert.Throws<DatabaseEntryNotFoundException>(() => controller.GetProcess(0));
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

                var controller = new ProcessController(context, converter, dbm);

                var expected = DbContextSeeder.GetSeededProcessDTOs(
                        3
                    )
                    .ToList();

                expected[0].Should().BeEquivalentTo(controller.GetProcess(1));
                expected[1].Should().BeEquivalentTo(controller.GetProcess(2));
                expected[2].Should().BeEquivalentTo(controller.GetProcess(3));
            }

        }

        [Fact]
        public void TestGetProcessesWithSubprocess()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new ProcessController(context, converter, dbm);

                var actual = controller.GetProcessesWithSubprocess(true).ToList().Count;
                var actual2 = controller.GetProcessesWithSubprocess(false).ToList().Count;

                int expected = 0;

                Assert.Equal(expected, actual);
                Assert.Equal(expected, actual2);
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

                var controller = new ProcessController(context, converter, dbm);

                var actual = controller.GetProcessesWithSubprocess(true).ToList();
                var actual2 = controller.GetProcessesWithSubprocess(false).ToList().Count;


                var expected = DbContextSeeder.GetSeededProcessDTOs(
                        3
                    )
                    .ToList();

                Assert.Equal(expected.Count, actual.Count);
                Assert.Equal(0, actual2);

                expected.Should().BeEquivalentTo(actual);

                context.Subprocess.First(x => x.SubprocessID == 1).Process =
                    context.Process.First(x => x.ProcessID == 2);
                context.SaveChanges();

                var actual3 = controller.GetProcessesWithSubprocess(true).ToList();
                var actual4 = controller.GetProcessesWithSubprocess(false).ToList();

                var expected2 = new List<ProcessDTO>() { expected[0] };
                expected.RemoveAt(0);

                Assert.Equal(actual3.Count, expected.Count);
                Assert.Equal(actual4.Count, expected2.Count);

                expected.Should().BeEquivalentTo(actual3);
                expected2.Should().BeEquivalentTo(actual4);
            }

        }
                
        [Fact]
        public void TestGetSubprocessesByProcessID()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new ProcessController(context, converter, dbm);

                var actual = controller.GetSubprocessesByProcessID(1).ToList().Count;

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

                var controller = new ProcessController(context, converter, dbm);

                var expected = DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                    )
                    .ToList();

                expected[0].Should().BeEquivalentTo(controller.GetSubprocessesByProcessID(1).ToList()[0]);
                expected[1].Should().BeEquivalentTo(controller.GetSubprocessesByProcessID(2).ToList()[0]);
                expected[2].Should().BeEquivalentTo(controller.GetSubprocessesByProcessID(3).ToList()[0]);
            }

        }
        
        [Fact]
        public void TestGetParentChildRelationsByProcessID()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new ProcessController(context, converter, dbm);

                var actual = controller.GetParentChildRelationsByProcessID(1).ToList().Count;

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

                var controller = new ProcessController(context, converter, dbm);

                var subprocessDTOs = DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                    ).ToList();

                var expected = DbContextSeeder.GetSeededParentChildRelationDTOs(
                        4,
                        new List<SubprocessDTO>() { subprocessDTOs[0], subprocessDTOs[1], subprocessDTOs[2], null },
                        new List<SubprocessDTO>() { null, subprocessDTOs[0], subprocessDTOs[1], subprocessDTOs[2] }
                    )
                    .ToList();

                new List<ParentChildRelationDTO>() { expected[0], expected[1] }
                    .Should()
                    .BeEquivalentTo(
                        controller.GetParentChildRelationsByProcessID(1).ToList()
                    );
                new List<ParentChildRelationDTO>() { expected[1], expected[2] }
                    .Should()
                    .BeEquivalentTo(
                        controller.GetParentChildRelationsByProcessID(2).ToList()
                    );
                new List<ParentChildRelationDTO>() { expected[2], expected[3] }
                    .Should()
                    .BeEquivalentTo(
                        controller.GetParentChildRelationsByProcessID(3).ToList()
                    );
            }

        }
        
        [Fact]
        public void TestNewProcess()
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

                var controller = new ProcessController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededProcessDTOs(4).ToList();
                var expected = DbContextSeeder.GetSeededProcesses(4).ToList();
                //dtos[3].Abbreviation = "test";
                //dtos[3].Description = "test";

                var actual = controller.NewProcess(dtos[3]);

                actual.Should().BeEquivalentTo(dtos[3]);

                var actualCount = context.Process.ToList().Count;
                Assert.Equal(4, actualCount);

                context.Process.FirstOrDefault(x => x.ProcessID == 4).Should().BeEquivalentTo(expected[3]);

            }


        }

        [Fact]
        public void TestEditProcess()
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

                var controller = new ProcessController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededProcessDTOs(
                    3
                    )
                    .ToList();

                var expectedText = "test";
                var expectedID = 1;

                dtos[2].Description = expectedText;

                var actual = controller.EditProcess(3, dtos[2]);

                actual.Should().BeEquivalentTo(dtos[2]);

                var actualCount = context.Process.ToList().Count;
                Assert.Equal(3, actualCount);

                var actualInDb = context.Process
                    .FirstOrDefault(x => x.ProcessID == 3);

                Assert.True(actualInDb != null);

                if (actualInDb != null)
                {
                    Assert.Equal(actualInDb.Description, expectedText);
                }
            }
        }

        [Fact]
        public void TestDeleteProcess()
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

                var controller = new ProcessController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededProcessDTOs(
                    3
                    )
                    .ToList();

                controller.DeleteProcess(dtos[2].ProcessID);

                var actualCount = context.Process.ToList().Count;
                Assert.Equal(2, actualCount);
                Assert.True(context.Process.FirstOrDefault(x => x.ProcessID == 3) == null);
                Assert.True(context.Process.FirstOrDefault(x => x.ProcessID != 3) != null);

                controller.DeleteProcess(dtos[1].ProcessID);

                actualCount = context.Process.ToList().Count;
                Assert.Equal(1, actualCount);
                Assert.True(context.Process.FirstOrDefault(x => x.ProcessID == 2) == null);

            }


        }
    }
}
