using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Application.Queries.Users.ExistsUser
{
    public record struct ExistsUserQuery(string Email) : IRequest<ExistsUserDto>;

    public class ExistsUserQueryHandler(IApplicationDbContext context) : IRequestHandler<ExistsUserQuery, ExistsUserDto>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<ExistsUserDto> Handle(ExistsUserQuery request, CancellationToken cancellationToken)
        {
            var email = await _context
                .Users
                .AsNoTracking()
                .Where(p => p.Email == request.Email.Trim().ToLower())
                .Select(p => p.Email)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return new ExistsUserDto
            {
                Exists = email != null
            };
        }
    }
}
