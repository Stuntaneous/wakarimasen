using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

// DEBUG TEMP
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace wakarimasen
{

    class Character
    {
        /// <summary>
        /// All characters.
        /// </summary>
        private static SortedDictionary<int, Character> charactersList = new SortedDictionary<int, Character>();
        /// <summary>
        /// All available actions.
        /// </summary>
        private static SortedDictionary<int, Action> actionsList = new SortedDictionary<int, Action>();
        /// <summary>
        /// All available effects, sorted by attached object type.
        /// </summary>
        private static SortedDictionary<int, Effect> effectsList = new SortedDictionary<int, Effect>();

        static Character thePlayer;
        public static Character player
        {
            get { return thePlayer; }
            set { thePlayer = value; }
        }

        private string myName = "Unnamed";
        public string name
        {
            get { return myName; }
            set { myName = value; }
        }
        private int mySpriteID = -1;
        public int spriteID
        {
            get { return mySpriteID; }
            set { mySpriteID = value; }
        }
        private double[] myPosition;
        public double[] position
        {
            get { return myPosition; }
            set { myPosition = value; }
        }

        private SortedDictionary<int, Action> myActions = new SortedDictionary<int, Action>();
        public SortedDictionary<int, Action> actions
        {
            get { return myActions; }
            set { myActions = value; }
        }

        /// <summary>
        /// A character is born.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spriteID">Who's this bitch?</param>
        /// <param name="locationX">Map coordinate.</param>
        /// <param name="locationY">Map coordinate.</param>
        public Character(string name, int spriteID, double locationX, double locationY)
        {
            myName = name;
            mySpriteID = spriteID;
            myPosition = new double[2]{locationX, locationY};

            myActions.Add(0, actionsList[0]);
            myActions[0].effects[0].attach(this); // SHIT: dirt.
            myActions.Add(1, actionsList[1]);
            myActions[1].effects[0].attach(this);
            //myActions.Add(2, actions[2]);
            //myActions.Add(3, actions[3]);

            charactersList.Add(charactersList.Count, this);

            Files.log(2, "Character created (\"" + name + "\" at " + locationX + ", " + locationY + ").");

            string myActionsList = ""; 
            for (int i = 0; i < myActions.Count; i++)
            {
                myActionsList += myActions[i].name + ", ";
                if (i == myActions.Count - 1)
                    myActionsList = myActionsList.Substring(0, myActionsList.Length - 2) + ".";
            }

            Files.log(3, "Actions given to \"" + this.name + "\" include " + myActionsList);
        }

        public static Character getCharacter(int ID)
        {
            return charactersList[ID];
        }
        public static Character getCharacter(string name)
        {
            return null; // To be completed. The null is meaningless :0:0
        }

        // Hard-coded actions to be taken outside to an external, moddable file.
        public static void initialise()
        {
            effectsList.Add(0, new Effect("Moving left", "", new Animation(0, new Frame[] { new Frame(3), new Frame(4), new Frame(5) }), -5, 0));
            actionsList.Add(0, new Action("Move left", "Standard movement to the left", effectsList[0], false));

            effectsList.Add(1, new Effect("Moving right", "", new Animation(1, new Frame[] { new Frame(0), new Frame(1), new Frame(2) }), 5, 0));
            actionsList.Add(1, new Action("Move right", "Standard movement to the right", effectsList[1], false));

            //actionsList.Add(2, new Action("Move up", "Standard movement upwards", effects[2], false));

            //actionsList.Add(3, new Action("Move down", "Standard movement downwards", effects[3], false));

            Files.log(2, "Actions created.");
        }

    }

    struct Action
    {
        // Holds the action's name as it appears in various tense. 
        // 0 = past
        // 1 = present
        // Consider: as necessary, the tense could be determined by checking the actual letters against the appropriate rules and changing sentences procedurally on the fly. 
        string myName;
        public string name
        {
            get { return myName; }
            set { myName = value; }
        }
        string myDescription;
        public string description
        {
            get { return myDescription; }
            set { myDescription = value; }
        }
        List<Effect> myEffects;
        public List<Effect> effects
        {
            get { return myEffects; }
            set { myEffects = value; }
        }

        // Whether or not the action is described to the player. 
        bool hidden;

        public Action(string name, string description, Effect effect, bool hidden)
        {
            myName = name;
            myDescription = description;

            myEffects = new List<Effect>();
            myEffects.Add(effect);

            this.hidden = hidden;
        }
        public Action(Action action)
        {
            myName = action.name;
            myDescription = action.description;
            myEffects = action.effects;
            this.hidden = action.hidden;
        }

        public void perform()
        {
            //Files.log(1, "DEBUG --- List<Effect> myEffects count: " + myEffects.Count);
            for (int i = 0; i < myEffects.Count; i++)
            {
                myEffects[i].perform();
            }
        }

    }

    class Effect
    {
    
        // Effects change coordinates, stats, animations and frames. They can fire off other actions and effects.
        // Effects can have a position on the map. They can have a duration or delay. They can disable or enable keys. 
        // Effects can have required conditions.

        string myName;
        string myDescription;
        Animation myAnimation;
        public Animation animation
        {
            get { return myAnimation; }
            
        }

        private object attachment;

        double changeX;
        double changeY;
        object changeAnimationTarget;
        Animation changeAnimationReplacement;

        float delay;
        float duration;

        Action triggerAction;
        Effect triggerEffect;
        Character wah;
        //public Effect(object attachment)
        //{
        //    this.attachment = attachment;
        //}
        public Effect(string myName, string myDescription, Animation animation, double changeX, double changeY, object changeAnimationTarget, Animation changeAnimationReplacement, float delay, float duration, Action triggerAction, Effect triggerEffect) : this(myName, myDescription, animation, changeX, changeY)
        {
            this.changeAnimationTarget = changeAnimationTarget;
            this.changeAnimationReplacement = changeAnimationReplacement;
            this.delay = delay;
            this.duration = duration;
            this.triggerAction = triggerAction;
            this.triggerEffect = triggerEffect;
        }
        public Effect(string myName, string myDescription, Animation animation, double changeX, double changeY) : this(myName, myDescription, animation)
        {
            this.changeX = changeX;
            this.changeY = changeY;
        }
        public Effect(string myName, string myDescription, Animation animation)
        {
            this.myName = myName;
            if (myDescription != "")
                this.myDescription = myDescription;
            myAnimation = animation;
        }

        public void attach(object attachment)
        {
            this.attachment = attachment;
        }

        public void perform()
        {
            // FIX: Classes and casting..
            // Casting alone won't let me access the Character methods, so a temporary swap-object has been required. 
            Character tempChar = (Character)attachment;

            if (changeX != 0)
            {
                double[] tempPosition = new double[] {tempChar.position[0] + changeX, tempChar.position[1]};
                tempChar.position = tempPosition;
                tempChar = null;
                tempPosition = null;
            }


        }

    }


}
