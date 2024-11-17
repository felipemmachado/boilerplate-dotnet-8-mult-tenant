using System.Diagnostics.CodeAnalysis;
namespace Application.UseCases.Companies.CreateCompany
{
    [ExcludeFromCodeCoverage]
    public record struct CreateCompanyDto(Guid TenantId, string UserPassword);
}
