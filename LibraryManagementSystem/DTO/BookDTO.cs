using LibraryManagmentSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.DTO
{
    public class BookDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public Author Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
