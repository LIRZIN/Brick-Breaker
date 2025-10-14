using System.Drawing;
using System.Numerics;

namespace Brick_Breaker;

public class BallManager
{
    private List<Ball> balls = new List<Ball>();

    public int nbBalls { get => balls.Count; }

    public Ball getBall(int index)
    {
        if (index < 0 || index >= nbBalls)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "The value given is out of range");
        }
        
        return balls[index];
    }

    public BallManager()
    {
        balls.Add(new Ball(0.5, 0.5, 0.2, 0.2, 0.001, 0.001, Color.Aqua));
    }

    public bool Update( double deltaTime, BrickWall brickWall, Paddle paddle )
    {
        foreach (var ball in balls)
        {
            ball.CheckCollissions(deltaTime, brickWall, paddle, Data.MAX_RECURSIVE_COLLISION_CALL );
            
            if (ball.PositionY < 0)
            {
                balls.Remove(ball);
            }
        }
        // retourne true si le jeu est terminé
        return balls.Count <= 0;
    }

    public void AddBall(Ball ball)
    {
        balls.Add(ball);
    }
    
}