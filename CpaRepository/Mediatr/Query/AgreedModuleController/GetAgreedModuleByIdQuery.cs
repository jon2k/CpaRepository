using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using Core.Models;
using MediatR;

namespace Web.Mediatr.Query.AgreedModuleController
{
    public class GetAgreedModuleByIdQuery : IRequest<AgreedModule>
    {
        public int Id { get; set; }

        public class GetAgreedModuleByIdQueryHandler : IRequestHandler<GetAgreedModuleByIdQuery, AgreedModule>
        {
            private readonly IAgreedModulesRepo _repo;

            public GetAgreedModuleByIdQueryHandler(IAgreedModulesRepo repo)
            {
                _repo = repo;
            }
            public async Task<AgreedModule> Handle(GetAgreedModuleByIdQuery request, CancellationToken cancellationToken)
            {
                return _repo.GetById(request.Id);
            }
        }
    }
}
