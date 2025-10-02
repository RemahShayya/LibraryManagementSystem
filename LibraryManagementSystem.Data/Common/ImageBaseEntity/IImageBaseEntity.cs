using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Common.ImageBaseEntity
{
    public interface IImageBaseEntity
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
    }
}
