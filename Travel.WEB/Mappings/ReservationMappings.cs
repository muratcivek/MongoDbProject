using AutoMapper;
using Travel.WEB.DTOs.ReservationDTOs;
using Travel.WEB.Entities;

namespace Travel.WEB.Mappings
{
    public class ReservationMappings : Profile
    {
        public ReservationMappings()
        {
            CreateMap<
                Reservation,
                CreateReservationDto
            >().ReverseMap();

            CreateMap<
                Reservation,
                ResultReservationDto
            >().ReverseMap();
        }
    }
}