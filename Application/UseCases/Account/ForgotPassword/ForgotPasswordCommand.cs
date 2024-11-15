using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
namespace Application.UseCases.Account.ForgotPassword
{
    [ExcludeFromCodeCoverage]
    public record struct ForgotPasswordCommand(string Email) : IRequest<Unit>;

    public class ForgotPasswordCommandHandler(
        IApplicationDbContext context,
        IEmailService emailService,
        IJwtService jwtService
    ) : IRequestHandler<ForgotPasswordCommand, Unit>
    {

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await context
                .Users
                .FirstOrDefaultAsync(p => p.Email == request.Email, cancellationToken);

            if (user == null)
                return Unit.Value;

            var token = jwtService.PasswordToken(
                user.Id.ToString(),
                user.TenantId.ToString());

            var success = await emailService.ForgotPassword(new ForgotPasswordDto(user.Email, user.Name, token));

            if (!success)
                throw new ValidationException(ApiResponseMessages.UnableToaAcceptYouRequest);
            
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
