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
    public class RoleControllerTests : ProTickSetupFixture
    {

        [Fact]
        public void TestGetRoles()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new RoleController(context, converter, dbm);

                var actual = controller.GetRoles().ToList().Count;

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

                var controller = new RoleController(context, converter, dbm);

                var actual = controller.GetRoles().ToList();

                var expected = DbContextSeeder.GetSeededRoleDTOs(
                        3
                    )
                    .ToList();

                Assert.Equal(expected.Count, actual.Count);

                expected.Should().BeEquivalentTo(actual);
            }
        }
    }
}
