using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProTick.Exceptions;
using ProTick.Singletons;
using ProTickDatabase;
using ProTickTest.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProTickTest
{
    public class TestDatabaseQueryManager : ProTickSetupFixture
    {
        [Fact]
        public void TestFindAllAddresses()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);

                int actual = dbm.FindAllAddresses().Count;

                int expected = 0;

                Assert.Equal(expected, actual);
            }

            // seeding DB
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                DbContextSeeder.SeedFull(context);
            }

            // db is full
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);

                var actual = dbm.FindAllAddresses();

                var expected = DbContextSeeder.GetSeededAddresses(3);

                Assert.Equal(expected.Count, actual.Count);

                expected.Should().BeEquivalentTo(actual);
            }

        }

        [Fact]
        public void TestFindAddressByID()
        {
            // db is empty
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);

                Assert.Throws<DatabaseEntryNotFoundException>(() => dbm.FindAddressByID(0));
            }

            // seeding DB
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                DbContextSeeder.SeedFull(context);
            }

            // db is full
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                var dbm = new DatabaseQueryManager(context);

                var actual = dbm.FindAddressByID(1);

                var expected = DbContextSeeder.GetSeededAddresses(1)[0];

                expected.Should().BeEquivalentTo(actual);

                Assert.Throws<DatabaseEntryNotFoundException>(() => dbm.FindAddressByID(4));
            }

        }



    }
}
