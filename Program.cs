using System;
using System.Threading;

namespace StopwatchApp
{
    public class Stopwatch
    {
        private TimeSpan timeElapsed;
        private bool isRunning;

        public delegate void StopwatchEventHandler(string message);
        public event StopwatchEventHandler OnStarted;
        public event StopwatchEventHandler OnStopped;
        public event StopwatchEventHandler OnReset;

        public Stopwatch()
        {
            timeElapsed = TimeSpan.Zero;
            isRunning = false;
        }

        public void Start()
        {
            if (!isRunning)
            {
                isRunning = true;
                OnStarted?.Invoke("Stopwatch Started!");
            }
        }

        public void Stop()
        {
            if (isRunning)
            {
                isRunning = false;
                OnStopped?.Invoke("Stopwatch Stopped!");
            }
        }

        public void Reset()
        {
            timeElapsed = TimeSpan.Zero;
            isRunning = false;
            OnReset?.Invoke("Stopwatch Reset!");
        }

        public void Tick()
        {
            if (isRunning)
            {
                timeElapsed = timeElapsed.Add(TimeSpan.FromSeconds(1));
            }
        }

        public TimeSpan GetTimeElapsed()
        {
            return timeElapsed;
        }

        public bool IsRunning
        {
            get { return isRunning; }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.OnStarted += message => Console.WriteLine(message);
            stopwatch.OnStopped += message => Console.WriteLine(message);
            stopwatch.OnReset += message => Console.WriteLine(message);

            bool exit = false;

            Console.WriteLine("Hello, Welcome to my Console Stopwatch App!");
            Console.WriteLine("Click 'S' to Start, 'T' to Stop, 'R' to Reset, or 'E' to Exit.");

            Thread updateThread = new Thread(() =>
            {
                while (!exit)
                {
                    if (stopwatch.IsRunning)
                    {
                        Console.Write($"\rElapsed Time: {stopwatch.GetTimeElapsed():hh\\:mm\\:ss}   ");
                        Thread.Sleep(1000);
                        stopwatch.Tick();
                    }
                }
            });

            updateThread.Start();

            while (!exit)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case ConsoleKey.S:
                            stopwatch.Start();
                            break;
                        case ConsoleKey.T:
                            stopwatch.Stop();
                            break;
                        case ConsoleKey.R:
                            stopwatch.Reset();
                            break;
                        case ConsoleKey.E:
                            exit = true;
                            stopwatch.Stop(); 
                            break;
                        default:
                            Console.WriteLine("Invalid key. Please press 'S', 'T', 'R', or 'E'.");
                            break;
                    }
                }
            }

            updateThread.Join();
            Console.WriteLine("\nThank you for using my Stopwatch Application!");
        }
    }
}