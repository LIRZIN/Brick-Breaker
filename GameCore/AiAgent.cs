namespace Brick_Breaker;
using System.Linq;
internal class AiAgent
{
    private Random rand;
    public AIBehavior Behaviour { get; }
    public bool IsReady = false;

    public AiAgent(AIBehavior behaviour)
    {
        Behaviour = behaviour;
        rand = new Random();
        Setup();
    }

    MLP mlp;
    LM lm;

    public void Setup()
    {
        switch (Behaviour)
        {
            case AIBehavior.LM:
                lm ??= new LM(54);
                lm.Train(10000, 0.01f);
                break;
            case AIBehavior.MLP:
                mlp ??= new MLP([54, 27, 2]);
                mlp.Train(10000, 0.01f);
                break;
        }
    }

    public PlayerMovement Predict(double ballPosX, double ballPosY, double ballVelX, double ballVelY, double paddlePosX, int[] bricks)
    {
        var fBricks = bricks.Select(b => (float)b).ToArray();
        var inputs = new float[] { (float)ballPosX, (float)ballPosY, (float)ballVelX, (float)ballVelY, (float)paddlePosX };
        inputs = [.. inputs, .. fBricks];

        switch (Behaviour)
        {
            case AIBehavior.Random:
                return (PlayerMovement)rand.Next(0, 3);

            case AIBehavior.FollowBall:
                return FollowBall(paddlePosX, ballPosX);

            case AIBehavior.LM:
                if (lm == null)
                    break;
                return GetMovementFromOutputs(lm.Predict(inputs));

            case AIBehavior.MLP:
                if (mlp == null)
                    break;
                return GetMovementFromOutputs(mlp.Predict(inputs));
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

    private static PlayerMovement GetMovementFromOutputs(float[] outputs)
    {
        if (outputs.Length != 2)
            return PlayerMovement.Nothing;

        //case [-1,-1] || [1,1]
        if (outputs[0] == outputs[1])
            return PlayerMovement.Nothing;

        if (outputs[0] == 1)
            return PlayerMovement.Left;
        if(outputs[1] == 1)
            return PlayerMovement.Right;

        return PlayerMovement.Nothing;
    }
}