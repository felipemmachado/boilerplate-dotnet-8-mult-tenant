using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
namespace Application.UseCases.Account.UpdatePassword
{
    [ExcludeFromCodeCoverage]
    public record struct UpdatePasswordCommand(
        Guid UserId,
        string? ActualPassword,
        string Password,
        string RePassword) : IRequest<Unit>;

    public class UpdatePasswordCommandHandler(
        IApplicationDbContext context,
        IPasswordService passwordService
    ) : IRequestHandler<UpdatePasswordCommand, Unit>
    {
        public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = passwordService.Hash(request.Password);

            var user = await context
                    .Users
                    .FirstOrDefaultAsync(p => p.Id == request.UserId, cancellationToken)
                ?? throw new ValidationException(ApiResponseMessages.UserNotFound);

            if (!string.IsNullOrWhiteSpace(request.ActualPassword))
            {
                user.SetForceChangePassword(false);
                if (!passwordService.Verify(user.Password, request.ActualPassword))
                    throw new ValidationException(ApiResponseMessages.InvalidActualPassword);
            }

            user.UpdatePassword(passwordHash);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
