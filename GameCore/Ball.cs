using System.Data;
using System.Numerics;

namespace Brick_Breaker;
using System.Drawing;

public class Ball
{
    private double positionX, positionY, directionX, directionY, speed, radius;
    private Color color;

    public double PositionX { get => positionX; set => positionX = value; }
    public double PositionY { get => positionY; set => positionY = value; }
    public double DirectionX
    {
        get => directionX;
        set
        {
            directionX = value;
            normalize();
        }
    }

    public double DirectionY
    {
        get => directionY;
        set
        {
            directionY = value;
            normalize();
        }
    }

    public double Speed { get => speed; set => speed = value; }
    public double Radius { get => radius; set => radius = value; }
    public Color Color { get => color; set => color = value; }

    public Ball(double positionX, double positionY, double directionX, double directionY, double speed, double radius, Color color)
    {
        this.positionX = positionX;
        this.positionY = positionY;
        this.directionX = directionX;
        this.directionY = directionY;
        this.speed = speed;
        this.radius = radius;
        this.color = color;
    }

    private void normalize()
    {
        double norm = double.Sqrt( directionX * directionX + directionY * directionY );
        directionX /= norm;
        directionY /= norm;
    }

    private void HandleReflectiveCollision(double CollisionX, double CollisionY, Side side)
    {
        PositionX = CollisionX;
        PositionY = CollisionY;
        
        switch (side)
        {
            case Side.Left:
            case Side.Right: DirectionX *= -1; break;
            case Side.Bottom:
            case Side.Top: DirectionY *= -1; break;
        }
    }

    private void HandlePaddleCollision(double CollisionX, double CollisionY, Side side)
    {
        if (side != Side.Top)
        {
            HandleReflectiveCollision( CollisionX, CollisionY, side );
        }
        else
        {
            PositionX = CollisionX;
            PositionY = CollisionY;
            
            // Special Reflection
        }
    }

    public void CheckCollissions(double deltaTime, BrickWall brickWall, Paddle paddle, int recursiveCount )
    {
        double dx = deltaTime * DirectionX * Speed;
        double dy = deltaTime * DirectionY * Speed;
        
        if (recursiveCount <= 0)
        {
            System.Console.WriteLine("Recursive Collision Method Call Count Reached");
            PositionX += dx;
            PositionY += dy;
        }

        int brickCollisionIndex = -1;
        double collisionX = 0;
        double collisionY = 0;
        double collisionU = 999;
        Side side = Side.None;
        
        // Brick Collision
        for( int i = 0; i < brickWall.brickCount; i++ )
        {
            Brick brick = brickWall.getBrick(i);
            if( CollisionChecker.circleRectCollision(PositionX, PositionY, Radius, dx, dy,
                                               brick.x, brick.y, brick.w, brick.h,
                                               ref collisionX, ref collisionY, ref collisionU, ref side))
            {
                brickCollisionIndex = i;
            }
        }
        
        if (brickCollisionIndex >= 0)
        {
            System.Console.WriteLine("Collision found with " + side + " part of brick n°" + brickCollisionIndex);
            HandleReflectiveCollision( collisionX, collisionY, side );
            brickWall.decreaseHealthBrick(brickCollisionIndex);
        }
        // Walls Collision
        else if( CollisionChecker.circleWallsCollision( PositionX, PositionY, Radius, dx, dy,
                                                      ref collisionX, ref collisionY, ref collisionU, ref side) )
        {
            System.Console.WriteLine("Collision found with the " + side + " wall");
            HandleReflectiveCollision( collisionX, collisionY, side );
        }
        // Paddle Collision
        else if (CollisionChecker.circleRectCollision(PositionX, PositionY, Radius, dx, dy,
                paddle.x, paddle.y, paddle.w, paddle.h,
                ref collisionX, ref collisionY, ref collisionU, ref side))
        {
            System.Console.WriteLine("Collision found with the paddle on its " + side + " side");
            HandlePaddleCollision( collisionX, collisionY, side );
        }

        if (side != Side.None)
        {
            double uDeltaTime = deltaTime * collisionU;
            CheckCollissions( uDeltaTime, brickWall, paddle, recursiveCount-1 );
        }
    }

    public void Afficher()
    {
        Console.WriteLine("Position : " + PositionX + ", " + PositionY);
        Console.WriteLine("Direction : " + DirectionX + ", " + DirectionY);
        Console.WriteLine("Vitesse : " + Speed);
        Console.WriteLine("Rayon : " + Radius);
        Console.WriteLine("Couleur : " + Color);
        Console.WriteLine("\n");
    }
}