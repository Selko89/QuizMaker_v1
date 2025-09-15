using QuizMaker.DTOs;
using QuizMaker.Exporters;
using QuizMaker.Models;
using System.Composition.Hosting;
using System.Reflection;

namespace QuizMaker.Services
{
    public class QuizExportService
    {
        private readonly List<IQuizExporter> _exporters;

        public QuizExportService()
        {
            // Compose and materialize exporters once
            var configuration = new ContainerConfiguration()
                .WithAssembly(Assembly.GetExecutingAssembly());

            using var container = configuration.CreateContainer();
            _exporters = container.GetExports<IQuizExporter>().ToList();
        }
        public IEnumerable<string> GetAvailableExporters()
        {
            return _exporters.Select(e => e.Name);
        }

        public (byte[] Content, string ContentType)? Export(string exporterName, QuizDto quiz)
        {
            var exporter = _exporters.FirstOrDefault(e =>
                e.Name.Equals(exporterName, StringComparison.OrdinalIgnoreCase));

            if (exporter == null) return null;

            var bytes = exporter.Export(quiz);
            return (bytes, exporter.ContentType);
        }

    }
}
