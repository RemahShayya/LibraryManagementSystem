using LibraryManagementSystem.Data.Identity;
using LibraryManagmentSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Entities
{
    public class ReturnedRental
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string CustomerId { get; set; }
        public DateTime RentedAt { get; set; }
        public DateTime ReturnedAt { get; set; }
        public Book Book { get; set; }
        public User Customer { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
