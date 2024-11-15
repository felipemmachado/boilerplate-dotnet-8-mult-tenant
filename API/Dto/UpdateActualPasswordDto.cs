namespace API.Dto
{
    public record struct UpdateActualPasswordDto(
        string ActualPassword,
        string Password,
        string RePassword);
}
