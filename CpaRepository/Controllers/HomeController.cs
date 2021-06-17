using AutoMapper;
using CpaRepository.EF;
using CpaRepository.Models;
using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using CpaRepository.ViewModel.ActualVendorModule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AgreedModulesRepo _repo;
        public HomeController(AgreedModulesRepo context, ILogger<HomeController> logger)
        {
            _repo = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var agreedModules = _repo.GetAll()
                                 .OrderByDescending(d => d.Letter.DateOfLetter)
                                 .Take(5);
                var mapper = new Mapper(GetMapConfigModelToViewModel());
                var vm = mapper.Map<List<ModuleViewModel>>(agreedModules).OrderByDescending(m => m.DateOfLetter);          
                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View(new ModuleViewModel());
            }
        }
       
      

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private MapperConfiguration GetMapConfigModelToViewModel()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<AgreedModule, ModuleViewModel>()
                  .ForMember(nameof(ModuleViewModel.ExistModule), opt => opt
                  .MapFrom(src => System.IO.File.Exists(src.PathVendorModule)))
                  .ForMember(nameof(ModuleViewModel.DateOfLetter), opt => opt.MapFrom(src => src.Letter.DateOfLetter))
                  .ForMember(nameof(ModuleViewModel.NumberLetter), opt => opt.MapFrom(src => src.Letter.NumberLetter))
                  .ForMember(nameof(ModuleViewModel.VendorId), opt => opt.MapFrom(src => src.VendorModule.VendorId))
                  .ForMember(nameof(ModuleViewModel.Vendor), opt => opt.MapFrom(src => src.VendorModule.Vendor))
                  .ForMember(nameof(ModuleViewModel.CpaModules), opt => opt.MapFrom(src => src.VendorModule.CpaModules)));
        }
    }
}
