namespace Application.Common.Interfaces
{
    public interface IPdfService
    {
        Stream FromHtml(string html);
    }
}
