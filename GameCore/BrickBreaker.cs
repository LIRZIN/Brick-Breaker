using System.Drawing;
using System.IO;

namespace Brick_Breaker;

// Conditions victoire/défaite
// faire le max de getters ici pour que l'état du jeu soit lisible 
// Affichage terminal

public class BrickBreaker
{
    private BallManager ballManager = new BallManager();
    private BrickWall brickWall = new BrickWall();
    private Paddle paddle = new Paddle();
    private bool isGameOver = false;
    private bool isGameWon = false;

    public int nbBalls { get => ballManager.nbBalls; }

    public double getBallAttribute(int index, BallAttribute attribute)
    {
        Ball ball = ballManager.getBall(index);

        switch (attribute)
        {
            case BallAttribute.PositionX: return ball.PositionX;
            case BallAttribute.PositionY: return ball.PositionY;
            case BallAttribute.DirectionX: return ball.DirectionX;
            case BallAttribute.DirectionY: return ball.DirectionY;
            case BallAttribute.Speed: return ball.Speed;
            case BallAttribute.Radius: return ball.Radius;
            case BallAttribute.Color: return (int)ball.Color;
        }

        return 0;
    }

    public double getBrickWallAttribute(BrickWallAttribute attribute)
    {
        switch (attribute)
        {
            case BrickWallAttribute.PositionX: return brickWall.x;
            case BrickWallAttribute.PositionY: return brickWall.y;
            case BrickWallAttribute.Width: return brickWall.w;
            case BrickWallAttribute.Height: return brickWall.h;
            case BrickWallAttribute.SpaceBetweenBricks: return brickWall.spaceBetweenBricks;
            case BrickWallAttribute.NbVerticalBricks: return brickWall.nbVerticalBricks;
            case BrickWallAttribute.NbHorizontalBricks: return brickWall.nbHorizontalBricks;
            case BrickWallAttribute.BrickCount: return brickWall.brickCount;
        }

        return 0;
    }

    public double getBrickAttribute(int index, BrickAttribute attribute)
    {
        Brick brick = brickWall.getBrick(index);

        switch (attribute)
        {
            case BrickAttribute.PositionX: return brick.x;
            case BrickAttribute.PositionY: return brick.y;
            case BrickAttribute.Width: return brick.w;
            case BrickAttribute.Height: return brick.h;
            case BrickAttribute.Health: return brick.health;
            case BrickAttribute.Color: return (int)brick.color;
        }
        return 0;
    }

    public double getPaddleAttribute(PaddleAttribute attribute)
    {
        switch (attribute)
        {
            case PaddleAttribute.PositionX: return paddle.x;
            case PaddleAttribute.PositionY: return paddle.y;
            case PaddleAttribute.Width: return paddle.w;
            case PaddleAttribute.Height: return paddle.h;
            case PaddleAttribute.Speed: return paddle.v;
            case PaddleAttribute.Color: return (int)paddle.color;
        }

        return 0;
    }

    public double screenSizeWidth { get => Utils.screenSizeWidth; }
    public double screenSizeHeight { get => Utils.screenSizeHeight; }

    public bool IsGameOver
    {
        get => isGameOver;
        set => isGameOver = value;
    }

    public bool IsGameWon
    {
        get => isGameWon;
        set => isGameWon = value;
    }

    public void init(int W_pixels, int H_pixels)
    {
        double min = (W_pixels < H_pixels) ? W_pixels : H_pixels;
        Utils.screenSizeWidth = (double)W_pixels / min;
        Utils.screenSizeHeight = (double)H_pixels / min;

        brickWall.init(1);
        paddle.init();
        ballManager.init(paddle);

        IsGameWon = false;
        IsGameOver = false;
    }

    public int addLevel(string filePath)
    {
        if (!File.Exists(filePath))
        {
            System.Console.WriteLine("File does not exist");
            return -1;
        }

        string fileContent = File.ReadAllText(filePath);
        char[] delimiterChars = [' ', ',', ';', ':', '\t', '{', '}', '(', ')', '\n'];
        string[] strValues = fileContent.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        double x = Double.Parse(strValues[0]);
        double y = Double.Parse(strValues[1]);
        double w = Double.Parse(strValues[2]);
        double h = Double.Parse(strValues[3]);
        double sBB = Double.Parse(strValues[4]);
        int nbVB = Int32.Parse(strValues[5]);
        int nbHB = Int32.Parse(strValues[6]);

        foreach (string str in strValues)
        {
            System.Console.WriteLine(str);
        }

        if (strValues.Length != (nbVB * nbHB * 4) + 7)
        {
            System.Console.WriteLine("Wrong number of values. Wanted : " + ((nbVB * nbHB * 4) + 7) + ". Read : " + strValues.Length);
            return -1;
        }

        List<int> healths = new List<int>();

        List<ColorEnum> colors = new List<ColorEnum>();
        ColorEnum[] allColors = Enum.GetValues<ColorEnum>();


        for (int brickIndex = 0; brickIndex < nbVB * nbHB; brickIndex++)
        {
            int colorIndex = brickIndex % allColors.Length;
            colors.Add(allColors[colorIndex]);
        }

        int newIndex = -1;

        try
        {
            newIndex = brickWall.addNewBrickWall(new BrickWallParameters(x, y, w, h, nbVB, nbHB, sBB, healths, colors));
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Could not create new Brick Wall : " + e.Message);
        }

        return newIndex;
    }

    public void update(double deltaTime, PlayerMovement move)
    {
        IsGameOver = ballManager.Update(deltaTime, brickWall, paddle);
        paddle.update(deltaTime, move);
        IsGameWon = true;
        foreach (var brickHealth in brickWall.brickHealth)
        {
            if (brickHealth >= 0)
            {
                IsGameWon = false;
            }
        }
    }
}

/* pistes d'amélioration :
 * faire les power ups
*/