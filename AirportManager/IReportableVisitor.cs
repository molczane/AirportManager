namespace AirportManager;

internal interface IReportableVisitor
{
    string Visit(PassengerPlane passengerPlane);
    string Visit(CargoPlane cargoPlane);
    string Visit(Airport airport);
}


