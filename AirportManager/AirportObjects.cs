using System.Globalization;
using System.Text.Json.Serialization;
using AirportManager;

// Here I made only one class that all the objects inherit from.
// It can be bad in the future but is easier for the reason of serialization
// and data visibility to other classes.
// The hierarchy can be changed in the future of the project.
// For now I left it this way.

/* INTERFACE FOR FOURTH PART */
internal interface IReportable
{
    public string Accept(IReportableVisitor visitor)
    {
        return visitor.Visit((dynamic)this);
    }
}

namespace AirportManager
{
    [JsonDerivedType(typeof(AirportObject), typeDiscriminator: "AirportObject")]
    [JsonDerivedType(typeof(Crew), typeDiscriminator: "Crew")]
    [JsonDerivedType(typeof(Passenger), typeDiscriminator: "Passenger")]
    [JsonDerivedType(typeof(Cargo), typeDiscriminator: "Cargo")]
    [JsonDerivedType(typeof(CargoPlane), typeDiscriminator: "CargoPlane")]
    [JsonDerivedType(typeof(PassengerPlane), typeDiscriminator: "PassengerPlane")]
    [JsonDerivedType(typeof(Airport), typeDiscriminator: "Airport")]
    [JsonDerivedType(typeof(Flight), typeDiscriminator: "Flight")]
    public class AirportObject
    {
        public virtual UInt64 ID { get; }

        public virtual string Identifier { get; }
    }

    public class Crew : AirportObject
    { 
        public Crew(string _Identifier, UInt64 _ID, string _Name, UInt64 _Age, string _Phone, 
                        string _Email, UInt16 _Practice, string _Role) 
        {
            ID = _ID;
            Name = _Name;
            Age = _Age;
            Phone = _Phone;
            Email = _Email;
            Practice = _Practice;
            Role = _Role;
            Identifier = _Identifier;
        }

        public Crew(string[] Data)
        {
            ID = UInt64.Parse(Data[1]);
            Name = Data[2];
            Age = UInt64.Parse(Data[3]);
            Phone = Data[4];
            Email = Data[5];
            Practice = UInt16.Parse(Data[6]);
            Role = Data[7];
            Identifier = Data[0];
        }
        public override string Identifier {  get; }
        public override UInt64 ID { get; }  
        public string Name { get; }
        public UInt64 Age { get; }
        public string Phone { get; }
        public string Email { get; }
        public UInt16 Practice { get; }
        public string Role { get; }
    }

    internal class  Passenger : AirportObject
    {
        public Passenger(string _Identifier, UInt64 _ID, string _Name, UInt64 _Age, string _Phone, string _Email, 
                            string _Class, UInt64 _Miles)
        {
            ID = _ID;
            Name = _Name;
            Age = _Age;
            Phone = _Phone;
            Email = _Email;
            Class = _Class;
            Miles = _Miles;
            Identifier = _Identifier;
        }
        public Passenger(string[] Data) 
        {
            ID = UInt64.Parse(Data[1]);
            Name = Data[2];
            Age = UInt64.Parse(Data[3]);
            Phone = Data[4];
            Email = Data[5];
            Class = Data[6];
            Miles = UInt64.Parse(Data[7]);
            Identifier = Data[0];
        }
        public override string Identifier { get; }
        public override UInt64 ID { get; }
        public string Name { get; }
        public UInt64 Age { get; }
        public string Phone { get; }
        public string Email { get; }
        public string Class {  get; }
        public UInt64 Miles { get; }
    }

    internal class Cargo : AirportObject
    {
        public Cargo(string _Identifier, UInt64 _ID, Single _Weight, string _Code, string _Description)
        {
            ID = _ID;
            Weight = _Weight;
            Code = _Code;
            Description = _Description;
            Identifier = _Identifier;
        }
        public Cargo(string[] Data) 
        {
            ID = UInt64.Parse(Data[1]);
            Weight = Single.Parse(Data[2], CultureInfo.InvariantCulture);
            Code = Data[3];
            Description = Data[4];
            Identifier = Data[0];
        }
        public override string Identifier { get; }
        public override UInt64 ID { get; }
        public Single Weight { get; }
        public string Code { get; }
        public string Description { get; }
    }

    internal class CargoPlane : AirportObject, IReportable
    {
        public CargoPlane(string _Identifier, UInt64 _ID, string _Serial, string _CountryISO, string _Model,
                            Single _MaxLoad)
        {
            ID = _ID;
            Serial = _Serial;
            CountryISO = _CountryISO;
            Model = _Model;
            MaxLoad = _MaxLoad;
            Identifier = _Identifier;
        }
        public CargoPlane(string[] Data) 
        {
            ID = UInt64.Parse(Data[1]);
            Serial = Data[2];
            CountryISO = Data[3];
            Model = Data[4];
            MaxLoad = Single.Parse(Data[5], CultureInfo.InvariantCulture);
            Identifier = Data[0];
        }
        public override string Identifier { get; }
        public override UInt64 ID { get; }
        public string Serial { get; }
        public string CountryISO { get; }
        public string Model { get; }
        public Single MaxLoad { get; }
    }

    internal class PassengerPlane : AirportObject, IReportable
    {
        public PassengerPlane(string _Identifier, UInt64 _ID, string _Serial, string _CountryISO, string _Model,
                                UInt16 _First, UInt16 _Business, UInt16 _Economy)
        {
            ID = _ID;
            Serial = _Serial;
            CountryISO = _CountryISO;
            Model = _Model;
            FirstClassSize = _First;
            BusinessClassSize = _Business;
            EconomyClassSize = _Economy;
            Identifier = _Identifier;
        }
        public PassengerPlane(string[] Data) 
        {
            ID = UInt64.Parse(Data[1]);
            Serial = Data[2];
            CountryISO = Data[3];
            Model = Data[4];
            FirstClassSize = UInt16.Parse(Data[5]);
            BusinessClassSize = UInt16.Parse(Data[6]);
            EconomyClassSize = UInt16.Parse(Data[7]);
            Identifier = Data[0];
        }
        public override string Identifier { get; }
        public override UInt64 ID { get; }
        public string Serial { get; }
        public string CountryISO { get; }
        public string Model { get; }
        public UInt16 FirstClassSize { get; }
        public UInt16 BusinessClassSize { get; }
        public UInt16 EconomyClassSize { get; }
    }

