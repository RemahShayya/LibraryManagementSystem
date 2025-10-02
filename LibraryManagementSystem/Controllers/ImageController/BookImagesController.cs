using AutoMapper;
using LibraryManagementSystem.API.DTO;
using LibraryManagementSystem.API.Services;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.DTO;
using LibraryManagementSystem.Data.Entities.ImageEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;




namespace LibraryManagementSystem.API.Controllers.ImageController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookImagesController : ControllerBase
    {
        private readonly IBookImageService bookImageService;
        private readonly IBookService bookService;
        private readonly ImageServiceBook imageService;
        private readonly IMapper mapper;

        public BookImagesController(IBookImageService bookImageService, IBookService bookService, ImageServiceBook imageService, IMapper mapper)
        {
            this.bookImageService = bookImageService;
            this.bookService = bookService;
            this.imageService = imageService;
            this.mapper = mapper;
        }

        [HttpPost("Upload")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ImageBookDTO>> SaveImageFile(IFormFile file, Guid bookId)
        {
            var book = await bookService.GetBookById(bookId);
            if (book == null)
            {
                return NotFound("Book Not Found");
            }

            Guid imageId = Guid.NewGuid();

            string extension = Path.GetExtension(file.FileName);
            string fileName = imageId.ToString() + extension;

            string filePath = imageService.SaveImage(file, fileName);

            var bookImage = new BookImages
            {
                Id = imageId,
                BookId = bookId,
                ImageUrl = fileName
            };

            await bookImageService.AddBookImages(bookImage);
            await bookImageService.Save(bookImage);

            var bookImageDTO = mapper.Map<ImageBookDTO>(bookImage);

            return Ok(bookImageDTO);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<ImageBookDTO>> GetImage(Guid id)
        {
            var bookImage = await bookImageService.GetBookImagesById(id);
            if (bookImage == null)
                return NotFound("Image not found");

            var bookImageDTO = mapper.Map<ImageBookDTO>(bookImage);
            return Ok(bookImageDTO);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<IEnumerable<ImageBookDTO>>> GetAllImages([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var images = await bookImageService.GetAllBookImages();
            var paged = images.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var bookImageDTO = mapper.Map<IEnumerable<ImageBookDTO>>(paged);

            return Ok(bookImageDTO);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateImage(Guid id, IFormFile newFile)
        {
            var existingImage = await bookImageService.GetBookImagesById(id);
            if (existingImage == null)
                return NotFound("Image not found");
            string newFilePath = imageService.UpdateImage(newFile, existingImage.ImageUrl, existingImage.Id);

            string newFileName = Path.GetFileName(newFilePath);
            existingImage.ImageUrl = newFileName;

            bookImageService.Update(existingImage);
            await bookImageService.Save(existingImage);

            return Ok("Image updated successfully");
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            var existingImage = await bookImageService.GetBookImagesById(id);
            if (existingImage == null)
                return NotFound("Image not found");

            bool deleted = imageService.DeleteImageById(existingImage.ImageUrl);

            bookImageService.Delete(existingImage);
            await bookImageService.Save(existingImage);

            return Ok(deleted ? "Image deleted successfully (file + DB)." : "Image deleted from DB, but file not found on disk.");
        }
    }
}
