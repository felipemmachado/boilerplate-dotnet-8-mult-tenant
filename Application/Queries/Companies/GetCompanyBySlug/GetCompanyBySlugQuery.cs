using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Application.Queries.Companies.GetCompanyBySlug
{
    public record struct GetCompanyBySlugQuery(string Slug) : IRequest<GetCompanyDto>;

    public class GetCompanyBySlugQueryHandler(
        IApplicationDbContext context,
        IFileService fileService)
        : IRequestHandler<GetCompanyBySlugQuery, GetCompanyDto>
    {
        public async Task<GetCompanyDto> Handle(GetCompanyBySlugQuery request, CancellationToken cancellationToken)
        {
            var company = await context
                    .Companies
                    .Where(p => p.Slug == request.Slug)
                    .Select(p => new
                    {
                        p.Id,
                        p.Logo,
                        p.TradingName,
                        p.CompanyName,
                        p.Customization
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken)
                ?? throw new ValidationException(ApiResponseMessages.CompanyNotFound);

            string? logo = null;
            if (company.Logo != null)
                logo = fileService.GetPublicObjectByKeyAsync(company.Logo.ResourceKey).Result.FileUrl;
            
            var primaryColor  = "#1368CE";
            var buttonColor = primaryColor;
            var tabBrowserTitle = company.TradingName;
            
            if (company.Customization == null)
            {
                return new GetCompanyDto(company.Id,
                    company.TradingName,
                    company.CompanyName,
                    primaryColor,
                    buttonColor,
                    tabBrowserTitle,
                    logo);
            }
            
            primaryColor = company.Customization.PrimaryColor;
            buttonColor = company.Customization.ButtonColor;
            tabBrowserTitle = company.Customization.TabBrowserTitle;

            return new GetCompanyDto(company.Id,
                company.TradingName,
                company.CompanyName,
                primaryColor,
                buttonColor,
                tabBrowserTitle,
                logo);
        }
    }

}
