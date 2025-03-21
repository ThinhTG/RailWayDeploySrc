using AutoMapper;
using DAO.Contracts;
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
            CreateMap<ApplicationUser, UserDTO>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

            CreateMap<Review, ReviewResponse>();
            CreateMap<ReviewRequest, Review>();

            CreateMap<ApplicationUser, UserResponseAdmin>();

            CreateMap<UpdateUserRequest, ApplicationUser>();



        }
    }
}
