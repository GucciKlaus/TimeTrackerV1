using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerLib
{
    public class DataContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<DataObjectV2> DataObjects { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DataContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine($"DB On Configuring: Is Configured = {optionsBuilder.IsConfigured}");
            if(optionsBuilder != null && !optionsBuilder.IsConfigured)
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string relativePath = @"DLDB.mdf"; // Passe den relativen Pfad entsprechend an
                string fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDirectory, relativePath));
                string connectionString = $@"Server=(LocalDB)\MSSQLLocalDB;AttachDbFilename={fullPath};Database=DLDB;Integrated Security=True;MultipleActiveResultSets=True;";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }
    }
}
