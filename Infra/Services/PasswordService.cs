using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
namespace Infra.Services
{
    public partial class PasswordService : IPasswordService
    {
        private const char Delimeter = '.';
        private static readonly int Iterations = 10000; //
        private static readonly int SaltSize = 128 / 8; // 128 bit
        private static readonly int KeySize = 256 / 8; // 256 bit
        private static readonly HashAlgorithmName HashAlgorithmName = HashAlgorithmName.SHA256;

        public string Hash(string password)
        {

            if (!HasNumber().IsMatch(password))
                throw new ValidationException(ApiResponseMessages.PasswordMostHaveAtLeastOneNumber);

            if (!HasUpperChar().IsMatch(password))
                throw new ValidationException(ApiResponseMessages.PasswordMostHaveAtLeastOneUpperLetter);

            if (!HasLowerChar().IsMatch(password))
                throw new ValidationException(ApiResponseMessages.PasswordMostHaveAtLeastOneLowerLetter);

            if (!HasMinimum8Chars().IsMatch(password))
                throw new ValidationException(ApiResponseMessages.PasswordMostHaveAtLeastCharactersLong);

            if (!HasSpecialCharacter().IsMatch(password))
                throw new ValidationException(ApiResponseMessages.PasswordMostHaveAtLeastSpecialCaracter);


            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);


            return string.Join(Delimeter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public string GenerateRandomPassword()
        {
            var requiredLength = 8;
            string[] randomChars =
            [
                "ABCDEFGHJKLMNOPQRSTUVWXYZ", // uppercase 
                "abcdefghijkmnopqrstuvwxyz", // lowercase
                "0123456789", // digits
                "!@$?_-" // non-alphanumeric
            ];

            var rand = new Random(Environment.TickCount);
            List<char> chars = [];


            chars.Insert(rand.Next(0, chars.Count),
                randomChars[0][rand.Next(0, randomChars[0].Length)]);


            chars.Insert(rand.Next(0, chars.Count),
                randomChars[1][rand.Next(0, randomChars[1].Length)]);


            chars.Insert(rand.Next(0, chars.Count),
                randomChars[2][rand.Next(0, randomChars[2].Length)]);


            chars.Insert(rand.Next(0, chars.Count),
                randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (var i = chars.Count;
                 i < requiredLength
                 || chars.Distinct().Count() < requiredLength;
                 i++)
            {
                var rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        public bool Verify(string passwordHash, string password)
        {
            var elements = passwordHash.Split(Delimeter);

            if (elements.Length != 2)
                return false;

            var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);

            var hashInput = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, hashInput);
        }


        [GeneratedRegex(@"[!@#$%^&*()_+{}[\]:;<>,.?~\\-]+")]
        private static partial Regex HasSpecialCharacter();

        [GeneratedRegex(@"[0-9]+")]
        private static partial Regex HasNumber();

        [GeneratedRegex(@"[A-Z]+")]
        private static partial Regex HasUpperChar();

        [GeneratedRegex(@"[a-z]+")]
        private static partial Regex HasLowerChar();

        [GeneratedRegex(@".{8,}")]
        private static partial Regex HasMinimum8Chars();
    }
}
