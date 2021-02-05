using BacPacker.Messaging;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace BacPacker.Web.Controllers
{
    public class DatabaseExportController : UmbracoAuthorizedApiController
    {
        private readonly ExportService exportService;

        public DatabaseExportController(ExportService exportService)
        {
            this.exportService = exportService;
        }

        [HttpGet]
        public IHttpActionResult Export()
        {
            exportService.TriggerExport(new ProgressReporter());
            return Ok();
        }
    }
}
