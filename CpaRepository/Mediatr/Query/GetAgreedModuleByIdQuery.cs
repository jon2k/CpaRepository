using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Mediatr.Query
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
