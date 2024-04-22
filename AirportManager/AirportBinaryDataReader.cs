using System.Text;

namespace AirportManager;

public interface IBinaryBuilder
{
    public AirportObject BuildObjectFromBytes(byte[] Data);
}

internal class AirportBinaryDataReader
{
    /* FIELDS */
    private NetworkSourceSimulator.NetworkSourceSimulator Source;
    private AirportManager Manager;
    private readonly Dictionary<string, IBinaryBuilder> Builders = new Dictionary<string, IBinaryBuilder>()
    {
        { "NCR", new BinaryCrewBuilder()},
        { "NPA", new BinaryPassengerBuilder() },
        { "NCA", new BinaryCargoBuilder() },
        { "NCP", new BinaryCargoPlaneBuilder() },
        { "NPP", new BinaryPassengerPlaneBuilder() },
        { "NAI", new BinaryAirportBuilder() },
        { "NFL", new BinaryFlightBuilder() }
    };
    /* END FIELDS */

    public AirportBinaryDataReader(AirportManager _Manager, NetworkSourceSimulator.NetworkSourceSimulator _Source) 
    {
        Source = _Source;
        Manager = _Manager;
        Manager = AirportManager.GetInstance;
    }

    public AirportObject BuildObjectFromBytes(byte[] Data)
    {
        byte[] ByteName = { Data[0], Data[1], Data[2] };
        char[] CharName = Encoding.ASCII.GetChars(ByteName);
        return Builders[new string(CharName)].BuildObjectFromBytes(Data);
    }

    public void HandleBinaryData(object Sender, NetworkSourceSimulator.NewDataReadyArgs e)
    {
        AirportObject NewObject = BuildObjectFromBytes(Source.GetMessageAt(e.MessageIndex).MessageBytes);
        Manager.AddObject(NewObject);
    }

    public class BinaryCrewBuilder : IBinaryBuilder
    { 
        public AirportObject BuildObjectFromBytes(byte[] Data)
        {
            UInt64 ID = BitConverter.ToUInt64(Data[7..15]);
            UInt16 NL = BitConverter.ToUInt16(Data[15..17]);
            string Name = Encoding.ASCII.GetString(Data[17..(17 + NL)]);
            UInt16 Age = BitConverter.ToUInt16(Data[(17 + NL)..(19 + NL)]);
            string PhoneNumber = Encoding.ASCII.GetString(Data[(19 + NL) .. (31 + NL)]);
            UInt16 EL = BitConverter.ToUInt16(Data[(31 + NL)..(33 + NL)]);
            string EmailAddress = Encoding.ASCII.GetString(Data[(33 + NL)..(33 + NL + EL)]);
            UInt16 Practice = BitConverter.ToUInt16(Data[(33 + NL + EL)..(35 + NL + EL)]);
            string Role = Encoding.ASCII.GetString(Data[(35 + NL + EL) ..(36 + NL + EL)]);
            return new Crew(
                "C", ID, Name, Age, PhoneNumber, EmailAddress, Practice, Role
            );
        }
    }

    public class BinaryPassengerBuilder() : IBinaryBuilder
    {
        public AirportObject BuildObjectFromBytes(byte[] Data)
        {
            UInt64 ID = BitConverter.ToUInt64(Data[7..15]);
            UInt16 NL = BitConverter.ToUInt16(Data[15..17]);
            string Name = Encoding.ASCII.GetString(Data[17..(17 + NL)]);
            UInt16 Age = BitConverter.ToUInt16(Data[(17 + NL)..(19 + NL)]);
            string PhoneNumber = Encoding.ASCII.GetString(Data[(19 + NL)..(31 + NL)]);
            UInt16 EL = BitConverter.ToUInt16(Data[(31 + NL)..(33 + NL)]);
            string EmailAddress = Encoding.ASCII.GetString(Data[(33 + NL)..(33 + NL + EL)]);
            string Class = Encoding.ASCII.GetString(Data[(33 + NL + EL)..(34 + NL + EL)]);
            UInt64 Miles = BitConverter.ToUInt64(Data[(34 + NL + EL)..(42 + NL + EL)]);
            return new Passenger(
                "P", ID, Name, Age, PhoneNumber, EmailAddress, Class, Miles
            );
        }
    }

    public class BinaryCargoBuilder : IBinaryBuilder
    {
        public AirportObject BuildObjectFromBytes(byte[] Data)
        {
            UInt64 ID = BitConverter.ToUInt64(Data[7..15]);
            Single Weight = BitConverter.ToSingle(Data[15..19]);
            string Code = Encoding.ASCII.GetString(Data[19..25]);
            UInt16 DL = BitConverter.ToUInt16(Data[25..27]);
            string Description = Encoding.ASCII.GetString(Data[27..(27 + DL)]);
            return new Cargo(
                "CA", ID, Weight, Code, Description
            );
        }
    }

