using Mapsui.Projections;

namespace AirportManager;

internal static class FlightsPositionsUpdater
{
    public static void StartAndRunUpdates(List<Flight> flightList, Dictionary<UInt64, Airport> airportDict, FlightGUIDataAdapter adapter)
    {
       
        StartupProcedure(flightList, airportDict);
        Task.Run(() => RunUpdates(flightList, adapter));
    }

    public static void StartupProcedure(List<Flight> flightList, Dictionary<UInt64, Airport> airportDict)
    {
        /* SETTING ALL THE NECCESSARY DATA */
        foreach (var flightRel in flightList)
        {
            /* SETTING STARTNING POSITIONS */
            Airport originAirport = airportDict[flightRel.OriginAsID];
            Airport targetAirport = airportDict[flightRel.TargetAsID];
            flightRel.OriginLatitude = originAirport.Latitude;
            flightRel.OriginLongitude = originAirport.Longitude;
            flightRel.TargetLatitude = targetAirport.Latitude;
            flightRel.TargetLongitude = targetAirport.Longitude;
            flightRel.Longitude = originAirport.Longitude;
            flightRel.Latitude = originAirport.Latitude;
            /* CALCULATING CARTESIAN DISTANCE TO FLY */
            double dist = CalcDistance(originAirport.Longitude, originAirport.Latitude, 
                targetAirport.Longitude, targetAirport.Latitude);
            /* CALCULATING TIME OF THE FLIGHT */
            DateTime startTime = DateTime.Parse(flightRel.TakeoffTime);
            DateTime landingTime = DateTime.Parse(flightRel.LandingTime);
            flightRel.DateTakeoffTime = startTime;
            flightRel.DateLandingTime = landingTime;
            double secondsFlightDuration = (landingTime - startTime).TotalSeconds;
            /* CALCULATING AVERAGE SPEED OF THE FLIGHT */
            double averageSpeed = dist / secondsFlightDuration;
            flightRel.AverageSpeed = averageSpeed;
            flightRel.Rotation = CalcRotation(originAirport.Longitude, originAirport.Latitude, 
                targetAirport.Longitude, targetAirport.Latitude);
            flightRel.IsFlying = true;
        }
    }
    public static void RunUpdates(List<Flight> flightList, FlightGUIDataAdapter adapter)
    {
        /* SETTING MAIN TIMER */
        DateTime today = DateTime.Now.Date; 
        DateTime actualTime = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
        /* STARTUP SLEEPING */
        Thread.Sleep(1000);
        FlightTrackerGUI.Runner.UpdateGUI(adapter);
        while (true)
        {
            Thread.Sleep(1);
            foreach (var flightRel in flightList)
            {
                if (flightRel.DateTakeoffTime <= actualTime && flightRel.DateLandingTime >= actualTime)
                {
                    (double newLatitude, double newLongitude) =
                                CalculateNewPosition(flightRel.AverageSpeed, (double)flightRel.Latitude,
                                    (double)flightRel.Longitude, flightRel.Rotation);
                    flightRel.Latitude = (Single)newLatitude;
                    flightRel.Longitude = (Single)newLongitude;
                    flightRel.IsFlying = true;
                }
            }
            /* UPDATING TIMER */
            actualTime = actualTime.AddSeconds(1);
            FlightTrackerGUI.Runner.UpdateGUI(adapter);
        }
    }
    
    private static double CalcRotation(double oLongitude, double oLatitude, double tLongitude, double tLatitude)
    {
        (double xOrigin, double yOrigin) = SphericalMercator.FromLonLat(oLongitude, oLatitude);
        (double xTarget, double yTarget) = SphericalMercator.FromLonLat(tLongitude, tLatitude);

        if (xTarget > xOrigin)
        {
            double tanAlpha = (yTarget - yOrigin) / (xTarget - xOrigin);
            double alpha = Math.Atan(tanAlpha);
            
            if (alpha < (Math.PI / 2.0))
                return (Math.PI / 2.0) - alpha;
            
            return (3.0 * Math.PI / 2.0) - alpha;
        }
        else
        {
            double tanAlpha = (yOrigin - yTarget) / (xOrigin - xTarget);
            double alpha = Math.Atan(tanAlpha);
           
            if (alpha < (Math.PI / 2.0))
                return (3.0 * Math.PI / 2.0) - alpha;
            
            return (5.0 * Math.PI / 2.0) - alpha;
        }
    }
    
    private static double CalcDistance(double oLongitude, double oLatitude, double tLongitude, double tLatitude)
    {
        (double xOrigin, double yOrigin) = SphericalMercator.FromLonLat(oLongitude, oLatitude);
        (double xTarget, double yTarget) = SphericalMercator.FromLonLat(tLongitude, tLatitude);

        return Math.Sqrt(Math.Pow(xTarget - xOrigin, 2.0) + Math.Pow(yTarget - yOrigin, 2.0));
    }
    
    private static (double latitude, double longitude) CalculateNewPosition(double speed, double currentLatitude, double currentLongitude, double direction)
    {
        (double xLongitude, double yLatitude) = SphericalMercator.FromLonLat(currentLongitude, currentLatitude);
        
        // Obliczamy nową szerokość geograficzną
        double yNewLatitude = yLatitude + speed * Math.Cos(direction);
        // Obliczamy nową długość geograficzną
        double xNewLongitude = xLongitude + speed * Math.Sin(direction);
        (double x, double y) = SphericalMercator.ToLonLat(xNewLongitude, yNewLatitude);
        return (y, x);
    }
}