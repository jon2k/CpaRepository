using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using Core.Models;
using MediatR;
using Web.ViewModel.Module;

namespace Web.Mediatr.Query.ModulesController
{
    public class GetModulesQuery : IRequest<IEnumerable<AgreedModule>>
    {
        public DataForFiltr Filtr { get; set; }


        public class GetModulesQueryHandler : IRequestHandler<GetModulesQuery, IEnumerable<AgreedModule>>
        {
            private readonly IAgreedModulesRepo _repo;

            public GetModulesQueryHandler(IAgreedModulesRepo repo)
            {
                _repo = repo;
            }
            public async Task<IEnumerable<AgreedModule>> Handle(GetModulesQuery request, CancellationToken cancellationToken)
            {
                IEnumerable<AgreedModule> agreedModules;
                if (!request.Filtr.IsArchive)
                {
                    if (request.Filtr.SelectedCpaModule == 0 && request.Filtr.SelectedVendor == 0)
                    {
                        agreedModules = _repo.GetAll()
                            .GroupBy(n => n.VendorModule)
                            .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).FirstOrDefault());
                    }
                    else if (request.Filtr.SelectedCpaModule == 0 && request.Filtr.SelectedVendor != 0)
                    {
                        agreedModules = _repo.GetAll()
                            .Where(v => v.VendorModule.VendorId == request.Filtr.SelectedVendor)
                            .GroupBy(n => n.VendorModule)
                            .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).FirstOrDefault());
                    }
                    else if (request.Filtr.SelectedCpaModule != 0 && request.Filtr.SelectedVendor == 0)
                    {
                        agreedModules = _repo.GetAll()
                            .Where(m => m.VendorModule.CpaModules.Any(m => m.Id == request.Filtr.SelectedCpaModule))
                            .GroupBy(n => n.VendorModule)
                            .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).FirstOrDefault());
                    }
                    else
                    {
                        agreedModules = _repo.GetAll()
                            .Where(v => v.VendorModule.VendorId == request.Filtr.SelectedVendor)
                            .Where(m => m.VendorModule.CpaModules.Any(m => m.Id == request.Filtr.SelectedCpaModule))
                            .GroupBy(n => n.VendorModule)
                            .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).FirstOrDefault());
                    }
                }
                else
                {
                    if (request.Filtr.SelectedCpaModule == 0 && request.Filtr.SelectedVendor == 0)
                    {
                        agreedModules = _repo.GetAll()
                            .GroupBy(n => n.VendorModule)
                            .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).Skip(1))
                            .SelectMany(n => n);
                    }
                    else if (request.Filtr.SelectedCpaModule == 0 && request.Filtr.SelectedVendor != 0)
                    {
                        agreedModules = _repo.GetAll()
                            .Where(v => v.VendorModule.VendorId == request.Filtr.SelectedVendor)
                            .GroupBy(n => n.VendorModule)
                            .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).Skip(1))
                            .SelectMany(n => n);
                    }
                    else if (request.Filtr.SelectedCpaModule != 0 && request.Filtr.SelectedVendor == 0)
                    {
                        agreedModules = _repo.GetAll()
                            .Where(m => m.VendorModule.CpaModules.Any(m => m.Id == request.Filtr.SelectedCpaModule))
                            .GroupBy(n => n.VendorModule)
                            .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).Skip(1))
                            .SelectMany(n => n);
                    }
                    else
                    {
                        agreedModules = _repo.GetAll()
                            .Where(v => v.VendorModule.VendorId == request.Filtr.SelectedVendor)
                            .Where(m => m.VendorModule.CpaModules.Any(m => m.Id == request.Filtr.SelectedCpaModule))
                            .GroupBy(n => n.VendorModule)
                            .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).Skip(1))
                            .SelectMany(n => n);
                    }

                }
                return agreedModules;
            }
        }
    }
}
