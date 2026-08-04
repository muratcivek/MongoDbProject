using AutoMapper;
using Travel.WEB.DTOs.BannerDTOs;
using Travel.WEB.Entities;

namespace Travel.WEB.Mappings
{
    public class BannerMappings : Profile
    {
        public BannerMappings()
        {
            CreateMap<Banner, ResultBannerDto>().ReverseMap();
            CreateMap<Banner, CreateBannerDto>().ReverseMap();
            CreateMap<Banner, UpdateBannerDto>().ReverseMap();
        }
    }
}
