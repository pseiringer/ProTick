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
    public class AddressControllerTests : ProTickSetupFixture
    {

        [Fact]
        public void TestGetAddresses()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new AddressController(context, converter, dbm);

                var actual = controller.GetAddresses().ToList().Count;

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

                var controller = new AddressController(context, converter, dbm);

                var actual = controller.GetAddresses().ToList();

                var expected = DbContextSeeder.GetSeededAddressDTOs(
                        3
                    )
                    .ToList();

                Assert.Equal(expected.Count, actual.Count);

                expected.Should().BeEquivalentTo(actual);
            }

        }

        [Fact]
        public void TestGetAddress()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);
                var converter = new ResourceDTOConverter(dbm);

                var controller = new AddressController(context, converter, dbm);

                Assert.Throws<DatabaseEntryNotFoundException>(() => controller.GetAddress(0));
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

                var controller = new AddressController(context, converter, dbm);

                var expected = DbContextSeeder.GetSeededAddressDTOs(
                        3
                    )
                    .ToList();

                expected[0].Should().BeEquivalentTo(controller.GetAddress(1));
                expected[1].Should().BeEquivalentTo(controller.GetAddress(2));
                expected[2].Should().BeEquivalentTo(controller.GetAddress(3));
            }

        }

        [Fact]
        public void TestNewAddress()
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

                var controller = new AddressController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededAddressDTOs(4).ToList();
                var expected = DbContextSeeder.GetSeededAddresses(4).ToList();
                //dtos[3].Abbreviation = "test";
                //dtos[3].Description = "test";

                var actual = controller.NewAddress(dtos[3]);

                actual.Should().BeEquivalentTo(dtos[3]);

                var actualCount = context.Address.ToList().Count;
                Assert.Equal(4, actualCount);

                context.Address.FirstOrDefault(x => x.AddressID == 4).Should().BeEquivalentTo(expected[3]);

            }


        }

        [Fact]
        public void TestEditAddress()
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

                var controller = new AddressController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededAddressDTOs(
                    3
                    )
                    .ToList();

                var expectedText = "test";
                var expectedID = 1;

                dtos[2].City = expectedText;
                dtos[2].Country = expectedText;
                dtos[2].PostalCode = expectedText;
                dtos[2].Street = expectedText;
                dtos[2].StreetNumber = expectedText;

                var actual = controller.EditAddress(3, dtos[2]);

                actual.Should().BeEquivalentTo(dtos[2]);

                var actualCount = context.Address.ToList().Count;
                Assert.Equal(3, actualCount);

                var actualInDb = context.Address
                    .FirstOrDefault(x => x.AddressID == 3);

                Assert.True(actualInDb != null);

                if (actualInDb != null)
                {
                    Assert.Equal(actualInDb.City, expectedText);
                    Assert.Equal(actualInDb.Country, expectedText);
                    Assert.Equal(actualInDb.PostalCode, expectedText);
                    Assert.Equal(actualInDb.Street, expectedText);
                    Assert.Equal(actualInDb.StreetNumber, expectedText);
                }
            }
        }

        [Fact]
        public void TestDeleteAddress()
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

                var controller = new AddressController(context, converter, dbm);

                var dtos = DbContextSeeder.GetSeededAddressDTOs(
                    3
                    )
                    .ToList();

                controller.DeleteAddress(dtos[2].AddressID);

                var actualCount = context.Address.ToList().Count;
                Assert.Equal(2, actualCount);
                Assert.True(context.Address.FirstOrDefault(x => x.AddressID == 3) == null);
                Assert.True(context.Address.FirstOrDefault(x => x.AddressID != 3) != null);

                controller.DeleteAddress(dtos[1].AddressID);

                actualCount = context.Address.ToList().Count;
                Assert.Equal(1, actualCount);
                Assert.True(context.Address.FirstOrDefault(x => x.AddressID == 2) == null);

            }


        }
    }
}
