using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using SFML;
using SFML.Window;

namespace wakarimasen
{
    class Input
    {

        // Keys currently being pressed.
        private static List<Keyboard.Key> keysPressed = new List<Keyboard.Key>(256);

        public static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (!keysPressed.Contains(e.Code))
            {
                keysPressed.Add(e.Code);
                Files.log(4, "Key pressed: " + e.Code);
            }       
        }

        public static void OnKeyReleased(object sender, KeyEventArgs e)
        {
            keysPressed.Remove(e.Code);
        }

        // Gets around the delay on a held down, repeating key. 
        public static void processKeys()
        {

            for (int i = 0; i < keysPressed.Count; i++)
            {
                switch (keysPressed[i])
                {
                    // Movement.
                    case SFML.Window.Keyboard.Key.A:
                    case SFML.Window.Keyboard.Key.Left:
                        {
                            // Move character left.
                            //Character.getCharacter(0).position[0] -= 15;
                            Character.player.actions[0].perform();

                            break;
                        }
                    case SFML.Window.Keyboard.Key.D:
                    case SFML.Window.Keyboard.Key.Right:
                        {
                            // Move character right.
                            //Character.getCharacter(0).position[0] += 15;
                            Character.player.actions[1].perform();

                            break;
                        }
                    case SFML.Window.Keyboard.Key.W:
                    case SFML.Window.Keyboard.Key.Up:
                        {
                            // Move character up / jump.

                            break;
                        }
                    case SFML.Window.Keyboard.Key.S:
                    case SFML.Window.Keyboard.Key.Down:
                        {
                            // Move character down / crouch.

                            break;
                        }

                    // Game function.
                    case SFML.Window.Keyboard.Key.Escape:
                        {
                            Files.log(1, "Game ended.");
                            Drawing.window.Close();
                            Console.Title = "Asialaide (ended)";

                            break;
                        }

                    default:

                        break;
                }
            }

        }

    }
}
