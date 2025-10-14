using System.Drawing;
using System.Numerics;

namespace Brick_Breaker;

public class BallManager
{
    private List<Ball> balls = new List<Ball>();

    public List<Ball> Balls
    {
        get => balls;
    }

    public BallManager()
    {
        balls.Add(new Ball(0.5, 0.1, 0.2, 0.2, 1, 1, Color.Aqua));
    }

    public bool Update( double deltaTime, BrickWall brickWall, Paddle paddle )
    {
        foreach (var ball in balls)
        {
            ball.CheckCollissions(deltaTime, brickWall, paddle);
            
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