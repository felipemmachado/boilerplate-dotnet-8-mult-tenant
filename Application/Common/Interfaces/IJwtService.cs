namespace Application.Common.Interfaces
{
    public interface IJwtService
    {
        string ApplicationAccessToken(
            string userId,
            string companyId,
            IEnumerable<string> roles);

        string PasswordToken(
            string userId,
            string companyId);

        Task<bool> ValidPasswordToken(string token);
    }
}
