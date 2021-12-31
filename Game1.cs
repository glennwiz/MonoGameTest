using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        private readonly Random random = new Random();

        private const float Delay = 2; // seconds
        private float remainingDelay = Delay;

        private int r = 51;
        private int g = 30;
        private int b = 150;
        private Direction rDirection = Direction.Front;
        private Direction gDirection = Direction.Front;
        private Direction bDirection = Direction.Front;

        private float lerpValue = 0.0f;
        private Direction lerpDirection = Direction.Front;
        
        Color lerpingColor = new Color(10, 150, 100); //3-integer based

        Color black = new Color();
        Texture2D pixel;

        private static Cell[,] cellArrayGen0;
        private static Cell[,] cellArrayGen1;

        private static int arrayHeight;
        private static int arrayWidth;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            bigRectangle = new Rectangle(10, 10, graphics.PreferredBackBufferWidth - 20,
                graphics.PreferredBackBufferHeight - 20);

            arrayHeight = 50; //graphics.PreferredBackBufferHeight;
            arrayWidth = 80; //graphics.PreferredBackBufferWidth;

            cellArrayGen0 = new Cell[arrayWidth, arrayHeight];
            cellArrayGen1 = new Cell[arrayWidth, arrayHeight];

            int progress = 0;
            for (var y = 0; y < arrayHeight; y++)
            {
                for (var x = 0; x < arrayWidth; x++)
                {
                    //Console.WriteLine("Progress: " + progress);
                    //Console.WriteLine("Cell localtion X: " + x + " Y: " + y);
                    var cell = new Cell
                    {
                        IsAlive = false, //GetNextRandomBool(50),
                        Rectangle = new Rectangle(x * cellSizeModifier, y * cellSizeModifier, cellSizeModifier - 2,
                            cellSizeModifier - 2),
                        Color = GetNextRainbowColor(progress)
                    };
                    cellArrayGen0[x, y] = cell;
                    progress++;
                }
            }

            

            #region Glider
            cellArrayGen0[10, 10].IsAlive = true;
            cellArrayGen0[12, 10].IsAlive = true;
            cellArrayGen0[11, 11].IsAlive = true;
            cellArrayGen0[12, 11].IsAlive = true;
            cellArrayGen0[11, 12].IsAlive = true;
            #endregion

            #region Blink

            // cellArrayGen0[12, 12].IsAlive = true;
            // cellArrayGen0[12, 13].IsAlive = true;
            // cellArrayGen0[12, 14].IsAlive = true;
            // cellArrayGen0[13, 12].IsAlive = true;
            // cellArrayGen0[13, 13].IsAlive = true;
            // cellArrayGen0[13, 14].IsAlive = true;
            // cellArrayGen0[14, 12].IsAlive = true;
            // cellArrayGen0[14, 13].IsAlive = true;
            // cellArrayGen0[14, 14].IsAlive = true;

            #endregion

            
            cellArrayGen1 = cellArrayGen0.Clone() as Cell[,];
            
            //texture used by background
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] {Color.DarkSlateGray});

            base.Initialize();
        }

        public static Color GetNextRainbowColor(float progress)
        {
            var div = progress % 6;

            int ascending = (int) ((progress % 255));

            int descending = 255 - ascending;

            switch ((int) div)
            {
                case 0:
                    return new Color(255, ascending, 0);
                case 1:
                    return new Color(descending, 255, 0);
                case 2:
                    return new Color(0, 255, ascending);
                case 3:
                    return new Color(0, descending, 255);
                case 4:
                    return new Color(ascending, 0, 255);
                default:
                    return new Color(255, 0, descending);
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] {Color.White});
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var timer = (float) gameTime.ElapsedGameTime.TotalSeconds;
            remainingDelay -= timer;
            var counter = 1;
            
            if (remainingDelay <= 0)
            {
                Console.WriteLine($"-Loop {counter++} -----" + DateTime.Now.ToString("hh:mm:ss") + "------");

                for (var y = 0; y < arrayHeight; y++)
                {
                    for (var x = 0; x < arrayWidth; x++)
                    {
                        if (x == 11 && y == 10)
                        {
                            Console.WriteLine("Cell localtion X: " + x + " Y: " + y);     //I'm here, bug is 10,10 getting set to not alive and not being counted in 11,10 secondgeneration state
                        }
                        
                        if (x == 0) continue; //skip first column
                        if (y == 0) continue; //skip first row       
                        if (x == 799) continue; //skip last column
                        if (y == 480) continue; //skip last row

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

                       
                        var isAlive = CheckIfGen0StillAliveInNextGen(x, y);

                        cellArrayGen1[x, y].IsAlive = isAlive ? true : false;

                        if (isAlive)
                        {
                            Console.WriteLine("||||Cell at {0},{1} is alive: {2}", x, y, isAlive);
                        }
                    }
                }

                //swap the arrays
                cellArrayGen0 = cellArrayGen1.Clone() as Cell[,];

                remainingDelay = Delay;
            }

            GetRollingBackgroundColor();
            lerpingColor = new Color(r, g, b);


            lerpDirection = LerpBounce(lerpDirection, lerpValue);
            lerpValue = LerpNext(lerpDirection, lerpValue);
            base.Update(gameTime);
        }
        private static bool DebugMode = true;
        private static bool CheckIfGen0StillAliveInNextGen(int x, int y)
        {
            int neighboursAlive = 0;

            // [10, 10]
            // [12, 10]
            // [11, 11]
            // [12, 11]
            // [11, 12]
            
            if (x == 10 && y == 10)
            {
                DebugMode = true;
            }
            
            if(x==11 && y==10)
            {
                DebugMode = true;

                for (var yy = 0; yy < arrayHeight; yy++)
                {
                    for (var xx = 0; xx < arrayWidth; xx++)
                    {
                        if (cellArrayGen0[xx,yy] == null) 
                        {
                            Console.WriteLine("*******Cell at {0},{1} is null", xx, yy);
                            continue;
                        }
                        
                        if (cellArrayGen0[xx,yy].IsAlive)
                        {
                            Console.WriteLine("*******Cell at {0},{1} is alive: {2}", xx, yy, cellArrayGen0[xx, yy].IsAlive);
                        }
                    }
                }
            }
            
            if (x == 12 && y == 10)
            {
                DebugMode = true;
            }
            if (x == 11 && y == 11)
            {
                DebugMode = true;
            }
            if (x == 12 && y == 11)
            {
                DebugMode = true;
            }
            if (x == 11 && y == 12)
            {
                DebugMode = true;
            }

            if (DebugMode)
            {
                Console.WriteLine(x + " " + y + "---------------debug mode---------------");
                
                neighboursAlive += CheckCordsDebug(x - 1, y - 1, "top left");  //top left
                neighboursAlive += CheckCordsDebug(x, y - 1,  "top");             //top 
                neighboursAlive += CheckCordsDebug(x + 1, y - 1,  "top right");  //top right
                neighboursAlive += CheckCordsDebug(x - 1, y, "left");             //left
                neighboursAlive += CheckCordsDebug(x + 1, y, "right");             //right
                neighboursAlive += CheckCordsDebug(x - 1, y + 1,"bottom left");  //bottom left
                neighboursAlive += CheckCordsDebug(x, y + 1,  "bottom");             //bottom
                neighboursAlive += CheckCordsDebug(x + 1, y + 1,  "bottom right");  //bottom right

                Console.WriteLine("neighboursAlive: " + neighboursAlive);
                
            }
            else
            {
                neighboursAlive += CheckCords(x - 1, y - 1);  //top left
                neighboursAlive += CheckCords(x, y - 1);             //top 
                neighboursAlive += CheckCords(x + 1, y - 1);  //top right
                neighboursAlive += CheckCords(x - 1, y);             //left
                neighboursAlive += CheckCords(x + 1, y);             //right
                neighboursAlive += CheckCords(x - 1, y + 1);  //bottom left
                neighboursAlive += CheckCords(x, y + 1);             //bottom
                neighboursAlive += CheckCords(x + 1, y + 1);  //bottom right
            }
                
            //Console.WriteLine("Checking cell at {0},{1}", x, y); //TODO: make debug,,, it sets top alive wrongly,,, fix it, bottom right is wrong 

            var cell = cellArrayGen0[x, y];

            if (DebugMode)
            {
                Console.WriteLine(x + " " + y +"");
            }
            
            switch (cell.IsAlive)
            {
                    //Any live cell with two or three live neighbours survives.
                    //Any dead cell with three live neighbours becomes a live cell.
                    //All other live cells die in the next generation. Similarly, all other dead cells stay dead.
                case true:
                    if (neighboursAlive == 2 || neighboursAlive == 3)
                    {
                        if (DebugMode)
                        {
                            Console.WriteLine("-----------------End of debug mode-----------------");
                        }
                        DebugMode = false;
                        return true;
                    }
                    else
                    {
                        if (DebugMode)
                        {
                            Console.WriteLine("-----------------End of debug mode-----------------");
                        }
                        DebugMode = false;
                        return false;
                    }
                case false:
                    if (neighboursAlive == 3)
                    {
                        if (DebugMode)
                        {
                            Console.WriteLine("-----------------End of debug mode-----------------");
                        }
                        DebugMode = false;
                        Console.WriteLine(neighboursAlive + " | x " + x + " y " + y);
                        Console.WriteLine("resurrect a cell at {0},{1}", x, y);
                        return true;
                    }
                    else
                    {
                        if (DebugMode)
                        {
                            Console.WriteLine("-----------------End of debug mode-----------------");
                        }
                        DebugMode = false;
                        return false;
                    }
            }
        }

        private static int CheckCordsDebug(int offsettX, int offsettY, string logmessage)
        {
            try
            {
                var cell = cellArrayGen0[offsettX, offsettY];
                {
                    if (cell.IsAlive == true)
                    {
                        Console.WriteLine(logmessage + "Cell at {0},{1} is alive: {2}", offsettX, offsettY, cell.IsAlive);
                        return 1;
                    }
                    else
                    {
                        //Console.WriteLine("Cell at {0},{1} is dead: {2}", offsettX, offsettY, cell.IsAlive);
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private static int CheckCords(int offsettX, int offsettY)
        {
            try
            {
                var cell = cellArrayGen0[offsettX, offsettY];
                {
                    if (cell.IsAlive == true)
                    {
                        //Console.WriteLine("Cell at {0},{1} is alive: {2}", offsettX, offsettY, cell.IsAlive);
                        return 1;
                    }
                    else
                    {
                        //Console.WriteLine("Cell at {0},{1} is dead: {2}", offsettX, offsettY, cell.IsAlive);
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
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
            GraphicsDevice.Clear(lerpingColor); //clear the screen with the background color     
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

                    if (cellArrayGen0[x, y].IsAlive && drawSquaresWithinBorders)
                    {
                        spriteBatch.Draw(pixel, cellArrayGen0[x, y].Rectangle, cellArrayGen0[x, y].Color);
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