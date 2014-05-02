using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using SFML.Graphics;

namespace wakarimasen
{
    class Files
    {
        // Logging verbosity.
        // 0 = off
        // 1 = errors only
        // 2 = errors, notables
        // 3 = all
        // 4 = all + keystrokes
        /// <summary>
        /// Logging verbosity.
        /// </summary>
        /// <seealso cref="log(int, string)"/>
        private const int LOG_DETAIL = 4;
        
        //Dictionary<int, string> sprites = new Dictionary<int,string>()
        private static SortedDictionary<int, Texture> sprites = new SortedDictionary<int, Texture>();

        /// <summary>
        /// Loads any asset required by the game.
        /// </summary>
        /// <param name="filename">A relative (e.g. "data\graphics\spritemap.png") or absolute file path.</param>
        /// <returns>Success or failure of the operation.</returns>
        public static bool load(string filename)
        {

            // Check for a valid filename? Either by using a try-catch statement or testing conditions, e.g. one and only one period. 
            
            // For now, the lazy try-catch.
            try
            {
                // Operate on file according to its extension.

                switch (filename.Substring(filename.Length - 4, 4))
                { 
                    case ".png":
                    case ".jpg":
                    case "jpeg":
                    case ".bmp":
                        {
                            // It's an image file.    
                            
                            // FIX: Can't add the key by total count of items. If some are lost or removed, shit will get retarded. 
                            // Graphics are held as textures so they're stored in speedy graphics memory. 
                            // Common scale figures could be generated at the same time to minimise effort later. 
                            
                            //sprites.Add(0, Drawing.removeMask(new Image(filename)));
                            sprites.Add(0, new Texture(new Image(filename), new IntRect(0, 0, 32, 32)));
                            sprites.Add(1, new Texture(new Image(filename), new IntRect(32, 0, 32, 32)));
                            sprites.Add(2, new Texture(new Image(filename), new IntRect(64, 0, 32, 32)));

                            sprites.Add(3, new Texture(new Image(filename), new IntRect(0, 32, 32, 32)));
                            sprites.Add(4, new Texture(new Image(filename), new IntRect(32, 32, 32, 32)));
                            sprites.Add(5, new Texture(new Image(filename), new IntRect(64, 32, 32, 32)));

                            sprites.Add(6, new Texture(new Image(filename), new IntRect(0, 64, 32, 32)));
                            sprites.Add(7, new Texture(new Image(filename), new IntRect(32, 64, 32, 32)));
                            sprites.Add(8, new Texture(new Image(filename), new IntRect(64, 64, 32, 32)));

                            //System.IO.
                            
                            log(2, "File loaded (" + filename + ").");
                            return true;
                        }
                    default:
                        {
                            log(1, "Error: file format unsupported (" + filename + ").");

                            return false;
                        }
                }
            }
            catch (Exception e)
            {
                log(1, "Error: file (" + filename + ") failed to load. " + e.ToString());

                return false;
            }
            finally
            {

            }
            
        }

        public static Texture getSprite(int ID)
        {
            if (ID < sprites.Count)
                return sprites[ID];
            else
            {
                log(1, "Error: sprite doesn't exist (" + ID + ").");
                return null;
            }
            
        }

        /// <summary>
        /// Writes a line of text to the log file, prefixed with the current date and time.
        /// </summary>
        /// <param name="detailLevel">1 = an error; 2 = notable; 3 = spam; 4 = keystrokes</param>
        /// <param name="line">The log entry.</param>
        public static void log(int detailLevel, string line)
        {
            System.DateTime time = new System.DateTime();
            time = System.DateTime.Now;

            TextWriter logWriter = new StreamWriter("log.txt", true);

            if (LOG_DETAIL >= detailLevel)
            {
                logWriter.WriteLine(time.ToString() + " :: " + line);

                // A copy for the debug feed.
                Debug.Print("[LOG] {0}", line);
                // A copy for the unofficial debug feed, the console.
                Console.WriteLine("[LOG] {0}", line);
            }

            logWriter.Close();
        }
        
        /// <summary>
        /// Copies any previously existing log to a new file ("log_older.txt") and wipes the current file ("log.txt") ahead of new use.
        /// </summary>
        /// <returns>Whether or not a log previously existed.</returns>
        public static bool backupPreviousLog()
        {
            TextWriter logWriter;

            // If a previous log exists, it's relegated to another file to make way for a new one. 
            try
            {
                StreamReader logChecker = new StreamReader("log.txt");
                string previousLog = logChecker.ReadToEnd();
                logChecker.Close();

                logWriter = new StreamWriter("log_older.txt", false);
                for (int i = 0; i < previousLog.Length; i++)
                {
                    logWriter.Write(previousLog.ElementAt(i));
                }
                logWriter.Close();

                // Wipe the primary log file for new use. 
                logWriter = new StreamWriter("log.txt", false);
                logWriter.WriteLine("Asialaide log");
                logWriter.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

    }
}
