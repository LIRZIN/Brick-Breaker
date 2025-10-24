using System.Drawing;

namespace Brick_Breaker;

public class BrickWallParameters
{
    private double _x, _y, _w, _h, _spaceBetweenBricks;
    private int _nbVerticalBricks,  _nbHorizontalBricks;

    public double x
    {
        get => _x;
        private set => _x = (value<0 || value>1)
                          ? throw new ArgumentOutOfRangeException(nameof(value), "The value given is invalid. ( is negative or is greater than 1)")
                          : value; 
    }

    public double y
    {
        get => _y;
        private set => _y = (value<0 || value>1)
                          ? throw new ArgumentOutOfRangeException(nameof(value), "The value given is invalid. ( is negative or is greater than 1)")
                          : value; 
    }

    public double w
    {
        get => _w;
        private set => _w = (value<=0 || value>1-x)
                          ? throw new ArgumentOutOfRangeException(nameof(value), "The value given is invalid. ( is negative or doesn't fit on the screen)")
                          : value; 
    }

    public double h
    {
        get => _h;
        private set => _h = (value<=0 || value>1-y)
                          ? throw new ArgumentOutOfRangeException(nameof(value), "The value given is invalid. ( is negative or doesn't fit on the screen)")
                          : value; 
    }
    
    public double spaceBetweenBricks
    {
        get => _spaceBetweenBricks;
        private set => _spaceBetweenBricks = (value<=0 || value>w || value>h)
                                            ? throw new ArgumentOutOfRangeException(nameof(value), "The value given is invalid. ( is negative or exceeds the screen)")
                                            : value; 
    }

    public int nbVerticalBricks
    {
        get => _nbVerticalBricks;
        private set => _nbVerticalBricks = (value<=0 || value>Utils.MAX_VERTICAL_BRICKS)
                    ? throw new ArgumentOutOfRangeException(nameof(value), "The value given is invalid. ( is negative or is greater than MAX_VERTICAL_BRICKS)")
                    : value;
    }
    public int nbHorizontalBricks 
    {
        get => _nbHorizontalBricks;
        private set => _nbHorizontalBricks = (value<=0 || value>Utils.MAX_HORIZONTAL_BRICKS)
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