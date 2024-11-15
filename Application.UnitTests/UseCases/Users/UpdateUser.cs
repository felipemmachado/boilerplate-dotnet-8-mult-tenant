using Application.Common.Constants;
using Application.UseCases.Users.UpdateUser;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.UnitTests.UseCases.Users
{
    public class UpdateUser
    {
        [Fact]
        public async Task Should_not_update_user_with_wrong_user_id()
        {
            // Arrange
            var helper = new TestHelper();
            await helper.CreateCompany();
            
            // Act
            var command = new UpdateUserCommand(Guid.NewGuid(), "Name", "Email");
            var handler = new UpdateUserCommandHandler(helper.ApplicationDbContext);
            var ex = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, new CancellationToken()));
            
            // Assert
            var error = ex.Errors.SelectMany(p => p.Errors.Where(e => e == ApiResponseMessages.UserNotFound));
            Assert.True(error.Any());
        }
    }
}
