using AutoMapper;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagmentSystem.Entities;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Data.DTO;
using LibraryManagementSystem.Data.DTO.Requests;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService publisherService;
        private readonly IMapper mapper;
        public PublisherController(IPublisherService publisherService, IMapper mapper)
        {
            this.publisherService = publisherService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PublisherDTO>> AddPublisher([FromBody] CreatePublisherRequest request)
        {
            var publisher = mapper.Map<Publisher>(request);
            if (publisher == null)
            {
                return BadRequest();
            }
            await publisherService.AddPublisher(publisher);
            var publisherDTO=mapper.Map<PublisherDTO>(publisher);
            Log.Information($"{publisherDTO.Name} Publisher Added");
            return CreatedAtAction(nameof(GetPublisherById), new { Id = publisher.Id }, publisherDTO);
        }

        [HttpGet("sorted")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<IEnumerable<PublisherDTO>>> GetAllPublishersSorted([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            IEnumerable<Publisher> publishers = await publisherService.GetAllPublishers();
            publishers=publishers.OrderBy(x=>x.Name);
            publishers=publishers.Skip((pageNumber-1)*pageSize).Take(pageSize);
            var publisherDTO = mapper.Map<List<PublisherDTO>>(publishers);
            return Ok(publisherDTO);
        }

        [HttpGet("filter")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<IEnumerable<PublisherDTO>>> GetPublishersFiltered(string? search, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            IEnumerable<Publisher> publishers=await publisherService.GetAllPublishers();
            publishers = publishers.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(search))
            {
                var publishersDto=mapper.Map<IEnumerable<PublisherDTO>>(publishers);
                return Ok(publishersDto);
            }
            if (!publishers.Any())
            {
                return NotFound($"{search} not found");
            }
            var publishersDTO=mapper.Map<IEnumerable<PublisherDTO>>(publishers);
            return Ok(publishersDTO);
        }

        [HttpGet("{Id}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<PublisherDTO>> GetPublisherById(Guid Id)
        {
            var publisher =await publisherService.GetPublisherById(Id);
            if (publisher == null)
            {
                return NotFound("Publisher Not Found!");
            }
            var publisherDTO = mapper.Map<PublisherDTO>(publisher);
            return Ok(publisherDTO);
        }

        [HttpPut("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PublisherDTO>> UpdatePublisher(Guid Id, CreatePublisherRequest request)
        {
            var publisher = await publisherService.GetPublisherById(Id);
            if (publisher == null)
            {
                return NotFound("Publisher Not Found!");
            }
            publisher.Name = request.Name;
            publisher.Location = request.Location;
            await publisherService.Update(publisher);
            var publisherDTO=mapper.Map<PublisherDTO>(publisher);
            return Ok(publisherDTO);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePublisher(Guid Id)
        {
            var publisher = await publisherService.GetPublisherById(Id);
            if (publisher == null)
            {
                return NotFound("Publisher Not Found!");
            }
            publisherService.Delete(publisher);
            var publisherDTO = mapper.Map<PublisherDTO>(publisher);
            return Ok();
        }
    }
}

