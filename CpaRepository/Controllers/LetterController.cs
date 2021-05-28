using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using CpaRepository.Service;
using CpaRepository.ViewModel.Letter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Controllers
{
    public class LetterController : Controller
    {
        private readonly ILogger<LetterController> _logger;
        private LetterRepo _repo;
        private IWebHostEnvironment _appEnvironment;
        private IFileService _fileService;
        private IPathService _pathService;
        public LetterController(LetterRepo context, ILogger<LetterController> logger,
            IWebHostEnvironment appEnvironment, IFileService fileService, IPathService pathService)
        {
            _repo = context;
            _logger = logger;
            _appEnvironment = appEnvironment;
            _fileService = fileService;
            _pathService = pathService;
        }
        public ActionResult Letters()
        {
            try
            {
                IEnumerable<Letter> vendors = _repo.GetAll();
                return View(vendors);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "HomeController");
            }
        }
        public ActionResult Create()
        {
            try
            {
                var vendor = _repo.GetAllVendors();
                ViewBag.VendorId = vendor.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.Name }).ToList();
                // var vendorModules = _repo.GetVendorModulesOneVendor(vendor.FirstOrDefault().Id);
                // ViewBag.VendorModuleId = vendorModules.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.NameModule }).ToList();
                return View();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Letters));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LetterViewModel letter)
        {
            try
            {
                if (letter.FileLetter != null)
                {
                    var nameVendor = _repo.GetNameVendor(letter.VendorId);
                    var pathFolder = _pathService.GetPathFolderForLetter(nameVendor, letter.DateOfLetter);
                    var fullPath = await _fileService.SaveFileAsync(letter.FileLetter, pathFolder);
                    try
                    {
                        var newLetter = new Letter()
                        {
                            DateOfLetter = letter.DateOfLetter,
                            NumberLetter = letter.NumberLetter,
                            VendorId = letter.VendorId,
                            PathLetter = fullPath
                        };
                        await _repo.AddAsync(newLetter);
                        return RedirectToAction(nameof(Letters));
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        // Удаляем файл, если запись в БД не прошла
                        _fileService.DeleteFile(fullPath);
                        return View();
                    }
                }
                else return View();
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
                // var vendorModules = _repo.GetVendorModulesOneVendor(model.VendorModule.VendorId);
                // ViewBag.VendorModuleId = vendorModules.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.NameModule }).ToList();
                var vm = new LetterViewModel
                {
                    Id = model.Id,
                    DateOfLetter = model.DateOfLetter,
                    Vendor = model.Vendor,
                    NumberLetter=model.NumberLetter
                };
                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Letters));
            }
        }
        [HttpPost]
        public async Task<ActionResult> Edit(LetterViewModel letter)
        {
            try
            {
                var letterDb = _repo.GetById(letter.Id);
                var nameVendor = _repo.GetNameVendor(letter.VendorId);
                var fullPath = letterDb.PathLetter;
                if (letter.FileLetter == null && (letter.VendorId!=letterDb.VendorId || letter.DateOfLetter!=letterDb.DateOfLetter))
                {
                    // Перемещаем файл в другую папку.
                    var pathFolder = _pathService.GetPathFolderForLetter(nameVendor, letter.DateOfLetter);
                    var fullNewPath = pathFolder +"\\" +letterDb.PathLetter.Split('\\').Last();
                    _fileService.Move(letterDb.PathLetter, fullNewPath);
                }
                if (letter.FileLetter != null)
                {
                    // Добавляем файл.
                    _fileService.DeleteFile(letterDb.PathLetter);
                  //  var nameVendor = _repo.GetNameVendor(letter.VendorId);
                    var pathFolder = _pathService.GetPathFolderForLetter(nameVendor, letter.DateOfLetter);
                    fullPath = await _fileService.SaveFileAsync(letter.FileLetter, pathFolder);
                }

                try
                {
                    var newLetter = _repo.GetById(letter.Id);
                    newLetter.NumberLetter = letter.NumberLetter;
                    newLetter.PathLetter = fullPath;
                    newLetter.VendorId = letter.VendorId;

                    await _repo.UpdateAsync(newLetter);
                    return RedirectToAction(nameof(Letters));
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    // Удаляем файл, если запись в БД не прошла
                    _fileService.DeleteFile(fullPath);
                    return View(letter.Id);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View(letter.Id);
            }
        }
    }
}
