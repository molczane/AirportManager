namespace AirportManager;

internal class FlightGUIDataAdapter : FlightsGUIData
{
    private List<Flight> flightList;
    
    public FlightGUIDataAdapter(List<Flight> _flightDict, Dictionary<UInt64, Airport> _airportDict)
    {
        flightList = _flightDict;
    }

    public override int GetFlightsCount()
    {
        return flightList.Count;
    }

    public override ulong GetID(int index)
    {
        return flightList[index].ID;
    }

    public override WorldPosition GetPosition(int index)
    {
        return new WorldPosition( (double)flightList[index].Latitude, (double)flightList[index].Longitude );
    }

    public override double GetRotation(int index)
    {
        return flightList[index].Rotation;
    }
}