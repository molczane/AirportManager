using System.Text.Json;

// Here I made a decision to let the user decide which format
// he chooses to serialize to.
// By now it has only been .json format, but it is suitable 
// to extend to other serializations format.

namespace AirportManager;

internal interface IAirportDataSerializer
{
    public string Serialize(List<AirportObject> AirportObjectsList);
}

internal class AirportDataSerializer
{
    public AirportDataSerializer()
    {
        Serializers = new Dictionary<string, IAirportDataSerializer>()
        {
            { "json", new JSONAirportDataSerializer() }
        };
    }
    private readonly Dictionary<string, IAirportDataSerializer> Serializers;
    public void SerializeToFile(List<AirportObject> AirportObjectsList, string FileName)
    {
        string FileNameSaved = FileName;
        string[] Data = FileName.Split('.');
        string Format = Data[Data.Length - 1];
        string JsonText = Serializers[Format].Serialize(AirportObjectsList);
        File.WriteAllText(FileNameSaved, JsonText);
    }
}

internal class JSONAirportDataSerializer : IAirportDataSerializer
{
    public JSONAirportDataSerializer() { }
    public string Serialize(List<AirportObject> AirportObjectsList)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        return JsonSerializer.Serialize(AirportObjectsList, options); ;
    }

}