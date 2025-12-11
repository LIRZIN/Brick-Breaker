namespace Brick_Breaker;

using System;

public class LM : IDisposable
{
    private IntPtr _ptr;

    public LM(int nbParam)
    {
        _ptr = NativeLM.LM_new(nbParam);

        if (_ptr == IntPtr.Zero)
            throw new Exception("Erreur : LM_new a retourné un pointeur nul.");
    }
    
    
    public LM(LM lm)
    {
        if (lm == null)
            throw new ArgumentNullException(nameof(lm));

        if (lm._ptr == IntPtr.Zero)
            throw new Exception("Impossible de copier un objet natif libéré.");

        _ptr = NativeLM.LM_copy(lm._ptr);

        if (_ptr == IntPtr.Zero)
            throw new Exception("Erreur : LM_copy a retourné un pointeur nul.");
    }
    
    public object Clone()
    {
        return new LM(this);
    }

    public void Dispose()
    {
        if (_ptr != IntPtr.Zero)
        {
            NativeLM.LM_delete(_ptr);
            _ptr = IntPtr.Zero;
        }

        GC.SuppressFinalize(this);
    }

    ~LM()
    {
        Dispose();
    }
    
    
    public void SetUsedForClassification(bool value)
    {
        NativeLM.LM_setUsedForClassification(_ptr, value);
    }

    public void InitElements(int nbElements)
    {
        NativeLM.LM_initElements(_ptr, nbElements);
    }

    public void AddElement(float[] values)
    {
        NativeLM.LM_addElementArray(_ptr, values);
    }

    public void Print(bool printX, bool printY, bool printW, bool printMSE)
    {
        NativeLM.LM_print(_ptr, printX, printY, printW, printMSE);
    }

    public float[] Predict(float[] inputs)
    {
        return [NativeLM.LM_predictArray(_ptr, inputs)];
    }

    public float Test()
    {
        return NativeLM.LM_test(_ptr);
    }

    public int MSESize()
    {
        return NativeLM.LM_getMSESize(_ptr);
    }

    public float GetMSE(int index)
    {
        return NativeLM.LM_MSE(_ptr, index);
    }

    public void Train(int iterations, float alpha, int mseInterval = 1)
    {
        NativeLM.LM_train(_ptr, iterations, alpha, mseInterval);
    }

    public void QuickTrain()
    {
        NativeLM.LM_quickTrain(_ptr);
    }
    
    public int GetNbInputNeurons()
    {
        return NativeLM.LM_getNbInputNeurons(_ptr);
    }

    public float GetWeight(int index)
    {
        return NativeLM.LM_getWeight(_ptr, index);
    }
    
    public void SetWeights(float[] weights)
    {
        NativeLM.LM_setWeights(_ptr, weights);
    }
}