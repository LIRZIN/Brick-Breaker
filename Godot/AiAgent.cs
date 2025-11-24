using Brick_Breaker;
using System;

public enum AIBehavior
{
    Random,
    FollowBall,
    MachineLearning,
    None,
}

internal class AiAgent
{
    private Random rand;
    public AIBehavior Behaviour { get; }

    public AiAgent(AIBehavior behaviour)
    {
        Behaviour = behaviour;
        rand = new Random();
    }

    public PlayerMovement Predict(double ballPosX, double ballPosY, double ballVelX, double ballVelY, double paddlePosX, int[] bricks)
    {
        switch (Behaviour)
        {
            case AIBehavior.Random:

                int move = rand.Next(0, 3);
                return (PlayerMovement)move;

            case AIBehavior.FollowBall:
                return FollowBall(paddlePosX, ballPosX);
        }
        return PlayerMovement.Nothing;
    }

    private PlayerMovement FollowBall(double paddleX, double ballX)
    {
        if (ballX < paddleX)
        {
            return PlayerMovement.Left;
        }
        else if (ballX > paddleX)
        {
            return PlayerMovement.Right;
        }
        else
        {
            return PlayerMovement.Nothing;
        }
    }
}