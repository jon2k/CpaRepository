using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Mediatr.Query.LetterController
{
    public class GetFileLetterQuery : IRequest<PhysicalFileResult>
    {
        public int Id { get; set; }
        public class GetFileLetterQueryHandler : ControllerBase, IRequestHandler<GetFileLetterQuery, PhysicalFileResult>
        {
            private readonly ILetterRepo _repo;

            public GetFileLetterQueryHandler(ILetterRepo repo)
            {
                _repo = repo;
            }
            public async Task<PhysicalFileResult> Handle(GetFileLetterQuery request, CancellationToken cancellationToken)
            {
                var module = _repo.GetById(request.Id);
                if (module.PathLetter != null)
                {
                    return PhysicalFile(module.PathLetter, "application/octet-stream", module.PathLetter.Split('\\').Last());
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
