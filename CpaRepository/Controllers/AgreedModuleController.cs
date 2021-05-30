using CpaRepository.EF;
using CpaRepository.Service;
using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using CpaRepository.ViewModel.AgreedModules;
using CpaRepository.ViewModel.VendorModule;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace CpaRepository.Controllers
{
    public class AgreedModuleController : Controller
    {
        private readonly ILogger<VendorModuleController> _logger;
        private AgreedModulesRepo _repo;
        private IWebHostEnvironment _appEnvironment;
        private IFileService _fileService;
        private IPathService _pathService;
        public AgreedModuleController(AgreedModulesRepo context, ILogger<VendorModuleController> logger,
            IWebHostEnvironment appEnvironment, IFileService fileService, IPathService pathService)
        {
            _repo = context;
            _logger = logger;
            _appEnvironment = appEnvironment;
            _fileService = fileService;
            _pathService = pathService;
        }
        public ActionResult AgreedModules()
        {
            try
            {
                var mapper = new Mapper(GetMapConfigModelToViewModel());
                var vm = mapper.Map<List<AgreedModuleViewModel>>(_repo.GetAll()).OrderByDescending(m => m.DateOfLetter);

                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "AgreedModuleController");
            }
        }
        public ActionResult Create()
        {
            try
            {
                var vendor = _repo.GetAllVendors();
                ViewBag.VendorId = vendor.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.Name }).ToList();
                var vendorModules = _repo.GetVendorModulesOneVendor(vendor.FirstOrDefault().Id);
                ViewBag.VendorModuleId = vendorModules.Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.NameModule
                }).ToList();
                var letters = _repo.GetLettersOneVendor(vendor.FirstOrDefault().Id);
                ViewBag.LettersId = letters.Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.NumberLetter
                }).ToList();
                return View();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(AgreedModules));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AgreedModuleViewModel agreedModuleVM)
        {
            try
            {
                if (agreedModuleVM.FileModule != null)
                {
                    var nameVendor = _repo.GetNameVendor(agreedModuleVM.VendorId);
                    var nameVendorModule = _repo.GetNameVendorModule(agreedModuleVM.VendorModuleId);
                    var letter = _repo.GetLetterById(agreedModuleVM.LetterId);
                    var path = _pathService.GetPathFolderForModule(nameVendor, nameVendorModule, letter.DateOfLetter);

                    var fullPath = await _fileService.SaveFileAsync(agreedModuleVM.FileModule, path);

                    try
                    {
                        var config = new MapperConfiguration(cfg => cfg.CreateMap<AgreedModuleViewModel, AgreedModule>()
                           .ForMember(nameof(AgreedModule.PathVendorModule), opt => opt.MapFrom(src => fullPath))
                           .ForMember(nameof(AgreedModule.Letter), opt => opt.MapFrom(src => letter)));
                        var mapper = new Mapper(config);
                        var agreedModule = mapper.Map<AgreedModule>(agreedModuleVM);

                        await _repo.AddAsync(agreedModule);
                        return RedirectToAction(nameof(AgreedModules));
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        // Удаляем файл, если запись в БД не прошла
                        _fileService.DeleteFile(fullPath);
                        return View();
                    }
                }
                else
                {
                    _logger.LogError("Отсуствует файл.");
                    return View();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            try
            {
                var model = _repo.GetById(id);
                var vendor = _repo.GetAllVendors();
                ViewBag.VendorId = vendor.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.Name }).ToList();
                var vendorModules = _repo.GetVendorModulesOneVendor(model.VendorModule.VendorId);
                ViewBag.VendorModuleId = vendorModules.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.NameModule }).ToList();
                var letters = _repo.GetLettersOneVendor(model.VendorModule.VendorId);
                ViewBag.LettersId = letters.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.NumberLetter }).ToList();
                var mapper = new Mapper(GetMapConfigModelToViewModel());
                var vm = mapper.Map<AgreedModuleViewModel>(model);

                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(AgreedModules));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AgreedModuleViewModel module)
        {
            try
            {
                var moduleDb = _repo.GetById(module.Id);
                var nameVendor = _repo.GetNameVendor(module.VendorId);
                var nameVendorModule = _repo.GetNameVendorModule(module.VendorModuleId);
                var letter = _repo.GetLetterById(module.LetterId);
                var fullPath = moduleDb.PathVendorModule;
                if (module.FileModule == null && (module.VendorId != moduleDb.VendorModule.Vendor.Id
                    || module.LetterId != moduleDb.Letter.Id || module.VendorModuleId != moduleDb.VendorModuleId))
                {
                    // Перемещаем файл в другую папку.
                    var pathFolder = _pathService.GetPathFolderForModule(nameVendor, nameVendorModule, letter.DateOfLetter);
                    fullPath = pathFolder + "\\" + moduleDb.PathVendorModule.Split('\\').Last();
                    _fileService.Move(moduleDb.PathVendorModule, fullPath);
                }
                if (module.FileModule != null)
                {
                    // Добавляем файл.
                    _fileService.DeleteFile(moduleDb.PathVendorModule);
                    var pathFolder = _pathService.GetPathFolderForModule(nameVendor, nameVendorModule, letter.DateOfLetter);
                    fullPath = await _fileService.SaveFileAsync(module.FileModule, pathFolder);
                }

                try
                {
                    var updateModule = _repo.GetById(module.Id);
                    updateModule.Changes = module.Changes;
                    updateModule.CRC = module.CRC;
                    updateModule.LetterId = module.LetterId;
                    updateModule.PathVendorModule = fullPath;
                    updateModule.VendorModuleId = module.VendorModuleId;
                    updateModule.Version = module.Version;

                    await _repo.UpdateAsync(updateModule);
                    return RedirectToAction(nameof(AgreedModules));
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    // Удаляем файл, если запись в БД не прошла
                    _fileService.DeleteFile(fullPath);
                    return View(module.Id);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View(module.Id);
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var model = _repo.GetById(id);
                var mapper = new Mapper(GetMapConfigModelToViewModel());
                var vm = mapper.Map<AgreedModuleViewModel>(model);

                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(AgreedModules));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var module = _repo.GetById(id);
                await _repo.DeleteAsync(module);
                _fileService.DeleteFile(module.PathVendorModule);
                return RedirectToAction(nameof(AgreedModules));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View();
            }
        }

        public ActionResult GetVendorModules(int id)
        {
            try
            {
                ViewBag.VendorModules = _repo.GetVendorModulesOneVendor(id);
                return PartialView();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return PartialView();
            }
        }
        public ActionResult GetLetters(int id)
        {
            try
            {
                ViewBag.Letters = _repo.GetLettersOneVendor(id);
                return PartialView();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return PartialView();
            }
        }
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
                    return RedirectToAction(nameof(AgreedModules));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(AgreedModules));
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
