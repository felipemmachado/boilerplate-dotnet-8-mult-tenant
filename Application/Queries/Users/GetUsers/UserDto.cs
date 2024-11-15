using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
namespace Application.Queries.Users.GetUsers
{
    public class UserDto : IMapFrom<User>
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public IEnumerable<string> Roles { get; init; } = [ string.Empty ];
        public DateTime? FirstAccess { get; init; }
        public DateTime? LastAccess { get; init; }
        public DateTime? DisabledAt { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email))
                .ForMember(d => d.FirstAccess, opt => opt.MapFrom(s => s.FirstAccess))
                .ForMember(d => d.LastAccess, opt => opt.MapFrom(s => s.LastAccess))
                .ForMember(d => d.DisabledAt, opt => opt.MapFrom(s => s.DisabledAt))
                .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.Roles))
                ;
        }
    }
}
