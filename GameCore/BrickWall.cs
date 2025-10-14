using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Brick_Breaker;

public class BrickWall
{
    private List<BrickWallParameters> parameters = new List<BrickWallParameters>();                                                                     
    private List<Brick> bricks = new List<Brick>();
    private int _currentBrickWall;   

    public int currentBrickWall
    {
        get => _currentBrickWall;
        set
        {
            if(value < 0 || value >= parameters.Count )
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    "The value given is invalid. ( is negative or is greater than the number of brick wall parameter sets)");
            }
            
            _currentBrickWall = value;
            buildWall();
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
    public int brickCount { get => bricks.Count; }

    public void init(int index)
    {
        buildParameters();
        currentBrickWall = index;
    }

    public Brick getBrick(int index)
    {
        return bricks[index];
    }

    public void decreaseHealthBrick(int index)
    {
        bricks[index].decreaseHealthBrick();
    }

    private void buildParameters()
    {
        parameters.Clear();
        parameters.Add( Data.getBrickWallParameters(0));
        parameters.Add( Data.getBrickWallParameters(1));
    }

    private void buildWall()
    {
        bricks.Clear();
        int index = 0;
        double building_width = w + spaceBetweenBricks;
        double building_height = h + spaceBetweenBricks;

        for (int i = 0; i < nbVerticalBricks; i++)
        {
            for (int j = 0; j < nbHorizontalBricks; j++, index++)
            {
                if (brickHealth[index] <= 0)
                {
                    continue;
                }
                
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
            System.Console.WriteLine("(w, h) : " + bricks[i].w + ", " + bricks[i].h);
            System.Console.WriteLine("(x, y, next_x, next_y, color, health ):" + bricks[i].x + ", " + bricks[i].y + ", " + ( bricks[i].x + bricks[i].w ) + ", " + ( bricks[i].y +bricks[i].h ) + ", " + bricks[i].color + ", " + bricks[i].health + "\n");
        }
    }
}