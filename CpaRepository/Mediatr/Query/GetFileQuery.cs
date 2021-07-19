using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Mediatr.Query
{
    public class GetFileQuery : IRequest<PhysicalFileResult>
    {
        public int Id { get; set; }
        public class GetFileQueryHandler : ControllerBase, IRequestHandler<GetFileQuery, PhysicalFileResult>
        {
            private readonly IAgreedModulesRepo _repo;

            public GetFileQueryHandler(IAgreedModulesRepo repo)
            {
                _repo = repo;
            }
            public async Task<PhysicalFileResult> Handle(GetFileQuery request, CancellationToken cancellationToken)
            {
                var module = _repo.GetById(request.Id);
                if (module.PathVendorModule != null)
                {
                    return PhysicalFile(module.PathVendorModule, "application/octet-stream", module.PathVendorModule.Split('\\').Last());
                }
                else
                {
                    return null;
                }
            }
        }
    }
}