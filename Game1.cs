//Author: Richard Kapsh
//File Name: PASS 3
//Project Name: PASS 3
//Creation Date: June 2, 2022
//Modified Date: June 21, 2022
//Description: This program is built to be a game that is fun for anyone who plays it

//Output: Putting objects and text onto the screen
//Variables: Numbers, Letters, Texts, Locations, Dimensions 
//Input: Asking and reading an option and doing it
//Arrays: Anything that has more than one of the same thing like trees and their locations
//Subprograms: Performs calculations that could be used for many times (directions or randomly generating banana locations) for elements in an array (trees)
//Selection: The if, if-else, and if-else if statements are used to follow the user's decision process for things like clicking buttons in certain situations
//Loops: Draw the right amount of trees based on the level

//Uses these libraries 
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helper;

namespace PASS_3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //Imports the ability to drawd
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Declares the game states that will be used
        const int MENU = 0;
        const int LEVELS = 1;
        const int INSTRUCTIONS = 2;
        const int GAMEPLAY = 3;
        const int PAUSE = 4;
        const int ENDGAME = 5;

        //Declares gravity for physics calculations
        const double GRAVITY = 9.81;

        //Declares the inital game state as the menu
        int gameState = MENU;

        //Sets up the random number generator
        Random rng = new Random();

        //Declares font types and sizes for future use
        SpriteFont titleFont;
        SpriteFont messageFont;
        SpriteFont betweenFont;

        //Declares both the width and height of the screen 
        int screenWidth;
        int screenHeight;

        //Declares what is going to be written on the menu title
        string titleText = "BANANA THROW";

        //Declares what is going to be written on the instructions title
        string instructionsTitleText = "INSTRUCTIONS";

        //Declares an array of instructions line by line
        string[] instructionsText = {"You are monkey that can move left (a), right (d), up (w), down (s), and can throw bananas (Left click on mouse)",
                                     "To throw the banana, you need to be touching the banana or else it will not throw",
                                     "You can only climb up and down on trees", 
                                     "You can move left and right on the ground or when you are up in any of the trees in a level",
                                     "The objective of this game is to throw as many bananas as possible to unlock the level, and eventually beat the game", 
                                     "That's it! Good luck and have fun!"};

        //Declares the location of the instructions
        Vector2[] instructionsLocs = new Vector2[6];

        //Declares the previous letter for tracking purposes 
        char lastLetter = ' ';

        //Declares an index that will be used to determine what random tree will have the random banana
        int randomTree;

        //Declares the location of the title
        Vector2 titleLoc;

        //Declares the location of the level title
        string pickLevel = "PICK A LEVEL";

        //Declares the time remaining for future purposes
        int timeRemaining;

        //Declares the location of the level title
        Vector2 pickLevelLoc;

        //Declares the location of the instructions title
        Vector2 instructionsTitleLoc;

        //Declares the location of the amount of time remaining in the game
        Vector2 timeRemainingLoc;

        //Declares the location of the title once the game is over
        Vector2 gameOverTitle;

        //Declares the parchment rectangle that is used as a background in instructions
        Rectangle parchmentRec;

        //Declares an array of locations of each level button
        Vector2[] levelLocs = new Vector2[5];

        //Declares the location of the score
        Vector2[] scoreLocs = new Vector2[2];

        //Declares the title monkey image and rectangle
        Texture2D backgroundMonkeyImg;
        Rectangle backgroundMonkeyRec;

        //Declares the background image and rectangle
        Texture2D backgroundLandscapeImg;
        Rectangle backgroundLandscapeRec;

        //Declares the monkey rectangle
        Rectangle monkeyRec;

        //Declares the button images
        Texture2D playImg;
        Texture2D goImg;
        Texture2D backImg;
        Texture2D instructionsImg;
        Texture2D menuImg;
        Texture2D exitImg;

        //Declares the two phases that a progress bar will be in between
        Texture2D progressEmpty;
        Texture2D progressFull;

        //Declares the rectangles of the progress bar
        Rectangle progressEmptyRec;
        Rectangle progressFullRec;

        //Declares the banana image
        Texture2D bananaImg;

        //Declares the array of walking images to the right side
        Texture2D[] walks = new Texture2D[8];

        //Declares the array of walking images to the left side
        Texture2D[] leftWalks = new Texture2D[8];

        //Declares the array of climbing images
        Texture2D[] climbs = new Texture2D[3];

        //Declares the array of throwing images
        Texture2D[] throws = new Texture2D[3];

        //Declares the tree image
        Texture2D tree;

        //Declares the platform image
        Texture2D platform;

        //Declares the background of each level button
        Texture2D levelBackground;

        //Declares the parchment image
        Texture2D parchmentImg;

        //Declares the lock image 
        Texture2D lockImg;

        //Declares the lock rectangle
        Rectangle[] lockRecs = new Rectangle[4];

        //Declares the rectangles of all of the buttons
        Rectangle playRec;
        Rectangle goRec;
        Rectangle exitRec;
        Rectangle backRec;
        Rectangle instructionsRec;
        Rectangle menuRec;

        //Declares an array of trees 
        Rectangle[] treeRecs = new Rectangle[6];
        
        //Declares an array of platforms
        Rectangle[] platformRecs = new Rectangle[2];

        //Declares the rectangle for bananas
        Rectangle bananaRec;

        //Declares an array of each level's rectangle
        Rectangle[] levelRecs = new Rectangle[5];

        //Declares a timer for the transition of buttons to pop up in the menu
        Timer menuTimer;

        //Declares the timer to throw bananas
        Timer throwTimer;

        //Declares the timer to track the amount of time passed in a game
        Timer gameTimer;

        //Declares the current and previous mouse states
        MouseState mouse;
        MouseState prevMouse;

        //Declares the current and previous keyboard states
        KeyboardState kb;
        KeyboardState prevkb;

        //Declares the current index of both direction of walking
        int image = 0;

        //Declares the amount of bananas in a basket
        int basketBananas = 0;

        //Declares the index of climbing
        int climbImage;

        //Declares the index of throwing
        int throwImage;
        
        //Declares the level number
        int level;

        //Declares the speed of the platform
        int platformSpeed = 5;

        //Declares the index of the current tree that the monkey is intersecting with
        int currentTree;

        //Declares an array of level scores for each level
        int[] prevLevelScores = new int[4];
        int[] levelScores = new int[4];

        int levelRequirement = 10;

        //Declares whether the banana is thrown facing left or right
        bool throwRight;

        //Enables access to the mgcb content pipeline 
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Sets the dimensions of the width and height of the screen
            this.graphics.PreferredBackBufferWidth = 1200;
            this.graphics.PreferredBackBufferHeight = 600;
            
            //Sets the mouse to visible when the program is running
            IsMouseVisible = true;

            //Applies changes to the graphics
            this.graphics.ApplyChanges();

            //Sets the menu timer to 3 seconds and not active immediately
            menuTimer = new Timer(3, false);

            //Sets the throw timer as infinite and not active immediately
            throwTimer = new Timer(Timer.INFINITE_TIMER, false);

            //Sets the game timer to 90 seconds and not active immediately 
            gameTimer = new Timer(90, false);

            //Intializes all code above in initialize
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Creates a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Loads the fonts from the computer
            titleFont = Content.Load<SpriteFont>("Fonts/titleFont");
            messageFont = Content.Load<SpriteFont>("Fonts/messageFont");
            betweenFont = Content.Load<SpriteFont>("Fonts/BetweenFont");

            //Sets the width and height of the screen based on the imported width and height
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;

            //Sets the location of the title
            titleLoc = new Vector2(screenWidth / 2 - titleFont.MeasureString(titleText).X / 2, 10);

            //Loads and sets the rectangle of the title screen monkey 
            backgroundMonkeyImg = Content.Load<Texture2D>("Images/Background/MonkeyTitle");
            backgroundMonkeyRec = new Rectangle(0, 100, screenWidth, screenHeight - 50);

            //Loads and sets the rectangle of the landscape
            backgroundLandscapeImg = Content.Load<Texture2D>("Images/Background/CoolBackground");
            backgroundLandscapeRec = new Rectangle(0, 0, screenWidth, screenHeight);

            //Loads the button images
            playImg = Content.Load<Texture2D>("Images/Sprites/PlayBtn2");
            goImg = Content.Load<Texture2D>("Images/Sprites/GoBtn2");
            exitImg = Content.Load<Texture2D>("Images/Sprites/ExitBtn2");
            backImg = Content.Load<Texture2D>("Images/Sprites/BackBtn2");
            instructionsImg = Content.Load<Texture2D>("Images/Sprites/InstructionsBtn2");
            menuImg = Content.Load<Texture2D>("Images/Sprites/MenuBtn2");
            
            //Loads the parchment image
            parchmentImg = Content.Load<Texture2D>("Images/Sprites/Parchment");

            //Sets the rectangles of all buttons
            playRec = new Rectangle(screenWidth / 3, 2 * screenHeight / 5, screenWidth / 3, screenHeight / 6);
            goRec = new Rectangle(screenWidth / 2, 120, screenWidth / 4, 166);
            exitRec = new Rectangle(screenWidth / 3, 4 * screenHeight / 5, screenWidth / 3, screenHeight / 6);
            backRec = new Rectangle(screenWidth / 3, 7 * screenHeight / 9, screenWidth / 3, screenHeight / 6);
            instructionsRec = new Rectangle(screenWidth / 3, 3 * screenHeight / 5, screenWidth / 3, screenHeight / 6);
            menuRec = new Rectangle(screenWidth / 2 - menuImg.Width/2, 2 * screenHeight / 3, screenHeight / 3, screenHeight / 6);

            //Loads all the walking images when going right
            walks[0] = Content.Load<Texture2D>("Images/Sprites/Walk1Removed");
            walks[1] = Content.Load<Texture2D>("Images/Sprites/Walk2Removed");
            walks[2] = Content.Load<Texture2D>("Images/Sprites/Walk3Removed");
            walks[3] = Content.Load<Texture2D>("Images/Sprites/Walk4Removed");
            walks[4] = Content.Load<Texture2D>("Images/Sprites/Walk5Removed");
            walks[5] = Content.Load<Texture2D>("Images/Sprites/Walk6Removed");
            walks[6] = Content.Load<Texture2D>("Images/Sprites/Walk7Removed");
            walks[7] = Content.Load<Texture2D>("Images/Sprites/Walk8Removed");

            //Loads all the walking images when going left
            leftWalks[0] = Content.Load<Texture2D>("Images/Sprites/Walk1Left");
            leftWalks[1] = Content.Load<Texture2D>("Images/Sprites/Walk2Left");
            leftWalks[2] = Content.Load<Texture2D>("Images/Sprites/Walk3Left");
            leftWalks[3] = Content.Load<Texture2D>("Images/Sprites/Walk4Left");
            leftWalks[4] = Content.Load<Texture2D>("Images/Sprites/Walk5Left");
            leftWalks[5] = Content.Load<Texture2D>("Images/Sprites/Walk6Left");
            leftWalks[6] = Content.Load<Texture2D>("Images/Sprites/Walk7Left");
            leftWalks[7] = Content.Load<Texture2D>("Images/Sprites/Walk8Left");

            //Loads all the climbing images
            climbs[0] = Content.Load<Texture2D>("Images/Sprites/Climb1-removebg-preview");
            climbs[1] = Content.Load<Texture2D>("Images/Sprites/Climb2-removebg-preview");
            climbs[2] = Content.Load<Texture2D>("Images/Sprites/Climb3-removebg-preview");

            //Loads all the throwing images
            throws[0] = Content.Load<Texture2D>("Images/Sprites/Throw1-removebg-preview");
            throws[1] = Content.Load<Texture2D>("Images/Sprites/Throw2-removebg-preview");
            throws[2] = Content.Load<Texture2D>("Images/Sprites/Throw3-removebg-preview");

            //Loads progress bars
            progressEmpty = Content.Load<Texture2D>("Images/Sprites/ProgressBarEmpty");
            progressFull = Content.Load<Texture2D>("Images/Sprites/ProgressBarFull");

            progressEmptyRec = new Rectangle(screenWidth / 2 - progressEmpty.Width / 2, 10, progressEmpty.Width, progressEmpty.Height);
            progressFullRec = new Rectangle(screenWidth / 2 - progressFull.Width / 2, 10, progressFull.Width, progressFull.Height);

            //Loads the lock image
            lockImg = Content.Load<Texture2D>("Images/Sprites/Lock");

            //Loads the tree image
            tree = Content.Load<Texture2D>("Images/Sprites/tree");

            //Loads the platform image
            platform = Content.Load<Texture2D>("Images/Sprites/Platform");

            //Loads the banana image
            bananaImg = Content.Load<Texture2D>("Images/Sprites/banana");

            //Loads the levels button
            levelBackground = Content.Load<Texture2D>("images/Sprites/levels");

            //Sets the monkey rectangle to the bottom and middle of the screen
            monkeyRec = new Rectangle(screenWidth / 2, screenHeight - 65, 2 * walks[image].Width / 3, 2 * walks[image].Height / 3);

            //Sets the banana rectangle 
            bananaRec = new Rectangle(screenWidth/2 + 75, screenHeight/3, bananaImg.Width / 9, bananaImg.Height / 9);

            //Sets the lock images as being on the level buttons
            lockRecs[0] = new Rectangle(screenWidth / 2 - levelBackground.Width / 4, 4 * screenHeight / 10, lockImg.Width/8, levelBackground.Height / 2);
            lockRecs[1] = new Rectangle(screenWidth / 2 - levelBackground.Width / 4, 5 * screenHeight / 10, lockImg.Width/8, levelBackground.Height / 2);
            lockRecs[2] = new Rectangle(screenWidth / 2 - levelBackground.Width / 4, 6 * screenHeight / 10, lockImg.Width/8, levelBackground.Height / 2);
            lockRecs[3] = new Rectangle(screenWidth / 2 - levelBackground.Width / 4, 7 * screenHeight / 10, lockImg.Width/8, levelBackground.Height / 2);

            //Sets all the tree rectangles positions and dimensions
            treeRecs[0] = new Rectangle(3 * screenWidth / 6 -25, 60, 275, 550);
            treeRecs[1] = new Rectangle(2 * screenWidth / 6 - 25, 60, 275, 550);
            treeRecs[2] = new Rectangle(4 * screenWidth / 6 - 25, 60, 275, 550);
            treeRecs[3] = new Rectangle(1 * screenWidth / 6 - 25, 60, 275, 550);
            treeRecs[4] = new Rectangle(5 * screenWidth / 6 - 25, 60, 275, 550);
            treeRecs[5] = new Rectangle(- 25, 60, 275, 550);

            //Sets all the platform rectangles
            platformRecs[0] = new Rectangle(screenWidth / 2 - screenWidth/12, 9 * screenHeight / 11, 2 * platform.Width / 5, bananaImg.Height / 6);
            platformRecs[1] = new Rectangle(screenWidth / 2 - screenWidth/12, 10 * screenHeight / 11, 2 * platform.Width / 5, bananaImg.Height / 6);

            //Sets all the level rectangles to the middle of the screen
            levelRecs[0] = new Rectangle(screenWidth / 2 - levelBackground.Width / 4, 3 * screenHeight / 10, levelBackground.Width / 2, levelBackground.Height / 2);
            levelRecs[1] = new Rectangle(screenWidth / 2 - levelBackground.Width / 4, 4 * screenHeight / 10, levelBackground.Width / 2, levelBackground.Height / 2);
            levelRecs[2] = new Rectangle(screenWidth / 2 - levelBackground.Width / 4, 5 * screenHeight / 10, levelBackground.Width / 2, levelBackground.Height / 2);
            levelRecs[3] = new Rectangle(screenWidth / 2 - levelBackground.Width / 4, 6 * screenHeight / 10, levelBackground.Width / 2, levelBackground.Height / 2);
            levelRecs[4] = new Rectangle(screenWidth / 2 - levelBackground.Width / 4, 7 * screenHeight / 10, levelBackground.Width / 2, levelBackground.Height / 2);

            //Locates all the level text
            levelLocs[0] = new Vector2(screenWidth / 2 - 15, 3 * screenHeight / 10);
            levelLocs[1] = new Vector2(screenWidth / 2 - 15, 4 * screenHeight / 10);
            levelLocs[2] = new Vector2(screenWidth / 2 - 15, 5 * screenHeight / 10);
            levelLocs[3] = new Vector2(screenWidth / 2 - 15, 6 * screenHeight / 10);
            levelLocs[4] = new Vector2(screenWidth / 2 - 15, 7 * screenHeight / 10);

            //Locates all the lines in instructions
            instructionsLocs[0] = new Vector2(screenWidth / 2 - messageFont.MeasureString(instructionsText[0]).X/2, 5 * screenHeight / 15);
            instructionsLocs[1] = new Vector2(screenWidth / 2 - messageFont.MeasureString(instructionsText[1]).X/2, 6 * screenHeight / 15);
            instructionsLocs[2] = new Vector2(screenWidth / 2 - messageFont.MeasureString(instructionsText[2]).X/2, 7 * screenHeight / 15);
            instructionsLocs[3] = new Vector2(screenWidth / 2 - messageFont.MeasureString(instructionsText[3]).X/2, 8 * screenHeight / 15);
            instructionsLocs[4] = new Vector2(screenWidth / 2 - messageFont.MeasureString(instructionsText[4]).X/2, 9 * screenHeight / 15);
            instructionsLocs[5] = new Vector2(screenWidth / 2 - messageFont.MeasureString(instructionsText[5]).X/2, 10 * screenHeight / 15);

            //Locates the title of instructions
            instructionsTitleLoc = new Vector2(screenWidth / 2 - titleFont.MeasureString(instructionsTitleText).X / 2, screenHeight / 15);

            //Locates the title of the levels
            pickLevelLoc = new Vector2(screenWidth / 2 - titleFont.MeasureString(pickLevel).X / 2, screenHeight / 15);

            //Locates the time remaining in the game 
            timeRemainingLoc = new Vector2(screenWidth / 2 - messageFont.MeasureString("Time Remaining: " + timeRemaining).X / 2, screenWidth / 50);

            //Locates the title after the game finishes
            gameOverTitle = new Vector2(screenWidth/2 - titleFont.MeasureString("GAME OVER").X/2, screenHeight/15);

            //Locates the score that the player has
            scoreLocs[0] = new Vector2(200, 10);
            scoreLocs[1] = new Vector2(screenWidth / 2 - betweenFont.MeasureString("Score: " + basketBananas).X / 2, screenHeight / 3);

            //Sets the parchment rectangle
            parchmentRec = new Rectangle (0 - screenHeight/4, 0, 5 * screenWidth/4, screenHeight);

            //Assings all elements of an array a value to make it eligible for comparison
            levelScores[0] = 0;
            levelScores[1] = 0;
            levelScores[2] = 0;
            levelScores[3] = 0;            
            prevLevelScores[0] = 0;
            prevLevelScores[1] = 0;
            prevLevelScores[2] = 0;
            prevLevelScores[3] = 0;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {        /// Allows the game to run logic such as updating the world,

            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Takes the previous mouse state and finds the new mouse state
            prevMouse = mouse;
            mouse = Mouse.GetState();

            //Updates different game states depending on the user's choice
            switch (gameState)
            {
                case MENU:

                    //Updates the timer
                    menuTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);

                    //Activates the timer if it is inactive
                    if (menuTimer.IsInactive())
                    {
                        //Activates the timer
                        menuTimer.Activate();
                    }

                    //Changes the game state depending on the button clicked 
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        //Changes the game state from the menu to levels
                        if (playRec.Contains(mouse.Position))
                        {
                            //Assigns the game state as the levels
                            gameState = LEVELS;
                        }
                        else if (instructionsRec.Contains(mouse.Position))
                        {
                            //Assigns the game state as the instructions
                            gameState = INSTRUCTIONS;
                        }
                        else if (exitRec.Contains(mouse.Position))
                        {
                            //Exits the program
                            Exit();
                        }
                    }
                    break;
                case LEVELS:

                    //Sets the level depending on the button clicked
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        //Sets the level depending on where the mouse is
                        if (levelRecs[0].Contains(mouse.Position))
                        {
                            //Sets level to 1 and begins the game
                            level = 1;
                            gameState = GAMEPLAY;                            
                        }
                        else if (levelRecs[1].Contains(mouse.Position) && prevLevelScores[0] >= levelRequirement)
                        {
                            //Sets level to 2 and begins the game
                            level = 2;
                            gameState = GAMEPLAY;                            
                        }
                        else if (levelRecs[2].Contains(mouse.Position) && prevLevelScores[1] >= levelRequirement)
                        {
                            //Sets level to 3 and begins the game
                            level = 3;
                            gameState = GAMEPLAY;                           
                        }
                        else if (levelRecs[3].Contains(mouse.Position) && prevLevelScores[2] >= levelRequirement)
                        {
                            //Sets level to 4 and begins the game
                            level = 4;
                            gameState = GAMEPLAY;                            
                        }
                        else if (levelRecs[4].Contains(mouse.Position) && prevLevelScores[3] >= levelRequirement)
                        {
                            //Sets level to 5 and begins the game
                            level = 5;
                            gameState = GAMEPLAY;                            
                        }
                        else if (backRec.Contains(mouse.Position))
                        {
                            //Sets the game state as the menu
                            gameState = MENU;
                        }
                    }

                    break;
                case INSTRUCTIONS:

                    //Sets the level depending on the button clicked
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        //Sets the game state depending on where the mouse is
                        if (backRec.Contains(mouse.Position))
                        {
                            //Sets the game state as menu
                            gameState = MENU;
                        }
                    }
                    break;
                case GAMEPLAY:

                    //Updates the timer
                    gameTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);

                    //Activates the game timer if it is not already active
                    if (gameTimer.IsInactive())
                    {
                        //Activates the game timer
                        gameTimer.Activate();

                        //Resets the amount of bananas collected
                        basketBananas = 0;
                    }

                    if (gameTimer.IsActive())
                    {
                        //Calculates time remaining as the total time in the timer subtracted by the time that is passed
                        timeRemaining = (int)(90 - gameTimer.GetTimePassed());

                        progressFullRec = new Rectangle(screenWidth / 2 - progressFull.Width / 2, 10, (int)timeRemaining, progressFull.Height);
                    }

                    //Takes the previous keyboard state and finds the new keyboard state
                    prevkb = kb;
                    kb = Keyboard.GetState();

                    //Updates the timer
                    throwTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);
                    
                    //Takes the timer's value as a double 
                    double throwTime = throwTimer.GetTimePassed();

                    //Randomizes a value as an index for the tree that will have the random banana
                    randomTree = rng.Next(0, level + 1);

                    //Flips the first platform's horizontal direction when it goes beyond the screen
                    if ((platformRecs[0].X >= screenWidth - platformRecs[0].Width) || (platformRecs[0].X <= 0))
                    {
                        //Flips the first platform's direction
                        platformSpeed = -platformSpeed;
                    }

                    //Flips the second platform's horizontal direction when it goes beyond the screen
                    if ((platformRecs[1].X >= screenWidth - platformRecs[1].Width) || (platformRecs[1].X <= 0))
                    {
                        //Flips the second platform's direction
                        platformSpeed = -platformSpeed;
                    }

                    //Moves in opposite directions 
                    platformRecs[0].X += platformSpeed;
                    platformRecs[1].X -= platformSpeed;

                    //Indicates which tree is closest based on collision detection
                    if (treeRecs[0].Contains(monkeyRec))
                    {
                        //Sets the closest tree to the first one
                        currentTree = 0;
                    }
                    else if (treeRecs[1].Contains(monkeyRec))
                    {
                        //Sets the closest tree to the second one
                        currentTree = 1;
                    }
                    else if (treeRecs[2].Contains(monkeyRec) && level >= 2)
                    {
                        //Sets the closest tree to the third one
                        currentTree = 2;
                    }
                    else if (treeRecs[3].Contains(monkeyRec) && level >= 3)
                    {
                        //Sets the closest tree to the fourth one
                        currentTree = 3;
                    }
                    else if (treeRecs[4].Contains(monkeyRec) && level >= 4)
                    {
                        //Sets the closest tree to the fifth one
                        currentTree = 4;
                    }
                    else if (treeRecs[5].Contains(monkeyRec) && level == 5)
                    {
                        //Sets the closest tree to the sixth one
                        currentTree = 5;
                    }

                    //Calculates direction if the 'a' key is down
                    if (kb.IsKeyDown(Keys.A))
                    {
                        //Calls the function to do calculations and restrictions for moving left
                        monkeyRec = MoveLeft(treeRecs[currentTree], walks.Length, monkeyRec, ref image, screenWidth, lastLetter);

                        //Sets the last letter pressed to 'a'
                        lastLetter = 'a';
                    }

                    //Calculates direction if the 'd' key is down
                    if (kb.IsKeyDown(Keys.D))
                    {
                        //Calls the function to do calculations and restrictions for moving right
                        monkeyRec = MoveRight(treeRecs[currentTree], walks.Length, monkeyRec, ref image, screenWidth, lastLetter);

                        //Sets the last letter pressed to 'd'
                        lastLetter = 'd';
                    }

                    //Calculates direction if the 's' key is down
                    if (kb.IsKeyDown(Keys.S) && (monkeyRec.X >= treeRecs[currentTree].X + treeRecs[currentTree].Width / 2 - 50 && monkeyRec.X + monkeyRec.Width <= treeRecs[currentTree].X + treeRecs[currentTree].Width / 2 + 50 || monkeyRec.Y <= 400))
                    {
                        //Calls the function to do calculations and restrictions for moving down
                        monkeyRec = MoveDown(treeRecs[currentTree], 3, monkeyRec, ref climbImage, screenHeight);

                        //Sets the last letter pressed to 's'
                        lastLetter = 's';
                    }

                    //Calculates direction if the 'w' key is down
                    if (kb.IsKeyDown(Keys.W) && ((monkeyRec.X >= treeRecs[currentTree].X + treeRecs[currentTree].Width / 2 - 50 && monkeyRec.X + monkeyRec.Width <= treeRecs[currentTree].X + treeRecs[currentTree].Width / 2 + 50) || monkeyRec.Y <= 400))
                    {
                        //Calls the function to do calculations and restrictions for moving up
                        monkeyRec = MoveUp(treeRecs[currentTree], 3, monkeyRec, ref climbImage);

                        //Sets the last letter pressed to 'w'
                        lastLetter = 'w';
                    }

                    //Calls for the function that determines the banana rectangles location based on how the monkey moves it
                    bananaRec = PickupBanana(monkeyRec, bananaImg, bananaRec, lastLetter);

                    //Activates the throw timer if button is pressed
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        //Calls for the function that activates the throw timer if the monkey rectangle collides with the banana's 
                        throwTimer = ActivateThrowTimer(throwTimer, bananaRec, monkeyRec);

                        //Sets direction based on last letter
                        if (lastLetter == 'd')
                        {
                            //Sets direction as right
                            throwRight = true;
                        }

                        //Sets direction based on last letter
                        if (lastLetter == 'a')
                        {
                            //Sets direction as left
                            throwRight = false;
                        }
                    }

                    //Calls the function that does all the calculations once the banana is thrown
                    bananaRec = ThrowBanana(bananaImg, bananaRec, lastLetter, throwTimer, throwImage, 3, throwTime, throwRight);

                    //Checks to see if the banana fell off the screen or collided with either of the platforms
                    if (bananaRec.Y >= 600 || bananaRec.Contains(platformRecs[0]) || bananaRec.Contains(platformRecs[1]))
                    {
                        //Calls a function that randomizes a coordinate as the banana's random value
                        Point point = RandomizeBanana(throwTimer, treeRecs[randomTree]);
                        bananaRec.X = point.X;
                        bananaRec.Y = point.Y;
                    }

                    //Adds the amount of bananas in the basket by adding one everytime the banana collides with the platform
                    if (platformRecs[0].Contains(bananaRec) || platformRecs[1].Contains(bananaRec))
                    {
                        //Adds one banana to the amount collided
                        basketBananas++;
                    }

                    //Checks to see if the game has finished
                    if (gameTimer.IsFinished())
                    {
                        //Resets the timer, although not immediately
                        gameTimer.ResetTimer(false);

                        //Calculates the amount of bananas that collided with platforms for that level
                        levelScores[level - 1] = basketBananas;

                        //Compares the score to previous attempts and sets it the best previous attempt if so
                        if (levelScores[level - 1] >= prevLevelScores[level - 1])
                        {
                            prevLevelScores[level - 1] = levelScores[level - 1];
                        }

                        //Sets the game state to after the game
                        gameState = ENDGAME;
                    }

                    break;
                case PAUSE:
                    break;
                case ENDGAME:

                    //Checks to see if the user clicked
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        //Checks if the user clicked the menu button
                        if (menuRec.Contains(mouse.Position))
                        {
                            //Resets the monkey's rectangle value potentially for another game
                            monkeyRec = new Rectangle(screenWidth / 2, screenHeight - 65, 2 * walks[image].Width / 3, 2 * walks[image].Height / 3);

                            //Sets the game state to menu
                            gameState = MENU;

                            //Randomizes values if the throw timer is still active
                            if (throwTimer.IsActive())
                            {
                                //Calls a function that randomizes a coordinate as the banana's random value
                                Point point = RandomizeBanana(throwTimer, treeRecs[randomTree]);
                                bananaRec.X = point.X;
                                bananaRec.Y = point.Y;
                            }

                        }

                    }

                    break;
            }

            //Updates all code above that is in update
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Sets the background color
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Begins the drawing
            spriteBatch.Begin();

            //Draws different game states depending on the user's choice
            switch (gameState)
            {
                case MENU:

                    //Draws initial menu
                    spriteBatch.DrawString(titleFont, titleText, titleLoc, Color.Black);
                    spriteBatch.Draw(backgroundLandscapeImg, backgroundLandscapeRec, Color.Yellow);
                    spriteBatch.Draw(backgroundMonkeyImg, backgroundMonkeyRec, Color.White);

                    //Shows buttons as a transition
                    if (menuTimer.IsFinished())
                    {
                        //Draws buttons in the menu
                        spriteBatch.Draw(playImg, playRec, Color.White);
                        spriteBatch.Draw(instructionsImg, instructionsRec, Color.White);
                        spriteBatch.Draw(exitImg, exitRec, Color.White);
                    }
                    break;
                case LEVELS:

                    //Draws the background
                    spriteBatch.Draw(backgroundMonkeyImg, backgroundMonkeyRec, Color.White);

                    //Draws the levels title
                    spriteBatch.DrawString(titleFont, pickLevel, pickLevelLoc, Color.Black);

                    //Draws the level rectangles
                    spriteBatch.Draw(levelBackground, levelRecs[0], Color.Orange);
                    spriteBatch.Draw(levelBackground, levelRecs[1], Color.Orange);
                    spriteBatch.Draw(levelBackground, levelRecs[2], Color.Orange);
                    spriteBatch.Draw(levelBackground, levelRecs[3], Color.Orange);
                    spriteBatch.Draw(levelBackground, levelRecs[4], Color.Orange);

                    //Draws the words that are on the level rectangles
                    spriteBatch.DrawString(messageFont, "Level 1", levelLocs[0], Color.Black);
                    spriteBatch.DrawString(messageFont, "Level 2", levelLocs[1], Color.Black);
                    spriteBatch.DrawString(messageFont, "Level 3", levelLocs[2], Color.Black);
                    spriteBatch.DrawString(messageFont, "Level 4", levelLocs[3], Color.Black);
                    spriteBatch.DrawString(messageFont, "Level 5", levelLocs[4], Color.Black);

                    //Checks if the previous best score for that level is less than the level requirement
                    if (prevLevelScores[0] < levelRequirement)
                    {
                        //Draws locks wherever there is levels not reached
                        spriteBatch.Draw(lockImg, lockRecs[0], Color.White);
                        spriteBatch.Draw(lockImg, lockRecs[1], Color.White);
                        spriteBatch.Draw(lockImg, lockRecs[2], Color.White);
                        spriteBatch.Draw(lockImg, lockRecs[3], Color.White);
                    }
                    else if (prevLevelScores[1] < levelRequirement)
                    {
                        //Draws locks wherever there is levels not reached
                        spriteBatch.Draw(lockImg, lockRecs[1], Color.White);
                        spriteBatch.Draw(lockImg, lockRecs[2], Color.White);
                        spriteBatch.Draw(lockImg, lockRecs[3], Color.White);
                    }
                    else if (prevLevelScores[2] < levelRequirement)
                    {
                        //Draws locks wherever there is levels not reached
                        spriteBatch.Draw(lockImg, lockRecs[2], Color.White);
                        spriteBatch.Draw(lockImg, lockRecs[3], Color.White);
                    }
                    else if (prevLevelScores[3] < levelRequirement)
                    {
                        //Draws locks wherever there is levels not reached
                        spriteBatch.Draw(lockImg, lockRecs[3], Color.White);
                    }

                    //Draws the back button
                    spriteBatch.Draw(backImg, backRec, Color.White);
                                       
                    break;
                case INSTRUCTIONS:

                    //Draws the landscape of the instructions
                    spriteBatch.Draw(backgroundLandscapeImg, backgroundLandscapeRec, Color.Yellow);

                    //Draws the parchment where the instructions are written
                    spriteBatch.Draw(parchmentImg, parchmentRec, Color.White);

                    //Draws the instructions title
                    spriteBatch.DrawString(titleFont, instructionsTitleText, instructionsTitleLoc, Color.Black);

                    //Draws the instructions in order from line to line
                    spriteBatch.DrawString(messageFont, instructionsText[0], instructionsLocs[0], Color.Black);
                    spriteBatch.DrawString(messageFont, instructionsText[1], instructionsLocs[1], Color.Black);
                    spriteBatch.DrawString(messageFont, instructionsText[2], instructionsLocs[2], Color.Black);
                    spriteBatch.DrawString(messageFont, instructionsText[3], instructionsLocs[3], Color.Black);
                    spriteBatch.DrawString(messageFont, instructionsText[4], instructionsLocs[4], Color.Black);
                    spriteBatch.DrawString(messageFont, instructionsText[5], instructionsLocs[5], Color.Black);

                    //Draws the back button
                    spriteBatch.Draw(backImg, backRec, Color.White);

                    break;
                case GAMEPLAY:

                    //Draws the progress bar
                    spriteBatch.Draw(progressEmpty, progressEmptyRec, Color.White);
                    spriteBatch.Draw(progressFull, progressFullRec, Color.White);

                    //Draws the background of the game
                    spriteBatch.Draw(backgroundLandscapeImg, backgroundLandscapeRec, Color.Yellow);

                    //Draws all the trees that need to be in the level including one extra
                    for (int i = 0; i < level + 1; i++)
                    {
                        //Draws the amount of trees
                        spriteBatch.Draw(tree, treeRecs[i], Color.White);
                    }

                    //Draws the monkey to the screen if the game just started
                    if (lastLetter.Equals(' '))
                    {
                        //Draws the monkey
                        spriteBatch.Draw(walks[1], monkeyRec, Color.White);
                    }

                    //Draws the monkey depending on the key pressed
                    if (kb.IsKeyDown(Keys.D))
                    {
                        //Draws the monkey when moving right
                        spriteBatch.Draw(walks[image], monkeyRec, Color.White);
                    }
                    else if (kb.IsKeyDown(Keys.A))
                    {
                        //Draws the monkey when moving left
                        spriteBatch.Draw(leftWalks[image], monkeyRec, Color.White);
                    }
                    else if (kb.IsKeyDown(Keys.W))
                    {
                        //Draws the monkey when moving up
                        spriteBatch.Draw(climbs[climbImage], monkeyRec, Color.White);
                    }
                    else if (kb.IsKeyDown(Keys.S))
                    {
                        //Draws the monkey when moving down
                        spriteBatch.Draw(climbs[climbImage], monkeyRec, Color.White);
                    }
                    else
                    {
                        //Tracks the last letter pressed
                        if (lastLetter.Equals('d'))
                        {
                            //Draws the monkey when moving right
                            spriteBatch.Draw(walks[5], monkeyRec, Color.White);
                        }
                        else if (lastLetter.Equals('a'))
                        {
                            //Draws the monkey when moving left
                            spriteBatch.Draw(leftWalks[5], monkeyRec, Color.White);
                        }
                        else if (lastLetter.Equals('w'))
                        {
                            //Draws the monkey when moving up
                            spriteBatch.Draw(climbs[2], monkeyRec, Color.White);
                        }
                        else if (lastLetter.Equals('s'))
                        {
                            //Draws the monkey when moving down
                            spriteBatch.Draw(climbs[2], monkeyRec, Color.White);
                        }
                    }

                    //Draws the platforms
                    spriteBatch.Draw(platform, platformRecs[0], Color.Pink);
                    spriteBatch.Draw(platform, platformRecs[1], Color.Pink);

                    //Draws the banana
                    spriteBatch.Draw(bananaImg, bananaRec, Color.White);

                    //Draws the time remaining text 
                    spriteBatch.DrawString(messageFont, "Time Remaining: " + timeRemaining, timeRemainingLoc, Color.Black);

                    //Draws the amount of bananas that have collided with the platforms
                    spriteBatch.DrawString(messageFont, "Score: " + basketBananas, scoreLocs[0], Color.Black);

                    break;
                case PAUSE:
                    break;
                case ENDGAME:

                    //Draws the background
                    spriteBatch.Draw(backgroundLandscapeImg, backgroundLandscapeRec, Color.Yellow);

                    //Draws the title after the game
                    spriteBatch.DrawString(titleFont, "GAME OVER", gameOverTitle, Color.Black);

                    //Draws the amount of bananas that collided with platforms
                    spriteBatch.DrawString(betweenFont, "Score: " + basketBananas, scoreLocs[1], Color.Black);

                    //Draws the menu button
                    spriteBatch.Draw(menuImg, menuRec, Color.White);

                    break;
            }

            //Finishes the drawing
            spriteBatch.End();

            //Draws all code above that is in draw
            base.Draw(gameTime);
        }

        //Pre: A monkey has something to do with climbing and a tree
        //Post: Return the monkey rectangle
        //Desc: Move monkey up on the tree when the user hits the 'u' key
        private static Rectangle MoveUp(Rectangle treeRec, int climbLength, Rectangle monkeyRec, ref int climbImage)
        {
            //Sets the speed
            const int speed = 5;

            //Determines whether the monkey is on the trunk of the tree or in the tree
            if (monkeyRec.X >= treeRec.X + treeRec.Width / 2 - 50 && monkeyRec.X + monkeyRec.Width <= treeRec.X + treeRec.Width / 2 + 50)
            {
                //Adds one to the climb image
                climbImage++;

                //Repeats the climb image if it exceeds the limit
                if (climbImage >= climbLength)
                {
                    //Resets the climb image to 0
                    climbImage = 0;
                }

                //Allows user to move up if they are on the screen
                if (monkeyRec.Y >= 0)
                {
                    //Changes the y value if the user is on the screen
                    monkeyRec.Y -= speed;
                }

                //Acts as a boundary
                if (monkeyRec.Y <= 100)
                {
                    //Sets the y value to never become less than 100
                    monkeyRec.Y = 100;
                }
            }
            else if (monkeyRec.Y <= 400 && monkeyRec.X > treeRec.X + 50 && monkeyRec.X <= treeRec.X + treeRec.Width - 100)
            {
                //Adds one to the climb image
                climbImage++;

                //Repeats the climb image if it exceeds that value
                if (climbImage >= climbLength)
                {
                    //Resets the climb image to 0
                    climbImage = 0;
                }
                
                //Allows user to move up if they are on the screen
                if (monkeyRec.Y >= 0)
                {
                    //Changes the y value if the user is on the screen
                    monkeyRec.Y -= speed;
                }

                if (monkeyRec.Y <= 100)
                {
                    //Sets the y value to never become less than 100
                    monkeyRec.Y = 100;
                }
            }

            //Returns the rectangle of the monkey
            return monkeyRec;
        }

        //Pre: A monkey has something to do with climbing and a tree
        //Post: Return the monkey rectangle
        //Desc: Move monkey down on the tree when the user hits the 's' key
        private static Rectangle MoveDown(Rectangle treeRec, int climbLength, Rectangle monkeyRec, ref int climbImage, int screenHeight)
        {
            //Sets the speed
            const int speed = 5;

            //Determines whether the monkey is on the trunk of the tree or in the tree
            if (monkeyRec.X >= treeRec.X + treeRec.Width / 2 - 50 && monkeyRec.X + monkeyRec.Width <= treeRec.X + treeRec.Width / 2 + 50)
            {
                //Subtracts one from the climb image
                climbImage--;

                //Repeats the climb image if it goes under 0
                if (climbImage <= climbLength)
                {
                    //Sets the climb image to 2
                    climbImage = 2;
                }

                //Allows the user to move down if the monkey is at the lowest part of the screen
                if (monkeyRec.Y <= screenHeight - monkeyRec.Height)
                {
                    //Decreases the y value 
                    monkeyRec.Y += speed;
                }
            }
            else if (treeRec.Contains(monkeyRec) && monkeyRec.Y <= 325)
            {
                //Subtracts one from the climb image
                climbImage--;

                //Repeats the climb image if it goes under 0
                if (climbImage <= climbLength)
                {
                    //Sets the climb image to 2
                    climbImage = 2;
                }

                //Allows the user to move down if the monkey is at the lowest part of the screen
                if (monkeyRec.Y <= screenHeight - monkeyRec.Height)
                {
                    //Decreases the y value
                    monkeyRec.Y += speed;
                }
            }

            //Returns the rectangle of the monkey
            return monkeyRec;
        }


        //Pre: A monkey has something to do with climbing and a tree
        //Post: Return the monkey rectangle
        //Desc: Move monkey right on the tree when the user hits the 'd' key
        private static Rectangle MoveRight(Rectangle treeRec, int walkLength, Rectangle monkeyRec, ref int image, int screenWidth, char lastLetter)
        {
            //Checks to see whether is on the ground or in the tree
            if ((monkeyRec.Y >= 525 && monkeyRec.Y <= 600) || treeRec.Contains(monkeyRec) == true)
            {
                //Sets the speed
                const int speed = 5;

                //Adds one from the right walk image 
                image++;

                //Repeats the right walk image if it exceeds the limit
                if (image >= walkLength)
                {
                    //Sets the right walk image to 0
                    image = 0;
                }

                //Checks to see if the monkey is on the tree
                if (monkeyRec.Y <= 375)
                {
                    //Sets the right boundary
                    if (monkeyRec.X < treeRec.X + treeRec.Width - monkeyRec.Width - 50)
                    {
                        //Makes the monkey move to the right
                        monkeyRec.X += speed;
                    }
                }
                else
                {
                    //Limits monkey to not go over the screen
                    if (monkeyRec.X <= screenWidth - monkeyRec.Width)
                    {
                        //Makes the monkey move to the right
                        monkeyRec.X += speed;
                    }
                }
            }

            //Checks to see if the monkey is in the tree trunk depending on the y value
            if (monkeyRec.Y <= 526 && monkeyRec.Y >= 376)
            {
                //Checks to see if the monkey is out of the tree trunk depending on the x value
                if (monkeyRec.X > treeRec.X + treeRec.Width / 2)
                {                    
                    //Puts the monkey in the tree trunk
                    monkeyRec.X = treeRec.X + treeRec.Width / 2;                    
                }
            }

            //Returns the rectangle for the monkey
            return monkeyRec;
        }


        //Pre: A monkey has something to do with climbing and a tree
        //Post: Return the monkey rectangle
        //Desc: Move monkey left on the tree when the user hits the 'a' key
        private static Rectangle MoveLeft(Rectangle treeRec, int walkLength, Rectangle monkeyRec, ref int image, int screenWidth, char lastLetter)
        {
            //Sets the speed
            const int speed = 5;

            //Adds one from the left walk image
            image++;

            //Repeats the left walk image if it goes below 0
            if (image >= walkLength)
            {
                //Sets the image to 0
                image = 0;
            }
                       
            //Checks to see whether the monkey is on the ground or in the tree
            if ((monkeyRec.Y >= 525 && monkeyRec.Y <= 600) || (treeRec.Contains(monkeyRec)))
            {
                //Checks to see if the monkey is on the tree
                if (monkeyRec.Y <= 375)
                {
                    //Checks to see if the monkey is out of the tree trunk depending on the x value 
                    if (monkeyRec.X >= treeRec.X + 60)
                    {
                        //Checks to see if the monkey is outside of the screen
                        if (monkeyRec.X >= 0)
                        {
                            //Allows the monkey to move left
                            monkeyRec.X -= speed;
                        }
                    }
                }
                else
                {
                    //Checks to see if the monkey is outside of the screen
                    if (monkeyRec.X >= 0)
                    {
                        //Allows the monkey to move left
                        monkeyRec.X -= speed;
                    }
                }
            }

            //Checks to see whether the monkey is on a tree trunk
            if (monkeyRec.Y <= 526 && monkeyRec.Y >= 376)
            {
                //Checks to see whether the x value of it goes under the minimum value
                if (monkeyRec.X < treeRec.X + treeRec.Width / 2 - 50)
                {
                    //Sets the monkey rectangle to the tree trunk
                    monkeyRec.X = treeRec.X + treeRec.Width / 2 - 50;
                }
            }

            //Returns the rectangle of the monkey
            return monkeyRec;
        }


        //Pre: The monkey has something to do with the banana
        //Post: Returns the banana rectangle
        //Desc: Moves banana with the monkey
        private static Rectangle PickupBanana(Rectangle monkeyRec, Texture2D bananaImg, Rectangle bananaRec, char lastLetter)
        {
            //Checks to see whether the monkey and banana collided
            if (monkeyRec.Contains(bananaRec))
            {
                //Checks to see what the last letter was
                if (lastLetter == 'a')
                {
                    //Sets the banana rectangle coordinates equal to the monkey's
                    bananaRec = new Rectangle(monkeyRec.X, monkeyRec.Y, bananaImg.Width / 9, bananaImg.Height / 9);
                }
                else if (lastLetter == 'd')
                {
                    ////Sets the banana rectangle coordinates equal to the monkey's
                    bananaRec = new Rectangle(monkeyRec.X + monkeyRec.Width / 4, monkeyRec.Y + 10, bananaImg.Width / 9, bananaImg.Height / 9);
                }
            }

            //Returns the banana rectangle 
            return bananaRec;
        }

        //Pre: The monkey uses the throw timer to throw the banana
        //Post: Returns the throw timer
        //Desc: Activates the timer for throwing purposes
        private static Timer ActivateThrowTimer(Timer throwTimer, Rectangle bananaRec, Rectangle monkeyRec)
        {
            //Checks to see whether the monkey collides with the banana
            if (monkeyRec.Contains(bananaRec))
            {
                //Activates the throw timer if it is not active
                if (throwTimer.IsInactive())
                {
                    //Activates the throw timer
                    throwTimer.Activate();
                }
            }
            
            //Returns the throw timer
            return throwTimer;
        }

        //Pre: The monkey uses many different components to throw the banana
        //Post: Returns the banana rectangle
        //Desc: Moves the banana's coordinates when thrown
        private static Rectangle ThrowBanana(Texture2D bananaImg, Rectangle bananaRec, char lastLetter, Timer throwTimer, int throwImage, int throwsLength, double throwTime, bool throwRight)
        {
            //Sets the velocity
            const int velocity = 5;

            //Declares the x and y values when thrown
            int thrownX = 0;
            int thrownY = 0;

            //Checks to see whether the timer is activated
            if (throwTimer.IsActive())
            {
                //Checks to see if the banana was thrown facing right
                if (throwRight == true)
                {
                    //Calculates the x value when being thrown
                    thrownX = Convert.ToInt32(velocity * throwTime) + bananaRec.X;
                }

                //Checks to see if the banana was thrown facing left
                if (throwRight == false)
                {
                    //Calculates the x value when being thrown
                    thrownX = Convert.ToInt32(-velocity * throwTime) + bananaRec.X;
                }

                //Calculates the y value when being thrown
                thrownY = Convert.ToInt32(GRAVITY * Math.Pow(throwTime, 2) / 2) + bananaRec.Y;

                //Uses the calculations to use as the banana's coordinates 
                bananaRec = new Rectangle(thrownX, thrownY, bananaImg.Width / 9, bananaImg.Height / 9);
            }

            //Returns the banana
            return bananaRec;
        }

        //Pre: Sets bananas in trees when thrown
        //Post: Returns the banana rectangle
        //Desc: Randomizes the banana's location
        private static Point RandomizeBanana(Timer throwTimer, Rectangle treeRec)
        {
            //Declares the location of the banana rec
            Point bananaRec = new Point(0, 0);

            //Sets up the random number generator
            Random rng = new Random();

            //Resets the timer, although not immidiately
            throwTimer.ResetTimer(false);

            //Calculates the x and y random values
            bananaRec.X = rng.Next(treeRec.X + 100, treeRec.X + treeRec.Width - 100);
            bananaRec.Y = rng.Next(treeRec.Y + 100, treeRec.Y + treeRec.Height / 2);

            //Returns the banana rectangle 
            return bananaRec;
        }
    }
}
