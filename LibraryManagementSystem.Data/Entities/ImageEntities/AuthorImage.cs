using LibraryManagementSystem.Data.Common.ImageBaseEntity;
using LibraryManagmentSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Entities.ImageEntities
{
    public class AuthorImage : ImageBaseEntity
    {
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
