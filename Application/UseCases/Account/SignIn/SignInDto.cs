namespace Application.UseCases.Account.SignIn
{
    public record struct SignInDto(
        string AccessToken,
        bool TemporaryPassword);
}
