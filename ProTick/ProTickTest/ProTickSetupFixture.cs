using Microsoft.EntityFrameworkCore;
using ProTickDatabase;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProTickTest
{
    public class ProTickSetupFixture : IDisposable
    {
        protected DbContextOptions dbOptions;

        public ProTickSetupFixture()
        {
            dbOptions =  new DbContextOptionsBuilder<ProTickDatabaseContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
        }

        public void Dispose()
        {
            // removing DB
            using (var context = new ProTickDatabaseContext(dbOptions))
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }
    }
}