    public class BinaryCargoPlaneBuilder : IBinaryBuilder
    {
        public AirportObject BuildObjectFromBytes(byte[] Data)
        {
            UInt64 ID = BitConverter.ToUInt64(Data[7..15]);
            string Serial = Encoding.ASCII.GetString(Data[15..25]);
            string SerialTrimmed = Serial.TrimEnd('\u0000');
            string ISOCountryCode = Encoding.ASCII.GetString(Data[25..28]);
            UInt16 ML = BitConverter.ToUInt16(Data[28..30]);
            string Model = Encoding.ASCII.GetString(Data[30..(30 + ML)]);
            Single MaxLoad = BitConverter.ToSingle(Data[(30 + ML)..(34 + ML)]);
            return new CargoPlane(
                "CP", ID, SerialTrimmed, ISOCountryCode, Model, MaxLoad
            );
        }
    }

    public class BinaryPassengerPlaneBuilder : IBinaryBuilder
    {
        public AirportObject BuildObjectFromBytes(byte[] Data)
        {
            UInt64 ID = BitConverter.ToUInt64(Data[7..15]);
            string Serial = Encoding.ASCII.GetString(Data[15..25]);
            string SerialTrimmed = Serial.TrimEnd('\u0000');
            string ISOCountryCode = Encoding.ASCII.GetString(Data[25..28]);
            UInt16 ML = BitConverter.ToUInt16(Data[28..30]);
            string Model = Encoding.ASCII.GetString(Data[30..(30 + ML)]);
            UInt16 FirstClassSize = BitConverter.ToUInt16(Data[(30 + ML) ..(32 + ML)]);
            UInt16 BusinessClassSize = BitConverter.ToUInt16(Data[(32 + ML) ..(34 + ML)]);
            UInt16 EconomyClassSize = BitConverter.ToUInt16(Data[(34 + ML) ..(36 + ML)]);
            return new PassengerPlane(
                "PP", ID, SerialTrimmed, ISOCountryCode, Model, FirstClassSize, BusinessClassSize, EconomyClassSize
            );
        }
    }

    public class BinaryAirportBuilder : IBinaryBuilder
    {
        public AirportObject BuildObjectFromBytes(byte[] Data)
        {
            UInt64 ID = BitConverter.ToUInt64(Data[7..15]);
            UInt16 NL = BitConverter.ToUInt16(Data[15..17]);
            string Name = Encoding.ASCII.GetString(Data[17..(17 + NL)]);
            string Code = Encoding.ASCII.GetString(Data[(17 + NL) .. (20 + NL)]);
            Single Longtitude = BitConverter.ToSingle(Data[(20 + NL) .. (24 + NL)]);
            Single Latitude = BitConverter.ToSingle(Data[(24 + NL)..(28 + NL)]);
            Single AMSL = BitConverter.ToSingle(Data[(28 + NL)..(32 + NL)]);
            string ISOCode = Encoding.ASCII.GetString(Data[(32 + NL) .. (35 + NL)]);
            return new Airport(
                "AI", ID, Name, Code, Longtitude, Latitude, AMSL, ISOCode
            );
        }
    }

    public class BinaryFlightBuilder : IBinaryBuilder
    {
        public AirportObject BuildObjectFromBytes(byte[] Data)
        {
            UInt64 ID = BitConverter.ToUInt64(Data[7..15]);
            UInt64 OriginAsID = BitConverter.ToUInt64(Data[15..23]);
            UInt64 TargetAsID = BitConverter.ToUInt64(Data[23..31]);
            Int64 TakeoffTimeInMs = BitConverter.ToInt64(Data[31..39]);
            DateTime UTCTakeoffTime = DateTimeOffset.FromUnixTimeMilliseconds(TakeoffTimeInMs).UtcDateTime;
            string TakeoffTime = UTCTakeoffTime.ToString("G");
            Int64 LandingTimeInMs = BitConverter.ToInt64(Data[39..47]);
            DateTime UTCLandingTime = DateTimeOffset.FromUnixTimeMilliseconds(LandingTimeInMs).UtcDateTime;
            if (UTCTakeoffTime > UTCLandingTime)
                UTCLandingTime = UTCLandingTime.AddDays(1);
            string LandingTime = UTCLandingTime.ToString("G");
            UInt64 PlaneID = BitConverter.ToUInt64(Data[47..55]);
            UInt16 CC = BitConverter.ToUInt16(Data[55..57]);
            UInt64[] CrewAsIDs = new UInt64[CC];
            for(int i = 0; i < CC; i++)
                CrewAsIDs[i] = BitConverter.ToUInt16(Data[(57 + 8*i)..(57 + 8*(i+1))]);
            UInt16 PCC = BitConverter.ToUInt16(Data[(57 + 8*CC)..(59 + 8*CC)]);
            UInt64[] PassengersAndCargoAsIDs = new UInt64[PCC];
            for (int i = 0; i < PCC; i++)
                PassengersAndCargoAsIDs[i] = BitConverter.ToUInt16(Data[(59 + 8*CC + 8*i)..(59 + 8*CC + 8*(i+1))]);
            return new Flight(
                "FL", ID, OriginAsID, TargetAsID, TakeoffTime, LandingTime, null, null, null, 
                PlaneID, CrewAsIDs, PassengersAndCargoAsIDs
            );

        }
    }
}