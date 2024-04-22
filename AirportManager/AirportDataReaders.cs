// Here I use dictionary to choose which format to read from.
// This solution enables me to read from various formats in the future.

namespace AirportManager;

internal interface IAirportReader
{
    public List<AirportObject> ReadAirportObjects(string FilePath);
}

internal class AirportDataReader
{
    private readonly Dictionary<string, IAirportReader> Readers;

    public AirportDataReader()
    {
        Readers = new Dictionary<string, IAirportReader>
        {
            { "ftr", new FTRReader() }
        };
    }

    public List<AirportObject> ReadAirportObjects(string FilePath)
    {
        string[] FilePathSplitted = FilePath.Split('.');
        string FileFormat = FilePathSplitted[FilePathSplitted.Length - 1];
        return Readers[FileFormat].ReadAirportObjects(FilePath);
    }
}

internal class FTRReader : IAirportReader
{
    private AirportObjectBuilder? _AirportObjectBuilder;
    public FTRReader()
    { }
    public List<AirportObject> ReadAirportObjects(string FilePath)
    {
        List<AirportObject> ListOfObjects = new List<AirportObject>();
        _AirportObjectBuilder = new AirportObjectBuilder();

        try
        {
            string[] lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                ListOfObjects.Add(_AirportObjectBuilder.Build(line));
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Błąd: Plik nie istnieje. {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Inny błąd: {ex.Message}");
        }

        return ListOfObjects;
    }

}