using BacPacker.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;

namespace BacPacker.Exporters
{
    public interface IDatabaseExporter
    {
        string Name { get; }

        string Description { get; }

        bool SupportsDatabase(string databaseProviderName);

        Task<string> ExportDatabase(
            string fileName,
            IUmbracoDatabaseFactory umbracoDatabase, 
            IProgress<ProgressMessage> progress, 
            CancellationToken ct);

    }
}
