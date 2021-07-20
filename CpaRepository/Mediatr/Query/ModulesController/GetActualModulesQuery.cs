using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Interfaces.EF;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.ViewModel.Module;

namespace Web.Mediatr.Query.ModulesController
{
    public class GetActualModulesQuery : IRequest<ModuleVM>
    {
        public int Id { get; set; }

        public class GetActualModulesQueryHandler : IRequestHandler<GetActualModulesQuery, ModuleVM>
        {
            private readonly IAgreedModulesRepo _repo;
            private readonly IMapper _mapper;

            public GetActualModulesQueryHandler(IAgreedModulesRepo repo, IMapper mappper)
            {
                _repo = repo;
                _mapper = mappper;
            }
            public async Task<ModuleVM> Handle(GetActualModulesQuery request, CancellationToken cancellationToken)
            {
                IEnumerable<AgreedModule> agreedModules;
                if (request.Id == 0)
                {
                    agreedModules = _repo.GetAll()
                                .GroupBy(n => n.VendorModule)
                                .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).FirstOrDefault());
                }
                else
                {
                    agreedModules = _repo.GetAgreedModulesOneVendor(request.Id)
                                .GroupBy(n => n.VendorModule)
                                .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).FirstOrDefault());
                }
                var vendor = _repo.GetAllVendors();
                var vendors = new List<SelectListItem>();
                vendors.Add(new SelectListItem() { Value = "0", Text = "Все вендоры" });
                vendors.AddRange(vendor.Select((n, i) => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name,
                    Selected = i + 1 == request.Id ? true : false
                }).ToList());

                var cpaModules = _repo.GetAllCpaModules();
                var cpaModulesList = new List<SelectListItem>();
                cpaModulesList.Add(new SelectListItem() { Value = "0", Text = "Все ТПР модули" });
                cpaModulesList.AddRange(cpaModules.Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.NameModule
                }).ToList());
   

                var vm = _mapper.Map<IEnumerable<ModuleViewModel>>(agreedModules).OrderByDescending(m => m.DateOfLetter).ToList();
                return new ModuleVM
                {
                    AllModule = vm,
                    AllCpaModuleId = cpaModulesList,
                    AllVendorId = vendors
                };
            }
        }
    }
}
