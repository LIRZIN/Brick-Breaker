using System;
using System.Drawing;
using System.Numerics;
using System.Text;
using Brick_Breaker;

namespace Console;

public class main
{
    static void Main()
    {
        BrickWall brickWall = new BrickWall();
        brickWall.init(1);
        brickWall.print_values();

        /*
        Ball ball = new Ball(new Vector2((float)0.2, (float)0.4), new Vector2(0.2f, 0.2f) , 10, 4, Color.Aqua);
        ball.Afficher();
        for (int i = 0; i < 1000; i++)
        {
            System.Threading.Thread.Sleep(100);
            ball.Update(0.005);
            ball.Afficher();
        }*/
    }
}

