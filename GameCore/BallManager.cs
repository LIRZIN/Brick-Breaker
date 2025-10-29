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

    public void init( Paddle paddle )
    {
        double radius = 0.05;
        double x = paddle.x + paddle.w / 2;
        double y = paddle.y - radius - 0.0001;
        balls.Add(new Ball(x, y, 0.2, -0.2, 3, radius, ColorEnum.Aqua));
    }

    public bool Update( double deltaTime, BrickWall brickWall, Paddle paddle )
    {
        for( int i = 0; i < balls.Count; i++ )
        {
            Ball ball = balls[i];
            ball.CheckCollisions(deltaTime, brickWall, paddle, CollisionType.None, Utils.MAX_RECURSIVE_COLLISION_CALL );
            if (ball.PositionY >= Utils.screenSizeHeight )
            {
                balls.RemoveAt(i);
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