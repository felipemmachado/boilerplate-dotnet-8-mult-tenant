using Application.Common.Constants;
using FluentValidation;
namespace Application.UseCases.Account.SignIn
{
    public class SignInValidator : AbstractValidator<SignInCommand>
    {
        public SignInValidator()
        {
            RuleFor(v => v.Email)
                .EmailAddress()
                .WithMessage(ApiResponseMessages.InvalidEmail);

            RuleFor(v => v.Password)
                .NotEmpty()
                .WithMessage(ApiResponseMessages.EnterAPassword);
        }
    }
}
