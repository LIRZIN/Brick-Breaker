using System.Globalization;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Brick_Breaker;

public class Files
{
    public string datasetPath = "Data/dataset_33_games_randomized.csv";
    public string weightPath = "Data/weight_data_MLP_10-12-01-38-21.csv";
    public List<float[]> csvData;
    public List<float[]> csvWeight;
    private FileStream _stream;
    private StreamWriter _writer;
    
    public static List<float[]> ReadCsvFile(string csvPath)
    {
        List<float[]> rows = new List<float[]>();

        try
        {
            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines(csvPath);

            // Process each line and split by the comma to get individual values
            foreach (string line in lines.Skip(1))
            {
                float[] values = line.Split(';').Select(p => float.Parse(p, CultureInfo.InvariantCulture)).ToArray();

                // Add the values to the list of rows
                rows.Add(values);
            }

            Console.WriteLine("CSV file read successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return rows;
    }

    //LM Functions
    
    public void AddDatasetToLm(LM lm, bool isLeftInput)
    {
        lm.InitElements(csvData.Count);
        foreach (var line in csvData)
        {
            float input = isLeftInput ? (line[0] == 0 ? -1 : 1)  : (line[1] == 0 ? -1 : 1);
            float[] values = line.Skip(2).Append(input).ToArray();
            lm.AddElement(values);
        }
    }
    
    public void WriteWeightFromLmToCSV(LM lm)
    {
        string fileName = $"weight_data_LM_{DateTime.Now:dd-MM-hh-mm-ss}.csv";
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;

        if (!Directory.Exists(baseDir + "\\Data"))
        {
            Directory.CreateDirectory(baseDir + "\\Data");
        }
        
        string path = Path.Combine(baseDir + "\\Data", fileName);
        
        _stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        _writer = new StreamWriter(_stream);
        
        //First line
        int L = lm.GetNbInputNeurons();
        _writer.WriteLine($"NbOfParameters;{L}");
        _writer.Flush();
        

        var sb = new StringBuilder();
        for (int i = 0; i <= L; i++)
        {
            sb.Append(lm.GetWeight(i));
            if (i < L)
            {
                sb.Append(";");
            }
        }
        
        _writer.WriteLine(sb.ToString());
        _writer.Flush();
    }
    
    public void SetWeightForLm(LM lm)
    {
        float[] line = csvWeight[0];
        lm.SetWeights(line);
    }
    
    
    //MLP Functions
    
    public void AddDatasetToMlp(MLP mlp)
    {
        mlp.InitElements(csvData.Count);
        foreach (var line in csvData)
        {
            float[] inputs = new float[2];
            inputs[0] = line[0] == 0 ? -1 : 1;
            inputs[1] = line[1] == 0 ? -1 : 1;
            float[] values = line.Skip(2).Append(inputs[0]).Append(inputs[1]).ToArray();
            mlp.AddElement(values);
        }
    }

    public void WriteWeightFromMlpToCSV(MLP mlp)
    {
        string fileName = $"weight_data_MLP_{DateTime.Now:dd-MM-hh-mm-ss}.csv";
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;

        if (!Directory.Exists(baseDir + "\\Data"))
        {
            Directory.CreateDirectory(baseDir + "\\Data");
        }
        
        string path = Path.Combine(baseDir + "\\Data", fileName);
        
        _stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        _writer = new StreamWriter(_stream);
        
        //First line
        _writer.WriteLine($"NbOfLayers;NbOfNeuronsInEachLayer");
        _writer.Flush();
        
        int L = mlp.GetL();
        var layers = "";
        for (int i = 0; i < L; i++)
        {
            layers += $";{mlp.GetD(i)}";
        }
        
        //Second line
        _writer.WriteLine($"{L}{layers}");
        _writer.Flush();
        
        for( int l = 1; l < L; l++ )
        {
            var sb = new StringBuilder();
            for( int i = 1; i <= mlp.GetD(l); i++ )
            {
                for (int j = 0; j <= mlp.GetD(l - 1); j++)
                {
                    sb.Append(mlp.GetW(l, j, i));
                    if (j < mlp.GetD(l - 1) || i < mlp.GetD(l))
                    {
                        sb.Append(";");
                    }
                }
            }
            _writer.WriteLine(sb.ToString());
            _writer.Flush();
        }
    }

    //retourne un MLP avec les poids déjà paramétrés
    public MLP CreateMlpFromCsv()
    {
        float[] temp = csvWeight[0];
        csvWeight.RemoveAt(0);
        int L = (int)temp[0];
        int[] layers = new int[L];
        for (int n = 0; n < L; n++)
        {
            layers[n] = (int)temp[n+1];
        }
        
        MLP mlp = new MLP(layers);
        
        return mlp;
    }
    
    public void SetWeightForMlp(MLP mlp)
    {
        int L = mlp.GetL();
        for( int l = 1; l < L; l++ )
        {
            float[] line = csvWeight[l-1];
            int index = 0;
            for( int i = 1; i <= mlp.GetD(l); i++ )
            {
                for (int j = 0; j <= mlp.GetD(l - 1); j++, index++)
                {
                    mlp.SetW(l, j, i, line[index]);
                }
            }
        }
    }
}