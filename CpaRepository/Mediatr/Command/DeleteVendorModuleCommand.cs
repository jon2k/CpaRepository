using Core.Interfaces.EF;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Mediatr.Command
{
    public class DeleteVendorModuleCommand : IRequest<(int,int)>
    {
        public int Id { get; set; }

        public class DeleteVendormoduleCommandHandler : IRequestHandler<DeleteVendorModuleCommand, (int,int)>
        {
            private readonly IVendorModuleRepo _repo;

            public DeleteVendormoduleCommandHandler(IVendorModuleRepo repo)
            {
                _repo = repo;
            }

            public async Task<(int,int)> Handle(DeleteVendorModuleCommand request, CancellationToken cancellationToken)
            {
                var module = _repo.GetById(request.Id);
                var sucs= await _repo.DeleteAsync(module);
                return new(sucs, module.VendorId);
            }
        }
    }
}
