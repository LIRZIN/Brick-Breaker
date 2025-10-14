namespace Brick_Breaker;

public enum PlayerMovement
{
    Left,
    Right,
    Nothing
};

public enum Side
{
    Top,
    Bottom,
    Left,
    Right, 
    None
};

public enum BallAttribute
{
    PositionX,
    PositionY,
    DirectionX, 
    DirectionY,
    Speed,
    Radius,
    Color
};

public enum BrickWallAttribute
{
    PositionX,
    PositionY, 
    Width, 
    Height, 
    SpaceBetweenBricks,
    NbVerticalBricks,
    NbHorizontalBricks, 
    BrickCount
};

public enum BrickAttribute
{
    PositionX,
    PositionY, 
    Width, 
    Height,
    Health,
    Color
};

public enum PaddleAttribute
{
    PositionX,
    PositionY,
    Width, 
    Height,
    Speed,
    Color
};