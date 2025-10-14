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

    void init( int W_pixels, int H_pixels )
    {
        double min = (W_pixels < H_pixels) ? W_pixels : H_pixels;
        Data.screenSizeWidth = (double)W_pixels / min; 
        Data.screenSizeHeight = (double)H_pixels / min; 
        brickWall.init(0);
    }

    void update(double deltaTime, PlayerMovement move)
    {
        ballManager.Update( deltaTime, brickWall, paddle );
        paddle.update( deltaTime, move );
    }
}

/* pistes d'amélioration :
 * faire des niveaux en txt
 * faire les power ups
*/