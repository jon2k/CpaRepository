using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using Core.Models;
using MediatR;

namespace Web.Mediatr.Query.LetterController
{
    public class GetLetterByIdLetterQuery : IRequest<Letter>
    {
        public int Id { get; set; }

        public class GetLetterByIdLetterQueryHandler : IRequestHandler<GetLetterByIdLetterQuery, Letter>
        {
            private readonly ILetterRepo _repo;

            public GetLetterByIdLetterQueryHandler(ILetterRepo repo)
            {
                _repo = repo;
            }
            public async Task<Letter> Handle(GetLetterByIdLetterQuery request, CancellationToken cancellationToken)
            {
                return _repo.GetById(request.Id);
            }
        }
    }
}
