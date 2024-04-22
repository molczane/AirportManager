namespace AirportManager;

internal class Television: IReportableVisitor
{
    public string name { get; }

    public Television(string name)
    {
        this.name = name;
    }
    public string Visit(PassengerPlane passengerPlane)
    {
        return $"<An image of {passengerPlane.Model} passenger plane>";
    }
    public string Visit(CargoPlane cargoPlane)
    {
        return $"<An image of {cargoPlane.Model} cargo plane>";
    }
    public string Visit(Airport airport)
    {
        return $"<An image of {airport.Name} airport>";
    }
}

internal class Radio: IReportableVisitor
{
    public string name { get; }
    public Radio(string name)
    {
        this.name = name;
    }
    
    public string Visit(PassengerPlane passengerPlane)
    {
        return $"Reporting for {this.name}, Ladies and gentelmen, we’ve just witnessed {passengerPlane.Serial} takeoff.";
    }
    public string Visit(CargoPlane cargoPlane)
    {
        return $"Reporting for {this.name}, Ladies and gentelmen, we are seeing the {cargoPlane.Serial} aircraft fly above us.";
    }
    public string Visit(Airport airport)
    {
        return $"Reporting for {this.name}, Ladies and gentelmen, we are at the {airport.Name} airport.";
    }
}

internal class Newspaper: IReportableVisitor
{
    public string name { get; }
    public Newspaper(string name)
    {
        this.name = name;
    }
    public string Visit(PassengerPlane passengerPlane)
    {
        return $"{this.name} - Breaking news! {passengerPlane.Model} aircraft loses EASA fails certification after inspection of {passengerPlane.Serial} .";
    }
    public string Visit(CargoPlane cargoPlane)
    {
        return $"{this.name} An interview with the crew of {cargoPlane.Serial}.";
    }
    public string Visit(Airport airport)
    {
        return $"{this.name} - A report from the {airport.Name} airport, {airport.CountryISO}.";
    }
} 

