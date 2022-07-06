using System.Diagnostics.CodeAnalysis;
using AtHomeProject.Data.Entities;
using AtHomeProject.Domain.Models;
using AutoMapper;

namespace AtHomeProject.Domain
{
    [ExcludeFromCodeCoverage]
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Device, DeviceModel>().ReverseMap();

            CreateMap<Device, DeviceListModel>()
                .ForMember(
                    dest => dest.FirmwareVersion,
                    opt => opt.MapFrom(src => $"{src.FirmwareVersion}"))
                .ReverseMap();

            CreateMap<SensorInput, SensorInputModel>().ReverseMap();

            CreateMap<SensorInput, SensorInputListModel>().ReverseMap();

            CreateMap<SensorAlert, SensorAlertModel>().ReverseMap();
        }
    }
}
