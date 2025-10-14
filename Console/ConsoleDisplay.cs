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
    
    
    
    public void Init()
    {
        brickBreaker = new BrickBreaker();
        W_pixels = System.Console.WindowWidth;
        h_pixels = System.Console.WindowHeight;
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
    }

    public void Update()
    {
        //draw balls
        for (int i = 0; i < brickBreaker.nbBalls; i++)
        {
            displaytab[(int)BrickBreaker.getBallAttribute(i, BallAttribute.PositionY) * H_pixels]
                [(int)BrickBreaker.getBallAttribute(i, BallAttribute.PositionX) * W_pixels] = 'O';

        }
        //draw paddle
        int paddleY = (int)BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionY) * H_pixels;
        int paddleX = (int)BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionX) * W_pixels;
        int paddleEndY = (int)(BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionY) + brickBreaker.getPaddleAttribute(PaddleAttribute.Height)) * H_pixels;
        int paddleEndX = (int)(BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionX) + brickBreaker.getPaddleAttribute(PaddleAttribute.Width)) * W_pixels;
        for (int i = paddleY; i < paddleEndY; i++)
        {
            for (int j = paddleX; j < paddleEndX; j++)
            {
                if (i == paddleY || i == paddleEndY)
                {
                    displaytab[i][j] = '-';
                }

                if (j == paddleX || j == paddleEndX)
                {
                    displaytab[i][j] = '|';
                }
            }
        }
        displaytab[paddleY][paddleX] = '[';
        displaytab[paddleY][paddleEndX] = ']';
        displaytab[paddleEndY][paddleX] = '[';
        displaytab[paddleEndY][paddleEndX] = ']';
        //draw brickWall
        //brickBreaker.getBrickWallAttribute(BrickWallAttribute.NbVerticalBricks)
        
        //en attente de consoleInputs
        //BrickBreaker.update(DeltaTime, );
    }

    public void DrawGame()
    {
        //BrickBreaker.BrickWall
        for (int i = 0; i < W_pixels; i++)
        {
            for (int j = 0; j < H_pixels; j++)
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
}