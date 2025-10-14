namespace Brick_Breaker;

// Conditions victoire/défaite
// faire le max de getters ici pour que l'état du jeu soit lisible 
// Affichage terminal

public class BrickBreaker
{
    private BallManager ballManager = new BallManager();
    private BrickWall brickWall =  new BrickWall();
    private Paddle paddle = new Paddle();
    private bool isGameOver = false;
    private bool isGameWon = false;

    public int nbBalls { get => ballManager.nbBalls; }

    public double getBallAttribute(int index, BallAttribute attribute)
    {
        Ball ball = ballManager.getBall(index);
        
        switch( attribute )
        {
            case BallAttribute.PositionX: return ball.PositionX;
            case BallAttribute.PositionY: return ball.PositionY;
            case BallAttribute.DirectionX : return ball.DirectionX;
            case BallAttribute.DirectionY : return ball.DirectionY;
            case BallAttribute.Speed : return ball.Speed;
            case BallAttribute.Radius : return ball.Radius;
            case BallAttribute.Color : return ball.Color.R | (ball.Color.G<<8) | (ball.Color.B<<16) |  (ball.Color.A<<24);
        }

        return 0;
    }

    public double getBrickWallAttribute(BrickWallAttribute attribute)
    {
        switch (attribute)
        {
            case BrickWallAttribute.PositionX : return brickWall.x;
            case BrickWallAttribute.PositionY : return brickWall.y;
            case BrickWallAttribute.Width : return brickWall.w;
            case BrickWallAttribute.Height : return brickWall.h;
            case BrickWallAttribute.SpaceBetweenBricks : return brickWall.spaceBetweenBricks;
            case BrickWallAttribute.NbVerticalBricks : return brickWall.nbVerticalBricks;
            case BrickWallAttribute.NbHorizontalBricks : return brickWall.nbHorizontalBricks;
            case BrickWallAttribute.BrickCount : return brickWall.brickCount;
        }
        
        return 0;
    }

    public double getBrickAttribute(int index, BrickAttribute attribute)
    {
        Brick brick = brickWall.getBrick(index);
        
        switch( attribute )
        {
            case BrickAttribute.PositionX : return brick.x;
            case BrickAttribute.PositionY : return brick.y;
            case BrickAttribute.Width : return brick.w;
            case BrickAttribute.Height : return brick.h;
            case BrickAttribute.Health : return brick.health;
            case BrickAttribute.Color : return brick.color.R | (brick.color.G<<8) | (brick.color.B<<16) |  (brick.color.A<<24);
        }
    }

    public double getPaddleAttribute(PaddleAttribute attribute)
    {
        switch (attribute)
        {
            case PaddleAttribute.PositionX : return paddle.x;
            case PaddleAttribute.PositionY : return paddle.y;
            case PaddleAttribute.Width : return paddle.w;
            case PaddleAttribute.Height : return paddle.h;
            case PaddleAttribute.Speed : return paddle.v;
            case PaddleAttribute.Color : return paddle.color.R | (paddle.color.G<<8) | (paddle.color.B<<16) |  (paddle.color.A<<24);
        }

        return 0;
    }

    public BallManager BallManager
    {
        get => ballManager;
    }

    public BrickWall BrickWall
    {
        get => brickWall;
    }

    public Paddle Paddle
    {
        get => paddle;
    }

    public bool IsGameOver
    {
        get => isGameOver;
        set  => isGameOver = value;
    }

    public bool IsGameWon
    {
        get => isGameWon;
        set => isGameWon = value;
    }
    
    void init( int W_pixels, int H_pixels )
    {
        double min = (W_pixels < H_pixels) ? W_pixels : H_pixels;
        Data.screenSizeWidth = (double)W_pixels / min; 
        Data.screenSizeHeight = (double)H_pixels / min; 
        brickWall.init(0);
    }

    void update(double deltaTime, PlayerMovement move)
    {
        IsGameOver = ballManager.Update( deltaTime, brickWall, paddle );
        paddle.update( deltaTime, move );
        IsGameWon = true;
        foreach (var brickHealth in brickWall.brickHealth)
        {
            if (brickHealth >= 0)
            {
                IsGameWon  = false;
            }
        }
    }
}

/* pistes d'amélioration :
 * faire des niveaux en txt
 * faire les power ups
*/