using System;
namespace Rocket_Elevators_Csharp_Controller
{
    class Program
    {
        static void Main(string[] args)
        {
            int scenarioNumber = Int32.Parse(args[0]);
            Scenarios scenarios = new Scenarios();
            scenarios.run(scenarioNumber);
        }
    }
}
