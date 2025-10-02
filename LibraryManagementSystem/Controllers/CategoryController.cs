using AutoMapper;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.DTO;
using LibraryManagementSystem.Data.DTO.Requests;
using LibraryManagmentSystem.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        [HttpGet("{Id}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(Guid Id)
        {
            var category = await categoryService.GetCategoryById(Id);
            if (category == null)
                return NotFound("Category Not Found!");

            var categoryDTO = mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }

        [HttpGet("sorted")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategoriesSorted([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var categories = await categoryService.GetAllCategories();
            categories = categories.OrderBy(c => c.Name);
            categories = categories.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var categoriesDTO = mapper.Map<List<CategoryDTO>>(categories);
            return Ok(categoriesDTO);
        }

        [HttpGet("filtered")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesFiltered(string? search, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var categories = await categoryService.GetAllCategories();
            if (string.IsNullOrEmpty(search))
            {
                var categoriesDto = mapper.Map<IEnumerable<CategoryDTO>>(categories);
                return Ok(categoriesDto);
            }
            if(!categories.Any())
            {
                return NotFound($"{search} Not Found");
            }
            categories = categories.Where(b => b.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            categories = categories.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var categoriesDTO=mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return Ok(categoriesDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDTO>> AddCategory([FromBody] CreateCategoryRequest request)
        {
            var category = mapper.Map<Category>(request);
            if (category == null)
                return BadRequest();

            await categoryService.AddCategory(category);
            await categoryService.Save(category);

            var categoryDTO = mapper.Map<CategoryDTO>(category);
            return CreatedAtAction(nameof(GetCategory), new { Id = category.Id }, categoryDTO);
        }

        [HttpPut("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(Guid Id, CreateCategoryRequest request)
        {
            var category = await categoryService.GetCategoryById(Id);
            if (category == null)
                return NotFound("Category Not Found!");

            category.Name = request.Name;
            await categoryService.Update(category);
            await categoryService.Save(category);

            var categoryDTO = mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCategory(Guid Id)
        {
            var category = await categoryService.GetCategoryById(Id);
            if (category == null)
                return NotFound("Category Not Found!");

            categoryService.Delete(category);
            return Ok();
        }
    }
}
