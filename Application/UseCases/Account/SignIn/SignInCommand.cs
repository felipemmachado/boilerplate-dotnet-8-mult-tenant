using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
namespace Application.UseCases.Account.SignIn
{
    [ExcludeFromCodeCoverage]
    public record struct SignInCommand(
        string Email,
        string Password) : IRequest<SignInDto>;

    public class SignInCommandHandler(
        IApplicationDbContext context,
        IPasswordService passwordService,
        IJwtService jwtService) : IRequestHandler<SignInCommand, SignInDto>
    {
        public async Task<SignInDto> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await context
                    .Users
                    .FirstOrDefaultAsync(p => p.Email == request.Email.ToLower(), cancellationToken)
                ??
                throw new ValidationException(ApiResponseMessages.PasswordOrEmailInvalid);

            if (user.IsDisabled())
                throw new ValidationException(ApiResponseMessages.UserDisabled);

            if (!passwordService.Verify(user.Password, request.Password))
                throw new ValidationException(ApiResponseMessages.PasswordOrEmailInvalid);

            user.UpdateLastAccess(DateTime.UtcNow);
            if (user.FirstAccess == null) user.UpdateFirstAccess(DateTime.UtcNow);

            await context.SaveChangesAsync(cancellationToken);

            var token = jwtService.ApplicationAccessToken(
                user.Id.ToString(),
                user.TenantId.ToString(),
                user.Roles.ToArray());

            return new SignInDto
            {
                AccessToken = token,
                TemporaryPassword = user.ForceChangePassword
            };
        }
    }
}
