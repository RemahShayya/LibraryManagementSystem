using ClosedXML.Excel;
using LibraryManagementSystem.Data.Entities;

namespace LibraryManagementSystem.API.Services
{
    public class ExcelExportService
    {
        public byte[] ExportBookRentalsToExcel(List<BookRentals> activeRentals, List<ReturnedRental> returnedRentals)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Library Rentals");

                // Title
                worksheet.Cell(1, 1).Value = "Library Rental Report";
                worksheet.Range(1, 1, 1, 7).Merge().Style
                    .Font.SetBold()
                    .Font.SetFontSize(16)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                // ===== ACTIVE RENTALS =====
                worksheet.Cell(3, 1).Value = "Active Book Rentals";
                worksheet.Range(3, 1, 3, 7).Merge().Style
                    .Font.SetBold()
                    .Fill.SetBackgroundColor(XLColor.LightGray)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                // Headers
                string[] headers = { "ID", "Book Title", "Customer Name", "Rent Start Date", "Rent End Date", "Quantity", "Price" };
                for (int j = 0; j < headers.Length; j++)
                {
                    worksheet.Cell(4, j + 1).Value = headers[j];
                    worksheet.Cell(4, j + 1).Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.AliceBlue);
                    worksheet.Cell(4, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                }

                // Data
                for (int i = 0; i < activeRentals.Count; i++)
                {
                    var rental = activeRentals[i];
                    worksheet.Cell(i + 5, 1).Value = rental.Id.ToString();
                    worksheet.Cell(i + 5, 2).Value = rental.Book?.Title ?? "N/A";
                    worksheet.Cell(i + 5, 3).Value = rental.Customer?.FirstName ?? "N/A";
                    worksheet.Cell(i + 5, 4).Value = rental.RentStartDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(i + 5, 5).Value = rental.RentEndDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(i + 5, 6).Value = rental.Quantity;
                    worksheet.Cell(i + 5, 7).Value = rental.Price;
                }

                int returnedStartRow = activeRentals.Count + 7;

                // ===== RETURNED RENTALS =====
                worksheet.Cell(returnedStartRow, 1).Value = "Returned Rentals";
                worksheet.Range(returnedStartRow, 1, returnedStartRow, 7).Merge().Style
                    .Font.SetBold()
                    .Fill.SetBackgroundColor(XLColor.LightGray)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                for (int j = 0; j < headers.Length; j++)
                {
                    worksheet.Cell(returnedStartRow + 1, j + 1).Value = headers[j];
                    worksheet.Cell(returnedStartRow + 1, j + 1).Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.AliceBlue);
                    worksheet.Cell(returnedStartRow + 1, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                }

                for (int i = 0; i < returnedRentals.Count; i++)
                {
                    var rental = returnedRentals[i];
                    worksheet.Cell(returnedStartRow + 2 + i, 1).Value = rental.Id.ToString();
                    worksheet.Cell(returnedStartRow + 2 + i, 2).Value = rental.BookTitle?? "N/A";
                    worksheet.Cell(returnedStartRow + 2 + i, 3).Value = rental.CustomerName ?? "N/A";
                    worksheet.Cell(returnedStartRow + 2 + i, 4).Value = rental.RentedAt.ToString("yyyy-MM-dd");
                    worksheet.Cell(returnedStartRow + 2 + i, 5).Value = rental.ReturnedAt.ToString("yyyy-MM-dd");
                    worksheet.Cell(returnedStartRow + 2 + i, 6).Value = rental.Quantity;
                    worksheet.Cell(returnedStartRow + 2 + i, 7).Value = rental.Price;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
