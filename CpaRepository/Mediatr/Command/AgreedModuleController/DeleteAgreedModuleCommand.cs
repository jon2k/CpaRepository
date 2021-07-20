using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using Core.Interfaces.FileSystem;

namespace Web.Mediatr.Command.AgreedModuleController
{
    public class DeleteAgreedModuleCommand : IRequest<int>
    {
        public int Id { get; set; }

        public class DeleteAgreedModuleCommandHandler : IRequestHandler<DeleteAgreedModuleCommand, int>
        {
            private readonly IAgreedModulesRepo _repo;
            private IFileService _fileService;

            public DeleteAgreedModuleCommandHandler(IAgreedModulesRepo repo, IFileService fileService)
            {
                _repo = repo;
                _fileService = fileService;
            }

            public async Task<int> Handle(DeleteAgreedModuleCommand request, CancellationToken cancellationToken)
            {
                var module = _repo.GetById(request.Id);
                _fileService.DeleteFile(module.PathVendorModule);
                return await _repo.DeleteAsync(module);

            }
        }
    }
}
