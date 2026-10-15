using System;
namespace RocketEngineSimulator
{
    class Runner
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Starting Rocket Engine Simulator");
            
            Engine engine = new Engine();
            engine.Solve();
            
        }
    }
}