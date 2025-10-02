using LibraryManagementSystem.API.Helpers;
using LibraryManagementSystem.API.Services;
using LibraryManagementSystem.API.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IBookRentalService _rentalService;
        private readonly IReturnedRentalService _returnedRentalService;
        private readonly IBookRentalService _bookRentalService;
        private readonly IPdfService _pdfService;
        public ReportsController(IBookRentalService rentalService, IReturnedRentalService returnedRentalService, IBookRentalService bookRentalService, IPdfService pdfService)
        {
            _rentalService = rentalService;
            _returnedRentalService = returnedRentalService;
            _bookRentalService = bookRentalService;
            _pdfService = pdfService;
        }

        [HttpGet("export-rentals")]
        public async Task<IActionResult> ExportRentals([FromServices] ExcelExportService excelExportService)
        {
            var rentals = await _rentalService.GetAllBookRentalsWithIncludes();
            var returnedRentals = await _returnedRentalService.GetAllBookReturnedRentalsWithIncludes();
            var fileContent = excelExportService.ExportBookRentalsToExcel(rentals.ToList(), returnedRentals.ToList());
            var fileName = $"BookRentals_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export-rentals-playwright")]
        public async Task<IActionResult> ExportRentalsPlaywright()
        {
            var active = await _bookRentalService.GetAllBookRentalsWithIncludes();
            var returned = await _returnedRentalService.GetAllBookReturnedRentalsWithIncludes();

            var html = PdfHtmlBuilder.BuildHtml(active.ToList(), returned.ToList());

            var pdfBytes = await _pdfService.GeneratePdfFromHtmlContentAsync(html);
            return File(pdfBytes, "application/pdf", "BookRentalsReport.pdf");
        }
    }
}
