using LibraryManagementSystem.Data.Entities;
using System.Net;
using System.Text;

namespace LibraryManagementSystem.API.Helpers
{
    public class PdfHtmlBuilder
    {
        public static string BuildHtml(List<BookRentals> active, List<ReturnedRental> returned)
        {
            var projectRoot = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\");
            var templatePath = Path.Combine(projectRoot, @"LibrarymanagementSystem.Utiles\HTML\PdfHtml.html");
            templatePath = Path.GetFullPath(templatePath);



            var html = File.ReadAllText(templatePath);

            string Encode(string s) => WebUtility.HtmlEncode(s ?? "");

            var activeRows = new StringBuilder();
            foreach (var r in active)
            {
                var customer = string.IsNullOrWhiteSpace(r.Customer?.FirstName)
                    ? "N/A"
                    : $"{r.Customer.FirstName} {r.Customer.LastName}";

                var endDate = r.RentEndDate == default
                    ? "Active"
                    : r.RentEndDate.ToString("yyyy-MM-dd");

                activeRows.AppendLine($@"
                  <tr>
                    <td>{Encode(r.Id.ToString())}</td>
                    <td>{Encode(r.Book?.Title ?? "N/A")}</td>
                    <td>{Encode(customer)}</td>
                    <td>{Encode(r.RentStartDate.ToString("yyyy-MM-dd"))}</td>
                    <td>{Encode(endDate)}</td>
                    <td class='right'>{Encode(r.Quantity.ToString())}</td>
                    <td class='right'>{Encode(r.Price.ToString("0.00"))}</td>
                  </tr>");
            }

            var returnedRows = new StringBuilder();
            foreach (var rr in returned)
            {
                var customer = !string.IsNullOrWhiteSpace(rr.CustomerName)
                    ? rr.CustomerName
                    : (rr.Customer != null ? $"{rr.Customer.FirstName} {rr.Customer.LastName}" : "N/A");

                returnedRows.AppendLine($@"
                  <tr>
                    <td>{Encode(rr.Id.ToString())}</td>
                    <td>{Encode(rr.BookTitle ?? rr.Book?.Title ?? "N/A")}</td>
                    <td>{Encode(customer)}</td>
                    <td>{Encode(rr.RentedAt.ToString("yyyy-MM-dd"))}</td>
                    <td>{Encode(rr.ReturnedAt.ToString("yyyy-MM-dd"))}</td>
                    <td class='right'>{Encode(rr.Quantity.ToString())}</td>
                    <td class='right'>{Encode(rr.Price.ToString("0.00"))}</td>
                  </tr>");
            }

            // Replace placeholders in template
            html = html.Replace("{{ACTIVE_ROWS}}", activeRows.ToString());
            html = html.Replace("{{RETURNED_ROWS}}", returnedRows.ToString());
            html = html.Replace("{{GENERATED_AT}}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            return html;
        }
    }
}
