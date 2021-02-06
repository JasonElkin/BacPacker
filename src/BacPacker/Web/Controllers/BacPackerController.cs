using BacPacker.Messaging;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace BacPacker.Web.Controllers
{
    public class BacPackerController : UmbracoAuthorizedApiController
    {
        private readonly ExportService exportService;

        public BacPackerController(ExportService exportService)
        {
            this.exportService = exportService;
        }

        [HttpGet]
        public IHttpActionResult GetExporters()
        {
            return Ok(exportService.GetCompatibleExporters());
        }

        [HttpGet]
        public IHttpActionResult Export()
        {
            exportService.TriggerExport(new ProgressReporter());
            return Ok();
        }
    }
}
