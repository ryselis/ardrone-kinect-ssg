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


namespace ARDroneKinect
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ARDroneKinect : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private LogManager logManager;

        private NUIcontroler _nui;
        private DroneControler _drone;

        private Timer _timer;
        private Timer _trotleft;
        private Timer _trotright;
        private Timer _tforward;
        private Timer _tbackward;
        private Timer _tflyleft;
        private Timer _tflyright;
        private Timer _takingOff;

        private KinectMotion _kMotion;

        private bool takeOffCom;
        private bool conMassage;
        private bool islanding;
        private bool command;
        public bool isVoiceActive;
        private bool isKeyRelease;

        public ARDroneKinect()
        {

            takeOffCom = true;
            conMassage = false;
            islanding = false;
            command = false;
            isVoiceActive = false;
            isKeyRelease = true;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 400;
            graphics.PreferredBackBufferWidth = 500;
            graphics.ApplyChanges();
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
         
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            logManager = new LogManager(this, 20, spriteBatch);
            Components.Add(logManager);
            _nui = new NUIcontroler(800, 640, logManager);

            _nui.init();

            _drone = new DroneControler(logManager);
            _drone.ConnectToNetwork();
            // TODO: use this.Content to load your game content here
        }

        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            _drone.Disconnect();
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {   
            
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
           // int x;
            if (!_drone.droneControl.IsFlying)
            {
                KeyboardState state = Keyboard.GetState();

                if (state.IsKeyDown(Keys.V) && isKeyRelease == true)
                {
                    if (isVoiceActive == true)
                    {
                        isVoiceActive = false;
                        isKeyRelease = false;
                    }
                    else
                    {
                        isVoiceActive = true;
                        isKeyRelease = false;
                    }
                }
                if (state.IsKeyUp(Keys.V))
                {
                    isKeyRelease = true;
                }




                if (_nui.voiceTakingOff && takeOffCom == true)
                {
                    _drone.Takeoff();
                    _nui.voiceTakingOff = false;
                    takeOffCom = false;

                    _nui.voiceLeft = false;
                    _nui.voiceRight = false;
                    _nui.voiceStop = false;
                    _nui.resetsound();
                    logManager.addLog("Command -> Takeoff");

                }
                if (state.IsKeyDown(Keys.Escape))
                {
                    _drone.Land();
                    logManager.addLog("Land pressed Escape");
                    takeOffCom = true;
                }
            }

            if (_drone.droneControl.IsFlying && takeOffCom == false) // && isVoiceActive galima prideti jeigu su v mgt
            {
                KeyboardState state = Keyboard.GetState();

                if (_nui.voiceLanding)
                {
                    _drone.Land();
                    logManager.addLog("Command -> Landing");
                    takeOffCom = true;
                    //_nui.resetsound();
                    _nui.voiceLanding = false;

                    _nui.voiceLeft = false;
                    _nui.voiceRight = false;
                    _nui.voiceStop = false;
                }
                if (_nui.voiceLeft)
                {
                    _drone.Navigate(0, 0, -0.5f, 0);
                    logManager.addLog("Command -> Rot Left");
                    _nui.resetsound();
                    _nui.voiceLeft = false;
                    //takeOffCom = false; // <-----
                    _nui.voiceRight = false;
                    _nui.voiceStop = false;
                }
                if (_nui.voiceRight)
                {
                    _drone.Navigate(0, 0, 0.5f, 0);
                    logManager.addLog("Command -> Rot Right");
                    _nui.resetsound();
                    _nui.voiceRight = false;
                    //takeOffCom = false; // <-----
                    _nui.voiceLeft = false;
                    _nui.voiceStop = false;
                }


                if (_nui.voiceStop)
                {
                    _drone.Navigate(0, 0, 0, 0);
                    logManager.addLog("Command -> Stop");
                    _nui.resetsound();
                    _nui.voiceStop = false;
                   //     takeOffCom = false; // <-----
                    _nui.voiceLeft = false;
                    _nui.voiceRight = false;
                }

                if (state.IsKeyDown(Keys.Escape))
                {
                    _drone.Land();
                    takeOffCom = true;
                    logManager.addLog("Land pressed Escape");
                }
                if (state.IsKeyDown(Keys.Space))
                {
                    _drone.Navigate(0, 0, 0, 0);
                    logManager.addLog("Pressed Stop");
                }
            }

            if (_drone.droneControl.IsConnected)
            {
                if (!conMassage)
                {
                    logManager.addLog("Connected");
                    conMassage = true;
                    _drone.FlatTrim();
                }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
