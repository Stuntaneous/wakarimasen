//
// License: GPLv2
// Authors: C.Fallins and V.Ivanovic
//
// Visual Studio Express 2013
// .NET Framework 4.0
// SFML.net 2.1 x86
//
// Note: Start without debugging.
//

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace wakarimasen
{
    /// <summary>
    /// Core processes.
    /// </summary>
    class Program 
    {
        // Used as a local reference. 
        static RenderWindow window;

        // The game clock, in milliseconds.
        static long gameClock;
        public static long clock
        {
            get { return gameClock; }
        }

        // A unit of game time, in milliseconds.
        const int TICK = 20; // 20ms gives 50fps.

        // The current framerate being managed. 
        static int framerate;
        public static int FPS
        {
            get { return framerate; }
        }

        static SortedList<long, Timer> gameTimers = new SortedList<long,Timer>();
        public static SortedList<long, Timer> timers
        {
            get { return gameTimers; }
        }

        /// <summary>
        /// Main.. main what?
        /// </summary>
        /// <param name="args">Command-line arguments yet to be implemented.</param>
        static void Main(string[] args)
        {
            long initialTicks = DateTime.Now.Ticks;

            Files.backupPreviousLog();
            Files.log(2, "Game started.");
            
            Console.Title = "Asialaide";

            Drawing.initialise();
            window = Drawing.window; // A local reference. 

            // Load files. 
            //Files.load("data\\graphics\\crf_char.png");
            Files.load("data\\graphics\\preview_creatures.png");           

            Character.initialise();
            Character.player = new Character("Player", 1, 200, 300);
            
            // Create events.
            window.Closed += new EventHandler(Program.OnClose);
            window.KeyPressed += new EventHandler<KeyEventArgs>(Input.OnKeyPressed);
            window.KeyReleased += new EventHandler<KeyEventArgs>(Input.OnKeyReleased);

            //int recordedSecond = DateTime.Now.Second;
            double preLoopTime = gameClock;// = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMilliseconds;
            double timeElapsed;

            // The game loop.
            while (window.IsOpen())
            {
                // Update game clock. 
                gameClock = (long)TimeSpan.FromTicks(DateTime.Now.Ticks - initialTicks).TotalMilliseconds;

                timeElapsed = gameClock - preLoopTime;
                if (timeElapsed >= TICK)
                {
                    // Record the current framerate. 
                    framerate = Convert.ToInt32(Math.Round(Convert.ToDouble(1000 / timeElapsed)));
                    
                    timeElapsed = 0;
                    preLoopTime = gameClock;

                    // Process events.
                    window.DispatchEvents();

                    // Process keys currently being pressed.
                    // Consider: timing interval; modifiers
                    Input.processKeys();

                    // Draw everything. 
                    Drawing.drawWindow();
                }
                
            }

        }

        public static void OnClose(object sender, EventArgs e)
        {
            // Close the window (end the program) when the OnClose event is received.
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

    }

    struct Timer
    {
        // In milliseconds.
        long startTime;
        long length;

        Effect attachment;

        public Timer(long length, Effect attachment)
        {
            startTime = Program.clock;
            this.length = length;
            this.attachment = attachment;
        }

        /// <returns>Whether or not the timer was ready to trigger, and has triggered.</returns>
        public bool poll()
        {
            if (Program.clock >= startTime + length)
            {
                attachment.perform();
                return true;
            }
            else
                return false;
        }
    }

}
