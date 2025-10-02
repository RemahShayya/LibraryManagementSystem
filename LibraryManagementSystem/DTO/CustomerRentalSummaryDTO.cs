namespace LibraryManagementSystem.API.DTO
{
    public class CustomerRentalSummaryDTO
    {
        public string CustomerEmail { get; set; }
        public List<string> BookTitles { get; set; } = new();
        public double TotalPrice { get; set; }
    }
}
