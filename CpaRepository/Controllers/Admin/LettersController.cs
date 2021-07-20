using AutoMapper;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Mediatr.Command.LetterController;
using Web.Mediatr.Query.LetterController;
using Web.ViewModel.Letter;

namespace Web.Controllers.Admin
{
    public class LettersController : Controller
    {
        private readonly ILogger<LettersController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public LettersController(IMapper mapper, IMediator mediator, ILogger<LettersController> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));      
        }
        public async Task< ActionResult> Letters()
        {
            try
            {             
                var letters = await _mediator.Send(new GetAllLettersQuery());
                var vm = _mapper.Map<IEnumerable<LetterViewModel>>(letters).OrderByDescending(n => n.DateOfLetter);
              
                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "HomeController");
            }
        }
        
        public async Task<ActionResult> Create()
        {
            try
            {
                var vm = await _mediator.Send(new GetVmForLetterCreateQuery());            
                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Letters));
            }
        }
       
        [HttpPost]
        public async Task<ActionResult> Create(LetterViewModel vm)
        {
            try
            {
                if (vm.FileLetter != null)
                {        
                    var letter = await _mediator.Send(new CreateLetterCommand()
                    {
                        Letter = _mapper.Map<Letter>(vm),
                        FileLetter = vm.FileLetter
                    });
                    if (letter != null)
                    {
                        _logger.LogInformation($"Добавлен согласованный модуль. " +
                           $"Вендор - {letter.Vendor.Name}, " +
                           $"Номер - {letter.NumberLetter}, " +
                           $"Время - {DateTime.Now}");
                        return RedirectToAction(nameof(Letters));
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
                var vm = await _mediator.Send(new GetVmForLetterEditQuery() { Id = id });                

                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Letters));
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(LetterViewModel vm)
        {
            try
            {
                var res = await _mediator.Send(new EditLetterCommand()
                {
                    Letter = _mapper.Map<Letter>(vm),
                    FileLetter = vm.FileLetter
                });

                return RedirectToAction(nameof(Letters));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View(vm.Id);
            }          
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var letter = await _mediator.Send(new GetLetterByIdLetterQuery() { Id = id });              
                return View(letter);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Letters));
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _mediator.Send(new DeleteLetterCommand() { Id = id });                           
                return RedirectToAction(nameof(Letters));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View();
            }
        }
       
        public async Task<IActionResult> DownloadFile(int id)
        {
            try
            {              
                var file = await _mediator.Send(new GetFileLetterQuery() { Id = id });
                if (file != null)
                {
                    return file;
                }
                else
                {
                    _logger.LogError("Отсутствует файл письма.");
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
