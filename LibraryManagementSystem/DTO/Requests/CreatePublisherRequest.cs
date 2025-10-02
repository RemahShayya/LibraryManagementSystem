using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.DTO.Requests
{
    public class CreatePublisherRequest
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
    }
}
