using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Mappings;
namespace Application.Queries.Users.GetUsers
{
    public class GetUsersQuery : Query, IRequest<PaginatedList<UserDto>> { }
    
    public class GetUsersQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetUsersQuery, PaginatedList<UserDto>>
    {
        public async Task<PaginatedList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await context
                .Users
                .Where(x => x.DisabledAt == null)
                .AsNoTracking()
                .OrderBy (x => x.CreatedAt)
                .ProjectTo<UserDto>(mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);

            return users;
        }
    }
}
