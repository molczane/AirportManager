using System.Collections;

namespace AirportManager;

internal class NewsGenerator
{
    private NewsColection newsColection;
    private IEnumerator<(IReportable reportable, IReportableVisitor mediaVisitor)> newsEnumerator;
    private (IReportable reportable, IReportableVisitor mediaVisitor) currentNewsPair;
    public NewsGenerator(List<IReportableVisitor> mediae, List<IReportable> reportables)
    {
        newsColection = new NewsColection(mediae, reportables);
        newsEnumerator = newsColection.GetEnumerator();
    }
    public string? GenerateNextNews()
    {
        if (newsEnumerator.MoveNext())
        {
            currentNewsPair = newsEnumerator.Current;
            return currentNewsPair.reportable.Accept(currentNewsPair.mediaVisitor);
        }
        return null;
    }
}