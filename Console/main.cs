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
        ConsoleInput.beginListening();
        
        ConsoleDisplay display = new ConsoleDisplay();
        int refreshRate = 60;

        System.Console.WriteLine("If you want to train press 'r', if you want to play press any other letter");
        char temp = new char();
        temp = System.Console.ReadKey().KeyChar;

        if (temp == 'r')
        {
            display.Init(150, 25, true);
            while (!ConsoleInput.isQuit)
            {
                if (display.BrickBreaker.IsGameWon || display.BrickBreaker.IsGameOver)
                {
                    display.Init(150,25, true);
                }
                System.Console.Clear();
                display.Update( 1.0/(float)refreshRate );
                display.DrawGame();
                System.Threading.Thread.Sleep(10000/refreshRate);
            }
        }
        else
        {
            display.Init(150, 25, false);
            while (!display.BrickBreaker.IsGameWon && !display.BrickBreaker.IsGameOver)
            {
                System.Console.Clear();
                display.Update( 1.0/(float)refreshRate );
                display.DrawGame();
                System.Threading.Thread.Sleep(10000/refreshRate);
            }
        }

        if (display.BrickBreaker.IsGameWon)
        {
            display.DrawWin();
        }
        else if (display.BrickBreaker.IsGameOver)
        {
            display.DrawLose();
        }
        
        ConsoleInput.stopListening();
    }
}

