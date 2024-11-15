using Domain.Common;
using System.Security.Cryptography;
namespace Domain.ValueObjects
{
    public class Protocol : ValueObject
    {
        public Protocol()
        {
            var bytes = new byte[3];
            RandomNumberGenerator.Create().GetBytes(bytes);

            ProtocolCode = $"{DateTime.Now:yyyyMMdd}{BitConverter.ToString(bytes, 0).Replace("-", string.Empty)}";
            Password = Guid.NewGuid().ToString()[..8];
        }
        private string ProtocolCode { get; }
        private string Password { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ProtocolCode;
            yield return Password;
        }
    }
}
