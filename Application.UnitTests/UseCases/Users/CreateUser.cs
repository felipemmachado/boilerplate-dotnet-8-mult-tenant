using Application.Common.Constants;
using Application.Queries.Users.ExistsUser;
using Application.UseCases.Users.CreateUser;
using Infra.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.UnitTests.UseCases.Users
{
    public class CreateUser
    {

        [InlineData("", "")]
        [InlineData("User Name", "")]
        [InlineData("", "user_validator@youin.digital")]
        [InlineData("User Name", "user_validator")]
        [Theory]
        public async Task Should_validate_command(string name, string email)
        {
            // Act
            var command = new CreateUserCommand(name, email);
            var validator = new CreateUserValidator();
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Should_not_create_user_with_existing_user()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            var passwordService = new PasswordService();
            var mediator = MockMediator(true);
            const string email = "test@youin.digital";

            // Act
            var command = new CreateUserCommand("New User", email);
            var handler = new CreateUserCommandHandler(helper.ApplicationDbContext, passwordService, mediator.Object);
            var ex = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, new CancellationToken()));

            // Assert
            var error = ex.Errors.SelectMany(p => p.Errors.Where(e => e == ApiResponseMessages.EmailAlreadyRegistered(email)));
            Assert.True(error.Any());
        }

        [Fact]
        public async Task Should_create_user()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            const string email = "test@youin.digital";

            var passwordService = new PasswordService();
            var mediator = MockMediator(false);

            // Act
            var command = new CreateUserCommand("New User", email);
            var handler = new CreateUserCommandHandler(helper.ApplicationDbContext, passwordService, mediator.Object);
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            var user = await helper.ApplicationDbContext
                .Users
                .FirstOrDefaultAsync(u => u.Id == result.Id);
            Assert.NotNull(user);
            Assert.NotEmpty(result.TemporaryPassword);
        }

        private static Mock<IMediator> MockMediator(bool exists)
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(m => m.Send(It.IsAny<ExistsUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ExistsUserDto()
                {
                    Exists = exists
                });

            return mediator;
        }
    }
}
