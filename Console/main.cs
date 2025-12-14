using Brick_Breaker;

namespace Console;

public class main
{
    static void Main()
    {
        //GenerateWeightsStuff();
        //return;

        ConsoleInput.beginListening();

        ConsoleDisplay display = new ConsoleDisplay();
        int refreshRate = 60;

        System.Console.WriteLine("If you want to train press 'r', if you want to play press any other letter");
        char temp = new char();
        temp = System.Console.ReadKey().KeyChar;
        AIBehavior aiBehaviour = AIBehavior.LM;

        if (temp == 'r')
        {
            display.Init(150, 25, aiBehaviour, true);
            while (!ConsoleInput.isQuit)
            {
                if (display.BrickBreaker.IsGameWon || display.BrickBreaker.IsGameOver)
                {
                    display.Init(150, 25, aiBehaviour, true);
                }
                System.Console.Clear();
                display.Update(1.0 / (float)refreshRate);
                display.DrawGame();
                System.Threading.Thread.Sleep(10000 / refreshRate);
            }
        }
        else
        {
            display.Init(150, 25, aiBehaviour, false);
            while (!display.BrickBreaker.IsGameWon && !display.BrickBreaker.IsGameOver)
            {
                System.Console.Clear();
                display.Update(1.0 / (float)refreshRate);
                display.DrawGame();
                System.Threading.Thread.Sleep(10000 / refreshRate);
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

    private static void GenerateWeightsStuff()
    {
        bool isMLP = false;
        bool isTesting = true;
        bool isTraining = false;
        Files files = new();
        #region LM & MLP stuff
        #region Train LM
        if (isTraining && !isMLP) 
        {
            LM lmLeft = new LM(54);
            LM lmRight = new LM(54);
            lmLeft.SetUsedForClassification(true);
            lmRight.SetUsedForClassification(true);

            
            // Call the method to read and process the CSV data
            files.csvData = files.ReadCsvFile(files.datasetPath);

            files.AddDatasetToLm(lmLeft, true);
            //lmLeft.Print(false, true, true, false);
            files.AddDatasetToLm(lmRight, false);
            //lmRight.Print(false, true, true, false);

            lmLeft.Train(1000000, 0.01f, 1000);
            lmRight.Train(1000000, 0.01f, 1000);

            //lmLeft.Print(false, true, true, false);
            //lmRight.Print(false, true, true, false);

            files.WriteWeightFromLmToCSV(lmRight, false);
            files.WriteWeightFromLmToCSV(lmLeft, true);

            for (int i = 0; i < lmLeft.MSESize(); i++)
            {
                System.Console.WriteLine("Left " + i + " : " + lmLeft.GetMSE(i) * 25 + "%");
            }
            for (int i = 0; i < lmRight.MSESize(); i++)
            {
                System.Console.WriteLine("Right " + i + " : " + lmRight.GetMSE(i) * 25 + "%");
            }

            System.Console.WriteLine("success for left " + lmLeft.Test() + " %");
            System.Console.WriteLine("success for right " + lmRight.Test() + " %");
        }

        #endregion

        #region Train MLP
        if (isTraining && isMLP)
        {
            files.csvData = files.ReadCsvFile(files.datasetPath);
            MLP mlp = new([54, 108, 54, 27, 2]);
            mlp.SetUsedForClassification(true);
            files.AddDatasetToMlp(mlp);
            mlp.Train(1000000, 0.01f, 1000);
            files.WriteWeightFromMlpToCSV(mlp);

            for (int i = 0; i < mlp.MSESize(); i++)
            {
                System.Console.WriteLine("Left " + i + " : " + mlp.GetMSE(i) * 25 + "%");
            }

            System.Console.WriteLine("success for mlp " + mlp.Test() + " %");
        }
        #endregion

        #region Test LM
        if (isTesting && !isMLP)
        {
            LM lmL = new (54); 
            LM lmR = new (54);
            files.csvWeight = files.ReadCsvFile(files.weightPathL);
            files.SetWeightForLm(lmL);
            files.csvWeight = files.ReadCsvFile(files.weightPathR);
            files.SetWeightForLm(lmR);
            float[] input = { 0, 0, 1.7163222666290556f, 1.2451554414004138f, 0.5324266333873037f, -0.846476154454372f, 1.3388922222222217f, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var l = lmL.Predict(input);
            var r = lmR.Predict(input);
            System.Console.WriteLine("Left prediction : " + l);
            System.Console.WriteLine("Right prediction : " + r);
        }
        #endregion

        #region Test MLP
        if (isTesting && isMLP)
        {
            MLP mlp = files.CreateMlpFromCsv();
            files.csvWeight = files.ReadCsvFile(files.mlpWeightsPaths);
        }
        #endregion

            //Console.WriteLine(File.GetLastWriteTime("ML_Lib.dll"));




            /*
            Files files = new Files();
            // Call the method to read and process the CSV data
            files.csvData = Files.ReadCsvFile(files.datasetPath);
            //files.csvWeight = Files.ReadCsvFile(files.weightPath);

            //int[] layers = { 54, 108, 54, 27, 2 };
            int[] layers = { 2, 3, 1 };

            MLP mlp = new MLP(layers);
            mlp.SetUsedForClassification(true);

            //files.AddDatasetToMlp(mlp);

            mlp.InitElements(4);
            float[] test1 = { 0, 0, -1 };
            float[] test2 = { 1, 0, 1 };
            float[] test3 = { 0, 1, 1 };
            float[] test4 = { 1, 1, -1 };
            mlp.AddElement(test1);
            mlp.AddElement(test2);
            mlp.AddElement(test3);
            mlp.AddElement(test4);

            mlp.Train(1000000, 0.01f, 10000);


            //float[] test5 = files.csvData[3];
            //float[] values = test5.Skip(2).ToArray();
            Console.WriteLine("success rate : " + mlp.Test() + "%");

            for (int i = 0; i < mlp.MSESize(); i++)
            {
                Console.WriteLine("MSE : " + mlp.GetMSE(i));
            }

            mlp.Print(12);

            Console.WriteLine("\n PREDICT \n\n");

            float[] result1 = mlp.Predict(test2);
            Console.WriteLine("resultat : ");
            for (int i = 0; i < mlp.GetNbOutputNeurons(); i++)
            {
               Console.WriteLine(result1[i] + " ");
            }

            mlp.Print(12);
            files.WriteWeightFromMlpToCSV(mlp);
            */



            //Files files = new Files();
            //files.csvWeight = Files.ReadCsvFile(files.weightPath);
            //MLP mlp = files.CreateMlpFromCsv();
            //mlp.SetUsedForClassification(true);

            //mlp.InitElements(1);

            //mlp.Print(12);

            //System.Console.WriteLine("\n SET WEIGHT AND TEST \n\n");

            //files.SetWeightForMlp(mlp);
            //float[] test2 = { 0.8f, 0, 1 };
            //mlp.AddElement(test2);
            //System.Console.WriteLine("success rate : " + mlp.Test() + "%");

            //mlp.Print(12);

            //System.Console.WriteLine("\n PREDICT \n\n");

            //float[] result1 = mlp.Predict(test2);

            //System.Console.WriteLine("resultat : ");
            //for (int i = 0; i < mlp.GetNbOutputNeurons(); i++)
            //{
            //    System.Console.WriteLine(result1[i] + " ");
            //}

            //mlp.Print(12);
            #endregion
    }

}

