using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Identity
{
    public class User
    {
        public Guid Id {  get; set; }
        public string Username {  get; set; }=string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
    }
}
