using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Brick_Breaker;

public class BrickWall
{
    private List<BrickWallParameters> parameters = new List<BrickWallParameters>{ Data.brickWallParameters_0, Data.brickWallParameters_1 };                                                                     
    private List<Brick> bricks = new List<Brick>();
    private int _currentBrickWall;   

    private int currentBrickWall
    {
        get => _currentBrickWall;
        set
        {
            if(value < 0 || value >= parameters.Count )
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    "The value given is invalid. ( is negative or is greater than MAX_VERTICAL_BRICKS)");
            }
            
            _currentBrickWall = value;
            build();
        }
    }
    
    public double x { get => parameters[currentBrickWall].x; }
    public double y { get => parameters[currentBrickWall].y; }
    public double w { get => parameters[currentBrickWall].w; }
    public double h { get => parameters[currentBrickWall].h; }
    
    public double spaceBetweenBricks { get => parameters[currentBrickWall].spaceBetweenBricks; }

    public int nbVerticalBricks { get => parameters[currentBrickWall].nbVerticalBricks; }
    public int nbHorizontalBricks { get => parameters[currentBrickWall].nbHorizontalBricks; }
    public List<int> brickHealth { get => parameters[currentBrickWall].brickHealth; }
    public List<Color> brickColor { get => parameters[currentBrickWall].brickColor; }

    public BrickWall( int index )
    {
        currentBrickWall = index;
    }

    private void build()
    {
        bricks.Clear();
        int index = 0;
        double building_width = w + spaceBetweenBricks;
        double building_height = h + spaceBetweenBricks;

        for (int i = 0; i < nbVerticalBricks; i++)
        {
            for (int j = 0; j < nbHorizontalBricks; j++, index++)
            {
                double position_x = ((double)j / (double)(nbHorizontalBricks)) * building_width + spaceBetweenBricks/2.0;
                double position_y = ((double)i / (double)(nbVerticalBricks)) * building_height + spaceBetweenBricks/2.0;
                double next_position_x = ((double)(j+1) / (double)(nbHorizontalBricks)) * building_width - spaceBetweenBricks/2.0;
                double next_position_y = ((double)(i+1) / (double)(nbVerticalBricks)) * building_height - spaceBetweenBricks/2.0;
                
                bricks.Add( new Brick(x + position_x - spaceBetweenBricks/2.0, y + position_y - spaceBetweenBricks/2.0, next_position_x - position_x, next_position_y - position_y, brickColor[index], brickHealth[index] ) );
            }
        }
    }

    public void print_values()
    {
        System.Console.WriteLine("( x, y ) : " + x + ", " + y);
        System.Console.WriteLine("( w, h ) : " + w + ", " + h);
        System.Console.WriteLine("( nbVertical, nbHorizontal ) : " + nbVerticalBricks + ", " + nbHorizontalBricks);
        System.Console.WriteLine("Space Between Bricks: " + spaceBetweenBricks + "\n\n");

        for( int i = 0; i < bricks.Count; i++ )
        {
            System.Console.WriteLine("Brick n°" + i);
            System.Console.WriteLine("(x, y, next_x, next_y, color, health ):" + bricks[i].x + ", " + bricks[i].y + ", " + ( bricks[i].x + bricks[i].w ) + ", " + ( bricks[i].y +bricks[i].h ) + ", " + bricks[i].color + ", " + bricks[i].health + "\n");
        }
    }
}