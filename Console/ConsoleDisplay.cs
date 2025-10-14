namespace Console;
using Brick_Breaker;

public class ConsoleDisplay
{
    private BrickBreaker brickBreaker;
    private double deltaTime;
    private int w_pixels, h_pixels;

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
    }

    public void Update()
    {
        //BrickBreaker.update(DeltaTime, );
    }

    public void DrawGame()
    {
        BrickBreaker.BrickWall
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