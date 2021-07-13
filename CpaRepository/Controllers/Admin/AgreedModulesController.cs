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
using MediatR;
using Web.Mediatr.AgreedModulesController;
using Web.Mediatr.Query;

namespace CpaRepository.Controllers
{
    public class AgreedModulesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<VendorModuleController> _logger;
        private IAgreedModulesRepo _repo;
        private IWebHostEnvironment _appEnvironment;
        private IFileService _fileService;
        private IPathService _pathService;
        public AgreedModulesController(IMapper mapper, IMediator mediator, IAgreedModulesRepo context, ILogger<VendorModuleController> logger,
            IWebHostEnvironment appEnvironment, IFileService fileService, IPathService pathService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repo = context;
            _logger = logger;
            _appEnvironment = appEnvironment;
            _fileService = fileService;
            _pathService = pathService;
        }
        public async Task<ActionResult> AgreedModules()
        {
            try
            {
                var agreedModules = await _mediator.Send(new GetAgreedModulesQuery());
                var agreedModulesVM = _mapper.Map<IEnumerable<AgreedModuleViewModel>>(agreedModules);
                return View(agreedModulesVM);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "AgreedModuleController");
            }
        }
        public async Task<ActionResult> Create()
        {
            try
            {
                var vm = await _mediator.Send(new GetVmForAgreedModuleCreateQuery());
                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(AgreedModules));
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create(AgreedModuleViewModel agreedModuleVM)
        {
            try
            {
                if (agreedModuleVM.FileModule != null)
                {
                    var agreedModule = _mapper.Map<AgreedModule>(agreedModuleVM);
                    var module = await _mediator.Send(new EditAgreedModuleCommand()
                    {
                        AgreedModule = agreedModule,
                        FileModule = agreedModuleVM.FileModule
                    });
                    if (module != null)
                    {
                        _logger.LogInformation($"Добавлен согласованный модуль. " +
                           $"Вендор - {agreedModule.VendorModule.Vendor.Name}, " +
                           $"Модуль - {agreedModule.VendorModule.NameModule}, " +
                           $"Версия - {agreedModule.Version}, " +
                           $"CRC - {agreedModule.CRC}, " +
                           $"Время - {DateTime.Now}");
                        return RedirectToAction(nameof(AgreedModules));
                    }
                    else
                    {
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
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var vm = await _mediator.Send(new GetVmForAgreedModuleEditQuery() { Id = id });
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
                var agreedModule = _mapper.Map<AgreedModule>(module);
                var res= await _mediator.Send(new EditAgreedModuleCommand()
                {
                    AgreedModule = agreedModule,
                    FileModule = module.FileModule
                });
               
                return RedirectToAction(nameof(AgreedModules));

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
