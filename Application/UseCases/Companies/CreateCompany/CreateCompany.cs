using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
namespace Application.UseCases.Companies.CreateCompany
{
    [ExcludeFromCodeCoverage]
    public record struct CreateCompanyCommand(
        string Slug,
        string CompanyName,
        string TradingName,
        string Document,
        string PrimaryColor,
        string ButtonColor,
        string TabBrowserTitle,
        string UserName,
        string UserEmail
    ) : IRequest<CreateCompanyDto>;
    
    public class CreateCompanyCommandHandler(
        IApplicationDbContext context,
        IPasswordService passwordService
    ) : IRequestHandler<CreateCompanyCommand, CreateCompanyDto>
    {
        public async Task<CreateCompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var existSlug = await context
                .Companies
                .Select(p => p.Slug)
                .FirstOrDefaultAsync(p => p == request.Slug, cancellationToken);

            if (existSlug != null)
                throw new ValidationException(ApiResponseMessages.UrlAlreadyUsed);

            var existDocument = await context
                .Companies
                .Select(p => p.Document)
                .FirstOrDefaultAsync(p => p == request.Document, cancellationToken);

            if (existDocument != null)
                throw new ValidationException(ApiResponseMessages.DocumentAlreadyRegistered);
            
            var company = new Company(request.Slug, request.CompanyName, request.TradingName, request.Document);
            var customization = new Customization(request.PrimaryColor, request.ButtonColor, request.TabBrowserTitle);
            
            var password = passwordService.GenerateRandomPassword();
            var user = new User(
                request.UserName,
                request.UserEmail,
                passwordService.Hash(password), []);
            
            company.UpdateCustomization(customization);
            company.AddUser(user);
            
            await  context.Companies.AddAsync(company, cancellationToken);
            
            await context.SaveChangesAsync(cancellationToken);

            return new CreateCompanyDto(company.Id, password);
        }
    }
}
