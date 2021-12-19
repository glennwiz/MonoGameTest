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

        private int R = 51;
        private int G = 255;
        private int B = 255;
        private Direction R_direction = Direction.Front;
        private Direction G_direction = Direction.Front;
        private Direction B_direction = Direction.Front;

        private float LerpValue = 0.0f;
        private Direction LerpDirection = Direction.Front;
        protected override void Update(GameTime gameTime)
        {
            R_direction = ShouldWeFlipDirection(R_direction, R);
            G_direction = ShouldWeFlipDirection(G_direction, G);
            B_direction = ShouldWeFlipDirection(B_direction, B);
            
            R = GetNext(R_direction, R);
            G = GetNext(G_direction, G);
            B = GetNext(B_direction, B);
            cl5 = new Color(R, G, B);

            LerpDirection = LerpBounce(LerpDirection, LerpValue);
            LerpValue = LerpNext(LerpDirection, LerpValue);
            cl10 = Color.Lerp(Color.Red, Color.Black, LerpValue);
            base.Update(gameTime);
        }

        private Direction LerpBounce(Direction lerpDirection, float lerpValue)
        {
            Console.WriteLine("LerpDirection => " + lerpDirection + " | LerpValue => " + lerpValue);
            switch (lerpDirection)
            {
                case Direction.Front when lerpValue >= 1.0f:
                    return Direction.Back;
                   
                case Direction.Back when lerpValue <= 0:
                    return Direction.Front;
                    break;
            }
            
            return lerpDirection;
        }

        private Direction ShouldWeFlipDirection(Direction direction, int value)
        {
            switch (direction)
            {
                case Direction.Front when value == 255:
                    return Direction.Back;
                   
                case Direction.Back when value == 0:
                    return Direction.Front;
                    break;
            }
            
            return direction;
        }

        private int counter = 0;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(cl5);
            counter++;
            //Console.WriteLine("Draw count" + counter);
            Console.WriteLine(cl5);
           
            spriteBatch.Begin();
            
            // Draw the big Fading rectangle
            spriteBatch.Draw(pixel, bigRectangle, cl10);
            
            // Draw the small rectangle that changes color on collision
            spriteBatch.Draw(pixel, smallRectangle, cl10);
            
            spriteBatch.Draw(texture, new Rectangle(100, 100, 100, 100), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public int GetNext(Direction dir, int value)
        {
            Console.WriteLine($"Comming in {dir} value => {value}");
            switch (dir)
            {
                case Direction.Front:
                {
                    Console.WriteLine("Inside front");
                    value = ++value;
                    Console.WriteLine(value);
                    return value;
                }
                case Direction.Back:
                {
                    Console.WriteLine("Inside back");
                    value = --value;
                    Console.WriteLine(value);
                    return value;
                }
                    
            }

            return value;
        }

        public float LerpNext(Direction dir, float value)
        {
            if (dir == Direction.Front)
            {
                return value + 0.01f;
            }
            
            if(dir == Direction.Back)
            {
                return value - 0.01f;
            }

            return value;
        }
    }

    public enum Direction
    {
        Front,
        Back,
        None
    }
}