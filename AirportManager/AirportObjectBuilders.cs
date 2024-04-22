namespace AirportManager;

internal interface IBuilder
{
    public AirportObject BuildObject(string[] Data);
}
internal class AirportObjectBuilder
{
    private readonly Dictionary<string, IBuilder> Builders = new Dictionary<string, IBuilder>()
    {
        { "C", new CrewBuilder()},
        { "P", new PassengerBuilder() },
        { "CA", new CargoBuilder() },
        { "CP", new CargoPlaneBuilder() },
        { "PP", new PassengerPlaneBuilder() },
        { "AI", new AirportBuilder() },
        { "FL", new FlightBuilder() }
    };

    public AirportObject Build(string Data)
    {
        string[] Records = Data.Split(',');
        return Builders[Records[0]].BuildObject(Records[0..]);
    }
}
internal class CrewBuilder : IBuilder
{
    public AirportObject BuildObject(string[] Data) => new Crew(Data); 
}
internal class PassengerBuilder : IBuilder
{
    public AirportObject BuildObject(string[] Data) => new Passenger(Data);
}
internal class CargoBuilder : IBuilder
{
    public AirportObject BuildObject(string[] Data) => new Cargo(Data);
}
internal class CargoPlaneBuilder : IBuilder
{
    public AirportObject BuildObject(string[] Data) => new CargoPlane(Data);
}
internal class PassengerPlaneBuilder : IBuilder
{ 
    public AirportObject BuildObject(string[] Data) =>new PassengerPlane(Data); 
}
internal class AirportBuilder : IBuilder
{ 
    public AirportObject BuildObject(string[] Data) => new Airport(Data);
}
internal class FlightBuilder : IBuilder 
{ 
    public AirportObject BuildObject(string[] Data) => new Flight(Data);
}