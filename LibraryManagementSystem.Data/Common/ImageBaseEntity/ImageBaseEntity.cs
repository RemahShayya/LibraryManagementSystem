using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Common.ImageBaseEntity
{
    public abstract class ImageBaseEntity : IImageBaseEntity
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
    }
}
