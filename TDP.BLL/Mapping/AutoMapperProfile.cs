using AutoMapper;
using TDP.BLL.DTOs;
using TDP.BLL.Models;

namespace TDP.BLL.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Employee to UserDto mapping
            CreateMap<Employee, UserDto>()
                .ForMember(dest => dest.DepartmentNameEN, opt => opt.MapFrom(src => src.Department.NameEN))
                .ForMember(dest => dest.DepartmentNameAR, opt => opt.MapFrom(src => src.Department.NameAR));

            // RegisterDto to Employee mapping
            CreateMap<RegisterDto, Employee>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower()))
                .ForMember(dest => dest.HashedPassword, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.IDGuid, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.ID, opt => opt.Ignore()) // Will be set by database
                .ForMember(dest => dest.IsActive, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.Department, opt => opt.Ignore()); // Will be loaded separately

            // UpdateUserInfoDto to Employee mapping (for updates)
            CreateMap<UpdateUserInfoDto, Employee>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower()))
                .ForMember(dest => dest.HashedPassword, opt => opt.Ignore()) // Don't update password
                .ForMember(dest => dest.IDGuid, opt => opt.Ignore()) // Don't update GUID
                .ForMember(dest => dest.ID, opt => opt.Ignore()) // Don't update ID
                .ForMember(dest => dest.IsActive, opt => opt.Ignore()) // Don't update active status
                .ForMember(dest => dest.Department, opt => opt.Ignore()); // Will be loaded separately
        }
    }
} 