using LibraryManagementSystem.Data.Data.Common;
using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Data.Entities.ImageEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagmentSystem.Entities
{
    public class Book: BaseEntity
    {
        public Book()
        {
            BookCategories = new HashSet<BookCategory>();
        }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public Author Author { get; set; }
        public Guid? PublisherId { get; set; } 
        public Publisher Publisher { get; set; }
        public ICollection<BookCategory> BookCategories { get; set; }
        public ICollection<BookImages> BookImages { get; set; } = new List<BookImages>();
        public ICollection<BookRentals> BookRentals { get; set; }
    }
}
