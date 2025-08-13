using AutoMapper;
using LibraryManagementSystem.API.Services;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.DTO;
using LibraryManagementSystem.Data.DTO.Requests;
using LibraryManagmentSystem.Entities;
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
        public async Task<IActionResult> GetCategory(Guid Id)
        {
            var category = await categoryService.GetCategoryById(Id);
            if (category == null)
                return NotFound("Category Not Found!");

            var categoryDTO = mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryService.GetAllCategories();
            var categoriesDTO = mapper.Map<List<CategoryDTO>>(categories);
            return Ok(categoriesDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryRequest request)
        {
            var category = mapper.Map<Category>(request);
            if (category == null)
                return BadRequest();

            await categoryService.AddCategory(category);
            await categoryService.Save(category);

            var categoryDTO = mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateCategory(Guid Id, CreateCategoryRequest request)
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
        public async Task<IActionResult> DeleteCategory(Guid Id)
        {
            var category = await categoryService.GetCategoryById(Id);
            if (category == null)
                return NotFound("Category Not Found!");

            await categoryService.Delete(Id);
            return Ok();
        }
    }
}
