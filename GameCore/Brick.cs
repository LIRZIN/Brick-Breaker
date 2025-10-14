using System.Drawing;

namespace Brick_Breaker;

public class Brick
{
    public double x { get; }
    public double y { get; }
    public double w { get; }
    public double h { get; }
    public Color color { get; }
    public int health { get; private set; }
    
    //private bool hasPowerUp;
    //private PowerUpType powerUpType;

    public Brick(double x, double y, double w, double h, Color color, int health)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
        this.color = color;
        this.health = health;
    }

    public void decreaseHealthBrick()
    {
        health--;
    }
}