using Application.Common.Mappings;
using Domain.Entities;
namespace Application.Queries.Companies.GetCompanyBySlug
{
    public record struct GetCompanyDto(
        Guid Id, 
        string? CompanyName, 
        string? TradingName, 
        string? PrimaryColor, 
        string? ButtonColor, 
        string? TabBrowserTitle, 
        string? Logo);
}
