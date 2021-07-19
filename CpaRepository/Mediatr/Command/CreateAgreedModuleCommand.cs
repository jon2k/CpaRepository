using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using Core.Interfaces.FileSystem;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Web.Mediatr.Command
{
    public class CreateAgreedModuleCommand : IRequest<AgreedModule>
    {
        public AgreedModule AgreedModule;
        public IFormFile FileModule { get; set; }

        public class CreateAgreedModuleCommandHandler : IRequestHandler<CreateAgreedModuleCommand, AgreedModule>
        {
            private readonly IAgreedModulesRepo _repo;
            private IFileService _fileService;
            private IPathService _pathService;
            private IWebHostEnvironment _appEnvironment;

            public CreateAgreedModuleCommandHandler(IAgreedModulesRepo repo, 
                IFileService fileService, IPathService pathService, IWebHostEnvironment appEnvironment)
            {
                _repo = repo ?? throw new ArgumentNullException(nameof(repo));
                _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
                _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
                _appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
            }

            public async Task<AgreedModule> Handle(CreateAgreedModuleCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var nameVendor = _repo.GetNameVendor(request.AgreedModule.VendorModule.VendorId);
                    var nameVendorModule = _repo.GetNameVendorModule(request.AgreedModule.VendorModuleId);
                    var letter = _repo.GetLetterById(request.AgreedModule.LetterId);
                    var path = _pathService.GetPathFolderForModule(_appEnvironment.WebRootPath, nameVendor, nameVendorModule, letter.DateOfLetter);

                    var fullPath = await _fileService.SaveFileAsync(request.FileModule, path);

                    try
                    {
                        request.AgreedModule.PathVendorModule = fullPath;
                        request.AgreedModule.Letter = letter;
                        request.AgreedModule.VendorModule = null;
                        return await _repo.AddAsync(request.AgreedModule);
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
