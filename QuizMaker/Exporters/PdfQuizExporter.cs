using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuizMaker.DTOs;
using QuizMaker.Models;
using System.Composition;
using System.IO;

namespace QuizMaker.Exporters
{
    [Export(typeof(IQuizExporter))]
    public class PdfQuizExporter : IQuizExporter
    {
        public string Name
        {
            get { return "pdf"; }
        }

        public string ContentType
        {
            get { return "application/pdf"; }
        }

        public byte[] Export(QuizDto quiz)
        {
            using var ms = new MemoryStream();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text($"Quiz: {quiz.Title}").FontSize(18).Bold();

                    page.Content().Column(col =>
                    {
                        col.Spacing(6);   // adds 6pt spacing between each item automatically
                        foreach (var q in quiz.Questions)
                        {
                            col.Item().Text(q.Text).FontSize(12);
                        }
                    });

                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.Span("Page ");
                        t.CurrentPageNumber();
                        t.Span(" / ");
                        t.TotalPages();
                    });
                });
            })
            .GeneratePdf(ms);

            return ms.ToArray();
        }


    }
}
