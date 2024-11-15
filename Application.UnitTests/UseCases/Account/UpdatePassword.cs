using Application.Common.Constants;
using Application.UseCases.Account.UpdatePassword;
using Infra.Services;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.UnitTests.UseCases.Account
{
    public class UpdatePassword
    {
        [InlineData(null, "123", "")]
        [InlineData(null, "", "123")]
        [InlineData(null, "1234", "123")]
        [Theory]
        public async Task Should_validate_command(string? actualPassword, string password, string rePassword)
        {
            // Act
            var command = new UpdatePasswordCommand(new Guid(), actualPassword, password, rePassword);
            var validator = new UpdatePasswordPasswordValidator();
            var result = await validator.ValidateAsync(command);
            
            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Should_not_update_password_with_wrong_user()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            await helper.CreateUser();
            var passwordService = new PasswordService();
            
            // Act
            var command = new UpdatePasswordCommand(Guid.NewGuid(), null, "Rhitmo@123","Rhitmo@123");
            var handler = new UpdatePasswordCommandHandler(
                helper.ApplicationDbContext,
                passwordService);
            var ex = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, new CancellationToken()));
            
            // Assert
            var error = ex.Errors.SelectMany(p => p.Errors.Where(e => e == ApiResponseMessages.UserNotFound));
            Assert.True(error.Any());
        }
        
        [Fact]
        public async Task Should_not_update_password_with_actual_password_wrong()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            var user = await helper.CreateUser();
            var passwordService = new PasswordService();
            
            // Act
            var command = new UpdatePasswordCommand(user.Id, "wrong-actual-password", "Rhitmo@123","Rhitmo@123");
            var handler = new UpdatePasswordCommandHandler(
                helper.ApplicationDbContext,
                passwordService);
            var ex = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, new CancellationToken()));
            
            // Assert
            var error = ex.Errors.SelectMany(p => p.Errors.Where(e => e == ApiResponseMessages.InvalidActualPassword));
            Assert.True(error.Any());
        }
        
        [Fact]
        public async Task Should_update_password_with_actual_password()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            var user = await helper.CreateUser();
            var passwordService = new PasswordService();
            
            // Act
            var command = new UpdatePasswordCommand(user.Id, "Rhitmo@123", "Rhitmo@4321","Rhitmo@4321");
            var handler = new UpdatePasswordCommandHandler(
                helper.ApplicationDbContext,
                passwordService);
            
            // Assert
            await handler.Handle(command, new CancellationToken());
        }
        
        [Fact]
        public async Task Should_update_password_without_actual_password()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            var user = await helper.CreateUser();
            var passwordService = new PasswordService();
            
            // Act
            var command = new UpdatePasswordCommand(user.Id, null, "Rhitmo@4321","Rhitmo@4321");
            var handler = new UpdatePasswordCommandHandler(
                helper.ApplicationDbContext,
                passwordService);
            
            // Assert
            await handler.Handle(command, new CancellationToken());
        }
    }
}
