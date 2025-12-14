namespace Brick_Breaker;
using System.Linq;
internal class AiAgent
{
    public bool IsReady { get; private set; }

    private Random rand;
    private MLP mlp;
    private LM lmL, lmR;
    private Files f;

    public AIBehavior Behaviour { get; }


    public AiAgent(AIBehavior behaviour)
    {
        Behaviour = behaviour;
        rand = new Random();
        Setup();
    }

    public void Setup()
    {
        Logger.Write($"Setting up AI Agent with behaviour: {Behaviour}");
        f = new Files();
        switch (Behaviour)
        {
            case AIBehavior.LM:
                lmL = new(54);
                lmR = new(54);
                f.csvWeight = f.ReadCsvFile(f.weightPathL);
                //Logger.Write($"Weights read from {f.weightPathL}, count: {f.csvWeight.Count}");
                f.SetWeightForLm(lmL);
                f.csvWeight = f.ReadCsvFile(f.weightPathR);
                f.SetWeightForLm(lmR);
                //float[] input = { 0, 0, 1.7163222666290556f, 1.2451554414004138f, 0.5324266333873037f, -0.846476154454372f, 1.3388922222222217f, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //var l = lmL.Predict(input);
                //var r = lmR.Predict(input);
                //Logger.Write($"Test LM : l = {l}, r = {r}");
                break;
            case AIBehavior.MLP:
                f.csvWeight = f.ReadCsvFile(f.mlpWeightsPaths);
                mlp ??= f.CreateMlpFromCsv();
                f.SetWeightForMlp(mlp);
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
                if(lmL == null || lmR == null)
                {
                    Logger.WriteError("LM models are not initialized.");
                    break;
                }
                    
                var l = lmL.Predict(inputs);
                var r = lmR.Predict(inputs);
                return GetMovementFromOutputs([l, r]);

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
        //if (outputs.Length != 2)
        //    return PlayerMovement.Nothing;

        //Logger.Write("--------------------------------------------------");
        //Logger.Write("Evaluating outputs for movement prediction...");
        //Logger.Write("Outputs: " + string.Join(", ", outputs));
        if (outputs[0] == outputs[1])
        {
            //Logger.Write("Predicted Move: Nothing");
            return PlayerMovement.Nothing;
        }
            

        if (outputs[0] == 1)
        {
            //Logger.Write("Predicted Move: Left");
            return PlayerMovement.Left;
        }

        else if (outputs[1] == 1)
        {
            //Logger.Write("Predicted Move: Right");
            return PlayerMovement.Right;
        }

        //Logger.Write("Predicted Move: Nothing! No cases were handled");
        return PlayerMovement.Nothing;
    }
}