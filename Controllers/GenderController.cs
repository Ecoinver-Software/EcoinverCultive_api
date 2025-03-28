using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/genders")]
    public class GenderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GenderController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/genders
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genders = await _context.Gender.ToListAsync();
            var genderDtos = _mapper.Map<IEnumerable<GenderDto>>(genders);
            return Ok(genderDtos);
        }

        // GET: api/genders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var gender = await _context.Gender.FindAsync(id);
            if (gender == null)
                return NotFound(new { message = "Gender not found." });

            var genderDto = _mapper.Map<GenderDto>(gender);
            return Ok(genderDto);
        }
    }
}
