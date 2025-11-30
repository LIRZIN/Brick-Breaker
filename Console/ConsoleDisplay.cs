namespace Console;
using Brick_Breaker;

public class ConsoleDisplay
{
    private BrickBreaker brickBreaker;
    private double deltaTime;
    private int w_pixels, h_pixels;
    private char[] displaytab;

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
    
    private void setCharDisplay(int x, int y, char c)
    {
        if (x < 0 || x > w_pixels || y < 0 || y >= h_pixels)
        {
            return;
        }
        displaytab[(W_pixels+1)*y+x] = c;
    }
    private void initCharDisplay()
    {
        displaytab = new char[H_pixels*(W_pixels+1)];
        for (int i = 0; i < H_pixels; i++)
        {
            for (int j = 0; j < W_pixels; j++)
            {
                setCharDisplay(j, i, '.');
            }
            
            setCharDisplay(W_pixels, i, '\n');
        }
    }
    
    public void Init( int init_w_pixels, int init_h_pixels, bool record)
    {
        brickBreaker = new BrickBreaker();
        W_pixels = init_w_pixels; 
        H_pixels = init_h_pixels;
        brickBreaker.init(W_pixels, H_pixels, record, "console");

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

        initCharDisplay();
        
        //Draw balls
        for (int i = 0; i < brickBreaker.nbBalls; i++)
        {
            setCharDisplay( (int)(BrickBreaker.getBallAttribute(i, BallAttribute.PositionX) * (W_pixels-1) / Utils.screenSizeWidth),
                            (int)(BrickBreaker.getBallAttribute(i, BallAttribute.PositionY) * (H_pixels-1) / Utils.screenSizeHeight),'O');
        }
        
        //Draw paddle
        DrawBox((int)((BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionX) * (W_pixels-1)) / Utils.screenSizeWidth),
            (int)((BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionY) * (H_pixels-1)) / Utils.screenSizeHeight),
            (int)(((BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionX) + brickBreaker.getPaddleAttribute(PaddleAttribute.Width)) * (W_pixels-1)) / Utils.screenSizeWidth),
            (int)(((BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionY) + brickBreaker.getPaddleAttribute(PaddleAttribute.Height)) * (H_pixels-1)) / Utils.screenSizeHeight),
            false);
        
        //draw brickWall
        for (int i = 0; i < brickBreaker.getBrickWallAttribute(BrickWallAttribute.BrickCount); i++)
        {
            int tempX = (int)((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionX) * (W_pixels-1)) / Utils.screenSizeWidth);
            int tempY = (int)((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionY) * (H_pixels-1)) / Utils.screenSizeHeight);
            int tempEndX =  (int)(((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionX) + brickBreaker.getBrickAttribute(i, BrickAttribute.Width)) * (W_pixels-1)) / Utils.screenSizeWidth);
            int tempEndY = (int)(((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionY) + brickBreaker.getBrickAttribute(i, BrickAttribute.Height)) * (H_pixels-1)) / Utils.screenSizeHeight);
            DrawBox(tempX, tempY, tempEndX, tempEndY, true, (int)BrickBreaker.getBrickAttribute(i,BrickAttribute.Health));
        }
    }

    public void DrawGame()
    {
        System.Console.WriteLine(displaytab);
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
    private void DrawBox(int x, int y, int endX, int endY, bool isBrick, int brickHealth = 0)
    {
        bool doDraw = brickHealth > 0;
        for (int i = y; i <= endY; i++)
        {
            for (int j = x; j <= endX; j++)
            {
                if (isBrick)
                {
                    //Desine une brique
                    
                    //rempli l'intérieur de la brique
                    if(doDraw)
                        setCharDisplay(j, i, brickHealth.ToString()[0]);
                }
                else
                {
                    setCharDisplay(j, i, '#');
                }
            }
        }

        if (isBrick && doDraw)
        {
            //dessine les coins de la brique
            setCharDisplay(x, y, '[');
            setCharDisplay(endX, y, ']');
            setCharDisplay(x, endY, '[');
            setCharDisplay(endX, endY, ']');
        }
    }
}