using System;
using System.Collections.Generic;
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
        Color cl4 = new Color(0.0f, 150.0f, 50.0f); //3-float based
        Color cl5 = new Color(10, 150, 100);   //3-integer based
        Color cl6 = new Color(0.0f, 15.0f, 50.0f, 100.0f); //4-float based
        Color cl7 = new Color(10, 20, 30, 40); //4-integer based
        Color cl8 = Color.FromNonPremultiplied(new Vector4(100.0f, 0.0f, 15.0f, 100.0f));  //Vector4-based
        Color cl9 = Color.FromNonPremultiplied(10, 20, 30, 100); //int based
        Color cl10 = Color.Lerp(Color.Yellow, Color.Black, 50.0f); //Lerp Function
        Color cl11 = Color.Multiply(Color.Pink, 5.0f);  //Multiply Function
        
        private const float Delay = 1; // seconds
        private float remainingDelay = Delay;
        
        private int r = 51;
        private int g = 30;
        private int b = 150;
        private Direction rDirection = Direction.Front;
        private Direction gDirection = Direction.Front;
        private Direction bDirection = Direction.Front;

        private float lerpValue = 0.0f;
        private Direction lerpDirection = Direction.Front;

        Texture2D pixel;

        private Cell[,] cellArray0;
        private Cell[,] cellArray1;
        
        private int arrayHeight;
        private int arrayWidth;

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

            cellArray0 = new Cell[graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight];
            cellArray1 = new Cell[graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight];
            
            for (var y = 0; y < arrayHeight; y++)
            {
                for (var x = 0; x < arrayWidth; x++)
                {
                    var cell = new Cell
                    {
                        IsAlive = false,
                        Rectangle = new Rectangle(x * cellSizeModifier, y * cellSizeModifier, cellSizeModifier -2 ,cellSizeModifier -2),
                        Color = GetRandomColour()
                    };
                    cellArray0[x, y] = cell;
                }
            }
            
            cellArray0[32, 10].IsAlive = true;
            cellArray0[33, 11].IsAlive = true;
            cellArray0[33, 12].IsAlive = true;
            cellArray0[34, 10].IsAlive = true;
            cellArray0[34, 11].IsAlive = true;

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

  
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            var timer = (float) gameTime.ElapsedGameTime.TotalSeconds;
            remainingDelay -= timer;
            
            if(remainingDelay <= 0)
            {
                Console.WriteLine("------");
                
                for (var y = 0; y < arrayHeight; y++)
                {
                    for (var x = 0; x < arrayWidth; x++)
                    {
                        if(x == 0) continue; //skip first column
                        if(y == 0) continue; //skip first row       
                        if(x == 799) continue; //skip last column
                        if(y == 480) continue; //skip last row

                        /*  Conways Game Of Life Rules 
                         
                            Any live cell with fewer than two live neighbours dies, as if by underpopulation.
                            Any live cell with two or three live neighbours lives on to the next generation.
                            Any live cell with more than three live neighbours dies, as if by overpopulation.
                            Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                            
                            These rules, which compare the behavior of the automaton to real life, can be condensed into the following:
                                    Any live cell with two or three live neighbours survives.
                                    Any dead cell with three live neighbours becomes a live cell.
                                    All other live cells die in the next generation. Similarly, all other dead cells stay dead.
                        */

                       


                    }
                }
                
                remainingDelay = Delay;
            }

            GetRollingBackgroundColor();
            cl5 = new Color(r, g, b);

            lerpDirection = LerpBounce(lerpDirection, lerpValue);
            lerpValue = LerpNext(lerpDirection, lerpValue);
            cl10 = Color.Lerp(Color.Red, Color.Black, lerpValue);
            base.Update(gameTime);
        }

        private Cell CellChecker(Cell[,] cells, int i, int i1)
        {
            try
            {
                return cells[i, i1];
            }
            catch (Exception)
            {
                return null;
            }
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
                    var drawSquaresWithinBorders = y > 1 && //Top border
                                             y < graphics.PreferredBackBufferHeight / cellSizeModifier - 2 && //Bottom border
                                             x < graphics.PreferredBackBufferWidth / cellSizeModifier - 2 && //Right border
                                             x > 1; //Left border
                            
                    if (cellArray0[x, y].IsAlive && drawSquaresWithinBorders)
                    {
                        spriteBatch.Draw(pixel, cellArray0[x, y].Rectangle, cellArray0[x, y].Color);
                    }
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private int GetNextRGBValue(Direction dir, int value)
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

        private bool GetNextRandomBool(int probabilty)
        {
            var prob = random.Next(100);
            return prob <= probabilty;
        }
        
        private void GetRollingBackgroundColor()
        {
            rDirection = ShouldWeFlipDirection(rDirection, r);
            gDirection = ShouldWeFlipDirection(gDirection, g);
            bDirection = ShouldWeFlipDirection(bDirection, b);

            r = GetNextRGBValue(rDirection, r);
            g = GetNextRGBValue(gDirection, g);
            b = GetNextRGBValue(bDirection, b);
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