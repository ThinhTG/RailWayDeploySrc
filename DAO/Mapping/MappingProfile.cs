using AutoMapper;
using Models;
using static DAO.Contracts.UserRequestAndResponse;


namespace DAO.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserResponse>();
            CreateMap<ApplicationUser, CurrentUserResponse>();
            CreateMap<UserRegisterRequest, ApplicationUser>();
            CreateMap<ApplicationUser, UserDTO>();

        }
    }
}
