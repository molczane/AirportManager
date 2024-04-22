using System.Collections;

namespace AirportManager;

public sealed class AirportManager
{
    /* FIELDS */
    private AirportDataReader DataReader;
    private NetworkSourceSimulator.NetworkSourceSimulator? SourceSimulator;
    private AirportBinaryDataReader BinaryDataReader;
    private static AirportManager _instance = null;
    private List<AirportObject> ListOfAirportObjects;
    private AirportDataSerializer DataSerializer;
    private Dictionary<string, Dictionary<UInt64, AirportObject>> AirportObjectsCategorized;
    private Thread? SimulatorThread;
    private Thread? GuiThread;
    private Dictionary<UInt64, Flight> FlightDict;
    private Dictionary<UInt64, Airport> AirportDict;
    private List<Flight> FlightList;
    private List<IReportable> Reportables;
    /* END FIELDS */

    private AirportManager()
    {
        DataReader = new AirportDataReader();
        DataSerializer = new AirportDataSerializer();
        ListOfAirportObjects = new List<AirportObject>();
        AirportObjectsCategorized = new Dictionary<string, Dictionary<UInt64, AirportObject>>()
        {
            { "C", new Dictionary<UInt64, AirportObject>() },
            { "P", new Dictionary<UInt64, AirportObject>() },
            { "CA", new Dictionary<UInt64, AirportObject>() },
            { "CP", new Dictionary<UInt64, AirportObject>() },
            { "PP", new Dictionary<UInt64, AirportObject>() },
            { "AI", new Dictionary<UInt64, AirportObject>() },
            { "FL", new Dictionary<UInt64, AirportObject>() }
        };
        FlightDict = new Dictionary<ulong, Flight>();
        AirportDict = new Dictionary<ulong, Airport>();
        FlightList = new List<Flight>();
        Reportables = new List<IReportable>();
    }

    public static AirportManager GetInstance
    {
        get
        {
            if (_instance == null)
                _instance = new AirportManager();
            return _instance;
        }
    }

    public void ReadDataFromFile(string FileName)
    {
        foreach(var airportObject in DataReader.ReadAirportObjects(FileName))
        {
            AddObject(airportObject);
        }
        Console.WriteLine($"Successfully read data from file: {FileName}!");
    }

    public void SerializeDataToFile(string FileName)
    {
        DataSerializer.SerializeToFile(ListOfAirportObjects, FileName);
        string[] Path = FileName.Split("/");
        Console.WriteLine($"Successfully serialized all data to file: {Path[Path.Length - 1]}!");
    }

    public void AddObject(AirportObject NewObject)
    {
        /* Adding object to good category list */
        AirportObjectsCategorized[NewObject.Identifier].Add(NewObject.ID, NewObject);
        /* Adding object to general list */
        ListOfAirportObjects.Add(NewObject);
        if (NewObject is Flight flight)
        {
            FlightDict.Add(flight.ID, flight);
            FlightList.Add(flight);
        }
        if (NewObject is Airport airport)
            AirportDict.Add(airport.ID, airport);
            
        /* ADDING TO REPORTABLES */
        if(NewObject is IReportable reportable)
            Reportables.Add(reportable);
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\nWhat do you want to do?");
            Console.WriteLine("1. read data from file - write 1 in the console.\n" +
                              "2. turn on simulator - write 2 in the console.\n" +
                              "3. do a snapshot of data to .json file - write \"print\" in the console.\n" +
                              "4. run Flight GUI - write \"rungui\" in the console.\n" +
                              "5. report \n" + 
                              "6. exit - write \"exit\" in the console.");
            string Input = Console.ReadLine();
            switch (Input)
            {
                case "1":
                {
                    Console.WriteLine("Provide filename:");
                    string FileName = Console.ReadLine();
                    ReadDataFromFile(FileName);
                }
                    break;
                case "2":
                {
                    Console.WriteLine("Provide a name od the .ftr file to work on (example_data.ftr):");
                    string FileName = Console.ReadLine();
                    Console.WriteLine("Provide a minimal offset in miliseconds:");
                    int MinOffset = int.Parse(Console.ReadLine());
                    Console.WriteLine("Provide a maximal offset in miliseconds:");
                    int MaxOffset = int.Parse(Console.ReadLine());
                    /* Running Source Simulator in another thread */
                    SourceSimulator = new NetworkSourceSimulator.NetworkSourceSimulator(FileName, MinOffset, MaxOffset);
                    BinaryDataReader = new AirportBinaryDataReader(this, SourceSimulator);
                    SourceSimulator.OnNewDataReady += BinaryDataReader.HandleBinaryData;
                    SimulatorThread = new Thread(() => SourceSimulator.Run()) { IsBackground = true };
                    SimulatorThread.Start();
                    Console.WriteLine("Simulator successfully turned on!");
                }
                    break;
                case "print":
                {
                    string FormattedTime = DateTime.Now.ToString("HH_mm_ss");
                    string FileName = "../../../Snapshots/snapshot_" + FormattedTime + ".json";
                    SerializeDataToFile(FileName);
                }
                    break;
                case "rungui":
                {
                    FlightGUIDataAdapter adapter = new FlightGUIDataAdapter(FlightList, AirportDict);
                    FlightsPositionsUpdater.StartAndRunUpdates(FlightList, AirportDict, adapter);
                    FlightTrackerGUI.Runner.Run();
                }
                    break;
                case "report":
                {
                    List<IReportableVisitor> mediaList = new List<IReportableVisitor>();
                    mediaList.Add(new Television("Telewizja Abelowa"));
                    mediaList.Add(new Television("Kanał TV-sensor"));
                    mediaList.Add(new Radio("Radio Kwantyfikator"));
                    mediaList.Add(new Radio("Radio Shmem"));
                    mediaList.Add(new Newspaper("Gazeta Kategoryczna"));
                    mediaList.Add(new Newspaper("Dziennik Politechniczny"));
                    /* SUPER ROZWIAZANIE ALE NIEZGODNE ZE SPECYFIKACJA */
                    // NewsColection newsColection = new NewsColection(mediaList, Reportables);
                    // int count = 0;
                    // foreach ((IReportable reportable, IReportableVisitor mediaVisitor) newsPair in newsColection)
                    // {
                    //     count++;
                    //     Console.WriteLine($"{count}. " + newsPair.reportable.Accept(newsPair.mediaVisitor));
                    // }
                    
                    /* ZGODNE ZE SPECYFIKACJA */
                    NewsGenerator newsGenerator = new NewsGenerator(mediaList, Reportables);
                    string? currentNews;
                    int count = 0;
                    while (null != (currentNews = newsGenerator.GenerateNextNews()))
                    {
                        count++;
                        Console.WriteLine($"{count}. " + currentNews);
                    }
                }
                    break;
                case "exit":
                {
                    string FormattedTime = DateTime.Now.ToString("HH_mm_ss");
                    string FileName = "../../../Snapshots/snapshot_" + FormattedTime + "_exitted.json";
                    SerializeDataToFile(FileName);
                }
                    return;
                default:
                    Console.WriteLine("Wrong command, come again!");
                    break;
            }
        }
    }
}