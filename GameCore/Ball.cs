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
            normalizeDirection();
        }
    }

    public double DirectionY
    {
        get => directionY;
        set
        {
            directionY = value;
            normalizeDirection();
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
        normalizeDirection();
    }
    
    private void normalizeDirection()
    {
        double norm = double.Sqrt( directionX * directionX + directionY * directionY );
        directionX /= norm;
        directionY /= norm;
    }

    private void HandleReflectiveCollision(double collisionX, double collisionY, Side side)
    {
        PositionX = collisionX;
        PositionY = collisionY;
        
        switch (side)
        {
            case Side.Left:
            case Side.Right: DirectionX *= -1; break;
            case Side.Bottom:
            case Side.Top: DirectionY *= -1; break;
        }
    }

    private void HandlePaddleCollision(double collisionX, double collisionY, Side side, Paddle paddle)
    {
        if (side != Side.Top)
        {
            HandleReflectiveCollision( collisionX, collisionY, side );
        }
        else
        {
            PositionX = collisionX;
            PositionY = collisionY;
            
            double u = ((collisionX - paddle.x)/paddle.w - 0.5)*Math.PI;
            DirectionX += double.Sin(u);
            DirectionY += double.Cos(u);
            DirectionY *= -1;

            double right_angle = Math.Acos(directionX); // = acos( ( directionX, directionY ) . ( 1, 0 ) )

            if (right_angle >= Math.PI-Utils.limit_angle_paddle_reflection)
            {
                DirectionX = -double.Sin(Utils.limit_angle_paddle_reflection);
                DirectionY = double.Cos(Utils.limit_angle_paddle_reflection);
            }
            if (right_angle < Utils.limit_angle_paddle_reflection)
            {
                DirectionX = double.Sin(Utils.limit_angle_paddle_reflection);
                DirectionY = double.Cos(Utils.limit_angle_paddle_reflection);
            }
        }
    }

    public void CheckCollisions(double deltaTime, BrickWall brickWall, Paddle paddle, CollisionType previousCollision, int recursiveCount )
    {
        double dx = deltaTime * DirectionX * Speed;
        double dy = deltaTime * DirectionY * Speed;
        
        if (recursiveCount <= 0)
        {
            PositionX += dx;
            PositionY += dy;
        }

        int brickCollisionIndex = -1;
        double collisionX = 0;
        double collisionY = 0;
        double collisionU = 999;
        CollisionType currentCollision = CollisionType.None;
        Side side = Side.None;
        
        // Brick Collision
        for( int i = 0; i < brickWall.brickCount; i++ )
        {
            Brick brick = brickWall.getBrick(i);
            if( !( previousCollision == CollisionType.Brick && Utils.lastBrickCollisionIndex == i ) 
                && CollisionChecker.circleRectCollision(PositionX, PositionY, Radius, dx, dy,
                                                     brick.x, brick.y, brick.w, brick.h,
                                                     ref collisionX, ref collisionY, ref collisionU, ref side) )
            {
                brickCollisionIndex = i;
            }
        }
        
        if (brickCollisionIndex >= 0)
        {
            HandleReflectiveCollision( collisionX, collisionY, side );
            brickWall.decreaseHealthBrick(brickCollisionIndex);
            currentCollision = CollisionType.Brick;
            Utils.lastBrickCollisionIndex = brickCollisionIndex;
        }
        // Walls Collision
        else if( CollisionChecker.circleWallsCollision( PositionX, PositionY, Radius, dx, dy, previousCollision,
                                                        ref collisionX, ref collisionY, ref collisionU, ref side) )
        {
            HandleReflectiveCollision( collisionX, collisionY, side );
            switch (side)
            {
                case Side.Left: currentCollision = CollisionType.LeftWall; break;
                case Side.Right: currentCollision = CollisionType.RightWall; break;
                case Side.Top: currentCollision = CollisionType.TopWall; break;
            }
        }
        // Paddle Collision
        else if (previousCollision != CollisionType.Paddle 
                 && CollisionChecker.circleRectCollision(PositionX, PositionY, Radius, dx, dy,
                                                         paddle.x, paddle.y, paddle.w, paddle.h,
                                                         ref collisionX, ref collisionY, ref collisionU, ref side))
        {
            HandlePaddleCollision( collisionX, collisionY, side, paddle );
            currentCollision = CollisionType.Paddle;
        }

        if (side != Side.None)
        {
            double uDeltaTime = deltaTime * ( 1 - collisionU );
            CheckCollisions( uDeltaTime, brickWall, paddle, currentCollision, recursiveCount-1 );
        }
        else
        {
            PositionX += dx;
            PositionY += dy;
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