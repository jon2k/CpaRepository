using CpaRepository.Repository;
using CpaRepository.Service;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Mediatr.Command
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
                _repo = repo ?? throw new ArgumentNullException(nameof(repo));
                _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));              
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
