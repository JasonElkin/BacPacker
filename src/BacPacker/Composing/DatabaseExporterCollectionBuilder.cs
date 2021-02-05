using BacPacker.Exporters;
using Umbraco.Core.Composing;

namespace BacPacker.Composing
{
    public class DatabaseExporterCollectionBuilder : 
        LazyCollectionBuilderBase<DatabaseExporterCollectionBuilder, DatabaseExporterCollection, IDatabaseExporter>
    {
        protected override DatabaseExporterCollectionBuilder This => this;
    }
}
