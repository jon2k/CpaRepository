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
    public class ModulesController : Controller
    {
        private readonly ILogger<ModulesController> _logger;
        private AgreedModulesRepo _repo;

        public ModulesController(AgreedModulesRepo context, ILogger<ModulesController> logger)
        {
            _repo = context;
            _logger = logger;
        }
        public IActionResult ActualModules(int id = 0)
        {
            try
            {
                ViewData["Title"] = "Актуальные модули";
                ViewBag.IsArchive = false;
                var vendor = _repo.GetAllVendors();
                var vendors = new List<SelectListItem>();
                vendors.Add(new SelectListItem() { Value = "0", Text = "Все вендоры" });
                vendors.AddRange(vendor.Select((n, i) => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name,
                    Selected = i + 1 == id ? true : false
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
                IEnumerable<AgreedModule> agreedModules;
                if (id == 0)
                {
                    agreedModules = _repo.GetAll()
                                .GroupBy(n => n.VendorModule)
                                .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).FirstOrDefault());
                }
                else
                {
                    agreedModules = _repo.GetAgreedModulesOneVendor(id)
                                .GroupBy(n => n.VendorModule)
                                .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).FirstOrDefault());
                }
                var mapper = new Mapper(GetMapConfigModelToViewModel());
                var vm = mapper.Map<List<ModuleViewModel>>(agreedModules).OrderByDescending(m => m.DateOfLetter);

                return View("Modules", vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "HomeController");
            }

        }
        public IActionResult ArchiveModules()
        {
            try
            {
                ViewData["Title"] = "Архивные модули";
                ViewBag.IsArchive = true;
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

                var agreedModules = _repo.GetAll()
                            .GroupBy(n => n.VendorModule)
                            .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).Skip(1)).SelectMany(n => n);
                var mapper = new Mapper(GetMapConfigModelToViewModel());
                var vm = mapper.Map<List<ModuleViewModel>>(agreedModules).OrderByDescending(m => m.DateOfLetter);

                return View("Modules", vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "HomeController");
            }

        }

        [HttpPost]
        public ActionResult GetModules(DataForFiltr filtr)
        {
            try
            {
                IEnumerable<AgreedModule> agreedModules;
                if (!filtr.IsArchive)
                {
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
                }
                else
                {
                    if (filtr.SelectedCpaModule == 0 && filtr.SelectedVendor == 0)
                    {
                        agreedModules = _repo.GetAll()
                            .GroupBy(n => n.VendorModule)
                            .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).Skip(1))
                            .SelectMany(n => n);
                    }
                    else if (filtr.SelectedCpaModule == 0 && filtr.SelectedVendor != 0)
                    {
                        agreedModules = _repo.GetAll()
                            .Where(v => v.VendorModule.VendorId == filtr.SelectedVendor)
                            .GroupBy(n => n.VendorModule)
                            .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).Skip(1))
                            .SelectMany(n => n);
                    }
                    else if (filtr.SelectedCpaModule != 0 && filtr.SelectedVendor == 0)
                    {
                        agreedModules = _repo.GetAll()
                            .Where(m => m.VendorModule.CpaModules.Any(m => m.Id == filtr.SelectedCpaModule))
                            .GroupBy(n => n.VendorModule)
                            .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).Skip(1))
                            .SelectMany(n => n);
                    }
                    else
                    {
                        agreedModules = _repo.GetAll()
                            .Where(v => v.VendorModule.VendorId == filtr.SelectedVendor)
                            .Where(m => m.VendorModule.CpaModules.Any(m => m.Id == filtr.SelectedCpaModule))
                            .GroupBy(n => n.VendorModule)
                            .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).Skip(1))
                            .SelectMany(n => n);
                    }
                }
                var mapper = new Mapper(GetMapConfigModelToViewModel());
                var vm = mapper.Map<List<ModuleViewModel>>(agreedModules).OrderByDescending(m => m.DateOfLetter);

                return PartialView(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return PartialView();
            }
        }
        [HttpPost]

        public IActionResult DownloadFile(int id)
        {
            try
            {
                var module = _repo.GetById(id);
                if (module.PathVendorModule != null)
                {
                    return PhysicalFile(module.PathVendorModule, "application/octet-stream", module.PathVendorModule.Split('\\').Last());
                }
                else
                {
                    _logger.LogError("Отсутствует полный путь к файлу письма.");
                    return RedirectToAction(nameof(ActualModules));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(ActualModules));
            }
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
