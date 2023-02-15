using AutoMapper;
using Parking.API.Application.DTOS.Request;
using Parking.API.Application.DTOS.Responses;

namespace Parking.API.Application.DTOS.Config
{
    public class MapperConfig : Profile
    {

        public class AutoMapperProfile : Profile
        {
            public AutoMapperProfile()
            {                
                CreateMap<Domain.Entities.Vehicle, VehicleResponseDTO>().ReverseMap(); 
                CreateMap<Domain.Entities.Vehicle, VehicleDTO>().ReverseMap(); 

                CreateMap<Domain.Entities.ParkingSpaces, ParkingSpaceDTO>().ReverseMap();
                CreateMap<Domain.Entities.ParkingSpaces, ParkingSpaceResponseDTO>().ReverseMap();

                CreateMap<Domain.Entities.Parking, ParkingDTO>().ReverseMap();
                CreateMap<Domain.Entities.Parking, ParkingResponseDTO>().ReverseMap();
                CreateMap<Domain.Entities.Parking, ParkingUpdateDTO>().ReverseMap();
              
            }
        }

    }
}
