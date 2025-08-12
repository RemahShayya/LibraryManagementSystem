using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.DTO
{
    public class CreatedBookRequest
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public int AuthorId { get; set; } 
        [Required]
        public int PublisherId { get; set; }
        public List<int> BookCategoryIds { get; set; }
    }
}
