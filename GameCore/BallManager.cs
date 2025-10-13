using System.Drawing;
using System.Numerics;

namespace Brick_Breaker;

public class BallManager
{
    private List<Ball> balls = new List<Ball>();


    public BallManager()
    {
        balls.Add(new Ball(0.5, 0.1, 0.2, 0.2, 1, 1, Color.Aqua));
    }

    public void Update()
    {
        foreach (var ball in balls)
        {
            if (ball.PositionY < 0)
            {
                balls.Remove(ball);
            }
        }

        if (balls.Count <= 0)
        {
            //GAME OVER
        }
    }

    public void AddBall(Ball ball)
    {
        balls.Add(ball);
    }
    
}