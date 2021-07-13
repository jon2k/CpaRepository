using CpaRepository.EF;
using CpaRepository.Service;
using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CpaRepository.Logger;
using System.IO;
using AutoMapper;
using Web.AutoMapper;
using MediatR;
using System.Reflection;
using Web;
using Web.Mediatr.Query;
using CpaRepository.ViewModel.ActualVendorModule;

namespace CpaRepository
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlite("Filename=Cpa.db"));
            services.AddControllersWithViews();
            services.AddScoped<IVendorModuleRepo, VendorModuleRepo>();
            services.AddScoped<IAgreedModulesRepo, AgreedModulesRepo>();
            services.AddScoped<LetterRepo>();
            services.AddScoped<IRepository<Vendor>, Repository<Vendor>>();
            services.AddScoped<Repository<CpaModule>>();
            services.AddScoped<Repository<AgreedModule>>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IPathService, PathService>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(Startup));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // services.AddMediatR(Assembly.GetExecutingAssembly());
            // services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(GetAgreedModulesQuery).GetTypeInfo().Assembly);


            services.AddTransient<IRequestHandler<GetActualModulesQuery, ModuleVM>, GetActualModulesQuery.GetActualModulesQueryHandler>();
            // services.AddTransient<IRequestHandler<GetArchiveModulesQuery, ModuleVM>, GetArchiveModulesQuery.GetArchiveModulesQueryHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            var logger = loggerFactory.CreateLogger("FileLogger");
        }
    }
}
