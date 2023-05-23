using ArenaGestor.API.Filters;
using ArenaGestor.APIContracts;
using ArenaGestor.APIContracts.ImportExport;
using ArenaGestor.BusinessInterface;
using ArenaGestor.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ArenaGestor.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImportExportController : ControllerBase, IImportExportAppService
    {
        private readonly IImportExportService importExportService;
        public ImportExportController(IImportExportService importExportService)
        {
            this.importExportService = importExportService;
        }

        [HttpGet]
        public IActionResult GetMethods()
        {
            List<string> methods = importExportService.GetMethods();
            return Ok(methods);
        }

        [AuthorizationFilter(RoleCode.Administrador)]
        [HttpPost("export")]
        public IActionResult Export([FromBody] ImportExportDto request)
        {
            importExportService.ExportData(request.Method, request.Path);
            return Ok("Data exported successfully in the file " + request.Path);
        }

        [AuthorizationFilter(RoleCode.Administrador)]
        [HttpPost("import")]
        public IActionResult Import([FromBody] ImportExportDto request)
        {
            ConcertsInsertResult result = importExportService.ImportData(request.Method, request.Path);
            return Ok(result);
        }
    }
}
