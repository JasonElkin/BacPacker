using BacPacker.Messaging;
using NPoco;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;

namespace BacPacker.Exporters
{
    public interface IDatabaseExporter
    {
        Guid  Id { get; }

        string Name { get; }

        string Description { get; }

        bool SupportsDatabase(DatabaseType databaseType);

        Task<string> ExportDatabase(
            string fileName,
            IUmbracoDatabaseFactory umbracoDatabase, 
            IProgress<ProgressMessage> progress, 
            CancellationToken ct);

    }
}
