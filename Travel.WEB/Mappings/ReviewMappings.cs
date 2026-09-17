using AutoMapper;
using Travel.WEB.DTOs.ReviewDTOs;
using Travel.WEB.Entities;

namespace Travel.WEB.Mappings
{
    public class ReviewMappings : Profile
    {
        public ReviewMappings()
        {
            CreateMap<Review, ResultReviewDto>()
                .ReverseMap();

            CreateMap<Review, CreateReviewDto>()
                .ReverseMap();

            CreateMap<Review, UpdateReviewDto>()
                .ReverseMap();

            CreateMap<ResultReviewDto, UpdateReviewDto>()
                .ReverseMap();
        }
    }
}