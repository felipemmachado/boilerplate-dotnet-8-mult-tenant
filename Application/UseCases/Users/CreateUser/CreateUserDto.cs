using System.Diagnostics.CodeAnalysis;
namespace Application.UseCases.Users.CreateUser
{
    [ExcludeFromCodeCoverage]
    public record struct CreateUserDto(Guid Id, string TemporaryPassword);
}
