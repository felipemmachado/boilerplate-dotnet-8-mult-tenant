using Application.Common.Authorization;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
namespace Application.UseCases.Users.UpdateUser
{
    [ExcludeFromCodeCoverage]
    public record struct UpdateUserCommand(
        Guid UserId,
        string Name,
        string Email
    ) : IRequest<Unit>;

    public class UpdateUserCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateUserCommand, Unit>
    {

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await context
                    .Users
                    .Where(p => p.Id == request.UserId)
                    .FirstOrDefaultAsync(cancellationToken)
                ??
                throw new ValidationException(ApiResponseMessages.UserNotFound);

            user.Update(request.Name);
            user.UpdateEmail(request.Email);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
