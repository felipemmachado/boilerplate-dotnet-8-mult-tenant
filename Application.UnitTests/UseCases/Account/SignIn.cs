using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.UseCases.Account.SignIn;
using Infra.Services;
using Moq;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.UnitTests.UseCases.Account
{
    public class SignIn
    {
        private const string MockJwt =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";


        [InlineData("", "")]
        [InlineData("user1@youin.com.br", "")]
        [InlineData("", "fasdfasf")]
        [InlineData("user1", "fasdfasf")]
        [Theory]
        public async Task Should_validate_command(string email, string password)
        {
            // Act
            var command = new SignInCommand(email, password);
            var validator = new SignInValidator();
            var result = await validator.ValidateAsync(command);
            
            // Assert
            Assert.False(result.IsValid);
        }
        
        [Fact]
        public async Task Should_login()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            var user = await helper.CreateUser("User 1", "user1@rhitmo.com.br", "Rhitmo@123");
            user.UpdateFirstAccess(DateTime.UtcNow);
            user.SetForceChangePassword(false);
            helper.ApplicationDbContext.Users.Update(user);
            await helper.ApplicationDbContext.SaveChangesAsync(new CancellationToken());
            
            var passwordService = new PasswordService();
            var mockJwtService = new Mock<IJwtService>();
            mockJwtService.Setup(s => s.ApplicationAccessToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(MockJwt);
            
            // Act
            var command = new SignInCommand(user.Email, "Rhitmo@123");
            var handler = new SignInCommandHandler(
                helper.ApplicationDbContext, 
                passwordService, 
                mockJwtService.Object);
            var result = await handler.Handle(command, new CancellationToken());
            
            // Assert
            Assert.True(!result.TemporaryPassword);
            Assert.Equal(MockJwt, result.AccessToken);
        }
        
        [Fact]
        public async Task Should_login_with_first_access()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            var user = await helper.CreateUser("User 1", "user1@rhitmo.com.br", "Rhitmo@123");
            var passwordService = new PasswordService();
            var mockJwtService = new Mock<IJwtService>();
            mockJwtService.Setup(s => s.ApplicationAccessToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(MockJwt);
            
            // Act
            var command = new SignInCommand(user.Email, "Rhitmo@123");
            var handler = new SignInCommandHandler(
                helper.ApplicationDbContext, 
                passwordService, 
                mockJwtService.Object);
            var result = await handler.Handle(command, new CancellationToken());
            
            // Assert
            Assert.True(result.TemporaryPassword);
            Assert.Equal(MockJwt, result.AccessToken);
        }
        
        [Fact]
        public async Task Should_not_login_with_non_existent_user()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            await helper.CreateUser();
            var mockPasswordService = new Mock<IPasswordService>();
            var mockJwtService = new Mock<IJwtService>();
            
            // Act
            var command = new SignInCommand("user2@youin.com.br", "12345");
            var handler = new SignInCommandHandler(
                helper.ApplicationDbContext, 
                mockPasswordService.Object, 
                mockJwtService.Object);
            var ex = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, new CancellationToken()));
            
            // Assert
            var error = ex.Errors.SelectMany(p => p.Errors.Where(e => e == ApiResponseMessages.PasswordOrEmailInvalid));
            Assert.True(error.Any());
        }
        
        [Fact]
        public async Task Should_not_login_with_disabled_user()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            var user = await helper.CreateUser();
            user.Disabled();
            helper.ApplicationDbContext.Users.Update(user);
            await helper.ApplicationDbContext.SaveChangesAsync(new CancellationToken());
            
            var command = new SignInCommand(user.Email, "12345");
            var mockPasswordService = new Mock<IPasswordService>();
            var mockJwtService = new Mock<IJwtService>();

            // Act
            var handler = new SignInCommandHandler(
                helper.ApplicationDbContext, 
                mockPasswordService.Object, 
                mockJwtService.Object);
            var ex = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, new CancellationToken()));
            
            // Assert
            var error = ex.Errors.SelectMany(p => p.Errors.Where(e => e == (ApiResponseMessages.UserDisabled)));
            Assert.True(error.Any());
        }
        
        [Fact]
        public async Task Should_not_login_with_wrong_password()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            var user = await helper.CreateUser("User 1", "user1@rhitmo.com.br", "Rhitmo@123");
            
            var command = new SignInCommand(user.Email, "Rhitmo@1234");
            var passwordService = new PasswordService();
            var mockJwtService = new Mock<IJwtService>();
            
            // Act
            var handler = new SignInCommandHandler(
                helper.ApplicationDbContext,
                passwordService,
                mockJwtService.Object);
            var ex = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, new CancellationToken()));
            
            // Assert
            var error = ex.Errors.SelectMany(p => p.Errors.Where(e => e == (ApiResponseMessages.PasswordOrEmailInvalid)));
            Assert.True(error.Any());
        }
    }
}
