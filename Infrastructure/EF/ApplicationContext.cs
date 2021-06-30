using CpaRepository.ModelsDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

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
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Use provider to MSSQL
            // optionsBuilder.UseSqlite("Filename=Cpa.db");
            optionsBuilder.UseLazyLoadingProxies();
        }


        public DbSet<AgreedModule> AgreedModules { get; set; }
        public DbSet<CpaModule> CpaModules { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorModule> VendorModules { get; set; }
        public DbSet<Letter> Letters { get; set; }

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
            modelBuilder.Entity<VendorModule>(entity =>
            {
                entity.HasOne(v => v.Vendor)
                    .WithMany(m => m.VendorModules);
            });
            modelBuilder.Entity<Vendor>().HasData(
                new Vendor[]
                {
                    new Vendor { Id=1, Name="Prosoft", Describtion="тра-та-та"},
                    new Vendor { Id=2, Name="Emicon", Describtion="тра-та-та"},
                    new Vendor { Id=3, Name="Siemens", Describtion="тра-та-та"},
                    new Vendor { Id=4, Name="B+R", Describtion="тра-та-та"},
                    new Vendor { Id=5, Name="Shneider Electric", Describtion="тра-та-та"},
                });
            var cpaModules = new CpaModule[]
                {
                    new CpaModule{Id=1,NameModule="OIP", Description="Измеряемый параметр"},
                    new CpaModule{Id=2,NameModule="KTPR", Description="тра-та-та"},
                    new CpaModule{Id=3,NameModule="KTPRAS", Description="тра-та-та"},
                    new CpaModule{Id=4,NameModule="KTPRA", Description="тра-та-та"},
                    new CpaModule{Id=5,NameModule="KTPRS", Description="тра-та-та"},
                    new CpaModule{Id=6,NameModule="CMNA", Description="тра-та-та"},
                    new CpaModule{Id=7,NameModule="UMPNA", Description="тра-та-та"},
                    new CpaModule{Id=8,NameModule="KRMPN", Description="тра-та-та"},
                    new CpaModule{Id=9,NameModule="KGMPNA", Description="тра-та-та"},
                    new CpaModule{Id=10,NameModule="UVS", Description="тра-та-та"},
                    new CpaModule{Id=11,NameModule="UZD", Description="тра-та-та"},
                    new CpaModule{Id=12,NameModule="MZD1", Description="тра-та-та"},
                    new CpaModule{Id=13,NameModule="UTS", Description="тра-та-та"},
                    new CpaModule{Id=14,NameModule="MPT", Description="тра-та-та"},
                    new CpaModule{Id=15,NameModule="SAR заслонка", Description="тра-та-та"},
                    new CpaModule{Id=16,NameModule="SAR ЧРП", Description="тра-та-та"},
                    new CpaModule{Id=17,NameModule="Other", Description="тра-та-та"},
                };
            modelBuilder.Entity<CpaModule>().HasData(
                cpaModules);
            modelBuilder.Entity<VendorModule>().HasData(
             new VendorModule[]
             {
                    new VendorModule{Id=1, VendorId=1, NameModule="OipLib", Description=""},
                    new VendorModule{Id=2, VendorId=1, NameModule="NaLib", Description=""},
                    new VendorModule{Id=3, VendorId=1, NameModule="MptLib", Description=""},
                    new VendorModule{Id=4, VendorId=1, NameModule="Tools", Description=""},
                    new VendorModule{Id=5, VendorId=2, NameModule="Oip", Description=""},
                    new VendorModule{Id=6, VendorId=2, NameModule="Na", Description="" },
                    new VendorModule{Id=7, VendorId=3, NameModule="OipLib", Description=""},
                    new VendorModule{Id=8, VendorId=3, NameModule="NaLib", Description=""},
                    new VendorModule{Id=9, VendorId=3, NameModule="MptLib", Description=""},
                    new VendorModule{Id=10, VendorId=4, NameModule="Tools", Description=""},
                    new VendorModule{Id=11, VendorId=4, NameModule="Oip", Description=""},
                    new VendorModule{Id=12, VendorId=4, NameModule="Na", Description="" }

             });
            modelBuilder.Entity<Letter>().HasData(
           new Letter[]
           {
                    new Letter{Id=1, VendorId=1, NumberLetter="123", DateOfLetter=new DateTime(2021,1,1), PathLetter="D:\\test.pdf"   },
                    new Letter{Id=2, VendorId=1, NumberLetter="456", DateOfLetter=new DateTime(2021,2,1), PathLetter="D:\\test.pdf"  },
                    new Letter{Id=3, VendorId=1, NumberLetter="789", DateOfLetter=new DateTime(2021,3,1), PathLetter="D:\\test.pdf" },
                    new Letter{Id=4, VendorId=1, NumberLetter="321", DateOfLetter=new DateTime(2021,4,1), PathLetter="D:\\test.pdf"},
                    new Letter{Id=5, VendorId=2, NumberLetter="987", DateOfLetter=new DateTime(2021,5,1), PathLetter="D:\\test.pdf"},
                    new Letter{Id=6, VendorId=2, NumberLetter="654", DateOfLetter=new DateTime(2021,5,5),  PathLetter="D:\\test.pdf"},
                    new Letter{Id=7, VendorId=3, NumberLetter="123", DateOfLetter=new DateTime(2021,5,7), PathLetter="D:\\test.pdf"   },
                    new Letter{Id=8, VendorId=3, NumberLetter="456", DateOfLetter=new DateTime(2021,6,1), PathLetter="D:\\test.pdf"  },
                    new Letter{Id=9, VendorId=3, NumberLetter="789", DateOfLetter=new DateTime(2021,4,8), PathLetter="D:\\test.pdf" },
                    new Letter{Id=10, VendorId=4, NumberLetter="321", DateOfLetter=new DateTime(2021,2,8), PathLetter="D:\\test.pdf"},
                    new Letter{Id=11, VendorId=4, NumberLetter="987", DateOfLetter=new DateTime(2021,2,8), PathLetter="D:\\test.pdf"},
                    new Letter{Id=12, VendorId=4, NumberLetter="654", DateOfLetter=new DateTime(2020,1,8),  PathLetter="D:\\test.pdf"}

           });
            modelBuilder.Entity<AgreedModule>().HasData(
           new AgreedModule[]
           {
                    new AgreedModule{Id=1, VendorModuleId=1, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=1 },
                    new AgreedModule{Id=2, VendorModuleId=1, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=2   },
                    new AgreedModule{Id=3, VendorModuleId=1, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=2  },
                    new AgreedModule{Id=4, VendorModuleId=1, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=3 },
                    new AgreedModule{Id=5, VendorModuleId=2, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=3 },
                    new AgreedModule{Id=6, VendorModuleId=2, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=2 },
                    new AgreedModule{Id=7, VendorModuleId=3, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=3 },
                    new AgreedModule{Id=8, VendorModuleId=3, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=2   },
                    new AgreedModule{Id=9, VendorModuleId=3, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=1  },
                    new AgreedModule{Id=10, VendorModuleId=4, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=1 },
                    new AgreedModule{Id=11, VendorModuleId=4, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=2 },
                    new AgreedModule{Id=12, VendorModuleId=4, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=3 },
                    new AgreedModule{Id=13, VendorModuleId=5, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=1 },
                    new AgreedModule{Id=14, VendorModuleId=5, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=5   },
                    new AgreedModule{Id=15, VendorModuleId=5, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=6  },
                    new AgreedModule{Id=16, VendorModuleId=6, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=5 },
                    new AgreedModule{Id=17, VendorModuleId=6, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=6 },
                    new AgreedModule{Id=18, VendorModuleId=6, Changes="test", CRC="123123123", Version="1.2.1", PathVendorModule="D:\\test.rar", LetterId=6 }

           });
        }

        //public DbSet<AgreedModuleViewModel> AgreedModuleViewModel { get; set; }
        // public DbSet<LetterViewModel> LetterViewModel { get; set; }
    }

}
