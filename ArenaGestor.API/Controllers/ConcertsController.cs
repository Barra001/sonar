using ArenaGestor.API.Filters;
using ArenaGestor.APIContracts;
using ArenaGestor.APIContracts.Concert;
using ArenaGestor.BusinessInterface;
using ArenaGestor.Domain;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ArenaGestor.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConcertsController : ControllerBase, IConcertsAppService
    {

        public readonly IConcertsService ConcertService;
        private readonly IMapper mapper;

        public ConcertsController(IConcertsService concertService, IMapper mapper)
        {
            this.ConcertService = concertService;
            this.mapper = mapper;
        }

        [HttpGet("{concertId}")]
        public IActionResult GetConcertById(int concertId)
        {
            var result = ConcertService.GetConcertById(concertId);
            var resultDto = mapper.Map<ConcertResultConcertDto>(result);
            return Ok(resultDto);
        }

        [HttpGet]
        public IActionResult GetConcerts([FromQuery] ConcertGetConcertsDto concertFilter = null)
        {
            var concert = mapper.Map<ConcertFilter>(concertFilter);

            var result = ConcertService.GetConcerts(concert);
            var resultDto = mapper.Map<IEnumerable<ConcertResultConcertDto>>(result);
            return Ok(resultDto);
        }

        [AuthorizationFilter(RoleCode.Artista)]
        [HttpGet("ByArtist")]
        public IActionResult GetConcertsByArtist([FromQuery] ConcertGetConcertsDto concertFilter = null)
        {
            var concert = mapper.Map<ConcertFilter>(concertFilter);
            var token = this.HttpContext.Request.Headers["token"];

            var result = ConcertService.GetConcerts(token, concert);
            var resultDto = mapper.Map<IEnumerable<ConcertResultConcertArtistDto>>(result);
            return Ok(resultDto);
        }

        [AuthorizationFilter(RoleCode.Administrador)]
        [HttpPost]
        public IActionResult PostConcert([FromBody] ConcertInsertConcertDto insertConcert)
        {
            var concert = mapper.Map<Concert>(insertConcert);
            var result = ConcertService.InsertConcert(concert);
            var resultDto = mapper.Map<ConcertResultConcertDto>(result);
            return Ok(resultDto);
        }

        [AuthorizationFilter(RoleCode.Administrador)]
        [HttpPut]
        public IActionResult PutConcert([FromBody] ConcertUpdateConcertDto updateConcert)
        {
            var concert = mapper.Map<Concert>(updateConcert);
            var result = ConcertService.UpdateConcert(concert);
            var resultDto = mapper.Map<ConcertResultConcertDto>(result);
            return Ok(resultDto);
        }

        [AuthorizationFilter(RoleCode.Administrador)]
        [HttpDelete("{concertId}")]
        public IActionResult DeleteConcert([FromRoute] int concertId)
        {
            ConcertService.DeleteConcert(concertId);
            return Ok();
        }

    }
}
