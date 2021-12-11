using BeetrootCqrs.DAL.DB;
using BeetrootCqrs.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace BeetrootCqrs.Test.Common
{
    public class FakeDbContextFactory
    {
        public static AppDbContext Create()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureCreated();

            dbContext.Addresses.AddRange(
                new Address() { Id = Guid.Parse("{12AEADE1-DF1C-4479-ADC0-944FC4B5B001}"), IpAddress="127.0.0.1" },
                new Address() { Id = Guid.Parse("{12AEADE1-DF1C-4479-ADC0-944FC4B5B002}"), IpAddress = "127.0.0.2" },
                new Address() { Id = Guid.Parse("{12AEADE1-DF1C-4479-ADC0-944FC4B5B003}"), IpAddress = "127.0.0.3" }
                );

            dbContext.Messages.AddRange(
                new Message()
                {
                    AddressId = Guid.Parse("{12AEADE1-DF1C-4479-ADC0-944FC4B5B001}"),
                    Text = "message 1",
                    Date = new DateTime(2021, 12, 01, 01, 01, 01)
                },
                new Message()
                {
                    AddressId = Guid.Parse("{12AEADE1-DF1C-4479-ADC0-944FC4B5B002}"),
                    Text = "message 2",
                    Date = new DateTime(2021, 12, 02, 01, 01, 01)
                },
                new Message()
                {
                    AddressId = Guid.Parse("{12AEADE1-DF1C-4479-ADC0-944FC4B5B003}"),
                    Text = "message 3",
                    Date = new DateTime(2021, 12, 04, 01, 01, 01)
                }
                );

            dbContext.SaveChanges();
            return dbContext;
        }

        public static void Destroy(AppDbContext dbContext)
        {
            if (dbContext.Database.EnsureDeleted())
                dbContext.Dispose();
        }
    }
}
