using System;
using System.Drawing;
using System.Text;
using Brick_Breaker;

namespace Console;

public class Class1
{
    static void Main()
    {
        // Brick b = new Brick(10, 0, 0.1, 0.1, Color.FromName("Blue"), 1);
        BrickWall brickWall = new BrickWall(1);
        brickWall.print_values();
        System.Console.WriteLine("Hello World!");
    }
}

