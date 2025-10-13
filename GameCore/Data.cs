using System.Drawing;

namespace Brick_Breaker;

static public class Data
{
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

    public static BrickWallParameters brickWallParameters_0 = new BrickWallParameters(bw0_x, bw0_y, bw0_w, bw0_h,
        bw0_nbVerticalBricks, bw0_nbHorizontalBricks, bw0_spaceBetweenBricks, bw0_health, bw0_color);
    
    // Brick Wall n°1
    private static double bw1_x = 0;
    private static double bw1_y = 0;
    private static double bw1_w = 1;
    private static double bw1_h = 1;
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

    public static BrickWallParameters brickWallParameters_1 = new BrickWallParameters(bw1_x, bw1_y, bw1_w, bw1_h,
        bw1_nbVerticalBricks, bw1_nbHorizontalBricks, bw1_spaceBetweenBricks, bw1_health, bw1_color);
}