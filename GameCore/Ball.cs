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
    public double DirectionX { get => directionX; set => directionX = value; }
    public double DirectionY { get => directionY; set => directionY = value; }
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

    // Paddle, bricks, murs
    public void CheckCollissions(double deltaTime, BrickWall brickWall, Paddle paddle)
    {
        
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

    public void Update(double deltaTime)
    {
        PositionX += Speed * DirectionX * deltaTime;
        PositionY += Speed * DirectionY * deltaTime;
        if (PositionX > 1)
        {
            PositionX = 1;
            DirectionX *= -1;
        }
        if (PositionY > 1)
        {
            PositionY = 1;
            DirectionY *= -1;
        }
        if (PositionX < 0)
        {
            PositionX = 0;
            DirectionX *= -1;
        }
        if (PositionY < 0)
        {
            PositionY = 0;
            DirectionY *= -1;
        }
    }
}