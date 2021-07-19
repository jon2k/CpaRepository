using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using Core.Interfaces.FileSystem;
using Core.Models;

namespace Web.Mediatr.Command
{
    public class EditLetterCommand : IRequest<Letter>
    {
        public Letter Letter;
        public IFormFile FileLetter { get; set; }

        public class EditLetterCommandHandler : IRequestHandler<EditLetterCommand, Letter>
        {
            private readonly ILetterRepo _repo;
            private IFileService _fileService;
            private IPathService _pathService;
            private IWebHostEnvironment _appEnvironment;

            public EditLetterCommandHandler(ILetterRepo repo,
                IFileService fileService, IPathService pathService, IWebHostEnvironment appEnvironment)
            {
                _repo = repo ?? throw new ArgumentNullException(nameof(repo));
                _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
                _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
                _appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
            }

            public async Task<Letter> Handle(EditLetterCommand request, CancellationToken cancellationToken)
            {
                var letterDb = _repo.GetById(request.Letter.Id);
                var nameVendor = _repo.GetNameVendor(request.Letter.VendorId);
                var fullPath = letterDb.PathLetter;
                if (request.FileLetter == null && (request.Letter.VendorId != letterDb.VendorId || request.Letter.DateOfLetter != letterDb.DateOfLetter))
                {
                    // Перемещаем файл в другую папку.
                    var pathFolder = _pathService.GetPathFolderForLetter(_appEnvironment.WebRootPath, nameVendor, request.Letter.DateOfLetter);
                    fullPath = pathFolder + "\\" + letterDb.PathLetter.Split('\\').Last();
                    _fileService.Move(letterDb.PathLetter, fullPath);
                }
                if (request.FileLetter != null)
                {
                    // Добавляем файл.
                    _fileService.DeleteFile(letterDb.PathLetter);
                    //  var nameVendor = _repo.GetNameVendor(letter.VendorId);
                    var pathFolder = _pathService.GetPathFolderForLetter(_appEnvironment.WebRootPath, nameVendor, request.Letter.DateOfLetter);
                    fullPath = await _fileService.SaveFileAsync(request.FileLetter, pathFolder);
                }

                try
                {
                    var updateLetter = _repo.GetById(request.Letter.Id);
                    updateLetter.NumberLetter = request.Letter.NumberLetter;
                    updateLetter.PathLetter = fullPath;
                    updateLetter.VendorId = request.Letter.VendorId;
                    updateLetter.DateOfLetter = request.Letter.DateOfLetter;

                    return await _repo.UpdateAsync(updateLetter);
                }
                catch
                {
                    // Удаляем файл, если запись в БД не прошла
                    _fileService.DeleteFile(fullPath);
                    throw;
                }
            }
        }
    }
}