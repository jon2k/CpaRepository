using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using Core.Interfaces.FileSystem;
using Core.Models;

namespace Web.Mediatr.Command
{
    public class CreateLetterCommand : IRequest<Letter>
    {
        public Letter Letter;
        public IFormFile FileLetter { get; set; }

        public class CreateLetterCommandHandler : IRequestHandler<CreateLetterCommand, Letter>
        {
            private readonly ILetterRepo _repo;
            private IFileService _fileService;
            private IPathService _pathService;
            private IWebHostEnvironment _appEnvironment;

            public CreateLetterCommandHandler(ILetterRepo repo,
                IFileService fileService, IPathService pathService, IWebHostEnvironment appEnvironment)
            {
                _repo = repo ?? throw new ArgumentNullException(nameof(repo));
                _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
                _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
                _appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
            }

            public async Task<Letter> Handle(CreateLetterCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var nameVendor = _repo.GetNameVendor(request.Letter.VendorId);
                    var pathFolder = _pathService.GetPathFolderForLetter(_appEnvironment.WebRootPath, nameVendor, request.Letter.DateOfLetter);
                    var fullPath = await _fileService.SaveFileAsync(request.FileLetter, pathFolder);

                    try
                    {
                        request.Letter.PathLetter = fullPath;            
                        return await _repo.AddAsync(request.Letter);
                    }
                    catch (Exception e)
                    {
                        // Удаляем файл, если запись в БД не прошла
                        _fileService.DeleteFile(fullPath);
                        throw;
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
