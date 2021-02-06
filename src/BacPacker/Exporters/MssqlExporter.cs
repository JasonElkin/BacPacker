using BacPacker.Messaging;
using Microsoft.SqlServer.Dac;
using NPoco;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace BacPacker.Exporters
{
    public class MssqlExporter : IDatabaseExporter
    {
        private readonly ILogger logger;

        private readonly IList<DatabaseType> compatibleDatabases = new List<DatabaseType>()
        {
            DatabaseType.SqlServer2005,
            DatabaseType.SqlServer2008,
            DatabaseType.SqlServer2012,
        };

        public MssqlExporter(ILogger logger)
        {
            this.logger = logger;
        }

        public Guid Id => new Guid("ab424efe-a4b9-4bf0-8de0-60da0aedfcd6");

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

        public bool SupportsDatabase(DatabaseType databaseType)
            => compatibleDatabases.Contains(databaseType);

    }
}
