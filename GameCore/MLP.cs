namespace Brick_Breaker;

using System;

public class MLP : IDisposable
{
    private IntPtr _ptr;
    
    public MLP(params int[] layers)
    {
        if (layers == null || layers.Length < 2)
            throw new ArgumentException("Un MLP doit avoir au moins 2 couches (entrée + sortie).");

        _ptr = NativeMLP.MLP_new_array(layers.Length, layers);

        if (_ptr == IntPtr.Zero)
            throw new Exception("Erreur : MLP_new_array a échoué.");
    }

    public void Dispose()
    {
        if (_ptr != IntPtr.Zero)
        {
            NativeMLP.MLP_delete(_ptr);
            _ptr = IntPtr.Zero;
        }
        GC.SuppressFinalize(this);
    }

    ~MLP()
    {
        Dispose();
    }
    
    public void SetUsedForClassification(bool value)
    {
        NativeMLP.MLP_setUsedForClassification(_ptr, value);
    }

    public void InitElements(int count)
    {
        NativeMLP.MLP_initElements(_ptr, count);
    }

    public void AddElement(float[] values)
    {
        NativeMLP.MLP_addElementArray(_ptr, values);
    }

    
    public void Train(int iterations, float alpha, int mseInterval = 1)
    {
        NativeMLP.MLP_train(_ptr, iterations, alpha, mseInterval);
    }

    public void QuickTrain()
    {
        NativeMLP.MLP_quickTrain(_ptr);
    }

    public float[] Predict(float[] inputs)
    {
        NativeMLP.MLP_generatePredictionArray(_ptr, inputs);

        int outputCount = GetNbOutputNeurons();
        float[] result = new float[outputCount];
        for (int i = 0; i < outputCount; i++)
            result[i] = NativeMLP.MLP_getPrediction(_ptr, i) > 0 ? 1 : -1;

        return result;
    }

    public float Test()
    {
        return NativeMLP.MLP_test(_ptr);
    }

    public int MSESize()
    {
        return NativeMLP.MLP_getMSESize(_ptr);
    }

    public float GetMSE(int index)
    {
        return NativeMLP.MLP_MSE(_ptr, index);
    }
    
    public int GetNbInputNeurons()
    {
        return NativeMLP.MLP_getNbInputNeurons(_ptr);
    }
    
    public int GetNbOutputNeurons()
    {
        return NativeMLP.MLP_getNbOutputNeurons(_ptr);
    }
    
    public void Print(int nbElementsToPrint)
    {
        NativeMLP.MLP_print(_ptr, nbElementsToPrint);
    }

    public int GetL()
    {
        return NativeMLP.MLP_getL(_ptr);
    }

    public int GetD(int layerIndex)
    {
        return NativeMLP.MLP_getD(_ptr, layerIndex);
    }

    public float GetW(int layer, int neuron_out, int neuron_in)
    {
        return NativeMLP.MLP_getW(_ptr, layer, neuron_out, neuron_in);
    }

    public void SetW(int layer, int neuron_out, int neuron_in, float weight)
    {
        NativeMLP.MLP_setW(_ptr, layer, neuron_out, neuron_in, weight);
    }
}