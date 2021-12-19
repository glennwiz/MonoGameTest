using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestingMonoGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Texture2D texture; 
      
        Color cl1 = new Color();
        Color cl2 = new Color(new Vector3(100.0f, 150.0f, 50.0f)); //Vector3
        Color cl3 = new Color(new Vector4(100.0f, 150.0f, 50.0f, 125.0f)); //Vector4
        Color cl4 = new Color(0.0f, 15.0f, 50.0f); //3-float based
        Color cl5 = new Color(10, 50, 25);   //3-integer based
        Color cl6 = new Color(0.0f, 15.0f, 50.0f, 100.0f); //4-float based
        Color cl7 = new Color(10, 20, 30, 40); //4-integer based
        Color cl8 = Color.FromNonPremultiplied(new Vector4(100.0f, 0.0f, 15.0f, 100.0f));  //Vector4-based
        Color cl9 = Color.FromNonPremultiplied(10, 20, 30, 100); //int based
        Color cl10 = Color.Lerp(Color.Red, Color.Black, 50.0f); //Lerp Function
        Color cl11 = Color.Multiply(Color.Pink, 5.0f);  //Multiply Function
        
        
        Texture2D pixel;
        Rectangle bigRectangle;
        Rectangle smallRectangle;
        Color smallRectangleColor;
        bool smallRectangleMovesLeft;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.DarkSlateGray });

          
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            bigRectangle = new Rectangle(200, 100, 300, 150);
            smallRectangle = new Rectangle(600, 150, 50, 50);
            smallRectangleColor = Color.White;

         
        }

        private int R = 0;
        private int G = 15;
        private int B = 50;
        private Direction R_direction = Direction.Front;
        private Direction G_direction = Direction.Front;
        private Direction B_direction = Direction.Front;
        
        protected override void Update(GameTime gameTime)
        {
            switch (R_direction)
            {
                case Direction.Front when R == 255:
                    R_direction = Direction.Back;
                    break;
                case Direction.Back when R == 0:
                    R_direction = Direction.Front;
                    break;
            }

            switch (G_direction)
            {
                case Direction.Front when G == 255:
                    G_direction = Direction.Back;
                    break;
                case Direction.Back when G == 0:
                    G_direction = Direction.Front;
                    break;
            }
            
            switch (B_direction)
            {
                case Direction.Front when B == 255:
                    B_direction = Direction.Back;
                    break;
                case Direction.Back when B == 0:
                    B_direction = Direction.Front;
                    break;
            }

            switch (R_direction)
            {
                case Direction.Front:
                    R += 1;
                    break;
                case Direction.Back:
                    R -= 1;
                    break;
            }
            
            switch (G_direction)
            {
                case Direction.Front:
                    G += 1;
                    break;
                case Direction.Back:
                    G -= 1;
                    break;
            }
            
            switch (B_direction)
            {
                case Direction.Front:
                    B += 1;
                    break;
                case Direction.Back:
                    B -= 1;
                    break;
            }


            cl5 = new Color(R, G, B); 
            base.Update(gameTime);
        }

        private int counter = 0;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(cl5);
            counter++;
            Console.WriteLine("Draw count" + counter);
            Console.WriteLine(cl4);
           
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            // Draw the big black rectangle
            spriteBatch.Draw(pixel, bigRectangle, Color.Black);
// Draw the small rectangle that changes color on collision
            spriteBatch.Draw(pixel, smallRectangle, smallRectangleColor);
            
            spriteBatch.Draw(texture, new Rectangle(100, 100, 100, 100), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }

    enum Direction
    {
        Front,
        Back
    }
}