using BacPacker.Composing;
using BacPacker.Exporters;
using BacPacker.Messaging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Hosting;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace BacPacker
{
    public class ExportService
    {
        private readonly string exportBasePath = HostingEnvironment.MapPath(Constants.ExportBasePath);

        private readonly DatabaseExporterCollection exporters;
        private readonly IUmbracoDatabaseFactory databaseFactory;
        private readonly ILogger logger;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        public ExportService(DatabaseExporterCollection databaseExporters, IUmbracoDatabaseFactory databaseFactory, ILogger logger)
        {
            this.exporters = databaseExporters;
            this.databaseFactory = databaseFactory;
            this.logger = logger;
        }

        public void CancelExports()
        {
            logger.Debug(this.GetType(), "Cancelling database export...");

            cts.Cancel();
        }

        public void TriggerExport(IProgress<ProgressMessage> reportTo, IDatabaseExporter exporter = null)
        {
            logger.Debug(this.GetType(), "Database export has been triggered.");

            exporter = exporter ?? GetExporterOrThrow();

            HostingEnvironment.QueueBackgroundWorkItem(async ct =>
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(ct, this.cts.Token);

                var filePath = Path.Combine(
                    exportBasePath,
                    DateTime.UtcNow.ToString(Constants.ExportDateFormat)
                    );

                Directory.CreateDirectory(filePath);

                var fileName = Path.Combine(filePath, Path.GetRandomFileName());

                logger.Info(this.GetType(), "Exporting database with exporter {exporter}", exporter);

                var exportPath = await exporter.ExportDatabase(fileName, databaseFactory, reportTo, cts.Token);

                logger.Info(this.GetType(), "Finished exporting database to exporter {exportPath}", exportPath);
            });
        }

        private IDatabaseExporter GetExporterOrThrow()
        {
            logger.Debug(this.GetType(), "Database exporter has not been supplied, finding first compatible exporter.");

            var dbType = databaseFactory.SqlContext.DatabaseType;

            var exporter = exporters.GetCompatibleExporters(dbType).FirstOrDefault();

            if (exporter == null)
            {
                throw new NoSupportedExporterException($"No IDatabaseExporter was found that supports a database provider \"{dbType.GetProviderName()}\"");
            }

            return exporter;
        }
    }
}
