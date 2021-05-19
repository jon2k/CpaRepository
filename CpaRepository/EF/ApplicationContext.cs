using CpaRepository.ModelsDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.EF
{
    public class ApplicationContext : DbContext
    {
        public ILogger<ApplicationContext> Logger { get; }
        // public string ConnString { get; set; }

        //public ApplicationContext() : base()
        public ApplicationContext(DbContextOptions<ApplicationContext> options, ILogger<ApplicationContext> logger) : base(options)
        {
            //Logger = logger;
            //Create DB, it it doesn't exist
           // Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //Use provider to MSSQL
        //    optionsBuilder.UseSqlite("Filename=Cpa.db");
        //}


        public DbSet<AgreedModule> AgreedModules { get; set; }
        public DbSet<CpaModule> CpaModules { get; set; }
        public DbSet<CpaSubModule> CpaSubModules { get; set; }
        public DbSet<RelationCpaModuleWithVendorModule> RelationCpaModuleWithVendorModules { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorModule> VendorModules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AgreedModule>()
            //    .HasIndex(b => b.CRC)
            //    .IsUnique();
            modelBuilder.Entity<AgreedModule>(entity =>
            {
                //entity.HasOne(a => a.Vendor)
                //    .WithMany(v => v.AgreedModules);
                entity.HasOne(a => a.VendorModule)
                    .WithMany(v => v.AgreedModules);
            });
            modelBuilder.Entity<CpaSubModule>(entity =>
            {
                entity.HasOne(s => s.Module)
                    .WithMany(m => m.CpaSubModules);
            });
            modelBuilder.Entity<RelationCpaModuleWithVendorModule>(entity =>
            {
                entity.HasOne(r => r.CpaSubModule)
                    .WithMany(m => m.Relations);
                entity.HasOne(r => r.VendorModule)
                    .WithMany(m => m.Relations);
            });
            modelBuilder.Entity<VendorModule>(entity =>
            {
                entity.HasOne(v => v.Vendor)
                    .WithMany(m => m.VendorModules);
            });
            modelBuilder.Entity<Vendor>().HasData(
                new Vendor[]
                {
                    new Vendor { Id=1, Name="Prosoft"},
                    new Vendor { Id=2, Name="Emicon"},
                    new Vendor { Id=3, Name="Siemens"},
                    new Vendor { Id=4, Name="B+R"},
                    new Vendor { Id=5, Name="Shneider Electric"},
                });
            modelBuilder.Entity<CpaModule>().HasData(
                new CpaModule[]
                {
                    new CpaModule{Id=1,NameModule="OIP", Description=""},
                    new CpaModule{Id=2,NameModule="NA", Description=""},
                    new CpaModule{Id=3,NameModule="MPT", Description=""},
                });
            modelBuilder.Entity<CpaSubModule>().HasData(
              new CpaSubModule[]
              {
                    new CpaSubModule{Id=1, ModuleId=1, NameSubModule="OipElectric", Description=""},
                    new CpaSubModule{Id=2, ModuleId=1, NameSubModule="OipVibration", Description=""},
                    new CpaSubModule{Id=3, ModuleId=1, NameSubModule="ProcLevels", Description=""},
                    new CpaSubModule{Id=4, ModuleId=1, NameSubModule="TankSpeed", Description=""},
                    new CpaSubModule{Id=5, ModuleId=2, NameSubModule="Umpna", Description=""},
                    new CpaSubModule{Id=6, ModuleId=2, NameSubModule="Cmna", Description=""},
              });
            modelBuilder.Entity<VendorModule>().HasData(
             new VendorModule[]
             {
                    new VendorModule{Id=1, VendorId=1, NameModule="OipLib", Description=""},
                    new VendorModule{Id=2, VendorId=1, NameModule="NaLib", Description=""},
                    new VendorModule{Id=3, VendorId=1, NameModule="MptLib", Description=""},
                    new VendorModule{Id=4, VendorId=1, NameModule="Tools", Description=""},
                    new VendorModule{Id=5, VendorId=2, NameModule="Oip", Description=""},
                    new VendorModule{Id=6, VendorId=2, NameModule="Na", Description=""},
             });
        }
    }

}
