using AutoMapper;
using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using CpaRepository.ViewModel.ActualVendorModule;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Mediatr.Query
{
    public class GetArchiveModulesQuery : IRequest<ModuleVM>
    {

        public class GetArchiveModulesQueryHandler : IRequestHandler<GetArchiveModulesQuery, ModuleVM>
        {
            private readonly IAgreedModulesRepo _repo;
            private readonly IMapper _mapper;

            public GetArchiveModulesQueryHandler(IAgreedModulesRepo repo, IMapper mappper)
            {
                _repo = repo;
                _mapper = mappper;
            }
            public async Task<ModuleVM> Handle(GetArchiveModulesQuery request, CancellationToken cancellationToken)
            {
                var agreedModules = _repo.GetAll()
                           .GroupBy(n => n.VendorModule)
                           .Select(g => g.OrderByDescending(d => d.Letter.DateOfLetter).Skip(1)).SelectMany(n => n);
                var vendor = _repo.GetAllVendors();
                var vendors = new List<SelectListItem>();
                vendors.Add(new SelectListItem() { Value = "0", Text = "Все вендоры" });
                vendors.AddRange(vendor.Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name
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
