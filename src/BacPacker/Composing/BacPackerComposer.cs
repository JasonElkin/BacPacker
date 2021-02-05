using BacPacker.Exporters;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace BacPacker.Composing
{
    class BacPackerComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.RegisterUnique<ExportService>();
            composition.DatabaseExporters().Add<MssqlExporter>();
        }
    }
}
