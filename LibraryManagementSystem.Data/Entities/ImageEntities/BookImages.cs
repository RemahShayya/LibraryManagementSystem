using LibraryManagementSystem.Data.Common.ImageBaseEntity;
using LibraryManagmentSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Entities.ImageEntities
{
    public class BookImages : ImageBaseEntity
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}
