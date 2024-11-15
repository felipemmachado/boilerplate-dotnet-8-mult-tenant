using Application.Common.Constants;
using FluentValidation;
namespace Application.UseCases.Account.ForgotPassword
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(v => v.Email)
                .EmailAddress()
                .WithMessage(ApiResponseMessages.InvalidEmail);
        }
    }
}
