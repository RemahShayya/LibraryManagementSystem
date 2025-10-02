using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.DTO
{
    public class BookRentalDTO
    {
        public string BookName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime RentStartDate { get; set; }
        public DateTime RentEndDate { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
