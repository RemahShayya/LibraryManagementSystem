using LibraryManagementSystem.Data.Data.Common;
using System.Security.Principal;
namespace LibraryManagementSystem.Data.Data.Common
{
    public abstract class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public DateTime? LastModifiedBy { get; set; }
    }
}