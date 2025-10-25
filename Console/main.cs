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
        display.Init(150,25);
        int refreshRate = 60;
        
        while (!display.BrickBreaker.IsGameWon && !display.BrickBreaker.IsGameOver)
        {
            System.Console.Clear();
            display.Update( 1.0/(float)refreshRate );
            display.DrawGame();
            System.Threading.Thread.Sleep(10000/refreshRate);
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

