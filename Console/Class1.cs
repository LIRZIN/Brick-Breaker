using System;
using System.Drawing;
using System.Numerics;
using System.Text;
using Brick_Breaker;

namespace Console;

public class Class1
{
    static void Main()
    {
        Brick b = new Brick(10, 0, 0.1, 0.1, Color.FromName("Blue"), 1);
        System.Console.WriteLine("Hello World!");
        System.Console.WriteLine(b.x);

        Ball ball = new Ball(0.2, 0.4, 0.2, 0.2, 10, 4, Color.Aqua);
        ball.Afficher();
        for (int i = 0; i < 1000; i++)
        {
            System.Threading.Thread.Sleep(100);
            ball.Update(0.005);
            ball.Afficher();
        }
    }
}

