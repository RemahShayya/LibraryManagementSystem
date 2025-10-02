using LibraryManagementSystem.Data.Data.Common;
using System.Collections.Generic;

namespace LibraryManagmentSystem.Entities
{
    public class Category: BaseEntity
    {
        public Category() 
        {
            BookCategories= new HashSet<BookCategory>();
        }
        public string? Name { get; set; }

        public ICollection<BookCategory> BookCategories { get; set; }
      
    }
}
