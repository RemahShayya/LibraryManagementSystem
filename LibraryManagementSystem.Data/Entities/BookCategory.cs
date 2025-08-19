using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryManagmentSystem.Entities
{
    public class BookCategory
    {
        public Guid BookId { get; set; }
        [JsonIgnore]
        public Book Book { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
