//Author: Ryan Simmons
//File Name: Monopoly.cs
//Project Name: Monopoly
//Creation Date: May 11, 2017
//Modification Date: June 12, 2017
//Description: A digitalization of the board game Monopoly.
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Monopoly
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Monopoly : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Variables for screen resolution
        int screenWidth;
        int screenHeight;

        //Variables for Game States
        int GameState;
        const int TITLE = 0;
        const int PLAY = 1;
        const int BUY_PROPERTY = 2;
        const int INSUFFICIENT_FUNDS = 3;
        const int CHARGE_RENT = 4;
        const int INCOME_TAX = 5;
        const int LUXURY_TAX = 6;
        const int CARD_SPACE = 7;
        const int SELL_PROPERTY = 8;
        const int MORTGAGE = 9;
        const int SETTLE_DEBT = 10;
        const int WIN = 11;

        //Variables for Title Screen
        Texture2D titleBackground;
        Rectangle titleBackgroundBox;
        Texture2D logo;
        Rectangle logoBox;
        Texture2D playButton;
        Rectangle playButtonBox;
        Texture2D exitButton;
        Rectangle exitButtonBox;

        //Variables for Gameplay Drawing
        Texture2D board;
        Rectangle boardBox;
        Rectangle currentPlayerSpaceBox;
        Vector2 currentPlayerTextLoc;
        SpriteFont miscFont;
        Texture2D rollButton;
        Rectangle rollButtonBox;
        SpriteFont rollFont;

        //Variables for mouse input
        MouseState mouse;
        MouseState prevMouse;

        //Variable for the current turn
        int currentPlayer;

        //Variables for player pieces
        Texture2D player1;
        Texture2D player2;
        Texture2D player3;
        Texture2D player4;

        //Variables for Rolling
        int roll1;
        int roll2;
        Random rng;
        Vector2 rollDisplayLoc;
        Vector2 rollDisplayTextLoc;
        int displayRollTimer;
        bool rollMode;
        string rollMessage;
        bool doubles;
        string rollString;
        int totalRoll;
        int prevRoll;

        //Variables for Sprite Sheet Animation
        Texture2D dice;
        Texture2D fireworks;
        int[] animImgW = new int[2];
        int[] animImgH = new int[2];
        Rectangle[] animSrcRec = new Rectangle[2];
        Rectangle[] animDestRec = new Rectangle[2];
        int[] animImgFramesWide = new int[] { 6, 6 };
        int[] animImgFramesHigh = new int[] { 1, 4 };
        int[] animImgTotalFrames = new int[] { 6, 23 };
        int[] animImgFrameNum = new int[2];
        Vector2[] animNewSrc = new Vector2[2];
        bool[] playAnimation = new bool[2];
        int[] animSmoothness = new int[] { 6, 6 };
        int[] animDelayCount = new int[2];

        //Variables for Sound Effects
        SoundEffect rollDice;
        SoundEffectInstance rollDiceInstance;
        SoundEffect moveSound;
        SoundEffectInstance moveSoundInstance;
        SoundEffect cardSound;
        SoundEffectInstance cardSoundInstance;
        SoundEffect buySellSound;
        SoundEffectInstance buySellSoundInstance;
        SoundEffect winSound;
        SoundEffectInstance winSoundInstance;
        int winSoundTimer;

        //Rectangle Array for Player Pieces
        Rectangle[] pieceLocs = new Rectangle[4];

        //Variables for moving around the board
        int edgeToSpace;
        int diameterBigSpace;
        int diameterSmallSpace;
        int bigToSmallMove;
        int smallToSmallMove;
        int moveDelay;
        int moveSmoothness = 13;

        //Array for the current space a player is on
        int[] currentSpace = new int[4];

        //Constant for number of players
        const int NUMPLAYERS = 4;
        
        //Constant for number of spaces
        const int NUMSPACES = 40;

        //Variables for Property Properties
        int[] spacePrice = new int[40];
        int[] spaceRent = new int[40];
        int[] spaceOwnership = new int[40];

        //Array for player money
        int[] wallet = new int[4];

        //Location for showing property information when buying a property
        Vector2 buyPropertyInfoLoc;

        //Font used when buying property
        SpriteFont buyPropertyFont;

        //Buying Property Buttons Variables
        Texture2D yesButton;
        Texture2D noButton;
        Texture2D okayButton;
        Rectangle leftButtonBox;
        Rectangle rightButtonBox;
        Rectangle okayButtonBox;

        //Money info location
        Vector2 moneyInfoLoc;

        //Buttons for charging income and luxury tax
        Texture2D twoHundredButton;
        Texture2D tenPercentButton;

        //Boolean array for determining if a particular player is in jail
        bool[] isInJail = new bool[4];

        //Vector2 for location of message displayed when a player is in jail
        Vector2 jailMessageLoc;

        //Variables for Pay Fine Button
        Texture2D payFineButton;
        Rectangle payFineButtonBox;

        //Counts the amount of times a player has rolled doubles
        int doublesCount;

        //String for displaying message on chance or community chest card
        string cardMessage;

        //Array for previous space
        int[] previousSpace = new int[4];

        //Variable for storing what type of card space player is currently on (used for drawing info on screen)
        string cardType;

        //Variable that is increased every update to ensure card subprogram is only ran once per card draw
        int cardTimer;

        //Int for storing card value
        int randomCard;

        //Variables for houses and hotels
        int[] numHouses = new int[40];
        Rectangle[] houseBox = new Rectangle[40];
        Texture2D buyHouseButton;
        Rectangle buyHouseButtonBox;
        int verticalHouseMove = 35;
        int horizontalHouseMove = 50;
        Texture2D house;
        Texture2D hotel;
        int[] totalHouses = new int[4];
        int[] totalHotels = new int[4];

        //Array of Vector2 variables to determine the locations of specific spaces on the board.
        Vector2[] spaceLocs = new Vector2[40];

        //Variables for selling property
        Texture2D sellIcon;
        Texture2D sellPropertyButton;
        Rectangle sellPropertyButtonBox;
        Texture2D backButton;
        int stage;
        Rectangle[] sellIconLocs = new Rectangle[40];
        int toBeSold;
        int playerToSellTo;
        string sellAmount;
        bool insufficientFundsToSell;
        int insufficientFundsToSellTimer;
        Vector2 insufficientFundsToSellLoc;

        //Variables for getting keyboard state
        KeyboardState kb;
        KeyboardState prevKb;

        //Timer for Utilities (making sure the dice is only rolled once)
        int utilitiesTimer;

        //Mortgaging Properties Variables
        double[] mortgageValues = new double[40];
        Texture2D mortgageButton;
        Rectangle mortgageButtonBox;

        //Variables for settling debt
        int chargeAmount;
        bool isInDebt;
        int playerInDebt;
        int receivingPlayer;
        bool toTheBank;

        //Array for determining the number of properties a player owns
        int[] numProperties = new int[4];

        //Aray for determining whether a player has been eliminated from the game or not
        bool[] eliminated = new bool[4];

        //Counts the number of players that have been eliminated
        int eliminatedCount;

        //Variable that stores the winning player
        int winningPlayer;

        //Boolean for determining if the game has been won or not
        bool finished;

        //Variable for background music
        Song backgroundMusic;

        public Monopoly()
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
            // TODO: Add your initialization logic here
            this.graphics.PreferredBackBufferWidth = 1500;
            this.graphics.PreferredBackBufferHeight = 844;

            this.graphics.ApplyChanges();

            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;

            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //Variables for Title Screen
            titleBackground = Content.Load<Texture2D>("Images/Backgrounds/Title Screen");
            titleBackgroundBox = new Rectangle(0, 0, screenWidth, screenHeight);
            logo = Content.Load<Texture2D>("Images/Backgrounds/Logo");
            logoBox = new Rectangle(125, -50, 1300, 700);
            playButton = Content.Load<Texture2D>("Images/Sprites/Play Button");
            playButtonBox = new Rectangle(440, 500, 300, 100);
            exitButton = Content.Load<Texture2D>("Images/Sprites/Exit Button");
            exitButtonBox = new Rectangle((playButtonBox.X + playButtonBox.Width + 50), 500, 300, 100);

            //Variables for Gameplay drawing
            board = Content.Load<Texture2D>("Images/Backgrounds/Board");
            boardBox = new Rectangle((int)(screenWidth / 4.5), 0, screenHeight, screenHeight);
            currentPlayerSpaceBox = new Rectangle(50, 625, 200, 200);
            currentPlayerTextLoc = new Vector2(currentPlayerSpaceBox.X, currentPlayerSpaceBox.Y - 20);
            miscFont = Content.Load<SpriteFont>("Fonts/MiscFont");
            rollButton = Content.Load<Texture2D>("Images/Sprites/Roll Button");
            rollButtonBox = new Rectangle((boardBox.X + boardBox.Width) + 45, 75, 240, 80);
            rollFont = Content.Load<SpriteFont>("Fonts/RollFont");

            //Variables for player pieces
            player1 = Content.Load<Texture2D>("Images/Sprites/Player 1");
            player2 = Content.Load<Texture2D>("Images/Sprites/Player 2");
            player3 = Content.Load<Texture2D>("Images/Sprites/Player 3");
            player4 = Content.Load<Texture2D>("Images/Sprites/Player 4");

            //Variables for Rolling
            rng = new Random();
            rollDisplayLoc = new Vector2(50, 100);
            rollDisplayTextLoc = new Vector2(rollDisplayLoc.X, rollDisplayLoc.Y - 20);


            //Variables for Sprite Sheet Animation
            dice = Content.Load<Texture2D>("Images/Sprites/Dice");
            animImgW[0] = dice.Width / animImgFramesWide[0];
            animImgH[0] = dice.Height / animImgFramesHigh[0];
            animSrcRec[0] = new Rectangle(0, 0, animImgW[0], animImgH[0]);
            animDestRec[0] = new Rectangle(550, 200, 400, 400);
            fireworks = Content.Load<Texture2D>("Images/Sprites/Fireworks");
            animImgW[1] = fireworks.Width / animImgFramesWide[1];
            animImgH[1] = fireworks.Height / animImgFramesHigh[1];
            animSrcRec[1] = new Rectangle(0, 0, animImgW[1], animImgH[1]);
            animDestRec[1] = new Rectangle(550, 200, 400, 400);

            //Variables for Sound Effects
            rollDice = Content.Load<SoundEffect>("Audio/Sound Effects/Roll Dice");
            rollDiceInstance = rollDice.CreateInstance();
            rollDiceInstance.Volume = 0.8f;
            moveSound = Content.Load<SoundEffect>("Audio/Sound Effects/Move");
            moveSoundInstance = moveSound.CreateInstance();
            moveSoundInstance.Volume = 0.7f;
            cardSound = Content.Load<SoundEffect>("Audio/Sound Effects/Card Sound");
            cardSoundInstance = cardSound.CreateInstance();
            cardSoundInstance.Volume = 0.7f;
            buySellSound = Content.Load<SoundEffect>("Audio/Sound Effects/Buy Sell Sound");
            buySellSoundInstance = buySellSound.CreateInstance();
            buySellSoundInstance.Volume = 0.7f;
            winSound = Content.Load<SoundEffect>("Audio/Sound Effects/Win Sound");
            winSoundInstance = winSound.CreateInstance();
            winSoundInstance.Volume = 0.8f;

            //Rectangle Array for Player Pieces
            pieceLocs[0] = new Rectangle(boardBox.X + 767, 767, 50, 50);
            pieceLocs[1] = new Rectangle(boardBox.X + 767, 767, 50, 50);
            pieceLocs[2] = new Rectangle(boardBox.X + 767, 767, 50, 50);
            pieceLocs[3] = new Rectangle(boardBox.X + 767, 767, 50, 50);

            //Dimensions of the board for moving around the board
            edgeToSpace = 732;
            diameterBigSpace = 111;
            diameterSmallSpace = 67;
            bigToSmallMove = 88;
            smallToSmallMove = 70;

            //Space Prices
            spacePrice[1] = 60;
            spacePrice[3] = 60;
            spacePrice[6] = 100;
            spacePrice[8] = 100;
            spacePrice[9] = 120;
            spacePrice[11] = 140;
            spacePrice[13] = 140;
            spacePrice[14] = 160;
            spacePrice[16] = 180;
            spacePrice[18] = 180;
            spacePrice[19] = 200;
            spacePrice[21] = 220;
            spacePrice[23] = 220;
            spacePrice[24] = 240;
            spacePrice[26] = 260;
            spacePrice[27] = 260;
            spacePrice[29] = 280;
            spacePrice[31] = 300;
            spacePrice[32] = 300;
            spacePrice[34] = 320;
            spacePrice[37] = 350;
            spacePrice[39] = 400;
            spacePrice[12] = 150;
            spacePrice[28] = 150;
            spacePrice[5] = 200;
            spacePrice[15] = 200;
            spacePrice[25] = 200;
            spacePrice[35] = 200;

            //Space Rents
            spaceRent[1] = 2;
            spaceRent[3] = 4;
            spaceRent[6] = 6;
            spaceRent[8] = 6;
            spaceRent[9] = 8;
            spaceRent[11] = 10;
            spaceRent[13] = 10;
            spaceRent[14] = 12;
            spaceRent[16] = 14;
            spaceRent[18] = 14;
            spaceRent[19] = 16;
            spaceRent[21] = 18;
            spaceRent[23] = 18;
            spaceRent[24] = 20;
            spaceRent[26] = 22;
            spaceRent[27] = 22;
            spaceRent[29] = 24;
            spaceRent[31] = 26;
            spaceRent[32] = 26;
            spaceRent[34] = 28;
            spaceRent[37] = 35;
            spaceRent[39] = 50;
            //Space Ownership (-1 indicates unowned, 4 indicates non-purchasable space, 0 - 3 indicates player ownership)
            for (int i = 0; i < 40; i++)
            {
                spaceOwnership[i] = -1;
            }
            spaceOwnership[0] = 4;
            spaceOwnership[2] = 4;
            spaceOwnership[4] = 4;
            spaceOwnership[7] = 4;
            spaceOwnership[10] = 4;
            spaceOwnership[17] = 4;
            spaceOwnership[20] = 4;
            spaceOwnership[22] = 4;
            spaceOwnership[30] = 4;
            spaceOwnership[33] = 4;
            spaceOwnership[36] = 4;
            spaceOwnership[38] = 4;
            //Array for Player Money
            for (int i = 0; i < 4; i++)
            {
                wallet[i] = 1500;
            }

            //Location for showing property information when buying a property
            buyPropertyInfoLoc = new Vector2(100, 100);

            //Font used for Buying Property
            buyPropertyFont = Content.Load<SpriteFont>("Fonts/BuyPropertyFont");

            //Buying Property Buttons Variables
            yesButton = Content.Load<Texture2D>("Images/Sprites/Yes Button");
            noButton = Content.Load<Texture2D>("Images/Sprites/No Button");
            okayButton = Content.Load<Texture2D>("Images/Sprites/Okay Button");
            leftButtonBox = new Rectangle(440, 500, 300, 100);
            rightButtonBox = new Rectangle((leftButtonBox.X + leftButtonBox.Width + 50), 500, 300, 100);
            okayButtonBox = new Rectangle(600, 625, 300, 100);

            //Location for Money Info
            moneyInfoLoc = new Vector2((boardBox.X + boardBox.Width) + 25, 700);

            //Buttons for charging income and luxury tax
            twoHundredButton = Content.Load<Texture2D>("Images/Sprites/$200 Button");
            tenPercentButton = Content.Load<Texture2D>("Images/Sprites/10% Button");

            //Variables for Pay Fine Button
            payFineButton = Content.Load<Texture2D>("Images/Sprites/Pay Fine Button");
            payFineButtonBox = new Rectangle(rollButtonBox.X, (rollButtonBox.Y + rollButtonBox.Height) + 15, rollButtonBox.Width, rollButtonBox.Height);

            //Location of message displayed when a player is in jail
            jailMessageLoc = new Vector2(payFineButtonBox.X, (payFineButtonBox.Y + payFineButtonBox.Height) + 30);

            //Variables for houses and hotels
            buyHouseButton = Content.Load<Texture2D>("Images/Sprites/Buy House Button");
            buyHouseButtonBox = new Rectangle(rollButtonBox.X, (rollButtonBox.Y + rollButtonBox.Height) + 15, rollButtonBox.Width, rollButtonBox.Height);
            for (int i = 0; i < 40; i++)
            {
                houseBox[i] = new Rectangle(0, 0, 30, 30);
            }
            house = Content.Load<Texture2D>("Images/Sprites/House");
            hotel = Content.Load<Texture2D>("Images/Sprites/Hotel");

            //Determine the locations of each space on the board
            DetermineSpaceLocations();

            //Variables for selling property
            sellIcon = Content.Load<Texture2D>("Images/Sprites/Sell Icon");
            sellPropertyButton = Content.Load<Texture2D>("Images/Sprites/Sell Property Button");
            sellPropertyButtonBox = new Rectangle((int)currentPlayerTextLoc.X - 35, (int)currentPlayerTextLoc.Y - 115, 300, 100);
            backButton = Content.Load<Texture2D>("Images/Sprites/Back Button");
            for (int i = 0; i < 40; i++)
            {
                sellIconLocs[i] = new Rectangle(0, 0, 50, 50);
            }
            insufficientFundsToSellLoc = new Vector2(buyPropertyInfoLoc.X, buyPropertyInfoLoc.Y + 200);

            //Mortaging property variables
            for (int i = 0; i < 40; i++)
            {
                mortgageValues[i] = spacePrice[i] * 0.75;
            }
            mortgageButton = Content.Load<Texture2D>("Images/Sprites/Mortgage Button");
            mortgageButtonBox = new Rectangle(sellPropertyButtonBox.X, (sellPropertyButtonBox.Y - sellPropertyButtonBox.Height - 15), sellPropertyButtonBox.Width, sellPropertyButtonBox.Height);

            //Variable for background music
            backgroundMusic = Content.Load<Song>("Audio/Music/Background Music");
            MediaPlayer.Volume = 0.4f;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            prevMouse = mouse;
            mouse = Mouse.GetState();
            prevKb = kb;
            kb = Keyboard.GetState();
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(backgroundMusic);
            }
            switch (GameState)
            {
                case TITLE:
                    TitleUpdate();
                    break;

                case PLAY:
                    previousSpace[currentPlayer] = currentSpace[currentPlayer];
                    UpdateRollCode();
                    //If the current player is in jail
                    if (isInJail[currentPlayer] == true)
                    {
                        //If the player has rolled doubles, and the message displaying as such has dissapeared
                        if (rollMode == true && displayRollTimer >= 240 && roll1 == roll2)
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            roll1 = 0;
                            roll2 = 0;
                            totalRoll = 0;
                            doubles = false;
                            rollMode = false;
                            isInJail[currentPlayer] = false;
                            doublesCount = 0;
                            DetermineNextPlayer();
                            displayRollTimer = 0;
                            rollMode = false;
                        }
                        else if (rollMode == true && displayRollTimer >= 120 && roll1 != roll2)
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            roll1 = 0;
                            roll2 = 0;
                            totalRoll = 0;
                            doublesCount = 0;
                            rollMode = false;
                            DetermineNextPlayer();
                            displayRollTimer = 0;
                            rollMode = false;
                        }
                        else if (totalRoll == 0 && WasMouseClicked(payFineButtonBox) == true && wallet[currentPlayer] >= 50)
                        {
                            wallet[currentPlayer] -= 50;
                            isInJail[currentPlayer] = false;
                            moveDelay = 0;
                            prevRoll = 0;
                            roll1 = 0;
                            roll2 = 0;
                            totalRoll = 0;
                            doublesCount = 0;
                            displayRollTimer = 0;
                            DetermineNextPlayer();
                            displayRollTimer = 0;
                            rollMode = false;
                        }
                        else if (totalRoll == 0 && WasMouseClicked(payFineButtonBox) == true && wallet[currentPlayer] < 50)
                        {
                            roll1 = 0;
                            roll2 = 0;
                            totalRoll = 0;
                            displayRollTimer = 0;
                            GameState = INSUFFICIENT_FUNDS;
                        }
                    }
                    else
                    {
                        NormalTurn();
                    }
                    break;
                //Code for buying property
                case BUY_PROPERTY:
                    if (WasMouseClicked(leftButtonBox) == true)
                    {
                        if (wallet[currentPlayer] < spacePrice[currentSpace[currentPlayer]])
                        {
                            GameState = INSUFFICIENT_FUNDS;
                        }
                        else
                        {
                            wallet[currentPlayer] -= spacePrice[currentSpace[currentPlayer]];
                            spaceOwnership[currentSpace[currentPlayer]] = currentPlayer;
                            numProperties[currentPlayer]++;
                            if (doubles == true)
                            {
                                moveDelay = 0;
                                prevRoll = 0;
                                doubles = false;
                            }
                            else
                            {
                                moveDelay = 0;
                                prevRoll = 0;
                                DetermineNextPlayer();
                                displayRollTimer = 0;
                                rollMode = false;
                                doublesCount = 0;
                            }
                            if (finished == false)
                            {
                                buySellSoundInstance.Play();
                                GameState = PLAY;
                            }
                        }
                    }
                    if (WasMouseClicked(rightButtonBox) == true)
                    {
                        if (doubles == true)
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            doubles = false;
                        }
                        else
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            DetermineNextPlayer();
                            displayRollTimer = 0;
                            rollMode = false;
                        }
                        if (finished == false)
                        {
                            GameState = PLAY;
                        }
                    }
                    break;
                //Code to tell a player that they do not have enough money
                case INSUFFICIENT_FUNDS:
                    if (WasMouseClicked(okayButtonBox) == true)
                    {
                        if (doubles == true)
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            doubles = false;
                        }
                        else
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            DetermineNextPlayer();
                            displayRollTimer = 0;
                            rollMode = false;
                            doublesCount = 0;
                        }
                        if (finished == false)
                        {
                            GameState = PLAY;
                        }
                    }
                    break;
                //Code for charging rent
                case CHARGE_RENT:
                    spaceRent[5] = RailwayRent();
                    spaceRent[15] = RailwayRent();
                    spaceRent[25] = RailwayRent();
                    spaceRent[35] = RailwayRent();
                    if (utilitiesTimer == 0)
                    {
                        spaceRent[12] = UtilitiesRent();
                        spaceRent[28] = UtilitiesRent();
                    }
                    utilitiesTimer++;
                    if (WasMouseClicked(okayButtonBox) == true)
                    {
                        CheckForDebt(currentPlayer, DetermineRent(), spaceOwnership[currentSpace[currentPlayer]]);
                        utilitiesTimer = 0;
                        if (doubles == true)
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            doubles = false;
                        }
                        else
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            DetermineNextPlayer();
                            displayRollTimer = 0;
                            rollMode = false;
                            doublesCount = 0;
                        }
                        if (isInDebt == false && finished == false)
                        {
                            GameState = PLAY;
                        }
                    }
                    break;
                //Code for charging income tax
                case INCOME_TAX:
                    if (WasMouseClicked(leftButtonBox) == true)
                    {
                        CheckForDebt(currentPlayer, 200, 4);
                        if (doubles == true)
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            doubles = false;
                        }
                        else
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            DetermineNextPlayer();
                            displayRollTimer = 0;
                            rollMode = false;
                            doublesCount = 0;
                        }
                        if (isInDebt == false && finished == false)
                        {
                            GameState = PLAY;
                        }
                    }

                    if (WasMouseClicked(rightButtonBox) == true)
                    {
                        CheckForDebt(currentPlayer, Convert.ToInt32(wallet[currentPlayer] * 0.10), 4);
                        if (doubles == true)
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            doubles = false;
                        }
                        else
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            DetermineNextPlayer();
                            displayRollTimer = 0;
                            rollMode = false;
                            doublesCount = 0;
                        }
                        if (isInDebt == false && finished == false)
                        {
                            GameState = PLAY;
                        }
                    }
                    break;
                //Code for charging luxury tax
                case LUXURY_TAX:
                    if (WasMouseClicked(okayButtonBox) == true)
                    {
                        CheckForDebt(currentPlayer, 100, 4);
                        if (doubles == true)
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            doubles = false;
                        }
                        else
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            DetermineNextPlayer();
                            displayRollTimer = 0;
                            rollMode = false;
                            doublesCount = 0;
                        }
                        if (isInDebt == false && finished == false)
                        {
                            GameState = PLAY;
                        }
                    }
                    break;
                //Code for cards
                case CARD_SPACE:
                    if (currentSpace[currentPlayer] == 2 || currentSpace[currentPlayer] == 17 || currentSpace[currentPlayer] == 33)
                    {
                        CommunityChest();
                        cardType = "Community Chest";
                    }
                    else
                    {
                        Chance();
                        cardType = "Chance";
                    }
                    cardTimer++;
                    if (WasMouseClicked(okayButtonBox) == true)
                    {
                        cardTimer = 0;
                        if (spaceOwnership[currentSpace[currentPlayer]] == -1)
                        {
                            GameState = BUY_PROPERTY;
                        }
                        else if (spaceOwnership[currentSpace[currentPlayer]] != -1 && spaceOwnership[currentSpace[currentPlayer]] != 4 && spaceOwnership[currentSpace[currentPlayer]] != 5 && spaceOwnership[currentSpace[currentPlayer]] != currentPlayer)
                        {
                            GameState = CHARGE_RENT;
                        }
                        else
                        {
                            moveDelay = 0;
                            prevRoll = 0;
                            if (doubles == true)
                            {
                                moveDelay = 0;
                                prevRoll = 0;
                                doubles = false;
                            }
                            else
                            {
                                moveDelay = 0;
                                prevRoll = 0;
                                DetermineNextPlayer();
                                displayRollTimer = 0;
                                rollMode = false;
                                doublesCount = 0;
                            }
                            if (isInDebt == false && finished == false)
                            {
                                GameState = PLAY;
                            }
                            cardSoundInstance.Play();
                        }
                    }
                    break;
                //Code for selling property
                case SELL_PROPERTY:
                    if (WasMouseClicked(sellPropertyButtonBox) == true)
                    {
                        if (isInDebt == true)
                        {
                            GameState = SETTLE_DEBT;
                        }
                        else
                        {
                            GameState = PLAY;
                        }
                        stage = 0;
                    }
                    if (stage == 0)
                    {
                        for (int i = 0; i < 40; i++)
                        {
                            if (WasMouseClicked(sellIconLocs[i]) == true)
                            {
                                toBeSold = i;
                                stage = 1;
                            }
                        }
                    }
                    else if (stage == 1)
                    {
                        if (DidKeyboardPress(Keys.D1) == true)
                        {
                            playerToSellTo = 0;
                            stage = 2;
                        }
                        if (DidKeyboardPress(Keys.D2) == true)
                        {
                            playerToSellTo = 1;
                            stage = 2;
                        }
                        if (DidKeyboardPress(Keys.D3) == true)
                        {
                            playerToSellTo = 2;
                            stage = 2;
                        }
                        if (DidKeyboardPress(Keys.D4) == true)
                        {
                            playerToSellTo = 3;
                            stage = 2;
                        }
                    }
                    else if (stage == 2)
                    {
                        //Determine sell price
                        if (DidKeyboardPress(Keys.D1) == true)
                        {
                            sellAmount += "1";
                        }
                        if (DidKeyboardPress(Keys.D2) == true)
                        {
                            sellAmount += "2";
                        }
                        if (DidKeyboardPress(Keys.D3) == true)
                        {
                            sellAmount += "3";
                        }
                        if (DidKeyboardPress(Keys.D4) == true)
                        {
                            sellAmount += "4";
                        }
                        if (DidKeyboardPress(Keys.D5) == true)
                        {
                            sellAmount += "5";
                        }
                        if (DidKeyboardPress(Keys.D6) == true)
                        {
                            sellAmount += "6";
                        }
                        if (DidKeyboardPress(Keys.D7) == true)
                        {
                            sellAmount += "7";
                        }
                        if (DidKeyboardPress(Keys.D8) == true)
                        {
                            sellAmount += "8";
                        }
                        if (DidKeyboardPress(Keys.D9) == true)
                        {
                            sellAmount += "9";
                        }
                        if (DidKeyboardPress(Keys.D0) == true)
                        {
                            sellAmount += "0";
                        }
                        if (DidKeyboardPress(Keys.Back) == true && sellAmount.Length != 0)
                        {
                            sellAmount = sellAmount.Substring(0, sellAmount.Length - 1);
                        }
                        if (DidKeyboardPress(Keys.Enter) == true && sellAmount.Length != 0)
                        {
                            //If the seller cannot afford the sell price tell them so
                            if (Convert.ToInt32(sellAmount) > wallet[playerToSellTo])
                            {
                                insufficientFundsToSell = true;
                            }
                            else
                            {
                                spaceOwnership[toBeSold] = playerToSellTo;
                                wallet[playerToSellTo] -= Convert.ToInt32(sellAmount);
                                wallet[DeterminePlayer()] += Convert.ToInt32(sellAmount);
                                numProperties[DeterminePlayer()]--;
                                buySellSoundInstance.Play();
                                sellAmount = "";
                                stage = 0;
                                //Checks if player is in debt and sets values accordingly
                                if (isInDebt == true)
                                {
                                    if (wallet[playerInDebt] >= chargeAmount)
                                    {
                                        wallet[playerInDebt] -= chargeAmount;
                                        if (toTheBank == false)
                                        {
                                            wallet[receivingPlayer] += chargeAmount;
                                        }
                                        isInDebt = false;
                                        if (finished == false)
                                        {
                                            GameState = PLAY;
                                        }
                                    }
                                    else
                                    {
                                        GameState = SETTLE_DEBT;
                                    }
                                }
                                else
                                {
                                    if (finished == false)
                                    {
                                        GameState = PLAY;
                                    }
                                }
                            }
                        }
                        if (insufficientFundsToSell == true)
                        {
                            if (insufficientFundsToSellTimer >= 120)
                            {
                                insufficientFundsToSell = false;
                                insufficientFundsToSellTimer = 0;
                            }
                            insufficientFundsToSellTimer++;
                        }
                    }
                    break;
                //Code for mortgaging property
                case MORTGAGE:
                    if (WasMouseClicked(sellPropertyButtonBox) == true)
                    {
                        if (isInDebt == true)
                        {
                            GameState = SETTLE_DEBT;
                        }
                        else
                        {
                            if (finished == false)
                            {
                                GameState = PLAY;
                            }
                        }
                    }
                    for (int i = 0; i < 40; i++)
                    {
                        if (WasMouseClicked(sellIconLocs[i]) == true)
                        {
                            wallet[DeterminePlayer()] += Convert.ToInt32(mortgageValues[i]);
                            spaceOwnership[i] = 5;
                            numProperties[DeterminePlayer()]--;
                            //Checks if player is in debt and sets values accordingly
                            if (isInDebt == true)
                            {
                                if (wallet[playerInDebt] >= chargeAmount)
                                {
                                    wallet[playerInDebt] -= chargeAmount;
                                    if (toTheBank == false)
                                    {
                                        wallet[receivingPlayer] += chargeAmount;
                                    }
                                    isInDebt = false;
                                    if (finished == false)
                                    {
                                        GameState = PLAY;
                                    }
                                }
                                else
                                {
                                    GameState = SETTLE_DEBT;
                                }
                            }
                            else
                            {
                                if (finished == false)
                                {
                                    GameState = PLAY;
                                }
                            }
                        }
                    }
                    break;
                //Code for deciding what to do when debt is settled
                case SETTLE_DEBT:
                    if (numProperties[playerInDebt] == 0)
                    {
                        eliminated[playerInDebt] = true;
                        eliminatedCount++;
                        if (WasMouseClicked(okayButtonBox) == true)
                        {
                            if (toTheBank == false)
                            {
                                wallet[receivingPlayer] += wallet[playerInDebt];
                            }
                            isInDebt = false;
                            //If three players are eliminated, end the game
                            if (eliminatedCount >= 3)
                            {
                                GameState = WIN;
                                winningPlayer = currentPlayer;
                                finished = true;
                            }
                            if (finished == false)
                            {
                                GameState = PLAY;
                            }
                        }
                    }
                    else
                    {
                        if (WasMouseClicked(leftButtonBox) == true)
                        {
                            GameState = SELL_PROPERTY;
                        }
                        if (WasMouseClicked(rightButtonBox) == true)
                        {
                            GameState = MORTGAGE;
                        }
                    }
                    break;
                case WIN:
                    if (winSoundTimer == 0)
                    {
                        winSoundInstance.Play();
                    }
                    winSoundTimer++;

                    //Update animation for fireworks
                    if (animDelayCount[1] == 0)
                    {
                        //Update source dimensions
                        animNewSrc[1].X = (animImgFrameNum[1] % animImgFramesWide[1]) * animImgW[1];
                        animNewSrc[1].Y = (animImgFrameNum[1] / animImgFramesWide[1]) * animImgH[1];

                        //Update source rectangle
                        animSrcRec[1] = new Rectangle(Convert.ToInt32(animNewSrc[1].X), Convert.ToInt32(animNewSrc[1].Y), animImgW[1], animImgH[1]);

                        //Update Frame Number
                        animImgFrameNum[1] = (animImgFrameNum[1] + 1) % animImgTotalFrames[1];
                    }
                    //Increase smoothness count
                    animDelayCount[1] = (animDelayCount[1] + 1) % animSmoothness[1];

                    if (WasMouseClicked(okayButtonBox) == true)
                    {
                        GameState = TITLE;
                        currentPlayer = 0;
                        roll1 = 0;
                        roll2 = 0;
                        displayRollTimer = 0;
                        rollMode = false;
                        rollMessage = "";
                        doubles = false;
                        rollString = "";
                        totalRoll = 0;
                        prevRoll = 0;
                        playAnimation[0] = false;
                        animDelayCount[0] = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            currentSpace[i] = 0;
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            spaceOwnership[i] = -1;
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            wallet[i] = 1500;
                        }
                        doublesCount = 0;
                        cardTimer = 0;
                        for (int i = 0; i < 40; i++)
                        {
                            numHouses[i] = 0;
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            totalHouses[i] = 0;
                            totalHotels[i] = 0;
                        }
                        stage = 0;
                        sellAmount = "";
                        insufficientFundsToSell = false;
                        insufficientFundsToSellTimer = 0;
                        utilitiesTimer = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            numProperties[i] = 0;
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            eliminated[i] = false;
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            pieceLocs[i] = new Rectangle(boardBox.X + 767, 767, 50, 50);
                        }
                        finished = false;
                        winSoundTimer = 0;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.Black);
            switch (GameState)
            {
                case TITLE:
                    GraphicsDevice.Clear(Color.Black);
                    TitleDraw();
                    break;

                case PLAY:
                    GraphicsDevice.Clear(Color.Black);
                    //Draw code for Gameplay
                    spriteBatch.Draw(board, boardBox, Color.White);

                    //Current Player Section Drawing Code
                    spriteBatch.Draw(determineCurrentPlayerForDrawing(currentPlayer), currentPlayerSpaceBox, Color.White);
                    spriteBatch.DrawString(miscFont, "Current Piece:", currentPlayerTextLoc, Color.White);

                    //Draw "Sell Property" Button
                    if (DetectCollision(sellPropertyButtonBox) == true)
                    {
                        spriteBatch.Draw(sellPropertyButton, sellPropertyButtonBox, Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(sellPropertyButton, sellPropertyButtonBox, Color.White);
                    }

                    //Draw "Mortgage" Button
                    if (DetectCollision(mortgageButtonBox) == true)
                    {
                        spriteBatch.Draw(mortgageButton, mortgageButtonBox, Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(mortgageButton, mortgageButtonBox, Color.White);
                    }

                    //Detect if mouse hovered over roll button
                    if (DetectCollision(rollButtonBox) == true)
                    {
                        spriteBatch.Draw(rollButton, rollButtonBox, Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(rollButton, rollButtonBox, Color.White);
                    }

                    //Draw Players
                    for (int i = 0; i < 4; i++)
                    {
                        if (eliminated[i] == false)
                        {
                            spriteBatch.Draw(determineCurrentPlayerForDrawing(i), pieceLocs[i], Color.White);
                        }
                    }

                    //Rolling Dice Drawing
                    if (playAnimation[0] == true)
                    {
                        spriteBatch.Draw(dice, animDestRec[0], animSrcRec[0], Color.White);
                    }
                    if (rollMode == true)
                    {
                        if (rollString != "0")
                        {
                        spriteBatch.DrawString(miscFont, rollMessage, rollDisplayTextLoc, Color.White);
                        if (displayRollTimer < 120)
                        {
                            spriteBatch.DrawString(rollFont, rollString, rollDisplayLoc, Color.White);
                        }
                        }
                    }

                    //Money Section Drawing
                    spriteBatch.DrawString(miscFont, "Money:\n\n  Car: $" + wallet[0] + "\n  Hat: $" + wallet[1] + "\n  Iron: $" + wallet[2] + "\n  Shoe: $" + wallet[3], moneyInfoLoc, Color.White);

                    //If a player is in jail, draw the jail message and pay fine button on screen
                    if (isInJail[currentPlayer] == true)
                    {
                        if (DetectCollision(payFineButtonBox) == true)
                        {
                            spriteBatch.Draw(payFineButton, payFineButtonBox, Color.Lime);
                        }
                        else
                        {
                            spriteBatch.Draw(payFineButton, payFineButtonBox, Color.White);
                        }
                        spriteBatch.DrawString(miscFont, "The current player is in\njail. To escape jail, they\nmust either roll doubles\nor pay a $50 fine.", jailMessageLoc, Color.White);
                    }

                    //If a player is on a space owned by themselves that is not a railway or utility, enable the option to buy a house for that property
                    if (spaceOwnership[currentSpace[currentPlayer]] == currentPlayer && currentSpace[currentPlayer] != 5 && currentSpace[currentPlayer] != 15 && currentSpace[currentPlayer] != 25 && currentSpace[currentPlayer] != 35 && currentSpace[currentPlayer] != 12 && currentSpace[currentPlayer] != 28)
                    {
                        if (DetectCollision(buyHouseButtonBox) == true)
                        {
                            spriteBatch.Draw(buyHouseButton, buyHouseButtonBox, Color.Lime);
                        }
                        else
                        {
                            spriteBatch.Draw(buyHouseButton, buyHouseButtonBox, Color.White);
                        }
                    }
                    //If there are houses or a hotel on a property, draw accordingly
                    for (int i = 0; i < 40; i++)
                    {
                        if (numHouses[i] > 0)
                        {
                            spriteBatch.Draw(HouseOrHotel(i), houseBox[i], WhatColour(i));
                        }
                    }
                    break;
                case BUY_PROPERTY:
                    GraphicsDevice.Clear(Color.AntiqueWhite);
                    //Draw code for Buying Property
                    spriteBatch.DrawString(buyPropertyFont, "This Property is Available for Purchase.\nWould You Like To Buy It?\n\nPrice: $" + spacePrice[currentSpace[currentPlayer]] + "\nRent: " + DetermineRentString(), buyPropertyInfoLoc, Color.Black);
                    if (DetectCollision(leftButtonBox) == true)
                    {
                        spriteBatch.Draw(yesButton, leftButtonBox, Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(yesButton, leftButtonBox, Color.White);
                    }

                    if (DetectCollision(rightButtonBox) == true)
                    {
                        spriteBatch.Draw(noButton, rightButtonBox, Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(noButton, rightButtonBox, Color.White);
                    }
                    //Money Section Drawing
                    spriteBatch.DrawString(miscFont, "Money:\n\n  Car: $" + wallet[0] + "\n  Hat: $" + wallet[1] + "\n  Iron: $" + wallet[2] + "\n  Shoe: $" + wallet[3], moneyInfoLoc, Color.Black);
                    break;
                case INSUFFICIENT_FUNDS:
                    GraphicsDevice.Clear(Color.DarkRed);
                    //Draw Code for when the player doesn't have enough money
                    spriteBatch.DrawString(buyPropertyFont, "Insufficient Funds", buyPropertyInfoLoc, Color.White);

                    if (DetectCollision(okayButtonBox) == true)
                    {
                        spriteBatch.Draw(okayButton, okayButtonBox, Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(okayButton, okayButtonBox, Color.White);
                    }
                    break;
                case CHARGE_RENT:
                    GraphicsDevice.Clear(Color.DarkRed);
                    //Draw code for when the player is being charged rent
                    spriteBatch.DrawString(buyPropertyFont, "This property is owned by another player.\nYou must pay $" + DetermineRent() + " in rent.", buyPropertyInfoLoc, Color.White);
                    if (DetectCollision(okayButtonBox) == true)
                    {
                        spriteBatch.Draw(okayButton, okayButtonBox, Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(okayButton, okayButtonBox, Color.White);
                    }
                    break;
                case INCOME_TAX:
                    GraphicsDevice.Clear(Color.DarkBlue);
                    spriteBatch.DrawString(buyPropertyFont, "You must pay income tax.\nYou can either pay $200 or 10% of all your money.\nWhich do you choose?", buyPropertyInfoLoc, Color.White);
                    if (DetectCollision(leftButtonBox) == true)
                    {
                        spriteBatch.Draw(twoHundredButton, leftButtonBox, Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(twoHundredButton, leftButtonBox, Color.White);
                    }

                    if (DetectCollision(rightButtonBox) == true)
                    {
                        spriteBatch.Draw(tenPercentButton, rightButtonBox, Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(tenPercentButton, rightButtonBox, Color.White);
                    }
                    //Money Section Drawing
                    spriteBatch.DrawString(miscFont, "Money:\n\n  Car: $" + wallet[0] + "\n  Hat: $" + wallet[1] + "\n  Iron: $" + wallet[2] + "\n  Shoe: $" + wallet[3], moneyInfoLoc, Color.White);
                    break;
                case LUXURY_TAX:
                    GraphicsDevice.Clear(Color.DarkBlue);
                    spriteBatch.DrawString(buyPropertyFont, "You must pay luxury tax.\nYou must pay $100.", buyPropertyInfoLoc, Color.White);
                    if (DetectCollision(okayButtonBox) == true)
                    {
                        spriteBatch.Draw(okayButton, okayButtonBox, Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(okayButton, okayButtonBox, Color.White);
                    }
                    break;
                case CARD_SPACE:
                    GraphicsDevice.Clear(Color.AntiqueWhite);
                    //Draw what the card is going to do on screen so that the user knows
                    spriteBatch.DrawString(buyPropertyFont, cardType + ":\n\n" + cardMessage, buyPropertyInfoLoc, Color.Black);
                    if (DetectCollision(okayButtonBox) == true)
                    {
                        spriteBatch.Draw(okayButton, okayButtonBox, Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(okayButton, okayButtonBox, Color.White);
                    }
                    break;
                case SELL_PROPERTY:
                    GraphicsDevice.Clear(Color.DarkGreen);
                    //Money Section Drawing
                    spriteBatch.DrawString(miscFont, "Money:\n\n  Car: $" + wallet[0] + "\n  Hat: $" + wallet[1] + "\n  Iron: $" + wallet[2] + "\n  Shoe: $" + wallet[3], moneyInfoLoc, Color.White);
                    if (stage == 0)
                    {
                        //Draw "Back" Button
                        if (DetectCollision(sellPropertyButtonBox) == true)
                        {
                            spriteBatch.Draw(backButton, sellPropertyButtonBox, Color.Lime);
                        }
                        else
                        {
                            spriteBatch.Draw(backButton, sellPropertyButtonBox, Color.White);
                        }
                        spriteBatch.Draw(board, boardBox, Color.White);
                        spriteBatch.DrawString(miscFont, "First, select the property\nyou want to sell.", jailMessageLoc, Color.White);
                        for (int i = 0; i < 40; i++)
                        {
                            if (spaceOwnership[i] == DeterminePlayer())
                            {
                                sellIconLocs[i].X = (int)spaceLocs[i].X;
                                sellIconLocs[i].Y = (int)spaceLocs[i].Y;
                                spriteBatch.Draw(sellIcon, sellIconLocs[i], Color.White);
                            }
                        }
                    }
                    else if (stage == 1)
                    {
                        //Draw "Back" Button
                        if (DetectCollision(sellPropertyButtonBox) == true)
                        {
                            spriteBatch.Draw(backButton, sellPropertyButtonBox, Color.Lime);
                        }
                        else
                        {
                            spriteBatch.Draw(backButton, sellPropertyButtonBox, Color.White);
                        }
                        spriteBatch.Draw(board, boardBox, Color.White);
                        spriteBatch.DrawString(miscFont, "Now, press the number\non the keyboard of the\nplayer you wish\nto sell to:\n(1 = Car, 2 = Hat, 3 = Iron,\n4 = Shoe)", jailMessageLoc, Color.White);
                    }
                    else if (stage == 2)
                    {
                        //Draw "Back" Button
                        if (DetectCollision(okayButtonBox) == true)
                        {
                            spriteBatch.Draw(backButton, okayButtonBox, Color.Lime);
                        }
                        else
                        {
                            spriteBatch.Draw(backButton, okayButtonBox, Color.White);
                        }
                        spriteBatch.DrawString(buyPropertyFont, "Finally, enter the amount the buyer is willing\nto pay the seller and press enter:\n$ " + sellAmount, buyPropertyInfoLoc, Color.White);
                        //Draw message if seller does not have enough money
                        if (insufficientFundsToSell == true)
                        {
                            spriteBatch.DrawString(buyPropertyFont, "Insufficient Funds", insufficientFundsToSellLoc, Color.White);
                        }
                    }
                    break;
                case MORTGAGE:
                    GraphicsDevice.Clear(Color.DarkGreen);
                    //Draw "Back" Button
                    if (DetectCollision(sellPropertyButtonBox) == true)
                    {
                        spriteBatch.Draw(backButton, sellPropertyButtonBox, Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(backButton, sellPropertyButtonBox, Color.White);
                    }
                    spriteBatch.Draw(board, boardBox, Color.White);
                    spriteBatch.DrawString(miscFont, "Select the property\nyou want to mortgage.", jailMessageLoc, Color.White);
                    for (int i = 0; i < 40; i++)
                    {
                        if (spaceOwnership[i] == DeterminePlayer())
                        {
                            sellIconLocs[i].X = (int)spaceLocs[i].X;
                            sellIconLocs[i].Y = (int)spaceLocs[i].Y;
                            spriteBatch.Draw(sellIcon, sellIconLocs[i], Color.White);
                        }
                    }
                    break;
                case SETTLE_DEBT:
                    GraphicsDevice.Clear(Color.DarkRed);
                    if (numProperties[playerInDebt] == 0)
                    {
                        spriteBatch.DrawString(buyPropertyFont, DeterminePieceFromPlayer(playerInDebt) + " has no property to sell or mortgage, and is in debt.\nAs such, they are eliminated from the game.", buyPropertyInfoLoc, Color.White);
                        if (DetectCollision(okayButtonBox) == true)
                        {
                            spriteBatch.Draw(okayButton, okayButtonBox, Color.Lime);
                        }
                        else
                        {
                            spriteBatch.Draw(okayButton, okayButtonBox, Color.White);
                        }
                    }
                    else
                    {
                        //Draw Message
                        spriteBatch.DrawString(buyPropertyFont, DeterminePieceFromPlayer(playerInDebt) + " cannot afford to pay the $" + chargeAmount + " required\nand as such is in debt. To pay off this debt, " + DeterminePieceFromPlayer(playerInDebt) + " can either\nsell their properties to another player or mortgage their properties.\nWhich do they choose?:", buyPropertyInfoLoc, Color.White);
                        //Draw "Sell Property" Button
                        if (DetectCollision(leftButtonBox) == true)
                        {
                            spriteBatch.Draw(sellPropertyButton, leftButtonBox, Color.Lime);
                        }
                        else
                        {
                            spriteBatch.Draw(sellPropertyButton, leftButtonBox, Color.White);
                        }
                        //Draw "Mortgage" Button
                        if (DetectCollision(rightButtonBox) == true)
                        {
                            spriteBatch.Draw(mortgageButton, rightButtonBox, Color.Lime);
                        }
                        else
                        {
                            spriteBatch.Draw(mortgageButton, rightButtonBox, Color.White);
                        }
                    }
                    break;
                case WIN:
                    GraphicsDevice.Clear(Color.Yellow);
                    spriteBatch.DrawString(buyPropertyFont, "Congratulations, all other players have been eliminated and as such " + DeterminePieceFromPlayer(winningPlayer) + " has won the game!", buyPropertyInfoLoc, Color.Black);
                    spriteBatch.Draw(fireworks, animDestRec[1], animSrcRec[1], Color.White);
                    if (DetectCollision(okayButtonBox) == true)
                    {
                        spriteBatch.Draw(okayButton, okayButtonBox, Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(okayButton, okayButtonBox, Color.White);
                    }
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Determines if the user hovered the mouse over the desired button
        /// </summary>
        /// <param name="button">A rectangle for an area that the user can hover over</param>
        /// <returns>Whether or not the user hovered over that area</returns>
        private bool DetectCollision(Rectangle button)
        {
            if (mouse.X >= button.X && mouse.X <= (button.X + button.Width) && mouse.Y >= button.Y && mouse.Y <= (button.Y + button.Height))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the mouse clicked the desired button
        /// </summary>
        /// <param name="button">A rectangle for a button that the user can click</param>
        /// <returns>Whether or not the user clicked the button</returns>
        private bool WasMouseClicked(Rectangle button)
        {
            //if mouse.Pressed && !prevMouse.Pressed
                //if mouse inside box
                    //return true

            //return false
            if (DetectCollision(button) == true)
            {
                if (mouse.LeftButton == ButtonState.Pressed && !(prevMouse.LeftButton == ButtonState.Pressed))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Code for update loop during the title screen
        /// </summary>
        private void TitleUpdate()
        {
            if (WasMouseClicked(playButtonBox) == true)
            {
                GameState = PLAY;
            }
            if (WasMouseClicked(exitButtonBox) == true)
            {
                this.Exit();
            }
        }

        /// <summary>
        /// Draw code for title screen
        /// </summary>
        private void TitleDraw()
        {
            spriteBatch.Draw(titleBackground, titleBackgroundBox, Color.White);
            spriteBatch.Draw(logo, logoBox, Color.White);
            if (DetectCollision(playButtonBox) == true)
            {
                spriteBatch.Draw(playButton, playButtonBox, Color.Lime);
            }
            else
            {
                spriteBatch.Draw(playButton, playButtonBox, Color.White);
            }

            if (DetectCollision(exitButtonBox) == true)
            {
                spriteBatch.Draw(exitButton, exitButtonBox, Color.Lime);
            }
            else
            {
                spriteBatch.Draw(exitButton, exitButtonBox, Color.White);
            }
        }

        /// <summary>
        /// Determines the current player and returns the corresponding sprite
        /// </summary>
        /// <param name="player">integer for the current player</param>
        /// <returns>The sprite of the current player</returns>
        private Texture2D determineCurrentPlayerForDrawing(int player)
        {
            if (player == 0)
            {
                return player1;
            }
            else if (player == 1)
            {
                return player2;
            }
            else if (player == 2)
            {
                return player3;
            }
            else
            {
                return player4;
            }
        }

        /// <summary>
        /// Moves a piece based on where it currently is on the board.
        /// </summary>
        private void MovePiece()
        {
            //Moving From GO
            if (pieceLocs[currentPlayer].X > (boardBox.X + edgeToSpace) && pieceLocs[currentPlayer].Y > edgeToSpace)
            {
                pieceLocs[currentPlayer].X -= bigToSmallMove;
            }
            //Moving Down the Right Hand Side of the Screen
            else if (pieceLocs[currentPlayer].X > (boardBox.X + edgeToSpace) && pieceLocs[currentPlayer].Y > (diameterBigSpace) && pieceLocs[currentPlayer].Y < (edgeToSpace - diameterSmallSpace))
            {
                pieceLocs[currentPlayer].Y += smallToSmallMove;
            }
            //Moving To GO
            else if (pieceLocs[currentPlayer].X > (boardBox.X + edgeToSpace) && pieceLocs[currentPlayer].Y > (edgeToSpace - diameterSmallSpace) && pieceLocs[currentPlayer].Y < edgeToSpace)
            {
                pieceLocs[currentPlayer].Y += bigToSmallMove;
            }
            //Moving From Go to Jail
            else if (pieceLocs[currentPlayer].X > (boardBox.X + edgeToSpace) && pieceLocs[currentPlayer].Y < diameterBigSpace)
            {
                pieceLocs[currentPlayer].Y += bigToSmallMove;
            }
            //Moving To Go to Jail
            else if (pieceLocs[currentPlayer].Y < diameterBigSpace && pieceLocs[currentPlayer].X > (boardBox.X + edgeToSpace - diameterSmallSpace) && pieceLocs[currentPlayer].X < (boardBox.X + edgeToSpace))
            {
                pieceLocs[currentPlayer].X += bigToSmallMove;
            }
            //Moving Along the Top of the Screen
            else if (pieceLocs[currentPlayer].X > (boardBox.X + diameterBigSpace) && pieceLocs[currentPlayer].X < (boardBox.X + edgeToSpace - diameterSmallSpace) && pieceLocs[currentPlayer].Y < diameterBigSpace)
            {
                pieceLocs[currentPlayer].X += smallToSmallMove;
            }
            //Moving From Free Parking
            else if (pieceLocs[currentPlayer].Y < diameterBigSpace && pieceLocs[currentPlayer].X < (boardBox.X + diameterBigSpace))
            {
                pieceLocs[currentPlayer].X += bigToSmallMove;
            }
            //Moving To Free Parking
            else if (pieceLocs[currentPlayer].X < (boardBox.X + diameterBigSpace) && pieceLocs[currentPlayer].Y > (diameterBigSpace) && pieceLocs[currentPlayer].Y < (diameterBigSpace + diameterSmallSpace))
            {
                pieceLocs[currentPlayer].Y -= bigToSmallMove;
            }
            //Moving Up the Left Side of the Screen
            else if (pieceLocs[currentPlayer].X < (boardBox.X + diameterBigSpace) && pieceLocs[currentPlayer].Y < edgeToSpace && pieceLocs[currentPlayer].Y > (diameterBigSpace + diameterSmallSpace))
            {
                pieceLocs[currentPlayer].Y -= smallToSmallMove;
            }
            //Moving From Jail (Just Visiting)
            else if (pieceLocs[currentPlayer].X < (boardBox.X + diameterBigSpace) && pieceLocs[currentPlayer].Y > edgeToSpace)
            {
                pieceLocs[currentPlayer].Y -= bigToSmallMove;
            }
            //Moving To Jail (Just Visiting)
            else if (pieceLocs[currentPlayer].X > (boardBox.X + diameterBigSpace) && pieceLocs[currentPlayer].X < (boardBox.X + diameterBigSpace + diameterSmallSpace) && pieceLocs[currentPlayer].Y > edgeToSpace)
            {
                pieceLocs[currentPlayer].X -= bigToSmallMove;
            }
            //Moving Along the Bottom of the Screen
            else if (pieceLocs[currentPlayer].X < (boardBox.X + edgeToSpace) && pieceLocs[currentPlayer].X > (boardBox.X + diameterBigSpace + diameterSmallSpace) && pieceLocs[currentPlayer].Y > edgeToSpace)
            {
                pieceLocs[currentPlayer].X -= smallToSmallMove;
            }
        }

        /// <summary>
        /// Update code for rolling the dice
        /// </summary>
        private void UpdateRollCode()
        {
            if (WasMouseClicked(rollButtonBox) == true && rollMode == false)
            {
                roll1 = rng.Next(1, 7);
                roll2 = rng.Next(1, 7);
                if (roll1 == roll2)
                {
                    doubles = true;
                    doublesCount++;
                    if (doublesCount >= 3)
                    {
                        GoToJail();
                        doublesCount = 0;
                        roll1 = 0;
                        roll2 = 0;
                        moveDelay = 0;
                        prevRoll = 0;
                        rollMessage = "";
                        DetermineNextPlayer();
                        displayRollTimer = 0;
                        rollMode = false;
                    }
                    else
                    {
                        totalRoll = roll1 + roll2;
                        rollString = Convert.ToString(roll1 + roll2);
                        playAnimation[0] = true;
                        displayRollTimer = 0;
                        rollDiceInstance.Play();
                    }
                }
                else
                {
                    totalRoll = roll1 + roll2;
                    rollString = Convert.ToString(roll1 + roll2);
                    playAnimation[0] = true;
                    displayRollTimer = 0;
                    rollDiceInstance.Play();
                }

            }

            //Play dice rolling animation
            if (playAnimation[0] == true)
            {
                if (animImgFrameNum[0] <= animImgTotalFrames[0])
                {
                    if (animDelayCount[0] == 0)
                    {
                        //Update source dimensions
                        animNewSrc[0].X = (animImgFrameNum[0] % animImgFramesWide[0]) * animImgW[0];
                        animNewSrc[0].Y = (animImgFrameNum[0] / animImgFramesWide[0]) * animImgH[0];

                        //Update source rectangle
                        animSrcRec[0] = new Rectangle(Convert.ToInt32(animNewSrc[0].X), Convert.ToInt32(animNewSrc[0].Y), animImgW[0], animImgH[0]);

                        //Update Frame Number
                        animImgFrameNum[0]++;
                    }
                    //Increase smoothness count
                    animDelayCount[0] = (animDelayCount[0] + 1) % animSmoothness[0];
                }
                else
                {
                    playAnimation[0] = false;
                    animImgFrameNum[0] = 0;
                    rollMode = true;
                }
            }
            //Display dice message
            if (displayRollTimer <= 120)
            {
                displayRollTimer++;
                rollMessage = DeterminePieceFromPlayer(currentPlayer) + " Has Just Rolled:";
            }
            else if (isInJail[currentPlayer] == true && displayRollTimer <= 240 && roll1 == roll2)
            {
                rollMessage = "Doubles were rolled.\nThe player is no longer\nin jail.";
                displayRollTimer++;
            }
            else if (displayRollTimer <= 240 && roll1 == roll2 && isInJail[currentPlayer] == false)
            {
                rollMessage = "Doubles Were Rolled.\nPlease Roll Again.";
                rollString = "";
                displayRollTimer++;
            }
            else
            {
                rollMode = false;
                rollMessage = "";
            }
        }

        /// <summary>
        /// Determines the rent when a player lands on an owned utility space.
        /// </summary>
        /// <returns>The rent the player will be charged</returns>
        private int UtilitiesRent()
        {   
            if (spaceOwnership[12] != -1 && spaceOwnership[28] != -1)
            {
                return (rng.Next(1, 13) * 10);
            }
            else
            {
                return (rng.Next(1, 13) * 4);
            }
        }

        /// <summary>
        /// Determines the rent if a player lands on railroad by determining which space they are on, exploring all possible combinations of railway ownership, and determining the rent accordingly.
        /// </summary>
        /// <returns>The rent the player is charged</returns>
        private int RailwayRent()
        {
            //If all are owned by the same player
            if (spaceOwnership[15] == spaceOwnership[5] && spaceOwnership[25] == spaceOwnership[5] && spaceOwnership[35] == spaceOwnership[5] && spaceOwnership[5] != -1)
            {
                return 200;
            }
            //If three are owned by the same player
            if (currentSpace[currentPlayer] == 5)
            {
                if ((spaceOwnership[5] == spaceOwnership[15] && spaceOwnership[5] == spaceOwnership[25]) || (spaceOwnership[5] == spaceOwnership[25] && spaceOwnership[5] == spaceOwnership[35]) || (spaceOwnership[5] == spaceOwnership[15] && spaceOwnership[5] == spaceOwnership[35]))
                {
                    return 100;
                }
            }
            else if (currentSpace[currentPlayer] == 15)
            {
                if ((spaceOwnership[5] == spaceOwnership[15] && spaceOwnership[5] == spaceOwnership[25]) || (spaceOwnership[5] == spaceOwnership[15] && spaceOwnership[5] == spaceOwnership[35]) || (spaceOwnership[15] == spaceOwnership[25] && spaceOwnership[15] == spaceOwnership[35]))
                {
                    return 100;
                }
            }
            else if (currentSpace[currentPlayer] == 25)
            {
                if ((spaceOwnership[5] == spaceOwnership[15] && spaceOwnership[5] == spaceOwnership[25]) || (spaceOwnership[5] == spaceOwnership[25] && spaceOwnership[5] == spaceOwnership[35]) || (spaceOwnership[15] == spaceOwnership[25] && spaceOwnership[15] == spaceOwnership[35]))
                {
                    return 100;
                }
            }
            else if (currentSpace[currentPlayer] == 35)
            {
                if ((spaceOwnership[5] == spaceOwnership[25] && spaceOwnership[5] == spaceOwnership[35]) || (spaceOwnership[5] == spaceOwnership[15] && spaceOwnership[5] == spaceOwnership[35]) || (spaceOwnership[15] == spaceOwnership[25] && spaceOwnership[15] == spaceOwnership[35]))
                {
                    return 100;
                }
            }
            //If two are owned by the same player
            if (currentSpace[currentPlayer] == 5)
            {
                if (spaceOwnership[5] == spaceOwnership[15] || spaceOwnership[5] == spaceOwnership[25] || spaceOwnership[5] == spaceOwnership[35])
                {
                    return 50;
                }
            }
            else if (currentSpace[currentPlayer] == 15)
            {
                if (spaceOwnership[5] == spaceOwnership[15] || spaceOwnership[15] == spaceOwnership[25] || spaceOwnership[15] == spaceOwnership[35])
                {
                    return 50;
                }
            }
            else if (currentSpace[currentPlayer] == 25)
            {
                if (spaceOwnership[5] == spaceOwnership[25] || spaceOwnership[15] == spaceOwnership[25] || spaceOwnership[25] == spaceOwnership[35])
                {
                    return 50;
                }
            }
            else if (currentSpace[currentPlayer] == 35)
            {
                if (spaceOwnership[5] == spaceOwnership[35] || spaceOwnership[15] == spaceOwnership[35] || spaceOwnership[25] == spaceOwnership[35])
                {
                    return 50;
                }
            }
            //If only one is owned by the same player
            return 25;
        }

        /// <summary>
        /// Determines what should be shown in the rent section when a player is buying a property
        /// </summary>
        /// <returns>What should be shown in the rent section</returns>
        private string DetermineRentString()
        {
            if (currentSpace[currentPlayer] == 12 || currentSpace[currentPlayer] == 28)
            {
                return "4 times roll of dice is one is owned, 10 times roll of dice if both are owned.";
            }
            else if (currentSpace[currentPlayer] == 5 || currentSpace[currentPlayer] == 15 || currentSpace[currentPlayer] == 25 || currentSpace[currentPlayer] == 35)
            {
                return "$25 if one owned, $50 if two owned, $100 if three owned,\n$200 if all owned by the same owner.";
            }
            else
            {
                return "$" + Convert.ToString(DetermineRent());
            }
        }

        /// <summary>
        /// Puts a player in jail if they land on "Go to Jail", roll doubles three times, or use a card that sends them to jail
        /// </summary>
        private void GoToJail()
        {
            /*pieceLocs[currentPlayer].X = (int)jail.X;
            pieceLocs[currentPlayer].Y = (int)jail.Y;
            currentSpace[currentPlayer] = 10;*/
            while (currentSpace[currentPlayer] != 10)
            {
                MovePiece();
                currentSpace[currentPlayer] = (currentSpace[currentPlayer] + 1) % NUMSPACES;
            }
            isInJail[currentPlayer] = true;
        }

        /// <summary>
        /// Update code that is executed every turn when a player is not in jail
        /// </summary>
        private void NormalTurn()
        {
            //If player clicks on the sell property button
            if (totalRoll == 0)
            {
                if (WasMouseClicked(sellPropertyButtonBox) == true)
                {
                    GameState = SELL_PROPERTY;
                }

                if (WasMouseClicked(mortgageButtonBox) == true)
                {
                    GameState = MORTGAGE;
                }
            }
            //If they are on a space owned by themselves that is not a railway or utility
            if (spaceOwnership[currentSpace[currentPlayer]] == currentPlayer && currentSpace[currentPlayer] != 5 && currentSpace[currentPlayer] != 15 && currentSpace[currentPlayer] != 25 && currentSpace[currentPlayer] != 35 && currentSpace[currentPlayer] != 12 && currentSpace[currentPlayer] != 28)
            {
                //If the player clicks on the buy house button
                if (WasMouseClicked(buyHouseButtonBox) == true && numHouses[currentSpace[currentPlayer]] < 4 && wallet[currentPlayer] >= CalcHousePrice(currentSpace[currentPlayer]))
                {
                    //Determine cost
                    wallet[currentPlayer] -= CalcHousePrice(currentSpace[currentPlayer]);
                    //If no houses are already on the property, determine the location of the first house
                    if (numHouses[currentSpace[currentPlayer]] == 0)
                    {
                        DetermineHouseLocation();
                    }
                    //Increase the variable for the number of houses on that property
                    numHouses[currentSpace[currentPlayer]]++;
                    if (numHouses[currentSpace[currentPlayer]] < 4)
                    {
                        totalHouses[currentPlayer]++;
                    }
                    else
                    {
                        totalHotels[currentPlayer]++;
                    }
                    buySellSoundInstance.Play();
                }
            }
            
            if (totalRoll > 0 && rollDiceInstance.State != SoundState.Playing)
            {
                if (moveDelay == 0)
                {
                    MovePiece();
                    prevRoll = totalRoll;
                    currentSpace[currentPlayer] = (currentSpace[currentPlayer] + 1) % NUMSPACES;
                    moveSoundInstance.Play();
                    if (currentSpace[currentPlayer] == 0)
                    {
                        wallet[currentPlayer] += 200;
                    }
                    totalRoll--;
                }
                moveDelay = (moveDelay + 1) % moveSmoothness;
            }
            else if (prevRoll > 0)
            {
                if (spaceOwnership[currentSpace[currentPlayer]] == -1)
                {
                    GameState = BUY_PROPERTY;
                }
                else if (spaceOwnership[currentSpace[currentPlayer]] != -1 && spaceOwnership[currentSpace[currentPlayer]] != 4 && spaceOwnership[currentSpace[currentPlayer]] != 5 && spaceOwnership[currentSpace[currentPlayer]] != currentPlayer)
                {
                    GameState = CHARGE_RENT;
                }
                else if (currentSpace[currentPlayer] == 4)
                {
                    GameState = INCOME_TAX;
                }
                else if (currentSpace[currentPlayer] == 38)
                {
                    GameState = LUXURY_TAX;
                }
                else if (currentSpace[currentPlayer] == 30)
                {
                    GoToJail();
                    moveDelay = 0;
                    prevRoll = 0;
                    DetermineNextPlayer();
                    displayRollTimer = 0;
                    rollMode = false;
                    doublesCount = 0;
                }
                else if (currentSpace[currentPlayer] == 2 || currentSpace[currentPlayer] == 7 || currentSpace[currentPlayer] == 17 || currentSpace[currentPlayer] == 22 || currentSpace[currentPlayer] == 33 || currentSpace[currentPlayer] == 36)
                {
                    GameState = CARD_SPACE;
                }
                else
                {
                    moveDelay = 0;
                    prevRoll = 0;
                    if (doubles == false)
                    {
                        DetermineNextPlayer();
                        displayRollTimer = 0;
                        rollMode = false;
                        doublesCount = 0;
                    }
                    else
                    {
                        doubles = false;
                    }
                }
            }
        }

        /// <summary>
        /// Randomly selects a community chest card, and executes it accordingly
        /// </summary>
        private void CommunityChest()
        {
            //Pick a card randomly
            if (cardTimer == 0)
            {
                randomCard = rng.Next(1, 17);
            }
            cardTimer++;

            //Execute code based on randomly generated number
            if (randomCard == 1)
            {
                cardMessage = "Advance to Go (Collect $200)";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    while (currentSpace[currentPlayer] != 0)
                    {
                        MovePiece();
                        currentSpace[currentPlayer] = (currentSpace[currentPlayer] + 1) % NUMSPACES;
                    }
                    wallet[currentPlayer] += 200;
                }
            }
            else if (randomCard == 2)
            {
                cardMessage = "Bank error in your favor, Collect $200";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    wallet[currentPlayer] += 200;
                }
            }
            else if (randomCard == 3)
            {
                cardMessage = "Doctor's fees, Pay $50";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    CheckForDebt(currentPlayer, 50, 4);
                }
            }
            else if (randomCard == 4)
            {
                cardMessage = "From sale of stock you get $50";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    wallet[currentPlayer] += 50;
                }
            }
            else if (randomCard == 5)
            {
                cardMessage = "Go to Jail, Go directly to jail, Do not pass Go- Do not collect $200";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    GoToJail();
                }
            }
            else if (randomCard == 6)
            {
                cardMessage = "Grand Opera Night, Collect $50 from every player for opening night seats";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    wallet[currentPlayer] += 150;
                    for (int i = 0; i < 4; i++)
                    {
                        if (i != currentPlayer)
                        {
                            CheckForDebt(i, 50, 4);
                        }
                    }
                }
            }
            else if (randomCard == 7)
            {
                cardMessage = "Holiday Fund matures, Receive $100";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    wallet[currentPlayer] += 100;
                }
            }
            else if (randomCard == 8)
            {
                cardMessage = "Income tax refund, Collect $20";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    wallet[currentPlayer] += 20;
                }
            }
            else if (randomCard == 9)
            {
                cardMessage = "It is your birthday, Collect $10 from each player";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    wallet[currentPlayer] += 30;
                    for (int i = 0; i < 4; i++)
                    {
                        if (i != currentPlayer)
                        {
                            CheckForDebt(i, 10, 4);
                        }
                    }
                }
            }
            else if (randomCard == 10)
            {
                cardMessage = "Life insurance matures, Collect $100";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    wallet[currentPlayer] += 100;
                }
            }
            else if (randomCard == 11)
            {
                cardMessage = "Pay hospital fees of $100";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    CheckForDebt(currentPlayer, 100, 4);
                }
            }
            else if (randomCard == 12)
            {
                cardMessage = "Pay school fees of $150";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    CheckForDebt(currentPlayer, 150, 4);
                }
            }
            else if (randomCard == 13)
            {
                cardMessage = "Receive $25 consultancy fee";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    wallet[currentPlayer] += 25;
                }
            }
            else if (randomCard == 14)
            {
                cardMessage = "You are assessed for street repairs, $40 per house - $115 per hotel";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    CheckForDebt(currentPlayer, totalHouses[currentPlayer] * 40, 4);
                    CheckForDebt(currentPlayer, totalHotels[currentPlayer] * 115, 4);
                }
            }
            else if (randomCard == 15)
            {
                cardMessage = "You have won second prize in a beauty contest, Collect $10";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    wallet[currentPlayer] += 10;
                }
            }
            else if (randomCard == 16)
            {
                cardMessage = "You inherit $100";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    wallet[currentPlayer] += 100;
                }
            }
        }

        /// <summary>
        /// Randomly selects a chance card and executes it accordingly
        /// </summary>
        private void Chance()
        {
            if (cardTimer == 0)
            {
                randomCard = rng.Next(1, 14);
            }
            cardTimer++;

            //Execute code based on randomly generated number
            if (randomCard == 1)
            {
                cardMessage = "Advance to Go (Collect $200)";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    while (currentSpace[currentPlayer] != 0)
                    {
                        MovePiece();
                        currentSpace[currentPlayer] = (currentSpace[currentPlayer] + 1) % NUMSPACES;
                    }
                    wallet[currentPlayer] += 200;
                }
            }
            else if (randomCard == 2)
            {
                cardMessage = "Advance to Illinois Ave. If you pass Go, collect $200";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    while (currentSpace[currentPlayer] != 24)
                    {
                        MovePiece();
                        currentSpace[currentPlayer] = (currentSpace[currentPlayer] + 1) % NUMSPACES;
                        if (currentSpace[currentPlayer] == 0)
                        {
                            wallet[currentPlayer] += 200;
                        }
                    }
                }
            }
            else if (randomCard == 3)
            {
                cardMessage = "Advance to St. Charles Place. If you pass Go, collect $200";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    while (currentSpace[currentPlayer] != 11)
                    {
                        MovePiece();
                        currentSpace[currentPlayer] = (currentSpace[currentPlayer] + 1) % NUMSPACES;
                        if (currentSpace[currentPlayer] == 0)
                        {
                            wallet[currentPlayer] += 200;
                        }
                    }
                }
            }
            else if (randomCard == 4)
            {
                cardMessage = "Advance token to nearest Utility. If unowned, you may buy it from the Bank.\nIf owned, throw dice and pay owner.";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    while (currentSpace[currentPlayer] != NearestUtility())
                    {
                        MovePiece();
                        currentSpace[currentPlayer] = (currentSpace[currentPlayer] + 1) % NUMSPACES;
                    }
                }
            }
            else if (randomCard == 5)
            {
                cardMessage = "Bank pays you dividend of $50";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    wallet[currentPlayer] += 50;
                }
            }
            else if (randomCard == 6)
            {
                cardMessage = "Go to Jail, Go directly to Jail, Do not pass Go, do not collect $200";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    GoToJail();
                }
            }
            else if (randomCard == 7)
            {
                cardMessage = "Make general repairs on all your property. For each house pay $25\nFor each hotel $100";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    CheckForDebt(currentPlayer, totalHouses[currentPlayer] * 25, 4);
                    CheckForDebt(currentPlayer, totalHotels[currentPlayer] * 100, 4);
                }
            }
            else if (randomCard == 8)
            {
                cardMessage = "Pay poor tax of $15";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    CheckForDebt(currentPlayer, 15, 4);
                }
            }
            else if (randomCard == 9)
            {
                cardMessage = "Take a trip to Reading Railroad. If you pass Go, collect $200";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    while (currentSpace[currentPlayer] != 5)
                    {
                        MovePiece();
                        currentSpace[currentPlayer] = (currentSpace[currentPlayer] + 1) % NUMSPACES;
                        if (currentSpace[currentPlayer] == 0)
                        {
                            wallet[currentPlayer] += 200;
                        }
                    }
                }
            }
            else if (randomCard == 10)
            {
                cardMessage = "Take a walk on the Boardwalk. Advance token to Boardwalk";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    while (currentSpace[currentPlayer] != 39)
                    {
                        MovePiece();
                        currentSpace[currentPlayer] = (currentSpace[currentPlayer] + 1) % NUMSPACES;
                    }
                }
            }
            else if (randomCard == 11)
            {
                cardMessage = "You have been elected Chairman of the Board. Pay each player $50";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    CheckForDebt(currentPlayer, 150, 4);
                    for (int i = 0; i < 4; i++)
                    {
                        if (i != currentPlayer)
                        {
                            wallet[i] += 50;
                        }
                    }
                }
            }
            else if (randomCard == 12)
            {
                cardMessage = "Your building loan matures, Collect $150";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    wallet[currentPlayer] += 150;
                }
            }
            else if (randomCard == 13)
            {
                cardMessage = "You have won a crossword competition, Collect $100";
                if (WasMouseClicked(okayButtonBox) == true)
                {
                    wallet[currentPlayer] += 100;
                }
            }
        }

        /// <summary>
        /// Determines the utility space closest to the player
        /// </summary>
        /// <returns>The space number for the closest utility space</returns>
        private int NearestUtility()
        {
            if (currentSpace[currentPlayer] - 12 > currentSpace[currentPlayer] - 28)
            {
                return 12;
            }
            else
            {
                return 28;
            }
        }

        /// <summary>
        /// If a player requests to buy a house on a property that does not yet have any houses, determines the location of the house.
        /// </summary>
        private void DetermineHouseLocation()
        {
            if (pieceLocs[currentPlayer].Y > edgeToSpace)
            {
                houseBox[currentSpace[currentPlayer]].X = pieceLocs[currentPlayer].X;
                houseBox[currentSpace[currentPlayer]].Y = pieceLocs[currentPlayer].Y - verticalHouseMove;
            }
            else if (pieceLocs[currentPlayer].X < (diameterBigSpace + boardBox.X) && pieceLocs[currentPlayer].X > boardBox.X)
            {
                houseBox[currentSpace[currentPlayer]].Y = pieceLocs[currentPlayer].Y;
                houseBox[currentSpace[currentPlayer]].X = pieceLocs[currentPlayer].X + horizontalHouseMove;
            }
            else if (pieceLocs[currentPlayer].Y < diameterBigSpace)
            {
                houseBox[currentSpace[currentPlayer]].X = pieceLocs[currentPlayer].X;
                houseBox[currentSpace[currentPlayer]].Y = pieceLocs[currentPlayer].Y - verticalHouseMove;
            }
            else if (pieceLocs[currentPlayer].X > (boardBox.X + edgeToSpace))
            {
                houseBox[currentSpace[currentPlayer]].Y = pieceLocs[currentPlayer].Y;
                houseBox[currentSpace[currentPlayer]].X = pieceLocs[currentPlayer].X - (horizontalHouseMove - 15);
            }
        }

        /// <summary>
        /// Determines if a house or a hotel should be drawn on the current space
        /// </summary>
        /// <param name="space">The current space as an integer</param>
        /// <returns>Either house or hotel based on the value for the number of houses (4 houses = a hotel)</returns>
        private Texture2D HouseOrHotel(int space)
        {
            if (numHouses[space] < 4)
            {
                return house;
            }
            else
            {
                return hotel;
            }
        }

        /// <summary>
        /// Determines what colour to shade a house on the board based on the number of houses on that space
        /// </summary>
        /// <param name="space">The current space as an integer</param>
        /// <returns>The colour the house should be on the space</returns>
        private Color WhatColour(int space)
        {
            //If there is only 1 house or there is a hotel on the space
            if (numHouses[space] == 1 || numHouses[space] == 4)
            {
                return Color.White;
            }
            //If there are 2 houses on the space
            else if (numHouses[space] == 2)
            {
                return Color.Blue;
            }
            //If there are 3 houses on the space
            else
            {
                return Color.Orange;
            }
        }

        /// <summary>
        /// Determines what rent a player should pay based on how many houses are on the property
        /// </summary>
        /// <returns></returns>
        private int DetermineRent()
        {
            if (numHouses[currentSpace[currentPlayer]] == 0)
            {
                return spaceRent[currentSpace[currentPlayer]];
            }
            else if (numHouses[currentSpace[currentPlayer]] == 1)
            {
                return (int)Math.Pow(spaceRent[currentSpace[currentPlayer]], 2);
            }
            else if (numHouses[currentSpace[currentPlayer]] == 2)
            {
                return (int)Math.Pow(spaceRent[currentSpace[currentPlayer]], 2) * 2;
            }
            else if (numHouses[currentSpace[currentPlayer]] == 3)
            {
                return (int)Math.Pow(spaceRent[currentSpace[currentPlayer]], 2) * 3;
            }
            else
            {
                return (int)Math.Pow(spaceRent[currentSpace[currentPlayer]], 2) * 4;
            }
        }

        /// <summary>
        /// Determines the positions of each space on the board.
        /// </summary>
        private void DetermineSpaceLocations()
        {
            for (int i = 0; i < 40; i++)
            {
                //Define Starting Point
                if (i == 0)
                {
                    spaceLocs[i] = new Vector2(1100, 767);
                }
                //Moving From GO
                else if (spaceLocs[i - 1].X > (boardBox.X + edgeToSpace) && spaceLocs[i - 1].Y > edgeToSpace)
                {
                    spaceLocs[i] = new Vector2(spaceLocs[i - 1].X - bigToSmallMove, spaceLocs[i - 1].Y);
                }
                //Moving Along the Bottom of the Screen
                else if (spaceLocs[i - 1].X < (boardBox.X + edgeToSpace) && spaceLocs[i - 1].X > (boardBox.X + diameterBigSpace + diameterSmallSpace) && spaceLocs[i - 1].Y > edgeToSpace)
                {
                    spaceLocs[i] = new Vector2(spaceLocs[i - 1].X - smallToSmallMove, spaceLocs[i - 1].Y);
                }
                //Moving To Jail (Just Visiting)
                else if (spaceLocs[i - 1].X > (boardBox.X + diameterBigSpace) && spaceLocs[i - 1].X < (boardBox.X + diameterBigSpace + diameterSmallSpace) && spaceLocs[i - 1].Y > edgeToSpace)
                {
                    spaceLocs[i] = new Vector2(spaceLocs[i - 1].X - bigToSmallMove, spaceLocs[i - 1].Y);
                }
                //Moving From Jail (Just Visiting)
                else if (spaceLocs[i - 1].X < (boardBox.X + diameterBigSpace) && spaceLocs[i - 1].Y > edgeToSpace)
                {
                    spaceLocs[i] = new Vector2(spaceLocs[i - 1].X, spaceLocs[i - 1].Y - bigToSmallMove);
                }
                //Moving Up the Left Side of the Screen
                else if (spaceLocs[i - 1].X < (boardBox.X + diameterBigSpace) && spaceLocs[i - 1].Y < edgeToSpace && spaceLocs[i - 1].Y > (diameterBigSpace + diameterSmallSpace))
                {
                    spaceLocs[i] = new Vector2(spaceLocs[i - 1].X, spaceLocs[i - 1].Y - smallToSmallMove);
                }
                //Moving To Free Parking
                else if (spaceLocs[i - 1].X < (boardBox.X + diameterBigSpace) && spaceLocs[i - 1].Y > (diameterBigSpace) && spaceLocs[i - 1].Y < (diameterBigSpace + diameterSmallSpace))
                {
                    spaceLocs[i] = new Vector2(spaceLocs[i - 1].X, spaceLocs[i - 1].Y - bigToSmallMove);
                }
                //Moving From Free Parking
                else if (spaceLocs[i - 1].Y < diameterBigSpace && spaceLocs[i - 1].X < (boardBox.X + diameterBigSpace))
                {
                    spaceLocs[i] = new Vector2(spaceLocs[i - 1].X + bigToSmallMove, spaceLocs[i - 1].Y);
                }
                //Moving Along the Top of the Screen
                else if (spaceLocs[i - 1].X > (boardBox.X + diameterBigSpace) && spaceLocs[i - 1].X < (boardBox.X + edgeToSpace - diameterSmallSpace) && spaceLocs[i - 1].Y < diameterBigSpace)
                {
                    spaceLocs[i] = new Vector2(spaceLocs[i - 1].X + smallToSmallMove, spaceLocs[i - 1].Y);
                }
                //Moving To Go to Jail
                else if (spaceLocs[i - 1].Y < diameterBigSpace && pieceLocs[currentPlayer].X > (boardBox.X + edgeToSpace - diameterSmallSpace) && spaceLocs[i - 1].X < (boardBox.X + edgeToSpace))
                {
                    spaceLocs[i] = new Vector2(spaceLocs[i - 1].X + bigToSmallMove, spaceLocs[i - 1].Y);
                }
                //Moving From Go to Jail
                else if (spaceLocs[i - 1].X > (boardBox.X + edgeToSpace) && spaceLocs[i - 1].Y < diameterBigSpace)
                {
                    spaceLocs[i] = new Vector2(spaceLocs[i - 1].X, spaceLocs[i - 1].Y + bigToSmallMove);
                }
                //Moving Down the Right Hand Side of the Screen
                else if (spaceLocs[i - 1].X > (boardBox.X + edgeToSpace) && spaceLocs[i - 1].Y > (diameterBigSpace) && spaceLocs[i - 1].Y < (edgeToSpace - diameterSmallSpace))
                {
                    spaceLocs[i] = new Vector2(spaceLocs[i - 1].X, spaceLocs[i - 1].Y + smallToSmallMove);
                }
                //Moving To GO
                else if (spaceLocs[i - 1].X > (boardBox.X + edgeToSpace) && spaceLocs[i - 1].Y > (edgeToSpace - diameterSmallSpace) && spaceLocs[i - 1].Y < edgeToSpace)
                {
                    spaceLocs[i] = new Vector2(spaceLocs[i - 1].X, spaceLocs[i - 1].Y + bigToSmallMove);
                }
            }
        }

        /// <summary>
        /// Determines whether or not a key on the keyboard was pressed
        /// </summary>
        /// <param name="input">The key the program should check for</param>
        /// <returns>whether or not the key was pressed</returns>
        private bool DidKeyboardPress(Keys input)
        {
            if (kb.IsKeyDown(input) == true && !(prevKb.IsKeyDown(input) == true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines the name of the piece from its player number
        /// </summary>
        /// <param name="player">the player number as an integer</param>
        /// <returns>the piece that corrosponds to that player number as a string</returns>
        private string DeterminePieceFromPlayer(int player)
        {
            if (player == 0)
            {
                return "Car";
            }
            else if (player == 1)
            {
                return "Hat";
            }
            else if (player == 2)
            {
                return "Iron";
            }
            else
            {
                return "Shoe";
            }
        }

        /// <summary>
        /// Determines if a player is in debt or not. If they aren't, subtract value as normal. If they are, put the player into the debt settling system.
        /// </summary>
        /// <param name="player">the player being charged an amount</param>
        /// <param name="amount">the amount the player is being charged</param>
        /// <param name="receivingPlayer">the player receiving the money (use 4 for payments to the bank)</param>
        private void CheckForDebt(int player, int amount, int receiving)
        {
            if (wallet[player] >= amount)
            {
                wallet[player] -= amount;
                if (receiving != 4)
                {
                    wallet[receiving] += amount;
                    toTheBank = false;
                }
                else
                {
                    toTheBank = true;
                }
            }
            else
            {
                if (receiving != 4)
                {
                    toTheBank = false;
                }
                else
                {
                    toTheBank = true;
                }
                playerInDebt = player;
                chargeAmount = amount;
                receivingPlayer = receiving;
                isInDebt = true;
                GameState = SETTLE_DEBT;
            }
        }

        /// <summary>
        /// Determines what player integer to use based on whether the game is on settling debt mode or not
        /// </summary>
        /// <returns>the appropriate player integer value</returns>
        private int DeterminePlayer()
        {
            if (isInDebt == true)
            {
                return playerInDebt;
            }
            else
            {
                return currentPlayer;
            }
        }

        /// <summary>
        /// Calculates the price of a new house on a property
        /// </summary>
        /// <param name="space">The number of the space the player is on</param>
        /// <returns>The price to buy a new house</returns>
        private int CalcHousePrice(int space)
        {
            //Determine the amount to charge based on how many houses are already on the property
            if (numHouses[space] == 3)
            {
                return (spacePrice[space] * 6);
            }
            else if (numHouses[space] == 2)
            {
                return (spacePrice[space] * 5);
            }
            else if (numHouses[space] == 1)
            {
                return (spacePrice[space] * 4);
            }
            else
            {
                return (spacePrice[space] * 3);
            }
        }

        /// <summary>
        /// Determines the next player that is not eliminated. If all players except one are eliminated, ends the game.
        /// </summary>
        private void DetermineNextPlayer()
        {
            currentPlayer = (currentPlayer + 1) % NUMPLAYERS;
            if (eliminated[currentPlayer] == true)
            {
                currentPlayer = (currentPlayer + 1) % NUMPLAYERS;
            }
            if (eliminated[currentPlayer] == true)
            {
                currentPlayer = (currentPlayer + 1) % NUMPLAYERS;
            }
        }

    }
}
