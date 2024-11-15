using Application.Common.Configs;
using Application.Common.Constants;
using Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Infra.Services
{
    public class JwtService(IOptions<JwtSettings> jwtSettings, IOptions<JwtPasswordConfig> jwtPasswordConfig) : IJwtService
    {
        private readonly JwtPasswordConfig _jwtPasswordConfig = jwtPasswordConfig.Value
            ?? throw new ArgumentNullException(nameof(jwtPasswordConfig));
        private readonly JwtSettings _jwtSettings = jwtSettings.Value
            ?? throw new ArgumentNullException(nameof(jwtSettings));

        public string ApplicationAccessToken(string userId, string companyId, IEnumerable<string> roles)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

            var claimCollection = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(Consts.IdentifyTenant, companyId)
            };

            claimCollection.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimCollection),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
        public string PasswordToken(string userId, string companyId)
        {
            var key = Encoding.UTF8.GetBytes(_jwtPasswordConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userId),
                    new Claim("companyId", companyId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtPasswordConfig.ExpiresInMinutes),
                Issuer = _jwtPasswordConfig.Issuer,
                Audience = _jwtPasswordConfig.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }

        public async Task<bool> ValidPasswordToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(_jwtPasswordConfig.Secret);

            var handler = new JwtSecurityTokenHandler();
            var result = await handler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidIssuer = _jwtPasswordConfig.Issuer,
                ValidAudience = _jwtPasswordConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            });

            return result.IsValid;
        }
    }
}
