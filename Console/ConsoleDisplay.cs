namespace Console;
using Brick_Breaker;

public class ConsoleDisplay
{
    private BrickBreaker brickBreaker;
    private double deltaTime;
    private int w_pixels, h_pixels;
    private char[][] displaytab;

    public BrickBreaker BrickBreaker
    {
        get => brickBreaker;
    }

    public double DeltaTime
    {
        get => deltaTime;
    }

    public int W_pixels
    {
        get => w_pixels;
        set  => w_pixels = value;
    }

    public int H_pixels
    {
        get => h_pixels;
        set => h_pixels = value;
    }
    
    public void Init( int init_w_pixels, int init_h_pixels )
    {
        brickBreaker = new BrickBreaker();
        W_pixels = init_w_pixels; 
        H_pixels = init_h_pixels;
        brickBreaker.init(W_pixels, H_pixels);
        displaytab = new char[H_pixels][];
        for (int i = 0; i < H_pixels; i++)
        {
            displaytab[i] = new char[W_pixels];
            for (int j = 0; j < W_pixels; j++)
            {
                displaytab[i][j] = '.';
            }
        }

        deltaTime = 1;
    }

    public void Update( double deltaTime )
    {
        //Update BrickBreaker et input
        PlayerMovement movement = PlayerMovement.Nothing;
        if (ConsoleInput.pressingLeft)
        {
            movement = PlayerMovement.Left;
        }
        else if (ConsoleInput.pressingRight)
        {
            movement = PlayerMovement.Right;
        }
        BrickBreaker.update(deltaTime, movement);
        //System.Console.WriteLine(BrickBreaker.getBallAttribute(0, BallAttribute.PositionX));
        //System.Console.WriteLine(BrickBreaker.getBallAttribute(0, BallAttribute.PositionY));
        
        //Draw all pixels
        for (int i = 0; i < H_pixels; i++)
        {
            displaytab[i] = new char[W_pixels];
            for (int j = 0; j < W_pixels; j++)
            {
                displaytab[i][j] = '.';
            }
        }
        
        //Draw balls
        for (int i = 0; i < brickBreaker.nbBalls; i++)
        {
            displaytab[(int)(BrickBreaker.getBallAttribute(i, BallAttribute.PositionY) * (H_pixels-1) / Data.screenSizeHeight)]
                [(int)(BrickBreaker.getBallAttribute(i, BallAttribute.PositionX) * (W_pixels-1) / Data.screenSizeWidth)] = 'O';

        }
        
        //Draw paddle
        DrawBox((int)((BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionX) * (W_pixels-1)) / Data.screenSizeWidth),
            (int)((BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionY) * (H_pixels-1)) / Data.screenSizeHeight),
            (int)(((BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionX) + brickBreaker.getPaddleAttribute(PaddleAttribute.Width)) * (W_pixels-1)) / Data.screenSizeWidth),
            (int)(((BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionY) + brickBreaker.getPaddleAttribute(PaddleAttribute.Height)) * (H_pixels-1)) / Data.screenSizeHeight));
        
        //draw brickWall
        for (int i = 0; i < brickBreaker.getBrickWallAttribute(BrickWallAttribute.BrickCount); i++)
        {
            int tempX = (int)((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionX) * (W_pixels-1)) / Data.screenSizeWidth);
            int tempY = (int)((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionY) * (H_pixels-1)) / Data.screenSizeHeight);
            int tempEndX =  (int)(((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionX) + brickBreaker.getBrickAttribute(i, BrickAttribute.Width)) * (W_pixels-1)) / Data.screenSizeWidth);
            int tempEndY = (int)(((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionY) + brickBreaker.getBrickAttribute(i, BrickAttribute.Height)) * (H_pixels-1)) / Data.screenSizeHeight);
            DrawBox(tempX, tempY, tempEndX, tempEndY);
            //System.Console.WriteLine("brique n°" + i + " :");
            //System.Console.WriteLine("x = " + BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionX) + " | " + tempX);
            //System.Console.WriteLine("y = " + BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionY)+ " | " + tempY);
            //System.Console.WriteLine("width = " + BrickBreaker.getBrickAttribute(i, BrickAttribute.Width)+ " | " + tempEndX);
            //System.Console.WriteLine("height = " + BrickBreaker.getBrickAttribute(i, BrickAttribute.Height)+ " | " + tempEndY);
        }
    }

    public void DrawGame()
    {
        //BrickBreaker.BrickWall
        for (int i = 0; i < H_pixels; i++)
        {
            for (int j = 0; j < W_pixels; j++)
            {
                System.Console.Write(displaytab[i][j]);
            }
            System.Console.Write("\n");
        }
    }
    
    public void DrawWin()
    {
        System.Console.WriteLine("Vous avez gagné ! Bravo !!!");
    }

    public void DrawLose()
    {
        System.Console.WriteLine("Vous avez perdu ! :(");
    }

    //dessine une boite (barre ou brique)
    private void DrawBox(int x, int y, int endX, int endY)
    {
        for (int i = y; i <= endY; i++)
        {
            for (int j = x; j <= endX; j++)
            {
                //rempli l'intérieur de la boite
                displaytab[i][j] = '#';
                
                //dessine les bords de la boite
                if (i == y || i == endY)
                {
                    displaytab[i][j] = '-';
                }

                if (j == x || j == endX)
                {
                    displaytab[i][j] = '|';
                }
            }
        }
        //dessine les coins de la boite
        displaytab[y][x] = '[';
        displaytab[y][endX] = ']';
        displaytab[endY][x] = '[';
        displaytab[endY][endX] = ']';
    }
}