using Application.Common.Interfaces;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
namespace Infra.Services
{
    public class PdfService : IPdfService
    {

        public Stream FromHtml(string html)
        {
            using var memory = new MemoryStream();
            var pdf = new PdfDocument(new PdfWriter(memory));
            var doc = new Document(pdf);

            var elements = HtmlConverter.ConvertToElements(html);

            foreach (var element in elements)
            {
                doc.Add((IBlockElement)element);
            }

            doc.Close();

            return new MemoryStream(memory.ToArray());
        }
    }
}
