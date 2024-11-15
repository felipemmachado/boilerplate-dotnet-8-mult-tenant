using Application.Common.Constants;
using FluentValidation;
namespace Application.UseCases.Account.UpdatePassword
{
    public class UpdatePasswordPasswordValidator : AbstractValidator<UpdatePasswordCommand>
    {
        public UpdatePasswordPasswordValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty()
                .NotNull()
                .WithMessage(ApiResponseMessages.UserIdRequired);

            RuleFor(v => v.Password)
                .NotEmpty()
                .WithMessage(ApiResponseMessages.EnterAPassword);

            RuleFor(v => v.Password).Equal(p => p.RePassword)
                .WithMessage(ApiResponseMessages.PasswordAreNotTheSame);
        }
    }
}
