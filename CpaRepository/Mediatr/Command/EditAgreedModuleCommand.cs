using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using CpaRepository.Service;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Mediatr.AgreedModulesController
{
    public class EditAgreedModuleCommand : IRequest<AgreedModule>
    {
        public AgreedModule AgreedModule;
        public IFormFile FileModule { get; set; }

        public class EditAgreedModuleCommandHandler : IRequestHandler<EditAgreedModuleCommand, AgreedModule>
        {
            private readonly IAgreedModulesRepo _repo;
            private IFileService _fileService;
            private IPathService _pathService;
            private IWebHostEnvironment _appEnvironment;

            public EditAgreedModuleCommandHandler(IAgreedModulesRepo repo,
                IFileService fileService, IPathService pathService, IWebHostEnvironment appEnvironment)
            {
                _repo = repo ?? throw new ArgumentNullException(nameof(repo));
                _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
                _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
                _appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
            }

            public async Task<AgreedModule> Handle(EditAgreedModuleCommand request, CancellationToken cancellationToken)
            {
                var moduleDb = _repo.GetById(request.AgreedModule.Id);
                var nameVendor = _repo.GetNameVendor(request.AgreedModule.VendorModule.VendorId);
                var nameVendorModule = _repo.GetNameVendorModule(request.AgreedModule.VendorModuleId);
                var letter = _repo.GetLetterById(request.AgreedModule.LetterId);
                var fullPath = moduleDb.PathVendorModule;
                if (request.FileModule == null && (request.AgreedModule.VendorModule.VendorId != moduleDb.VendorModule.Vendor.Id
                    || request.AgreedModule.LetterId != moduleDb.Letter.Id || request.AgreedModule.VendorModuleId != moduleDb.VendorModuleId))
                {
                    // Перемещаем файл в другую папку.
                    var pathFolder = _pathService.GetPathFolderForModule(_appEnvironment.WebRootPath, nameVendor, nameVendorModule, letter.DateOfLetter);
                    fullPath = pathFolder + "\\" + moduleDb.PathVendorModule.Split('\\').Last();
                    _fileService.Move(moduleDb.PathVendorModule, fullPath);
                }
                if (request.FileModule != null)
                {
                    // Добавляем файл.
                    _fileService.DeleteFile(moduleDb.PathVendorModule);
                    var pathFolder = _pathService.GetPathFolderForModule(_appEnvironment.WebRootPath, nameVendor, nameVendorModule, letter.DateOfLetter);
                    fullPath = await _fileService.SaveFileAsync(request.FileModule, pathFolder);
                }

                try
                {
                    var updateModule = _repo.GetById(request.AgreedModule.Id);
                    updateModule.Changes = request.AgreedModule.Changes;
                    updateModule.CRC = request.AgreedModule.CRC;
                    updateModule.LetterId = request.AgreedModule.LetterId;
                    updateModule.PathVendorModule = fullPath;
                    updateModule.VendorModuleId = request.AgreedModule.VendorModuleId;
                    updateModule.Version = request.AgreedModule.Version;

                    return await _repo.UpdateAsync(updateModule);
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
