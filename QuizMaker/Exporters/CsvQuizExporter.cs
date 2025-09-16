using System.Text;
using System.Composition; // MEF
using QuizMaker.Models;
using QuizMaker.DTOs;

namespace QuizMaker.Exporters
{
    [Export(typeof(IQuizExporter))]
    public class CsvQuizExporter : IQuizExporter
    {
        public string Name
        {
            get { return "csv"; }
        }

        public string ContentType
        {
            get { return "text/csv"; }
        }

        public byte[] Export(QuizDto quiz)
        {
            var sb = new StringBuilder();
            sb.AppendLine(quiz.Title); // header row

            foreach (var q in quiz.Questions)
            {
                // Escape quotes / commas
                var text = q.Text?.Replace("\"", "\"\"") ?? string.Empty;
                sb.AppendLine($"\"{text}\"");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
