using AutoMapper;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagmentSystem.Entities;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Data.DTO;
using LibraryManagementSystem.Data.DTO.Requests;

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
        public async Task<IActionResult> AddPublisher([FromBody] CreatePublisherRequest request)
        {
            var publisher = mapper.Map<Publisher>(request);
            if (publisher == null)
            {
                return BadRequest();
            }
            await publisherService.AddPublisher(publisher);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPublishers()
        {
            List<Publisher> publishers = await publisherService.GetAllPublishers();
            var publisherDTO = mapper.Map<List<PublisherDTO>>(publishers);
            return Ok(publisherDTO);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetPublisherById(Guid Id)
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
        public async Task<IActionResult> UpdatePublisher(Guid Id, CreatePublisherRequest request)
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
        public async Task<IActionResult> DeletePublisher(Guid Id)
        {
            var publisher = await publisherService.GetPublisherById(Id);
            if (publisher == null)
            {
                return NotFound("Publisher Not Found!");
            }
            await publisherService.Delete(Id);
            var publisherDTO = mapper.Map<PublisherDTO>(publisher);
            return Ok(publisherDTO);
        }
    }
}

