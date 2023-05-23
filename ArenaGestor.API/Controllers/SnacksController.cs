using System.Collections.Generic;
using ArenaGestor.API.Filters;
using ArenaGestor.APIContracts;
using ArenaGestor.APIContracts.Snack;
using ArenaGestor.BusinessInterface;
using ArenaGestor.Domain;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ArenaGestor.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ExceptionFilter]
    public class SnacksController : ControllerBase, ISnacksAppService
    {
        private readonly ISnackService snackService;
        private readonly IMapper mapper;
        public SnacksController(ISnackService snackService, IMapper mapper)
        {
            this.snackService = snackService;
            this.mapper = mapper;
        }

        [AuthorizationFilter(RoleCode.Administrador)]
        [HttpPost]
        public IActionResult PostSnacks(SnackDto snackDto)
        {
            Snack snack = mapper.Map<Snack>(snackDto);
            snackService.AddSnack(snack);
            SnackResponseDto snackResponseDto = new SnackResponseDto
            {
                Message = "Snack added successfully"
            };
            return Ok(snackResponseDto);
        }
        [AuthorizationFilter(RoleCode.Administrador)]
        [HttpDelete("{snackId}")]
        public IActionResult DeleteSnacks([FromRoute] int snackId)
        {
            snackService.RemoveSnack(snackId);
            SnackResponseDto snackResponseDto = new SnackResponseDto
            {
                Message = "Snack deleted successfully"
            };
            return Ok(snackResponseDto);
        }


        [AuthorizationFilter(RoleCode.Espectador)]
        [HttpGet]
        public IActionResult GetSnacks()
        {
            List<Snack> snacks = snackService.GetSnacks();
            return Ok(snacks);
        }
    }
}
