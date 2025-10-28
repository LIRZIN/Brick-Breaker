using System.Drawing;

namespace Brick_Breaker;


// STRUCTS

public enum PlayerMovement
{
    Left,
    Right,
    Nothing
};

public enum Side
{
    Top,
    Bottom,
    Left,
    Right, 
    None
};

public enum BallAttribute
{
    PositionX,
    PositionY,
    DirectionX, 
    DirectionY,
    Speed,
    Radius,
    Color
};

public enum BrickWallAttribute
{
    PositionX,
    PositionY, 
    Width, 
    Height, 
    SpaceBetweenBricks,
    NbVerticalBricks,
    NbHorizontalBricks, 
    BrickCount
};

public enum BrickAttribute
{
    PositionX,
    PositionY, 
    Width, 
    Height,
    Health,
    Color
};

public enum PaddleAttribute
{
    PositionX,
    PositionY,
    Width, 
    Height,
    Speed,
    Color
};

public enum CollisionType
{
    Brick,
    Paddle,
    
    LeftWall,
    RightWall,
    TopWall,
    
    None
}

// DATA
static public class Utils
{
    public static double screenSizeWidth = 1.0; 
    public static double screenSizeHeight = 1.0;
    
    public const int MAX_VERTICAL_BRICKS = 10;
    public const int MAX_HORIZONTAL_BRICKS = 10;

    public const int MAX_RECURSIVE_COLLISION_CALL = 5;

    public static int lastBrickCollisionIndex = 0;

    public const double limit_angle_paddle_reflection = 0.01;
    
    // Brick Wall n°0
    private static double bw0_x = 0.1;
    private static double bw0_y = 0.1;
    private static double bw0_w = 0.8;
    private static double bw0_h = 0.8;
    private static int bw0_nbVerticalBricks = 2;
    private static int bw0_nbHorizontalBricks = 2;
    private static double bw0_spaceBetweenBricks = 0.01;
    private static List<int> bw0_health = new List<int>(new int[]
    {
        1, 2,
        3, 4
    });
    private static List<Color> bw0_color = new List<Color>(new Color[]
    {
        Color.Blue, Color.Aqua, 
        Color.Chocolate, Color.DarkKhaki
    });
    
    // Brick Wall n°1
    private static double bw1_x = 0.01;
    private static double bw1_y = 0.01;
    private static double bw1_w = 0.98;
    private static double bw1_h = 0.4;
    private static int bw1_nbVerticalBricks = 4;
    private static int bw1_nbHorizontalBricks = 4;
    private static double bw1_spaceBetweenBricks = 0.04;
    private static List<int> bw1_health = new List<int>(new int[]
    {
        2, 3, 3, 2,
        1, 0, 0, 1,
        1, 1, 1, 1,
        0, 1, 1, 0,
    });
    private static List<Color> bw1_color = new List<Color>(new Color[]
    {
        Color.Blue, Color.Red, Color.Blue, Color.Red,
        Color.Red, Color.Blue, Color.Red, Color.Blue, 
        Color.Blue, Color.Red, Color.Blue, Color.Red,
        Color.Red, Color.Blue, Color.Red, Color.Blue, 
    });

    private static List<ColorEnum> bw2_color = new List<ColorEnum>(new ColorEnum[]
    {
        ColorEnum.Blue, ColorEnum.Red, ColorEnum.Blue, ColorEnum.Red,
        ColorEnum.Red, ColorEnum.Blue, ColorEnum.Red, ColorEnum.Blue,
        ColorEnum.Blue, ColorEnum.Red, ColorEnum.Blue, ColorEnum.Red,
        ColorEnum.Red, ColorEnum.Blue, ColorEnum.Red, ColorEnum.Blue,
    });

    private static List<ColorEnum> bw3_color = new List<ColorEnum>(new ColorEnum[]
{
        ColorEnum.Blue, ColorEnum.Aqua,
        ColorEnum.Chocolate, ColorEnum.DarkKhaki
});

    public static BrickWallParameters getBrickWallParameters(int index)
    {
        switch( index )
        {
            case 1 : return new BrickWallParameters(bw1_x, bw1_y, bw1_w, bw1_h, bw1_nbVerticalBricks, bw1_nbHorizontalBricks, bw1_spaceBetweenBricks, bw1_health, bw2_color);
            default : return new BrickWallParameters(bw0_x, bw0_y, bw0_w, bw0_h, bw0_nbVerticalBricks, bw0_nbHorizontalBricks, bw0_spaceBetweenBricks, bw0_health, bw3_color);
        }
    }
    
    
    // METHODS

    
}