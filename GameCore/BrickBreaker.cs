namespace Brick_Breaker;

public enum PlayerMovement
{
    Left,
    Right,
    Nothing
};

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