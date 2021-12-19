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
        
        private Texture2D ballTexture;
        
        private Texture2D background;
        private Texture2D shuttle;
        private Texture2D earth;
        
        
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
            // TODO: Add your initialization logic here
            ballPosition = new Vector2(graphics.PreferredBackBufferWidth / 2f,
                graphics.PreferredBackBufferHeight / 2f);
            ballSpeed = 100f;

            //draw circle monogame
            
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ballTexture = Content.Load<Texture2D>("ball");
            background = Content.Load<Texture2D>("stars"); // change these names to the names of your images
            shuttle = Content.Load<Texture2D>("shuttle");  // if you are using your own images.
            earth = Content.Load<Texture2D>("earth");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.W))
                ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(kstate.IsKeyDown(Keys.S))
                ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.A))
                ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if(kstate.IsKeyDown(Keys.D))
                ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(ballPosition.X > graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
                ballPosition.X = graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
            else if(ballPosition.X < ballTexture.Width / 2f)
                ballPosition.X = ballTexture.Width / 2f;

            if(ballPosition.Y > graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
                ballPosition.Y = graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
            else if(ballPosition.Y < ballTexture.Height / 2f)
                ballPosition.Y = ballTexture.Height / 2f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

             spriteBatch.Begin();
             
             spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
             
             spriteBatch.Draw(ballTexture,
                 ballPosition,
                 null,
                 Color.White,
                 0f,
                 new Vector2(ballTexture.Width / 2f, ballTexture.Height / 2f),
                 Vector2.One,
                 SpriteEffects.None,
                 0f);
             
             spriteBatch.Draw(earth, new Vector2(400, 240), Color.White);
             spriteBatch.Draw(shuttle, new Vector2(450, 240), Color.White);
             
             
             //circle.Draw();

             
             spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
