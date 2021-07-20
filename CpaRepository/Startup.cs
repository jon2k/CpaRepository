using AutoMapper;
using Core.Interfaces.EF;
using Core.Interfaces.FileSystem;
using Core.Models;
using Infrastructure.EF;
using Infrastructure.FileSystem;
using Infrastructure.Logger;
using Infrastructure.Repository;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using Web.AutoMapper;
using Web.Mediatr.Query.ModulesController;
using Web.ViewModel.Module;

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
            services.AddScoped<ILetterRepo,LetterRepo>();
            services.AddScoped<IRepository<Vendor>, Repository<Vendor>>();
            services.AddScoped<IRepository<CpaModule>, Repository<CpaModule>>();
            services.AddScoped<IRepository<AgreedModule>, Repository<AgreedModule>>();
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

            services.AddTransient<IRequestHandler<GetActualModulesQuery, ModuleVM>, GetActualModulesQuery.GetActualModulesQueryHandler>();          
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
