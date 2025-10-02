namespace LibraryManagementSystem.Data.Data.Common;

public interface IEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public DateTime? LastModifiedBy { get; set; }
}
