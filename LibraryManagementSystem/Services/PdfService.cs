using DocumentFormat.OpenXml.Spreadsheet;
using LibraryManagementSystem.API.Services.IServices;
using Microsoft.Playwright;

namespace LibraryManagementSystem.API.Services
{
    public class PdfService : IPdfService
    {
        private readonly IBrowser _browser;

        public PdfService(IBrowser browser)
        {
            _browser = browser;
        }

        public async Task<byte[]> GeneratePdfFromHtmlContentAsync(string htmlContent)
        {
            var context = await _browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.SetContentAsync(htmlContent);

            var pdfBytes = await page.PdfAsync(new PagePdfOptions
            {
                Format = "A4",
                PrintBackground = true,
                Margin = new Margin { Top = "20px", Bottom = "20px", Left = "12px", Right = "12px" }
            });

            await context.CloseAsync();
            return pdfBytes;
        }
    }
}