    internal class Airport : AirportObject, IReportable
    {
        public Airport(string _Identifier, UInt64 _ID, string _Name, string _Code, Single _Longitude,
                        Single _Latitude, Single _AMSL, string _CountryISO)
        {
            ID = _ID;
            Name = _Name;
            Code = _Code;
            Longitude = _Longitude;
            Latitude = _Latitude;
            AMSL = _AMSL;
            CountryISO = _CountryISO;
            Identifier = _Identifier;
        }
        public Airport(string[] Data) 
        {
            ID = UInt64.Parse(Data[1]);
            Name = Data[2];
            Code = Data[3];
            Longitude = Single.Parse(Data[4], CultureInfo.InvariantCulture);
            Latitude = Single.Parse(Data[5], CultureInfo.InvariantCulture);
            AMSL = Single.Parse(Data[6], CultureInfo.InvariantCulture);
            CountryISO = Data[7];
            Identifier = Data[0];
        }
        public override string Identifier { get; }
        public override UInt64 ID { get; }
        public string Name { get; }
        public string Code { get; }
        public Single Longitude { get; }
        public Single Latitude { get; }
        public Single AMSL { get; }
        public string CountryISO { get; }
        /* IMPLEMENTING INTERFACE */
    }

    internal class Flight : AirportObject
    {
        public Flight(string _Identifier, UInt64 _ID, UInt64 _OriginAsID, UInt64 _TargetAsID, 
                        string _TakeoffTime, string _LandingTime, Single? _Longitude, Single? _Latitude,
                        Single? _AMSL, UInt64 _PlaneID, UInt64[] _CrewAsIDs, UInt64[] _LoadAsIDs)
        {
            ID = _ID;
            OriginAsID = _OriginAsID;
            TargetAsID = _TargetAsID;
            TakeoffTime = _TakeoffTime;
            LandingTime = _LandingTime;
            Longitude = _Longitude;
            Latitude = _Latitude;
            AMSL = _AMSL;
            PlaneID = _PlaneID;
            CrewAsIDs = _CrewAsIDs;
            LoadAsIDs = _LoadAsIDs;
            Identifier = _Identifier;
        }
        public Flight(string[] Data)
        {
            ID = UInt64.Parse(Data[1]);
            OriginAsID = UInt64.Parse(Data[2]);
            TargetAsID = UInt64.Parse(Data[3]);
            DateTime today = DateTime.Today;
            TimeSpan parsedTakeoff = TimeSpan.Parse(Data[4]);
            TimeSpan parsedLanding = TimeSpan.Parse(Data[5]);
            DateTime Takeoff = today.Add(parsedTakeoff);
            DateTime Landing = today.Add(parsedLanding);
            if (Takeoff > Landing)
                Landing = Landing.AddDays(1);
            TakeoffTime = Takeoff.ToString("G");
            LandingTime = Landing.ToString("G");
            if (Data[6] != null) Longitude = Single.Parse(Data[6], CultureInfo.InvariantCulture);
            if (Data[7] != null) Latitude = Single.Parse(Data[7], CultureInfo.InvariantCulture);
            if (Data[8] != null) AMSL = Single.Parse(Data[8], CultureInfo.InvariantCulture);
            PlaneID = UInt64.Parse(Data[9]);
            string[] CrewIDs = Data[10].Trim('[', ']').Split(';');
            CrewAsIDs = CrewIDs.Select(n => Convert.ToUInt64(n)).ToArray();
            string[] LoadIDs = Data[11].Trim('[', ']').Split(';');
            LoadAsIDs = LoadIDs.Select(n => Convert.ToUInt64(n)).ToArray();
            Identifier = Data[0];
        }
        public override string Identifier { get; }
        public override UInt64 ID { get;  }
        public UInt64 OriginAsID { get;  }
        public UInt64 TargetAsID {  get; }
        public string TakeoffTime { get; }
        public string LandingTime { get; }
        public Single? Longitude { get; set; } // might add set; here
        public Single? Latitude { get; set; } // might add set; here
        public Single? AMSL { get; } // i dont know what is it - above mean sea level, should also add set; to it
        public UInt64 PlaneID { get; }
        public UInt64[] CrewAsIDs { get; }
        public UInt64[] LoadAsIDs { get; }
        /* PROPERTIES ADDED TO SPEED UP FLIGHT SIMULATION IN GUI */
        [JsonIgnore] public double AverageSpeed { get; set; }
        [JsonIgnore] public double Rotation { get; set; }
        [JsonIgnore] public bool IsFlying { get; set; }
        [JsonIgnore] public DateTime DateLandingTime { get; set; }
        [JsonIgnore] public DateTime DateTakeoffTime { get; set; }
        [JsonIgnore] public Single OriginLatitude { get; set; }
        [JsonIgnore] public Single OriginLongitude { get; set; }
        [JsonIgnore] public Single TargetLatitude { get; set; }
        [JsonIgnore] public Single TargetLongitude { get; set; }
        // Note: LoadAsIDs can be either list of Cargo IDs or a list of Passenger IDs.
        // This is dependent on the plane that was assigned to the flight (CargoPlane or PassengerPlane)
    }
}
