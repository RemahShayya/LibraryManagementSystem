using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.DTO.Requests
{
    public class CreatedBookRequest
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public Guid AuthorId { get; set; } 
        public List<Guid> BookCategoryIds { get; set; }
    }
}
