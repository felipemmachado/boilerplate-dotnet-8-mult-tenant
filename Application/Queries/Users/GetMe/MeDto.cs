using Application.Common.Mappings;
using Domain.Entities;
namespace Application.Queries.Users.GetMe
{
    public class MeDto : IMapFrom<User>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
