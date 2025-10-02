using AutoMapper;
using LibraryManagementSystem.API.DTO;
using LibraryManagementSystem.API.Services;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Entities.ImageEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.API.Controllers.ImageController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorImageController : ControllerBase
    {
        private readonly IAuthorImageService authorImageService;
        private readonly IAuthorService authorService;
        private readonly ImageServiceAuthor imageService;
        private readonly IMapper mapper;

        public AuthorImageController(IAuthorImageService authorImageService, IAuthorService authorService, ImageServiceAuthor imageService, IMapper mapper)
        {
            this.authorImageService = authorImageService;
            this.authorService = authorService;
            this.imageService = imageService;
            this.mapper = mapper;
        }

        [HttpPost("Upload")]
        public async Task<ActionResult<ImageAuthorDTO>> SaveImageFile(IFormFile file, Guid authorId)
        {
            var author = await authorService.GetAuthorById(authorId);
            if (author == null)
            {
                return NotFound("Author Not Found");
            }

            Guid imageId = Guid.NewGuid();

            string extension = Path.GetExtension(file.FileName);
            string fileName = imageId.ToString() + extension;

            string filePath = imageService.SaveImage(file, fileName);

            var authorImage = new AuthorImage
            {
                Id = imageId,
                AuthorId = authorId,
                ImageUrl = fileName
            };

            await authorImageService.AddAuthorImage(authorImage);
            await authorImageService.Save(authorImage);

            var authorImageDTO = mapper.Map<ImageAuthorDTO>(authorImage);
            return Ok(authorImageDTO);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<ImageAuthorDTO>> GetImage(Guid id)
        {
            var authorImage = await authorImageService.GetAuthorImageById(id);
            if (authorImage == null)
                return NotFound("Image not found");

            var authorImageDTO = mapper.Map<ImageAuthorDTO>(authorImage);
            return Ok(authorImageDTO);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<IEnumerable<ImageAuthorDTO>>> GetAllImages([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var images = await authorImageService.GetAllAuthorsImages();
            var paged = images.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var authorImageDTO = mapper.Map<IEnumerable<ImageAuthorDTO>>(paged);
            return Ok(authorImageDTO);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateImage(Guid id, IFormFile newFile)
        {
            var existingImage = await authorImageService.GetAuthorImageById(id);
            if (existingImage == null)
                return NotFound("Image not found");
            string newFilePath = imageService.UpdateImage(newFile, existingImage.ImageUrl, existingImage.Id);

            string newFileName = Path.GetFileName(newFilePath);
            existingImage.ImageUrl = newFileName;

            authorImageService.Update(existingImage);
            await authorImageService.Save(existingImage);

            return Ok("Image updated successfully");
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            var existingImage = await authorImageService.GetAuthorImageById(id);
            if (existingImage == null)
                return NotFound("Image not found");

            bool deleted = imageService.DeleteImageById(existingImage.ImageUrl);

            authorImageService.Delete(existingImage);
            await authorImageService.Save(existingImage);

            return Ok(deleted ? "Image deleted successfully (file + DB)." : "Image deleted from DB, but file not found on disk.");
        }

    }
}
