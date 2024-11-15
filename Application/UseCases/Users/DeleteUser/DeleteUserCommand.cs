using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
namespace Application.UseCases.Users.DeleteUser
{
    [ExcludeFromCodeCoverage]
    public record struct DeleteUserCommand(Guid UserId) : IRequest<Unit>;

    public class DisableUserCommandHandler(
        IApplicationDbContext context
    ) : IRequestHandler<DeleteUserCommand, Unit>
    {

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await context
                    .Users
                    .FirstOrDefaultAsync(p => p.Id == request.UserId, cancellationToken)
                ?? throw new ValidationException(ApiResponseMessages.UserNotFound);

            user.Disabled();

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
