using LibraryManagementSystem.Data.Data.Common;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibraryManagmentSystem.Entities
{
    public class Publisher : BaseEntity
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        [JsonIgnore]
        public ICollection<Book> Books { get; set; }
    }
}
