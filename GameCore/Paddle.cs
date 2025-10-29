using System.Drawing;

namespace Brick_Breaker;

public class Paddle
{
    public double x { get; private set; }
    public double y { get; private set; }
    public double w { get; private set; }
    public double h { get; private set; }
    public double v { get; internal set; }

    
    public ColorEnum color { get; private set; }
    
    public void init()
    {
        w = Utils.screenSizeWidth/5.0;
        h = Utils.screenSizeHeight/20.0;
        x = (Utils.screenSizeWidth - w) / 2.0;
        y = Utils.screenSizeHeight - h;
        v = 5;
        color = ColorEnum.Gray;
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
        else if (x + w >= Utils.screenSizeWidth)
        {
            x = Utils.screenSizeWidth - w;
        }
    }
}