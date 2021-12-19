using System;
using System.Diagnostics;
using System.Threading;
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

        private Random random = new Random();
      
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
        
        
        private int r = 51;
        private int g = 255;
        private int b = 255;
        private Direction rDirection = Direction.Front;
        private Direction gDirection = Direction.Front;
        private Direction bDirection = Direction.Front;

        private float lerpValue = 0.0f;
        private Direction lerpDirection = Direction.Front;

        Texture2D pixel;
        Rectangle bigRectangle;
       
        private int arrayHeight;
        private int arrayWidth;
        Cell[,] array2D;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        

        protected override void Initialize()
        {
            arrayHeight = graphics.PreferredBackBufferHeight;
            arrayWidth = graphics.PreferredBackBufferWidth;;

            array2D = new Cell[arrayWidth,arrayHeight];

            for (int y = 0; y < arrayHeight; y++)
            {
                for (int x = 0; x < arrayWidth; x++)
                {
                    var cell = new Cell();
                    cell.IsAlive = true;
                    cell.Rectangle = new Rectangle(x * 10, y * 10, 8, 8);
                    array2D[x, y] = cell;
                }
            }
            
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.DarkSlateGray });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            
            bigRectangle = new Rectangle(10, 10, 300, 150);
        }

        protected override void Update(GameTime gameTime)
        {
            rDirection = ShouldWeFlipDirection(rDirection, r);
            gDirection = ShouldWeFlipDirection(gDirection, g);
            bDirection = ShouldWeFlipDirection(bDirection, b);
            
            r = GetNext(rDirection, r);
            g = GetNext(gDirection, g);
            b = GetNext(bDirection, b);
            cl5 = new Color(r, g, b);

            lerpDirection = LerpBounce(lerpDirection, lerpValue);
            lerpValue = LerpNext(lerpDirection, lerpValue);
            cl10 = Color.Lerp(Color.Red, Color.Black, lerpValue);
            base.Update(gameTime);
        }

        private Direction LerpBounce(Direction direction, float value)
        {
            switch (direction)
            {
                case Direction.Front when value >= 1.0f:
                    return Direction.Back;
                   
                case Direction.Back when value <= 0:
                    return Direction.Front;
            }
            
            return direction;
        }

        private Direction ShouldWeFlipDirection(Direction direction, int value)
        {
            switch (direction)
            {
                case Direction.Front when value == 255:
                    return Direction.Back;
                   
                case Direction.Back when value == 0:
                    return Direction.Front;
            }
            
            return direction;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(cl5);
            spriteBatch.Begin();

            for (int y = 0; y < arrayHeight; y++)
            {
                for (int x = 0; x < arrayWidth; x++)
                {
                    spriteBatch.Draw(texture, array2D[x, y].Rectangle, cl10);
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public int GetNext(Direction dir, int value)
        {
            switch (dir)
            {
                case Direction.Front:
                {
                    value = ++value;
                    return value;
                }
                case Direction.Back:
                {
                    value = --value;
                    return value;
                }
                default:
                    return value;
            }
        }

        public float LerpNext(Direction dir, float value)
        {
            return dir switch
            {
                Direction.Front => value + 0.01f,
                Direction.Back => value - 0.01f,
                _ => value
            };
        }

        public bool GetNextRandomBool()
        {
            int prob = random.Next(100);
            return prob <= 80;
        }
    }

    public enum Direction
    {
        Front,
        Back,
        None
    }
    
    public class Cell
    {
        public bool IsAlive { get; set; }
        public Rectangle Rectangle { get; set; }
    }
}