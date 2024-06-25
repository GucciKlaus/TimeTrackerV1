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
            DataObjectV2 d1 = new DataObjectV2 { DataObjectV2ID = 1, Date = DateTime.Now.ToString("dd.MM.yyyy"), Timespan = "2 h 30 min", Title = "Test", Description = "Testversuch", UserUserID=1};
            modelBuilder.Entity<User>().HasData(u1);
            modelBuilder.Entity<DataObjectV2>().HasData(d1);
        }
    }
}
