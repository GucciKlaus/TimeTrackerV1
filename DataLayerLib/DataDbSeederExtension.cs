using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerLib
{
    public static class DataDbSeederExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            Console.WriteLine("DB Seeder started");
            User u1 = new User { UserID = 1,UserName = "TestUser"};
            DataObjectV2 d1 = new DataObjectV2 { DataObjectV2ID = 1, Date = DateTime.Now, Timespan = "01:10:05", Title = "Test", Description = "Testversuch", UserUserID=1};
            DataObjectV2 d2 = new DataObjectV2 { DataObjectV2ID = 2, Date = DateTime.Now, Timespan = "04:30:00", Title = "Test", Description = "Testversuch", UserUserID = 1 };
            modelBuilder.Entity<User>().HasData(u1);
            modelBuilder.Entity<DataObjectV2>().HasData(d1);
            modelBuilder.Entity<DataObjectV2>().HasData(d2);
        }
    }
}
