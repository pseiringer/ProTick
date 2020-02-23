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
    public class StateControllerTests : ProTickSetupFixture
    {

        [Fact]
        public void TestGetStates()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new StateController(context, converter, dbm);

                var actual = controller.GetStates().ToList().Count;

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

                var controller = new StateController(context, converter, dbm);

                var actual = controller.GetStates().ToList();

                var expected = DbContextSeeder.GetSeededStateDTOs(
                        3
                    )
                    .ToList();

                Assert.Equal(expected.Count, actual.Count);

                expected.Should().BeEquivalentTo(actual);
            }

        }

        [Fact]
        public void TestGetState()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new StateController(context, converter, dbm);

                Assert.Throws<DatabaseEntryNotFoundException>(() => controller.GetState(0));
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

                var controller = new StateController(context, converter, dbm);

                var expected = DbContextSeeder.GetSeededStateDTOs(
                        3
                    )
                    .ToList();

                expected[0].Should().BeEquivalentTo(controller.GetState(1));
                expected[1].Should().BeEquivalentTo(controller.GetState(2));
                expected[2].Should().BeEquivalentTo(controller.GetState(3));
            }

        }

        [Fact]
        public void TestGetTicketsByStateID()
        {

            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new StateController(context, converter, dbm);

                var actual = controller.GetTicketsByStateID(1).ToList().Count;

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

                var controller = new StateController(context, converter, dbm);

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

                var act1 = controller.GetTicketsByStateID(1).ToList();
                var act2 = controller.GetTicketsByStateID(2).ToList();
                var act3 = controller.GetTicketsByStateID(3).ToList();

                int expectedCount = 1;
                Assert.Equal(expectedCount, act1.Count);
                Assert.Equal(expectedCount, act2.Count);
                Assert.Equal(expectedCount, act3.Count);

                expected[0].Should().BeEquivalentTo(act1[0]);
                expected[1].Should().BeEquivalentTo(act2[0]);
                expected[2].Should().BeEquivalentTo(act3[0]);
            }
        }


        [Fact]
        public void TestPostState()
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

                var controller = new StateController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededStateDTOs(4).ToList();
                var expected = DbContextSeeder.GetSeededStates(4).ToList();
                //dtos[3].Abbreviation = "test";
                //dtos[3].Description = "test";

                var actual = controller.PostState(dtos[3]);

                actual.Should().BeEquivalentTo(dtos[3]);

                var actualCount = context.State.ToList().Count;
                Assert.Equal(4, actualCount);

                context.State.FirstOrDefault(x => x.StateID == 4).Should().BeEquivalentTo(expected[3]);

            }


        }

        [Fact]
        public void TestPutState()
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

                var controller = new StateController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededStateDTOs(
                    3
                    )
                    .ToList();

                var expectedText = "test";
                var expectedID = 1;

                dtos[2].Description = expectedText;

                var actual = controller.PutState(3, dtos[2]);

                actual.Should().BeEquivalentTo(dtos[2]);

                var actualCount = context.State.ToList().Count;
                Assert.Equal(3, actualCount);

                var actualInDb = context.State
                    .FirstOrDefault(x => x.StateID == 3);

                Assert.True(actualInDb != null);

                if (actualInDb != null)
                {
                    Assert.Equal(actualInDb.Description, expectedText);
                }
            }
        }

        [Fact]
        public void TestDeleteState()
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

                var controller = new StateController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededStateDTOs(
                    3
                    )
                    .ToList();

                controller.DeleteState(dtos[2].StateID);

                var actualCount = context.State.ToList().Count;
                Assert.Equal(2, actualCount);
                Assert.True(context.State.FirstOrDefault(x => x.StateID == 3) == null);
                Assert.True(context.State.FirstOrDefault(x => x.StateID != 3) != null);

                controller.DeleteState(dtos[1].StateID);

                actualCount = context.State.ToList().Count;
                Assert.Equal(1, actualCount);
                Assert.True(context.State.FirstOrDefault(x => x.StateID == 2) == null);

            }


        }
    }
}
