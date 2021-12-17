using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestingMonoGame
{
    public class Game1 : Game
    {
        
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Texture2D ballTexture;
        Vector2 ballPosition;
        float ballSpeed;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            int?  x = 1;
            x = null;
            x = 2;

            DateTime? zz = null;
            zz = DateTime.Now;
            zz = null;
            
            
            // TODO: Add your initialization logic here
            ballPosition = new Vector2(graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 100f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ballTexture = Content.Load<Texture2D>("ball");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Up))
                ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(kstate.IsKeyDown(Keys.Down))
                ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Left))
                ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if(kstate.IsKeyDown(Keys.Right))
                ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(ballPosition.X > graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
                ballPosition.X = graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
            else if(ballPosition.X < ballTexture.Width / 2)
                ballPosition.X = ballTexture.Width / 2;

            if(ballPosition.Y > graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
                ballPosition.Y = graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
            else if(ballPosition.Y < ballTexture.Height / 2)
                ballPosition.Y = ballTexture.Height / 2;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

             spriteBatch.Begin();
             
             spriteBatch.Draw(ballTexture,
                 ballPosition,
                 null,
                 Color.White,
                 0f,
                 new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                 Vector2.One,
                 SpriteEffects.None,
                 0f);
             
             spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
