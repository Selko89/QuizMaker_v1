using QuizMaker.DTOs;
using QuizMaker.Models;

namespace QuizMaker.Exporters
{
    public interface IQuizExporter
    {
        /// <summary>Unique short name of the exporter, e.g. "csv", "pdf"</summary>
        string Name { get; }

        /// <summary>MIME content type returned for files produced by this exporter</summary>
        string ContentType { get; }

        /// <summary>Export quiz to a byte[] (file contents)</summary>
        byte[] Export(QuizDto quiz);
    }
}
