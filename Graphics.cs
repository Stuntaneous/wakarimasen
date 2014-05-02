using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace wakarimasen
{

        // Need to..
        //
        // - Read all graphics from a designated directory, e.g. a bunch of spritemaps and tilesets.
        // - Catalogue and assign ID numbers to everything loaded.
        // - Produce and maintain an orderly database of visual assets.
        // - Consider what categories graphics can fall under, e.g. sprites, tiles, effects, animation frames of, etc. 

        // Consider: value that sets speed at which frames are cycled in an animation 
          
    
    /// <summary>
    /// Have a guess. 
    /// </summary>
    class Animation
    {
        // The default framerate of animations. 
        const int DEFAULT_FRAMERATE = 5;

        // This animations collection needs to have its size dynamically managed. Consider using multiple dimensions in the array below. 
        static List<Animation> animations = new List<Animation>();    

        int ID;
        Frame firstFrame;
        Frame currentFrame;
        int loop;
        bool sequential; // An animation can randomly choose the order of frames (every run-through, if looping). 
        int framerate;
        
        /// <summary>
        /// I heard you want to make a new animation.
        /// </summary>
        /// <param name="ID">A reference number.</param>
        /// <param name="frames">All yo frames.</param>
        /// <param name="framerate">Per second. Default is ... to be decided.</param>
        /// <param name="loop">How many times to loop the animation, while the associated action is occurring. Set to -1 to loop forever.</param>
        /// <param name="sequential">Whether or not frames are iterated sequentially or the order chosen randomly upon each run-through.</param>
        public Animation(int ID, Frame[] frames, int framerate, int loop, bool sequential) : this(ID, frames, loop, sequential)
        { 
            // TODO: Implement framerates for animations.
        }
        public Animation(int ID, Frame[] frames, int loop, bool sequential) : this(ID, frames)
        {
            // TODO: Allow for randomly ordered progression through animation frames.
        }
        public Animation(int ID, Frame[] frames, int loop) : this(ID, frames)
        {
            this.loop = loop;

            // TODO: Animation loops of a finite number.
            if (loop > 0)
                frames[frames.Length - 1].next = frames[0];
        }
        public Animation(int ID, Frame[] frames) : this(frames)
        {
            this.ID = ID;

            // FIX: Animation creation can double up on logging due to overloaded constructors, etc. Easy fix.
            Files.log(2, "Animation (" + ID + ") created.");
        }
        public Animation(Frame[] frames)
        {
            firstFrame = frames[0];
            currentFrame = frames[0];

            // Connect the given frames into an animation.
            for (int i = 0; i < frames.Length; i++)
            {
                if (i + 1 < frames.Length)
                    frames[i].next = frames[i + 1];
            }

            // TODO: Ability to assign animation IDs automatically (properly).
            // SHIT: Automated ID assignment.
            Random randomIntGen = new Random();
            ID = randomIntGen.Next(0, 1000);

            framerate = DEFAULT_FRAMERATE;

            Files.log(2, "Animation (" + ID + ") created.");
        }
        
        // Iterate and loop through frames in an animation.
        public static Frame nextFrame(int animationID)
        {
            return null;
        }

    }

    /// <summary>
    /// Used in isolation or in multiples associated with an <see cref="Animation"/>.
    /// </summary>
    class Frame
    {
        private Texture myTexture;
        /// <value>Stored in speedy graphics memory.</value>
        public Texture texture
        {
            get { return myTexture; }
            set { myTexture = value; }
        }
        // Points to the next frame in the animation, if one exists. 
        private Frame nextFrame;
        public Frame next
        {
            get { return nextFrame; }
            set { nextFrame = value; }
        }

        public Frame(Texture texture, Frame nextFrame)
        {
            myTexture = texture;
            this.nextFrame = nextFrame;
        }
        /// <summary>
        /// A solitary frame.
        /// </summary>
        public Frame(Texture texture)
        {
            myTexture = texture;
        }
        /// <summary>
        /// A solitary frame.
        /// </summary>
        /// <param name="image">Will be converted to an <see cref="SFML.Graphics.Texture"/>.</param>
        public Frame(Image image)
        {
            myTexture = new Texture(image);
        }
        /// <summary>
        /// A solitary frame.
        /// </summary>
        /// <param name="ID">Represents a previously loaded Texture or Image.</param>
        public Frame(int ID)
        {
            myTexture = Files.getSprite(0);
        }
        // TEMP REMOVE
        public Frame()
        {

        }
    }
}
