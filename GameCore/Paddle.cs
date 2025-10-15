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
    
    public void init()
    {
        w = Data.screenSizeWidth/5.0;
        h = Data.screenSizeHeight/20.0;
        x = (Data.screenSizeWidth - w) / 2.0;
        y = Data.screenSizeHeight - h;
        v = 5;
        color = Color.Gray;
    }

    public void update(double deltaTime, PlayerMovement move)
    {
        double speed_factor;
        switch (move)
        {
            case PlayerMovement.Left: speed_factor = -1; break;
            case PlayerMovement.Right: speed_factor = 1; break;
            default: return;
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