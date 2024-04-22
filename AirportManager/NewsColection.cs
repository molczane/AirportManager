using System.Collections;

namespace AirportManager;

internal class NewsColection : IEnumerable<(IReportable reportable, IReportableVisitor mediaVisitor)>
{
    private List<IReportableVisitor> mediae;
    private List<IReportable> reportables;

    public NewsColection(List<IReportableVisitor> mediae, List<IReportable> reportables)
    {
        this.mediae = mediae;
        this.reportables = reportables;
    }

    public IEnumerator<(IReportable reportable, IReportableVisitor mediaVisitor)> GetEnumerator()
    {
        foreach (var media in mediae)
        {
            foreach (var reportable in reportables)
            {
                yield return (reportable, media);
            }
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        // uses the strongly typed IEnumerable<T> implementation
        return GetEnumerator();
    }
}