using AutoMapper;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagmentSystem.Entities;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Data.DTO;
using LibraryManagementSystem.Data.DTO.Requests;


using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService authorService;
        private readonly IMapper mapper;

        public AuthorController(IAuthorService authorService, IMapper mapper)
        {
            this.authorService = authorService;
            this.mapper = mapper;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAuthor(Guid Id)
        {
            var author =await authorService.GetAuthorById(Id);
            if (author == null)
            {
                return NotFound("Author Not Found!");
            }

            var authorDTO = mapper.Map<AuthorDTO>(author);
            return Ok(authorDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            List<Author> authors =await authorService.GetAllAuthors();
            var authorsDTO = mapper.Map<List<AuthorDTO>>(authors);
            return Ok(authorsDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromBody] CreateAuthorRequest request)
        {
            var addedAuthor=mapper.Map<Author>(request);
            if(addedAuthor == null)
            {
                return BadRequest();
            }
            await authorService.AddAuthor(addedAuthor);
            await authorService.Save(addedAuthor);
            var authorDTO=mapper.Map<AuthorDTO>(addedAuthor);
            return Ok(authorDTO);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAuthor(Guid Id)
        {
            var author=await authorService.GetAuthorById(Id);
            if(author == null)
            {
                return NotFound("Author Not Found!");
            }
            await authorService.Delete(Id);
            return Ok();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateAuthor(Guid Id, CreateAuthorRequest request)
        {
            var author=await authorService.GetAuthorById(Id);
            if(author == null)
            {
                return NotFound("Author Not Found!");
            }
            author.FirstName = request.FirstName;
            author.LastName = request.LastName;
            author.BirthDate=request.BirthDate;
            author.Country=request.Country;
            author.BookId=request.BookId;
            await authorService.Update(author);
            await authorService.Save(author);

            var authorDTO=mapper.Map<AuthorDTO>(author);
            return Ok(authorDTO);
        }
    }
}
