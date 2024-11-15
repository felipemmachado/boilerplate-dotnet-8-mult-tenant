using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.UseCases.Account.ForgotPassword;
using Moq;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.UnitTests.UseCases.Account
{
    public class ForgotPassword
    {
        [Fact]
        public async Task Should_validate_command()
        {
            // Act
            var command = new ForgotPasswordCommand("wrong-email");
            var validator = new ForgotPasswordValidator();
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.False(result.IsValid);
        }
        
        [Fact]
        public async Task Should_send_email()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            var user = await helper.CreateUser();

            var emailService = new Mock<IEmailService>();
            emailService.Setup(service => service.ForgotPassword(It.IsAny<ForgotPasswordDto>())).ReturnsAsync(true);

            // Act
            var command = new ForgotPasswordCommand(user.Email);
            var handler = new ForgotPasswordCommandHandler(
                helper.ApplicationDbContext,
                emailService.Object,
                new Mock<IJwtService>().Object);

            // Assert
            await handler.Handle(command, new CancellationToken());
        }

        [Fact]
        public async Task Should_not_send_email()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            var user = await helper.CreateUser();

            var emailService = new Mock<IEmailService>();
            emailService.Setup(service => service.ForgotPassword(It.IsAny<ForgotPasswordDto>())).ReturnsAsync(false);

            // Act
            var command = new ForgotPasswordCommand(user.Email);
            var handler = new ForgotPasswordCommandHandler(
                helper.ApplicationDbContext,
                emailService.Object,
                new Mock<IJwtService>().Object);
            var ex = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, new CancellationToken()));

            // Assert
            var error = ex.Errors.SelectMany(p => p.Errors.Where(e => e == ApiResponseMessages.UnableToaAcceptYouRequest));
            Assert.True(error.Any());
        }

        [Fact]
        public async Task Should_return_success_with_wrong_email()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            await helper.CreateUser();

            var emailService = new Mock<IEmailService>();
            emailService.Setup(service => service.ForgotPassword(It.IsAny<ForgotPasswordDto>())).ReturnsAsync(false);

            // Act
            var command = new ForgotPasswordCommand("email2@gmail.com");
            var handler = new ForgotPasswordCommandHandler(
                helper.ApplicationDbContext,
                emailService.Object,
                new Mock<IJwtService>().Object);

            // Assert
            await handler.Handle(command, new CancellationToken());
        }
    }
}
