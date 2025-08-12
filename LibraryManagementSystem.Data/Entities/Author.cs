using LibraryManagementSystem.Data.Data.Common;
using System.Text.Json.Serialization;

namespace LibraryManagmentSystem.Entities
{
    public class Author : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Country { get; set; }

        public Guid BookId {  get; set; }
        [JsonIgnore]
        public Book Book { get; set; }
    }
}
