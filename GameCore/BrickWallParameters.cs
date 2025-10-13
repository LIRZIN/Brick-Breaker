using System.Drawing;

namespace Brick_Breaker;

public class BrickWallParameters
{
    private double _x, _y, _w, _h, _spaceBetweenBricks;
    private int _nbVerticalBricks,  _nbHorizontalBricks;
    private const int MAX_VERTICAL_BRICKS = 10;
    private const int MAX_HORIZONTAL_BRICKS = 10;

    public double x
    {
        get => _x;
        private set => _x = value; // check if negative or y > screen_space.w ...
    }

    public double y
    {
        get => _y;
        private set => _y = value; // check if negative or x > screen_space.h ...
    }

    public double w
    {
        get => _w;
        private set => _w = value; // check if w <= screen_space.w
    }

    public double h
    {
        get => _h;
        private set => _h = value; // check if h <= screen_space.h
    }
    
    public double spaceBetweenBricks
    {
        get => _spaceBetweenBricks;
        private set => _spaceBetweenBricks = value; // Check if negative or more than min( W/2, H/2 )
    }

    public int nbVerticalBricks
    {
        get => _nbVerticalBricks;
        private set => _nbVerticalBricks = (value<=0 || value>MAX_VERTICAL_BRICKS)
                    ? throw new ArgumentOutOfRangeException(nameof(value), "The value given is invalid. ( is negative or is greater than MAX_VERTICAL_BRICKS)")
                    : value;
    }
    public int nbHorizontalBricks 
    {
        get => _nbHorizontalBricks;
        private set => _nbHorizontalBricks = (value<=0 || value>MAX_HORIZONTAL_BRICKS)
                    ? throw new ArgumentOutOfRangeException(nameof(value), "The value given is invalid. ( is negative or is greater than MAX_HORIZONTAL_BRICKS)")
                    : value;
    }
    
    public List<int> brickHealth { get; }
    public List<Color> brickColor { get; }
    public BrickWallParameters(double x, double y, double w, double h, int nbVerticalBricks, int nbHorizontalBricks, double spaceBetweenBricks, List<int> brickHealth, List<Color> brickColor)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
        this.nbVerticalBricks = nbVerticalBricks;
        this.nbHorizontalBricks = nbHorizontalBricks;
        this.spaceBetweenBricks = spaceBetweenBricks;
        this.brickHealth = brickHealth;
        this.brickColor = brickColor;
    }
}