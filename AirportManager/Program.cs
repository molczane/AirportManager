
namespace AirportManager;

class Program
{
    public static void Main(string[] args)
    {
        /* Airport manager is a Singleton - design pattern */
        AirportManager Manager = AirportManager.GetInstance;
        Manager.Run(); 
    }
}