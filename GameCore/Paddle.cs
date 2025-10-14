using System.Drawing;

namespace Brick_Breaker;

public class Paddle
{
    public double x { get; private set; }
    public double y { get; private set; }
    public double w { get; private set; }
    public double h { get; private set; }
    public double v { get; private set; }
    public Color color { get; private set; }

    public Paddle()
    {
        reset();
    }

    public void reset()
    {
        w = 0.2;
        h = 0.05;
        x = Data.screenSizeWidth - w / 2.0;
        y = Data.screenSizeHeight - h - 0.05;
        v = 0.03;
        color = Color.Gray;
    }

    public void update(double deltaTime, PlayerMovement move)
    {
        double speed_factor;
        switch (move)
        {
            case PlayerMovement.Left: speed_factor = -1; break;
            case PlayerMovement.Right: speed_factor = 1; break;
            default: speed_factor = 0; break;
        }
        
        x += deltaTime * v * speed_factor;

        if (x <= 0)
        {
            x = 0; 
        }
        else if (x + w >= Data.screenSizeWidth)
        {
            x = Data.screenSizeWidth - w;
        }
    }
}