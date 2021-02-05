using Umbraco.Core.Composing;

namespace BacPacker.Composing
{
    public static class CompositionExtensions
    {
        public static DatabaseExporterCollectionBuilder DatabaseExporters(this Composition composition)
            => composition.WithCollectionBuilder<DatabaseExporterCollectionBuilder>();
    }
}
