using AutoMapper;
using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using CpaRepository.ViewModel.ActualVendorModule;
using CpaRepository.ViewModel.AgreedModules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Controllers
{
    public class ActualVendorModuleController : Controller
    {
        private readonly ILogger<ActualVendorModuleController> _logger;
        private AgreedModulesRepo _repo;

        public ActualVendorModuleController(AgreedModulesRepo context, ILogger<ActualVendorModuleController> logger)
        {
            _repo = context;
            _logger = logger;
        }
        public IActionResult ActualModules()
        {
            var vendor = _repo.GetAllVendors();
            var vendors = new List<SelectListItem>();
            vendors.Add(new SelectListItem() { Value = "0", Text = "Все вендоры" });
            vendors.AddRange(vendor.Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.Name
            }).ToList());
            ViewBag.VendorId = vendors;

            var cpaModules = _repo.GetAllCpaModules();
            var cpaModulesList = new List<SelectListItem>();
            cpaModulesList.Add(new SelectListItem() { Value = "0", Text = "Все ТПР модули" });
            cpaModulesList.AddRange(cpaModules.Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.NameModule
            }).ToList());
            ViewBag.CpaModuleId = cpaModulesList;

            var agreedModules = _repo.GetAll();
            var mapper = new Mapper(GetMapConfigModelToViewModel());
            var vm = mapper.Map<List<AgreedModuleViewModel>>(agreedModules).OrderByDescending(m => m.DateOfLetter);

            return View(vm);


        }
        [HttpPost]
        public ActionResult GetActualModules(DataForFiltr filtr)
        {
            try
            {
                IEnumerable<AgreedModule> agreedModules;
                if (filtr.SelectedCpaModule == 0 && filtr.SelectedVendor == 0)
                {
                    agreedModules = _repo.GetAll()
                        .GroupBy(n => n.VendorModule)
                        .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).FirstOrDefault());
                }
                else if (filtr.SelectedCpaModule == 0 && filtr.SelectedVendor != 0)
                {
                    agreedModules = _repo.GetAll()
                        .Where(v => v.VendorModule.VendorId == filtr.SelectedVendor)
                        .GroupBy(n => n.VendorModule)
                        .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).FirstOrDefault());
                }
                else if (filtr.SelectedCpaModule != 0 && filtr.SelectedVendor == 0)
                {
                    agreedModules = _repo.GetAll()
                        .Where(m => m.VendorModule.CpaModules.Any(m => m.Id == filtr.SelectedCpaModule))
                        .GroupBy(n => n.VendorModule)
                        .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).FirstOrDefault());
                }
                else
                {
                    agreedModules = _repo.GetAll()
                        .Where(v => v.VendorModule.VendorId == filtr.SelectedVendor)
                        .Where(m => m.VendorModule.CpaModules.Any(m => m.Id == filtr.SelectedCpaModule))
                        .GroupBy(n => n.VendorModule)
                        .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).FirstOrDefault());
                }
                var mapper = new Mapper(GetMapConfigModelToViewModel());
                var vm = mapper.Map<List<AgreedModuleViewModel>>(agreedModules).OrderByDescending(m => m.DateOfLetter);

                return PartialView(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return PartialView();
            }
        }
        private MapperConfiguration GetMapConfigModelToViewModel()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<AgreedModule, AgreedModuleViewModel>()
                  .ForMember(nameof(AgreedModuleViewModel.ExistModule), opt => opt
                  .MapFrom(src => System.IO.File.Exists(src.PathVendorModule)))
                  .ForMember(nameof(AgreedModuleViewModel.DateOfLetter), opt => opt.MapFrom(src => src.Letter.DateOfLetter))
                  .ForMember(nameof(AgreedModuleViewModel.NumberLetter), opt => opt.MapFrom(src => src.Letter.NumberLetter))
                  .ForMember(nameof(AgreedModuleViewModel.VendorId), opt => opt.MapFrom(src => src.VendorModule.VendorId))
                  .ForMember(nameof(AgreedModuleViewModel.Vendor), opt => opt.MapFrom(src => src.VendorModule.Vendor)));
        }
    }
}
