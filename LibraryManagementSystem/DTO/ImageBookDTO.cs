namespace LibraryManagementSystem.API.DTO
{
    public class ImageBookDTO
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string ImageUrl { get; set; }
    }
}
