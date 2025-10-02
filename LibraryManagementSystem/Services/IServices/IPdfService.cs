namespace LibraryManagementSystem.API.Services.IServices
{
    public interface IPdfService
    {
        Task<byte[]> GeneratePdfFromHtmlContentAsync(string htmlContent);
    }
}
