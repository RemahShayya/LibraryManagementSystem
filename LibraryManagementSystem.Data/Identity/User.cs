using LibraryManagementSystem.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Identity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; }= string.Empty;
        public DateTime DateCreated { get; set; }
        public ICollection<BookRentals> BookRentals { get; set; }
    }
}
