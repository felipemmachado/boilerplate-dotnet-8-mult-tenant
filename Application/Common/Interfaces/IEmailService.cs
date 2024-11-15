using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task<bool> ForgotPassword(ForgotPasswordDto forgotPassword);
    }

    [ExcludeFromCodeCoverage]
    public record struct ForgotPasswordDto(string Email, string Name, string Token);
}
