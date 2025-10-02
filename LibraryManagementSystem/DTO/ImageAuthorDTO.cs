namespace LibraryManagementSystem.API.DTO
{
    public class ImageAuthorDTO
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string ImageUrl { get; set; }
    }
}
