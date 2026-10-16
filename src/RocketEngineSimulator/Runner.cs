using System;
using Avalonia;

namespace RocketEngineSimulator
{
    class Runner
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Rocket Engine Simulator");
            // 3. Start the UI loop on the main thread (blocks execution until the window closes)
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
