using LibraryManagementSystem.Data.Identity;
using LibraryManagmentSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Entities
{
    public class BookRentals
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public string CustomerId { get; set; }
        public User Customer { get; set; }
        public DateTime RentStartDate { get; set; }
        public DateTime RentEndDate { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
