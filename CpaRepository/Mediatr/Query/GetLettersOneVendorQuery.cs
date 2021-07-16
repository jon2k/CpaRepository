using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Mediatr.Query
{
    public class GetLettersOneVendorQuery : IRequest<IEnumerable<Letter>>
    {
        public int Id { get; set; }

        public class GetLettersOneVendorQueryHandler : IRequestHandler<GetLettersOneVendorQuery, IEnumerable<Letter>>
        {
            private readonly IAgreedModulesRepo _repo;

            public GetLettersOneVendorQueryHandler(IAgreedModulesRepo repo)
            {
                _repo = repo;
            }
            public async Task<IEnumerable<Letter>> Handle(GetLettersOneVendorQuery request, CancellationToken cancellationToken)
            {
                return _repo.GetLettersOneVendor(request.Id);
            }
        }
    }
}
