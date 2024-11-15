using Application.Common.Authorization;
using Application.Common.Interfaces;
using Application.UnitTests.Mocks;
using Domain.Entities;
using Domain.ValueObjects;
using Infra.Persistence;
using Infra.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Moq;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;


namespace Application.UnitTests
{
    public class TestHelper
    {
        public readonly TestApplicationDbContext ApplicationDbContext;
        public readonly Guid CompanyId = new Guid("e8cb51e2-0ab0-42cb-8b51-e20ab022cb41");

        public TestHelper()
        {
            ApplicationDbContext = CreateDbContext();
            ApplicationDbContext.Database.EnsureDeleted();
        }

        public async Task<Company> CreateCompany(
            string url = "new-company",
            string companyName = "new company",
            string tradingName = "new company",
            string document = "24955585000161",
            int sla = 15)
        {
            var company = new Company(url, companyName, tradingName, document)
            {
                Id = CompanyId
            };
            ApplicationDbContext.Companies.Add(company);
            await ApplicationDbContext.SaveChangesAsync(new CancellationToken());
            return company;
        }

        public async void AddLogo(Guid companyId)
        {
            var company = await ApplicationDbContext.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
            company?.SetLogo(new Attachment("logo.png", "key.png", false));
        }

        public async Task<User> CreateUser(
            string name = "user 1",
            string email = "user1@youin.digital",
            string password = "Rhitmo@123")
        {
            var roles = Roles.AllRoles.Select(p => p.Role).ToList();
            var passwordService = new PasswordService();
            var user = new User(name, email, passwordService.Hash(password), roles);
            ApplicationDbContext.Users.Add(user);
            await ApplicationDbContext.SaveChangesAsync(new CancellationToken());
            return user;
        }

        public IFormFile CreateFile(string fileName)
        {
            const string content = $"iVBORw0KGgoAAAANSUhEUgAAARYAAACoCAMAAAASJDHMAAAAdVBMVEUYNKrW2NgOsod4hsNJXbamr856iMJabbpldr4bXp8bbpwVno5VaboySrAjPawZOqgZiZR1g8JabLqRnMl/" +
                $"jMRCV7UcZZ4ae5gahJUYjpK3vdKEkMVre8A8UrMbS6UbdJoUo4y6v9IaQ6YcUaIbeJkbeJgNrInCR0wWAAAEN0lEQVR42u3c6XbTQAyGYcmSWNIdCqXs+/1fIoVDe9xIljInGTeJv+cf2I7j1" +
                $"zOuaxIIAAAAAAAAAAAAAAAAAAAAAAAAAAAA4OLt5ckwDCfvXhI8WP0e7p0gzL3LYewngaty5y0B0WpYd0FAX12WFcHLwTkh+DV4n2nx/l1wcdFddzt4N7R4N4P3hRbv5eDhTpc" +
                $"uBu87wTtX5ZaAPrssPwiIbnCTG7rEj6HQasDzlsjF6vYrns7F8DwBAI6U/pUv3ug1RHXTvWm+you/3kxsTnMQ/s80XOyWOmrGIyLZAT+salOrfXrN967PJnZk0jeO8oiFUVy2J" +
                $"EmVRngkXueaxz66oNHmXav4LsKPJOfekbCK253zgR/76PfUP0x+BtQv9VEawmi9yhted+57+q49BstYGo3FnfoyTL6Fm5ev/Yu4Dbt3sfxI/NJs27pLHe6cnW9uw65d6vep/k0kE6juo" +
                $"vVRnbJ3RsTzdmGvzuIXbfiuJV5cTczTNAsr7Vy+F5s+Ct6Y5cdMjz1j73mxO9o5YUfSLG5JTXpn0TmyWDaW1G9W085ZjJyeFxedWibcZgdZnn64SHnF5UbSOYvNmsX8aUkGi+kd4dDB" +
                $"ZaFkJz5LMlg0uxjL9lme/mfRZDGbHiya3/zuIouZzX/r4s+tTuy9+FVWw/e9dRbJ7q2Fdk7KLPUcsupG1bbNom4wxvvvP4ssObb0bHXIMtrBTFmoyNI+E4w93SrLmKVZ+s+ipjk0p" +
                $"l2z6JM9XZBw9zb5rqTOIjvLQpa8St/hEh6cFlnyi8uxZHF7z2fImB1FlngWWXRg9aH1zyJzZZEoixsSLVm8A8xCwW7U/dXyskiYJX4ut+Qs4g5NaXHXFqI6Cy0xi7ijiA6r/Qd0" +
                $"fTun+5zF37ut/7klCx9LFjLO5Xf2R5tFOGX5nX39WjKV1PY6C3FK83FVv+/kwd1eZxHO1I8N6jlUZ9H9y6IbzqF6JkiWJbsyiexflvSiq9WKUkxHv8g3M97HLJIMlrqfpq9jRXoRMea9zE" +
                $"KbZKmngtULY/uaRdI5VK8oqtU/4+gxZbF8XNXokLNQMVjqgPXn5+wAs0hxssujq7fXA8xC2Ryq19xosB1iFikOqz7r5e+RcixZjCLaUmXM8nWv2Dutd01dVYOl/bIrLT1l8uPK5Yeeu5KGP" +
                $"apxybSpp4Qfbn9Pa+r4/bNotnZBGjeT8KsQV7TG5s5ibbNWLB8qjTk1+eLMmHavsu2sFSmitF/ar/mxF/X21JfFZ7A9jJkWWyUVr1wVR6oTMPcFt05jpko1FZs6sLMPD1fbq/OpreeZQfVz+" +
                $"fav8dYmt/j06s7pWd71jir1pRwQWjjhCC2bMGOwOMYho0Wbvr1aMmVMIUyhTWEKhTCFQqgSQpWQoUpEUCWEKiHB/UrIMFRCgijT/3OcIQoAAAAAAAAAAAAAAAAAAADAbv0BvtwvGxvYLLsAAAA" +
                $"ASUVORK5CYII=";

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            return file;
        }
        
        private TestApplicationDbContext CreateDbContext()
        {
            var connectionString = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: connectionString)
                .ReplaceService<IModelCacheKeyFactory, TestContextModalCacheKeyFactory>()
                .Options;

            var currentTenantService = new CurrentTenantService
            {
                TenantId = CompanyId
            };

            var mockIConfigurationSection = new Mock<IConfiguration>();
            var configurationSection = new Mock<IConfigurationSection>();

            configurationSection
                .Setup(a => a.Value).Returns(connectionString);

            mockIConfigurationSection.Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings:AppConnection")))
                .Returns(configurationSection.Object);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var userService = new UserService(mockHttpContextAccessor.Object);

            var dbContext = new TestApplicationDbContext(
                options,
                userService,
                currentTenantService,
                mockIConfigurationSection.Object);

            return dbContext;
        }
    }

    public record struct TestManifestSetup(Guid ChannelId, Guid ManifestTypeId, Guid DepartmentId, Guid QuestionId, Guid StateId, Guid CityId, Guid RelationshipId, Guid UserId, Guid CompanyRoleId);
}
