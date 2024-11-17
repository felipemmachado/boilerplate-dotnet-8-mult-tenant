using Application.Common.Constants;
using FluentValidation;
namespace Application.UseCases.Companies.CreateCompany
{
    public class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyValidator()
        {
            
            RuleFor(v => v.CompanyName)
                .MaximumLength(100)
                .NotEmpty()
                .WithMessage(ApiResponseMessages.EnterACompanyName);

            RuleFor(v => v.TradingName)
                .MaximumLength(100)
                .NotEmpty()
                .WithMessage(ApiResponseMessages.EnterATradingName);

            RuleFor(v => v.Document)
                .MaximumLength(100)
                .NotEmpty()
                .WithMessage(ApiResponseMessages.EnterADocument)
                .IsValidCNPJ()
                .WithMessage(ApiResponseMessages.InvalidDocument);

            RuleFor(v => v.Url)
                .MaximumLength(100)
                .NotEmpty()
                .WithMessage(ApiResponseMessages.EnterAUrl)
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .WithMessage(ApiResponseMessages.InvalidUrl);
            
            RuleFor(v => v.PrimaryColor)
                .MaximumLength(100)
                .NotEmpty()
                .WithMessage(ApiResponseMessages.EnterAPrimaryColor)
                .Matches("^#(?:[0-9a-fA-F]{3}){1,2}$")
                .WithMessage(ApiResponseMessages.InvalidPrimaryColor);
            
            RuleFor(v => v.ButtonColor)
                .MaximumLength(100)
                .NotEmpty()
                .WithMessage(ApiResponseMessages.EnterAButtonColor)
                .Matches("^#(?:[0-9a-fA-F]{3}){1,2}$")
                .WithMessage(ApiResponseMessages.InvalidButtonColor);
            
            RuleFor(v => v.TabBrowserTitle)
                .MaximumLength(100)
                .NotEmpty()
                .WithMessage(ApiResponseMessages.InvalidTabBrowserTitle);
            
            RuleFor(v => v.CompanyName)
                .MaximumLength(100)
                .NotEmpty()
                .WithMessage(ApiResponseMessages.EnterAName);
            
            RuleFor(v => v.CompanyName)
                .MaximumLength(100)
                .NotEmpty()
                .WithMessage(ApiResponseMessages.EnterAName);
            
            RuleFor(v => v.UserName)
                .MaximumLength(100)
                .NotEmpty()
                .WithMessage(ApiResponseMessages.EnterAName);

            RuleFor(v => v.UserEmail)
                .EmailAddress()
                .WithMessage(ApiResponseMessages.InvalidEmail);
        }
    }
}
