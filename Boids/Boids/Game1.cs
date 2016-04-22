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

namespace Boids
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Texture2D boidText;
        public static SpriteFont font;
        public static int deaths = 0;
        private BoidAlgorithmManager BAM;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = Globals.xScreen;
            graphics.PreferredBackBufferHeight = Globals.yScreen;
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            boidText = Content.Load<Texture2D>("boidtext");
            font = Content.Load<SpriteFont>("font");
            BAM = new BoidAlgorithmManager();
            // TODO: use this.Content to load your game content here
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
            KeyMouseReader.Update();
            // Allows the game to exit
            if (KeyMouseReader.KeyPressed(Keys.Escape))
            {
                this.Exit();
            }

            if (KeyMouseReader.KeyPressed(Keys.F2))
            {
                BAM.BlocksToggle();
            }

            if (KeyMouseReader.KeyPressed(Keys.F3))
            {
                BAM.LBIToggle();
            }

            BAM.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            Vector2 offset = new Vector2(20, 20);
            spriteBatch.DrawString(font, "F2: Show/Hide Boxes.", offset, Color.White);
            offset.Y += 30;
            spriteBatch.DrawString(font, "F3: Show/Hide LBI grid.", offset, Color.White);
            offset.Y += 30;
            spriteBatch.DrawString(font, "Total Boid deaths: "+deaths, offset, Color.White);
            offset.Y += 30;

            BAM.Draw(spriteBatch);

            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
