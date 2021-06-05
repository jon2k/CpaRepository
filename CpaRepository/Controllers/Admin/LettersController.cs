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
using AutoMapper;

namespace CpaRepository.Controllers
{
    public class LettersController : Controller
    {
        private readonly ILogger<LettersController> _logger;
        private LetterRepo _repo;
        private IWebHostEnvironment _appEnvironment;
        private IFileService _fileService;
        private IPathService _pathService;
        public LettersController(LetterRepo context, ILogger<LettersController> logger,
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
                var letters = _repo.GetAll().OrderByDescending(n => n.DateOfLetter);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Letter, LetterViewModel>()
                    .ForMember(nameof(LetterViewModel.ExistLetter), opt => opt
                    .MapFrom(src => System.IO.File.Exists(src.PathLetter))));
                var mapper = new Mapper(config);
                var vm = mapper.Map<List<LetterViewModel>>(letters);

                return View(vm);
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
        public async Task<ActionResult> Create(LetterViewModel letterViewModel)
        {
            try
            {
                if (letterViewModel.FileLetter != null)
                {
                    var nameVendor = _repo.GetNameVendor(letterViewModel.VendorId);
                    var pathFolder = _pathService.GetPathFolderForLetter(nameVendor, letterViewModel.DateOfLetter);
                    var fullPath = await _fileService.SaveFileAsync(letterViewModel.FileLetter, pathFolder);
                    try
                    {
                        var config = new MapperConfiguration(cfg => cfg.CreateMap<LetterViewModel, Letter>()
                            .ForMember(nameof(Letter.PathLetter), opt => opt
                            .MapFrom(src => fullPath)));
                        var mapper = new Mapper(config);
                        var letter = mapper.Map<Letter>(letterViewModel);

                        await _repo.AddAsync(letter);
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
                var letter = _repo.GetById(id);
                var vendor = _repo.GetAllVendors();
                ViewBag.VendorId = vendor.Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name,
                }).ToList();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Letter, LetterViewModel>());
                var mapper = new Mapper(config);
                var letterViewModel = mapper.Map<LetterViewModel>(letter);

                return View(letterViewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Letters));
            }
        }
        [HttpPost]
        public async Task<ActionResult> Edit(LetterViewModel letterViewModel)
        {
            try
            {
                var letterDb = _repo.GetById(letterViewModel.Id);
                var nameVendor = _repo.GetNameVendor(letterViewModel.VendorId);
                var fullPath = letterDb.PathLetter;
                if (letterViewModel.FileLetter == null && (letterViewModel.VendorId != letterDb.VendorId || letterViewModel.DateOfLetter != letterDb.DateOfLetter))
                {
                    // Перемещаем файл в другую папку.
                    var pathFolder = _pathService.GetPathFolderForLetter(nameVendor, letterViewModel.DateOfLetter);
                    fullPath = pathFolder + "\\" + letterDb.PathLetter.Split('\\').Last();
                    _fileService.Move(letterDb.PathLetter, fullPath);
                }
                if (letterViewModel.FileLetter != null)
                {
                    // Добавляем файл.
                    _fileService.DeleteFile(letterDb.PathLetter);
                    //  var nameVendor = _repo.GetNameVendor(letter.VendorId);
                    var pathFolder = _pathService.GetPathFolderForLetter(nameVendor, letterViewModel.DateOfLetter);
                    fullPath = await _fileService.SaveFileAsync(letterViewModel.FileLetter, pathFolder);
                }
                try
                {
                    var updateLetter = _repo.GetById(letterViewModel.Id);
                    updateLetter.NumberLetter = letterViewModel.NumberLetter;
                    updateLetter.PathLetter = fullPath;
                    updateLetter.VendorId = letterViewModel.VendorId;
                    updateLetter.DateOfLetter = letterViewModel.DateOfLetter;

                    await _repo.UpdateAsync(updateLetter);
                    return RedirectToAction(nameof(Letters));
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    // Удаляем файл, если запись в БД не прошла
                    _fileService.DeleteFile(fullPath);
                    return View(letterViewModel.Id);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View(letterViewModel.Id);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var module = _repo.GetById(id);
                // ViewBag.Vendor = _repo.GetNameVendor(module.VendorId);
                return View(module);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Letters));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var letter = _repo.GetById(id);
                await _repo.DeleteAsync(letter);
                _fileService.DeleteFile(letter.PathLetter);

                return RedirectToAction(nameof(Letters));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View();
            }
        }
       
        public IActionResult DownloadFile(int id)
        {
            try
            {
                var letter = _repo.GetById(id);
                if (letter.PathLetter != null)
                {
                    return PhysicalFile(letter.PathLetter, "application/octet-stream", letter.PathLetter.Split('\\').Last());
                }
                else
                {
                    _logger.LogError("Отсутствует полный путь к файлу письма.");
                    return RedirectToAction(nameof(Letters));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Letters));
            }
        }
    }
}
