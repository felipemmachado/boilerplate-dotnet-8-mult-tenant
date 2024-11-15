using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Application.Queries.Users.GetMe
{
    public record struct GetMeQuery(Guid UserId) : IRequest<MeDto>;

    public class GetMeQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetMeQuery, MeDto>
    {

        public async Task<MeDto> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            return await context
                    .Users
                    .AsNoTracking()
                    .Where(p => p.Id == request.UserId)
                    .ProjectTo<MeDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new ValidationException(ApiResponseMessages.UserNotFound);
        }
    }
}
