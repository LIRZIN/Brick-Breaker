namespace Brick_Breaker;

public static class CollisionChecker
{
    private static bool circleLineCollision(double ballX, double ballY, double nextBallX, double nextBallY, 
                                  double p1X, double p1Y, double p2X, double p2Y, 
                                  ref double collisionX, ref double collisionY, ref double collisionU )
    {
        double dBallX = nextBallX -  ballX;
        double dBallY = nextBallY -  ballY;
        double dLineX = p2X - p1X;
        double dLineY = p2Y - p1Y;
        
        double denominator = dLineY * dBallX - dLineX * dBallY;
        if( !(-0.00001 < denominator && denominator < 0.00001) )
        {
            double dy = ballY - p1Y;
            double dx = ballX - p1X;
            
            double u_x = (dLineX * dy - dLineY * dx) / denominator;
            double u_y = (dBallX * dy - dBallY * dx) / denominator;
            
            if (0 <= u_x && u_x <= 1 && 0 <= u_y && u_y <= 1)
            {
                collisionX = ballX + u_x * dBallX;
                collisionY = ballY + u_x * dBallY;
                collisionU = u_x;
                return true;
            }
        }

        return false;
    }

    public static bool circleRectCollision(double ballX, double ballY, double ballR, double dx, double dy,
                                     double rectX, double rectY, double rectW, double rectH,
                                     ref double collisionX, ref double collisionY, ref double collisionU, ref Side side)
    {
        bool collision = false;
        double tmpCollisionX = 0;
        double tmpCollisionY = 0;
        double tmpCollisionU = 0;
        
        double minRectX = rectX - ballR;
        double minRectY = rectY - ballR;
        double maxRectX = rectX + rectW + ballR;
        double maxRectY = rectY + rectH + ballR;
        
        if (dx < 0)
        {
            if( circleLineCollision( ballX, ballY, ballX+dx, ballY+dy, 
                           maxRectX, minRectY, maxRectX, maxRectY, 
                           ref tmpCollisionX, ref tmpCollisionY, ref tmpCollisionU )
               && tmpCollisionU < collisionU )
            {
                collision = true;
                collisionX = tmpCollisionX;
                collisionY = tmpCollisionY;
                collisionU = tmpCollisionU;
                side = Side.Right;
            }
        }
        else
        {
            if( circleLineCollision( ballX, ballY, ballX+dx, ballY+dy, 
                           minRectX, minRectY, minRectX, maxRectY, 
                           ref tmpCollisionX, ref tmpCollisionY, ref tmpCollisionU )
                && tmpCollisionU < collisionU )
            {
                collision = true;
                collisionX = tmpCollisionX;
                collisionY = tmpCollisionY;
                collisionU = tmpCollisionU;
                side = Side.Left;
            }
        }

        if (collision)
        {
            return true;
        }
        
        if (dy < 0)
        {
            if( circleLineCollision( ballX, ballY, ballX+dx, ballY+dy, 
                           minRectX, maxRectY, maxRectX, maxRectY, 
                           ref tmpCollisionX, ref tmpCollisionY, ref tmpCollisionU )
                && tmpCollisionU < collisionU )
            {
                collision = true;
                collisionX = tmpCollisionX;
                collisionY = tmpCollisionY;
                collisionU = tmpCollisionU;
                side = Side.Bottom;
            }
        }
        else
        {
            if( circleLineCollision( ballX, ballY, ballX+dx, ballY+dy, 
                           minRectX, minRectY, maxRectX, minRectY, 
                           ref tmpCollisionX, ref tmpCollisionY, ref tmpCollisionU )
                && tmpCollisionU < collisionU )
            {
                collision = true;
                collisionX = tmpCollisionX;
                collisionY = tmpCollisionY;
                collisionU = tmpCollisionU;
                side = Side.Top;
            }
        }

        return collision;
    }

    public static bool circleWallsCollision( double ballX, double ballY, double ballR, double dx, double dy,
                                           ref double collisionX, ref double collisionY, ref double collisionU, ref Side side )
    {
        bool collision = false;
        double tmpCollisionX = 0;
        double tmpCollisionY = 0;
        double tmpCollisionU = 0;

        double minX = ballR;
        double minY = ballR;
        double maxX = Data.screenSizeWidth - ballR;
        double maxY = Data.screenSizeHeight - ballR;
        
        if( circleLineCollision( ballX, ballY, ballX+dx, ballY+dy, 
                               maxX, maxY, maxX, minY,
                               ref tmpCollisionX, ref tmpCollisionY, ref tmpCollisionU )
            && tmpCollisionU < collisionU )
        {
            collision = true;
            collisionX = tmpCollisionX;
            collisionY = tmpCollisionY;
            collisionU = tmpCollisionU;
            side = Side.Right;
        }
        else if( circleLineCollision( ballX, ballY, ballX+dx, ballY+dy, 
                                    minX, maxY, minX, minY,
                                    ref tmpCollisionX, ref tmpCollisionY, ref tmpCollisionU )
                 && tmpCollisionU < collisionU )
        {
            collision = true;
            collisionX = tmpCollisionX;
            collisionY = tmpCollisionY;
            collisionU = tmpCollisionU;
            side = Side.Left;
        }
        else if( circleLineCollision( ballX, ballY, ballX+dx, ballY+dy, 
                                    minX, minY, maxX, minY,
                                    ref tmpCollisionX, ref tmpCollisionY, ref tmpCollisionU )
                 && tmpCollisionU < collisionU )
        {
            collision = true;
            collisionX = tmpCollisionX;
            collisionY = tmpCollisionY;
            collisionU = tmpCollisionU;
            side = Side.Top;
        }
        
        return collision;
    }
}