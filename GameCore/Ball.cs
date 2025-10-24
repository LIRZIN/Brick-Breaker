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

    private void HandlePaddleCollision(double CollisionX, double CollisionY, Side side, Paddle paddle)
    {
        if (side != Side.Top)
        {
            HandleReflectiveCollision( CollisionX, CollisionY, side );
        }
        else
        {
            double collisionCos = ((CollisionX - paddle.x)/paddle.w - 0.5)*2;
            //On obtient une valeur entre 0 et 1 à laquelle on soustrait 0.5 puis on multiplie le tout par 2 pour avoir le
            //cosinus de l'angle à rajouter à celui de la balle
            
            double collAngle = GetAngle(collisionCos);
            
            /*Console.WriteLine("collision X : " + CollisionX + " paddleX : " + paddle.x + " paddlW : " + paddle.w);
            Console.WriteLine("collision cosinus : " + collisionCos);
            Console.WriteLine("angle col : " + RadToDeg(collAngle) + " angle ball : " + RadToDeg(GetAngle(DirectionX)));*/
            
            //on soustrait 90° à l'angle final, de cette façon plus le point d'impact se trouche proche du centre du paddle
            //moins l'angle final est affecté, plus le point est gauche plus la direction de la balle ira vers la gauche et plus
            //le point est à droite plus la direction de la balle ira vers la droite
            double finalAngle = GetAngle(DirectionX) + collAngle - Math.PI/2;
            if (finalAngle >= Math.PI)
            {
                finalAngle = Math.PI - 0.1;
            }
            else if (finalAngle <= 0)
            {
                finalAngle = 0.1;
            }
            PositionX = CollisionX;
            PositionY = CollisionY;
            DirectionX = GetCosinus(finalAngle);
            DirectionY = -1 * GetSinus(finalAngle);
            //Console.WriteLine("x : " + positionX + " y : " + positionY + " angle : " + RadToDeg(finalAngle));
            //Console.WriteLine("dx : " + directionX + " dy : " + DirectionY);
        }
    }

    public void CheckCollisions(double deltaTime, BrickWall brickWall, Paddle paddle, CollisionType previousCollision, int recursiveCount )
    {
        double dx = deltaTime * DirectionX * Speed;
        double dy = deltaTime * DirectionY * Speed;
        /*
        System.Console.WriteLine("x = " + positionX);
        System.Console.WriteLine("y = " + positionY);
        System.Console.WriteLine("dx = " + dx);
        System.Console.WriteLine("dy = " + dy);
        System.Console.WriteLine(
            "= deltaTime " + deltaTime + " + ( DirectionX, DirectionY ) ( "  + DirectionY + ", " + DirectionX + " ) + Speed " + Speed);
        */
        
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
        CollisionType currentCollision = CollisionType.None;
        Side side = Side.None;
        
        // Brick Collision
        for( int i = 0; i < brickWall.brickCount; i++ )
        {
            Brick brick = brickWall.getBrick(i);
            if( !( previousCollision == CollisionType.Brick && Data.lastBrickCollisionIndex == i ) 
                && CollisionChecker.circleRectCollision(PositionX, PositionY, Radius, dx, dy,
                                                     brick.x, brick.y, brick.w, brick.h,
                                                     ref collisionX, ref collisionY, ref collisionU, ref side) )
            {
                brickCollisionIndex = i;
            }
        }
        
        if (brickCollisionIndex >= 0)
        {
            System.Console.WriteLine("Collision found with " + side + " part of brick n°" + brickCollisionIndex);
            HandleReflectiveCollision( collisionX, collisionY, side );
            brickWall.decreaseHealthBrick(brickCollisionIndex);
            currentCollision = CollisionType.Brick;
            Data.lastBrickCollisionIndex = brickCollisionIndex;
        }
        // Walls Collision
        else if( CollisionChecker.circleWallsCollision( PositionX, PositionY, Radius, dx, dy, previousCollision,
                                                        ref collisionX, ref collisionY, ref collisionU, ref side) )
        {
            System.Console.WriteLine("Collision found with the " + side + " wall");
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
            System.Console.WriteLine("Collision found with the paddle on its " + side + " side");
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

    //retourne l'angle en radian basé sur le cosinus
    private double GetAngle(double cos)
    {
        // Calcule l'angle en radians
        return Math.Acos(cos);
    }

    //retourne le cosinus de l'angle passé en radian
    private double GetCosinus(double angle)
    {
        return Math.Cos(angle);
    }
    
    //retourne le sinus de l'angle passé en radian
    private double GetSinus(double angle)
    {
        return Math.Sin(angle);
    }

    //utilisé pour le debug
    private double RadToDeg(double rad)
    {
        return rad * 180 / Math.PI;
    }
}