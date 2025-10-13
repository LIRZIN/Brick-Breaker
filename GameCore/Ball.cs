using System.Data;
using System.Numerics;

namespace Brick_Breaker;
using System.Drawing;

public class Ball
{
    private Vector2 position, direction;
    private double speed, radius;
    private Color color;

    public Vector2 Position
    {
        get => position;
        set => position = value;
    }

    public void PositionX(float x)
    {
        position.X = x;
    }

    public void PositionY(float y)
    {
        position.Y = y;
    }

    public Vector2 Direction
    {
        get => direction;
        set => direction = value;
    }
    public void DirectionX(float x)
    {
        direction.X = x;
    }

    public void DirectionY(float y)
    {
        direction.Y = y;
    }

    public double Speed { get => speed; set => speed = value; }
    public double Radius { get => radius; set => radius = value; }
    public Color Color { get => color; set => color = value; }

    public Ball(Vector2 position, Vector2 direction, double speed, double radius, Color color)
    {
        this.position = position;
        this.direction = direction;
        this.speed = speed;
        this.radius = radius;
        this.color = color;
    }
    //public CheckCollissions()

    public void Afficher()
    {
        Console.WriteLine("Position : " + Position);
        Console.WriteLine("Direction : " + Direction);
        Console.WriteLine("Vitesse : " + Speed);
        Console.WriteLine("Rayon : " + Radius);
        Console.WriteLine("Couleur : " + Color);
        Console.WriteLine("\n");
    }

    public void Update(double deltaTime)
    {
        Position += (float)Speed * Direction * (float)deltaTime;
        if (Position.X > 1)
        {
            PositionX(1);
            DirectionX(Direction.X * -1);
        }
        if (Position.Y > 1)
        {
            PositionY(1);
            DirectionY(Direction.Y * -1);
        }
        if (Position.X < 0)
        {
            PositionX(0);
            DirectionX(Direction.X * -1);
        }
        if (Position.Y < 0)
        {
            PositionY(0);
            DirectionY(Direction.Y * -1);
        }
    }
}