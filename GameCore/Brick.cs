using System.Drawing;

namespace Brick_Breaker;

public class Brick
{
    public double x { get; }
    public double y { get; }
    public double W { get; }
    public double H { get; }
    public Color color; 
    public int vie;
    //private bool hasPowerUp;
    //private PowerUpType powerUpType;

    public Brick(double x, double y, double w, double h, Color color, int vie)
    {
        this.x = x;
        this.y = y;
        this.W = w;
        this.H = h;
        this.color = color;
        this.vie = vie;
    }
}