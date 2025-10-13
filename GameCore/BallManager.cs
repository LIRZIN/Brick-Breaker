using System.Drawing;
using System.Numerics;

namespace Brick_Breaker;

public class BallManager
{
    private int nbBalls;
    private List<Ball> balls = new List<Ball>();

    public int NbBalls
    {
        get => nbBalls;
        set => nbBalls = value;
    }

    public BallManager()
    {
        NbBalls = 1;
        balls.Add(new Ball(new Vector2(0.5f, 0.1f), new Vector2(0.2f, 0.2f), 1, 1, Color.Aqua));
    }

    public void Update()
    {
        foreach (var ball in balls)
        {
            if (ball.Position.Y < 0)
            {
                balls.Remove(ball);
                NbBalls--;
            }
        }

        if (NbBalls <= 0)
        {
            //GAME OVER
        }
    }

    public void AddBall(Ball ball)
    {
        balls.Add(ball);
        NbBalls++;
    }
    
}