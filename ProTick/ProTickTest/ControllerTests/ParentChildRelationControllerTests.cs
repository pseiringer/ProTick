using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProTick.Controllers;
using ProTick.Exceptions;
using ProTick.ResourceDTOs;
using ProTick.Singletons;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;
using ProTickTest.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace ProTickTest
{
    public class ParentChildRelationControllerTests : ProTickSetupFixture
    {

        [Fact]
        public void TestGetAllParentChildRelations()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new ParentChildRelationController(context, converter, dbm);

                var actual = controller.GetAllParentChildRelations().ToList().Count;

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

                var controller = new ParentChildRelationController(context, converter, dbm);

                var actual = controller.GetAllParentChildRelations().ToList();

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


                Assert.Equal(expected.Count, actual.Count);

                expected.Should().BeEquivalentTo(actual);
            }

        }

        [Fact]
        public void TestGetParentChildRelationByID()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new ParentChildRelationController(context, converter, dbm);

                Assert.Throws<DatabaseEntryNotFoundException>(() => controller.GetParentChildRelationByID(0));
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

                var controller = new ParentChildRelationController(context, converter, dbm);

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

                expected[0].Should().BeEquivalentTo(controller.GetParentChildRelationByID(1));
                expected[1].Should().BeEquivalentTo(controller.GetParentChildRelationByID(2));
                expected[2].Should().BeEquivalentTo(controller.GetParentChildRelationByID(3));
                expected[3].Should().BeEquivalentTo(controller.GetParentChildRelationByID(4));
            }

        }

        [Fact]
        public void TestPostParentChildRelation()
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

                var controller = new ParentChildRelationController(context, converter, dbm);

                var subprocesses = DbContextSeeder.GetSeededSubprocesses(
                        4,
                        DbContextSeeder.GetSeededProcesses(4),
                        DbContextSeeder.GetSeededTeams(4)
                    ).ToList();
                var expected = DbContextSeeder.GetSeededParentChildRelations(
                        5,
                        new List<Subprocess>() { subprocesses[0], subprocesses[1], subprocesses[2], null, subprocesses[1] },
                        new List<Subprocess>() { null, subprocesses[0], subprocesses[1], subprocesses[2], subprocesses[0] }
                    )
                    .ToList();

                var subprocessDTOs = DbContextSeeder.GetSeededSubprocessDTOs(
                        4,
                        DbContextSeeder.GetSeededProcessDTOs(4),
                        DbContextSeeder.GetSeededTeamDTOs(4)
                    ).ToList();
                var dtos = DbContextSeeder.GetSeededParentChildRelationDTOs(
                        5,
                        new List<SubprocessDTO>() { subprocessDTOs[0], subprocessDTOs[1], subprocessDTOs[2], null, subprocessDTOs[1] },
                        new List<SubprocessDTO>() { null, subprocessDTOs[0], subprocessDTOs[1], subprocessDTOs[2], subprocessDTOs[0] }
                    )
                    .ToList();
                //dtos[3]. = "test";
                //dtos[3].Description = "test";

                var actual = controller.PostParentChildRelation(dtos[4]);

                actual.Should().BeEquivalentTo(dtos[4]);

                var actualCount = context.ParentChildRelation.ToList().Count;
                Assert.Equal(5, actualCount);

                context.ParentChildRelation.FirstOrDefault(x => x.ParentChildRelationID == 5).Should().BeEquivalentTo(expected[4]);

            }


        }

        [Fact]
        public void TestPutParentChildRelation()
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

                var controller = new ParentChildRelationController(context, converter, dbm);

                var subprocessDTOs = DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                    ).ToList();

                var dtos = DbContextSeeder.GetSeededParentChildRelationDTOs(
                        4,
                        new List<SubprocessDTO>() { subprocessDTOs[0], subprocessDTOs[1], subprocessDTOs[2], null },
                        new List<SubprocessDTO>() { null, subprocessDTOs[0], subprocessDTOs[1], subprocessDTOs[2] }
                    )
                    .ToList();

                var expectedParent = 1;
                var expectedChild = 2;

                dtos[2].ParentID = expectedParent;
                dtos[2].ChildID = expectedChild;

                var actual = controller.PutParentChildRelation(3, dtos[2]);

                actual.Should().BeEquivalentTo(dtos[2]);

                var actualCount = context.ParentChildRelation.ToList().Count;
                Assert.Equal(4, actualCount);

                var actualInDb = context.ParentChildRelation
                    .Include(x => x.Parent)
                    .Include(x => x.Child)
                    .FirstOrDefault(x => x.ParentChildRelationID == 3);

                Assert.True(actualInDb != null);

                if (actualInDb != null)
                {
                    Assert.Equal(actualInDb.Parent.SubprocessID, expectedParent);
                    Assert.Equal(actualInDb.Child.SubprocessID, expectedChild);
                }
            }
        }

        [Fact]
        public void TestDeleteParentChildRelation()
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

                var controller = new ParentChildRelationController(context, converter, dbm);

                var subprocessDTOs = DbContextSeeder.GetSeededSubprocessDTOs(
                        3,
                        DbContextSeeder.GetSeededProcessDTOs(3),
                        DbContextSeeder.GetSeededTeamDTOs(3)
                    ).ToList();

                var dtos = DbContextSeeder.GetSeededParentChildRelationDTOs(
                        4,
                        new List<SubprocessDTO>() { subprocessDTOs[0], subprocessDTOs[1], subprocessDTOs[2], null },
                        new List<SubprocessDTO>() { null, subprocessDTOs[0], subprocessDTOs[1], subprocessDTOs[2] }
                    )
                    .ToList();

                controller.DeleteParentChildRelation(dtos[3].ParentChildRelationID);

                var actualCount = context.ParentChildRelation.ToList().Count;
                Assert.Equal(3, actualCount);
                Assert.True(context.ParentChildRelation.FirstOrDefault(x => x.ParentChildRelationID == 4) == null);
                Assert.True(context.ParentChildRelation.FirstOrDefault(x => x.ParentChildRelationID != 4) != null);

                controller.DeleteParentChildRelation(dtos[2].ParentChildRelationID);

                actualCount = context.ParentChildRelation.ToList().Count;
                Assert.Equal(2, actualCount);
                Assert.True(context.ParentChildRelation.FirstOrDefault(x => x.ParentChildRelationID == 3) == null);
                Assert.True(context.ParentChildRelation.FirstOrDefault(x => x.ParentChildRelationID != 3) != null);

                controller.DeleteParentChildRelation(dtos[1].ParentChildRelationID);

                actualCount = context.ParentChildRelation.ToList().Count;
                Assert.Equal(1, actualCount);
                Assert.True(context.ParentChildRelation.FirstOrDefault(x => x.ParentChildRelationID == 2) == null);

            }


        }
    }
}
