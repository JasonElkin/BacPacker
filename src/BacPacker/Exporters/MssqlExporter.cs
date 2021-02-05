using BacPacker.Messaging;
using Microsoft.SqlServer.Dac;
using NPoco;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Core.Logging;

namespace BacPacker.Exporters
{
    public class MssqlExporter : IDatabaseExporter
    {
        private readonly ILogger logger;

        public IList<string> compatibleDatabases = new List<string>()
        {
            DatabaseType.SqlServer2005.GetProviderName(),
            DatabaseType.SqlServer2008.GetProviderName(),
            DatabaseType.SqlServer2012.GetProviderName(),
        };

        public MssqlExporter(ILogger logger)
        {
            this.logger = logger;
        }

        public string Name => "MSSQL";

        public string Description => "Exports an MSSQL database to a .bacpac file.";

        public Task<string> ExportDatabase(
            string fileName,
            IUmbracoDatabaseFactory umbracoDatabase,
            IProgress<ProgressMessage> progress,
            CancellationToken ct)
        {
            return Task.Run(() =>
            {
                return Export(fileName, umbracoDatabase, progress, ct);
            });
        }

        private string Export(
            string fileName,
            IUmbracoDatabaseFactory umbracoDatabase,
            IProgress<ProgressMessage> progress,
            CancellationToken ct)
        {
            var dacService = new DacServices(umbracoDatabase.ConnectionString);

            dacService.ProgressChanged += (s, e) =>
            {
                progress.Report(new ProgressMessage()
                {
                    Message = e.Message,
                    Status = e.Status.ToString()
                });

                logger.Debug(this.GetType(), e.Message);
            };

            dacService.Message += (s, e) =>
            {
                progress.Report(new ProgressMessage()
                {
                    Message = e.Message.Message
                });

                logger.Debug(this.GetType(), $"Database export message: {e.Message}");
            };

            var dbName = new SqlConnectionStringBuilder(umbracoDatabase.ConnectionString).InitialCatalog;

            fileName += ".bacpac";

            dacService.ExportBacpac(fileName, dbName, cancellationToken: ct);

            return fileName;
        }

        public bool SupportsDatabase(string databaseProviderName)
            => compatibleDatabases.Contains(databaseProviderName);

    }
}
