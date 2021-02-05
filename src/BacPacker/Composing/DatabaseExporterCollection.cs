using BacPacker.Exporters;
using NPoco;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Composing;

namespace BacPacker.Composing
{
    public class DatabaseExporterCollection : BuilderCollectionBase<IDatabaseExporter>
    {
        public DatabaseExporterCollection(IEnumerable<IDatabaseExporter> items)
          : base(items)
        { }

        public IEnumerable<IDatabaseExporter> GetCompatibleExporters(DatabaseType databaseType)
            => this.Where(x => x.SupportsDatabase(databaseType.GetProviderName()));
    }
}
