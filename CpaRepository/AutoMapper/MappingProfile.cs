using AutoMapper;
using CpaRepository.ModelsDb;
using CpaRepository.ViewModel.ActualVendorModule;
using CpaRepository.ViewModel.AgreedModules;
using CpaRepository.ViewModel.Letter;
using CpaRepository.ViewModel.VendorModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VendorModuleViewModel, VendorModule>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<VendorModule, VendorModuleViewModel>().ForMember(x => x.CpaModulesId,
                opt => opt.MapFrom(src => src.CpaModules.Select(n => n.Id).ToArray()));
            CreateMap<Letter, LetterViewModel>();
            CreateMap<LetterViewModel, Letter>();
            CreateMap<AgreedModule, AgreedModuleViewModel>()
                .ForMember(x => x.NumberLetter, opt => opt.MapFrom(src => src.Letter.NumberLetter))
                  .ForMember(x => x.VendorId, opt => opt.MapFrom(src => src.VendorModule.VendorId))
                  .ForMember(x => x.Vendor, opt => opt.MapFrom(src => src.VendorModule.Vendor))
                  .ForMember(nameof(AgreedModuleViewModel.ExistModule), opt => opt
                  .MapFrom(src => System.IO.File.Exists(src.PathVendorModule)))
                  .ForMember(nameof(AgreedModuleViewModel.DateOfLetter), opt => opt.MapFrom(src => src.Letter.DateOfLetter));
            CreateMap<AgreedModuleViewModel, AgreedModule>()
                  .ForPath(x=>x.VendorModule.VendorId, opt => opt.MapFrom(src => src.VendorId));               
            CreateMap<AgreedModule, ModuleViewModel>()
                  .ForMember(nameof(ModuleViewModel.ExistModule), opt => opt
                  .MapFrom(src => System.IO.File.Exists(src.PathVendorModule)))
                  .ForMember(nameof(ModuleViewModel.DateOfLetter), opt => opt.MapFrom(src => src.Letter.DateOfLetter))
                  .ForMember(nameof(ModuleViewModel.NumberLetter), opt => opt.MapFrom(src => src.Letter.NumberLetter))
                  .ForMember(nameof(ModuleViewModel.VendorId), opt => opt.MapFrom(src => src.VendorModule.VendorId))
                  .ForMember(nameof(ModuleViewModel.Vendor), opt => opt.MapFrom(src => src.VendorModule.Vendor))
                  .ForMember(nameof(ModuleViewModel.CpaModules), opt => opt.MapFrom(src => src.VendorModule.CpaModules));
        }
    }
}
