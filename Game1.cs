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
        private static readonly Random Random = new Random();
        
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Texture2D texture;

        private static int cellSizeModifier = 10;
        
        Rectangle bigRectangle;

        private Random random = new Random();
      
        Color black = new Color();
        Color cl2 = new Color(new Vector3(100.0f, 150.0f, 50.0f)); //Vector3
        Color cl3 = new Color(new Vector4(100.0f, 150.0f, 50.0f, 125.0f)); //Vector4
        Color cl4 = new Color(0.0f, 15.0f, 50.0f); //3-float based
        Color cl5 = new Color(10, 50, 25);   //3-integer based
        Color cl6 = new Color(0.0f, 15.0f, 50.0f, 100.0f); //4-float based
        Color cl7 = new Color(10, 20, 30, 40); //4-integer based
        Color cl8 = Color.FromNonPremultiplied(new Vector4(100.0f, 0.0f, 15.0f, 100.0f));  //Vector4-based
        Color cl9 = Color.FromNonPremultiplied(10, 20, 30, 100); //int based
        Color cl10 = Color.Lerp(Color.Yellow, Color.Black, 50.0f); //Lerp Function
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
            bigRectangle = new Rectangle(10, 10, graphics.PreferredBackBufferWidth -20, graphics.PreferredBackBufferHeight -20);
            
            arrayHeight = graphics.PreferredBackBufferHeight;
            arrayWidth = graphics.PreferredBackBufferWidth;

            array2D = new Cell[arrayWidth,arrayHeight];

            for (var y = 0; y < arrayHeight; y++)
            {
                for (var x = 0; x < arrayWidth; x++)
                {
                    var cell = new Cell
                    {
                        IsAlive = true,
                        Rectangle = new Rectangle(x * cellSizeModifier, y * cellSizeModifier, cellSizeModifier -2 ,cellSizeModifier -2),
                        Color = GetRandomColour()
                    };
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
            pixel.SetData(new[] { Color.White });
        }

        private const float Delay = 1; // seconds
        private float remainingDelay = Delay;
        protected override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
                ++cellSizeModifier;

            if (kstate.IsKeyDown(Keys.Down))
                --cellSizeModifier;
          

            base.Update(gameTime);
            
            var timer = (float) gameTime.ElapsedGameTime.TotalSeconds;
            remainingDelay -= timer;
            
            Console.WriteLine($"timer is {timer}");
            
            if(remainingDelay <= 0)
            {
                Console.WriteLine("------");
                
                for (var y = 0; y < arrayHeight; y++)
                {
                    for (var x = 0; x < arrayWidth; x++)
                    {
                        //Console.WriteLine(x +" | " + y);
                        if(x == 0) continue;
                        if(y == 0) continue;
                        if(x == 799) continue;
                        if(y == 480) continue;
                        
                        if(array2D[x,y].IsConnected)
                            continue;
                        
                        var cell = array2D[x, y];
                        var leftCell = array2D[x - 1, y];
                        var rightCell = array2D[x + 1, y];
                        
                        
                        var topRange = cell.Color.B + 10;
                        var bottomRange = cell.Color.B - 10;
                        
                        if (leftCell.Color.B < topRange && leftCell.Color.B > bottomRange)
                        {
                            cell.Color = Color.Black;
                            ;
                            rightCell.IsConnected = true;
                            rightCell.IsAlive = true;
                            rightCell.Color = cell.Color;
                            
                            leftCell.IsConnected = true;
                            leftCell.IsAlive = true;
                            leftCell.Color = Color.Black;

                        }
                        else
                        {
                            cell.Color = GetRandomColour();
                        }
                    }
                }
                
                remainingDelay = Delay;
            }

            rDirection = ShouldWeFlipDirection(rDirection, r);
            gDirection = ShouldWeFlipDirection(gDirection, g);
            bDirection = ShouldWeFlipDirection(bDirection, b);
            
            r = GetNextRGBValue(rDirection, r);
            g = GetNextRGBValue(gDirection, g);
            b = GetNextRGBValue(bDirection, b);
            cl5 = new Color(r, g, b);

            lerpDirection = LerpBounce(lerpDirection, lerpValue);
            lerpValue = LerpNext(lerpDirection, lerpValue);
            cl10 = Color.Lerp(Color.Red, Color.Black, lerpValue);
            base.Update(gameTime);
        }

        private Direction LerpBounce(Direction direction, float value)
        {
            return direction switch
            {
                Direction.Front when value >= 1.0f => Direction.Back,
                Direction.Back when value <= 0 => Direction.Front,
                _ => direction
            };
        }

        private Direction ShouldWeFlipDirection(Direction direction, int value)
        {
            return direction switch
            {
                Direction.Front when value == 255 => Direction.Back,
                Direction.Back when value == 0 => Direction.Front,
                _ => direction
            };
        }

        private Color GetRandomColour()
        {
            return new Color(Random.Next(256), Random.Next(256), Random.Next(256));
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(cl5);
            spriteBatch.Begin();
            
            spriteBatch.Draw(texture, bigRectangle, Color.Black);

            for (var y = 0; y < arrayHeight; y++)
            {
                for (var x = 0; x < arrayWidth; x++)
                {
                    if (array2D[x, y].IsAlive && y > 1 && y < graphics.PreferredBackBufferHeight / cellSizeModifier - 2 &&
                        x < graphics.PreferredBackBufferWidth / cellSizeModifier - 2 && x > 1)
                    {
                        spriteBatch.Draw(pixel, array2D[x, y].Rectangle, array2D[x, y].Color);
                    }
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public int GetNextRGBValue(Direction dir, int value)
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

        private static float LerpNext(Direction dir, float value)
        {
            return dir switch
            {
                Direction.Front => value + 0.01f,
                Direction.Back => value - 0.01f,
                _ => value
            };
        }

        private bool GetNextRandomBool()
        {
            var prob = random.Next(100);
            return prob <= 50;
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
        public Color Color { get; set; }
        public bool IsConnected { get; set; }
    }
}