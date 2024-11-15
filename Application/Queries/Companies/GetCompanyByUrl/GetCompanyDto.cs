using Application.Common.Mappings;
using Domain.Entities;
namespace Application.Queries.Companies.GetCompanyByUrl
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
