namespace Brick_Breaker;

using System;
using System.Runtime.InteropServices;

public class NativeLM
{
    private const string DLL = "ML_Lib.dll";

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr LM_new(int nb_neurons_input_layer);
    
    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr LM_copy(IntPtr obj);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void LM_delete(IntPtr obj);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void LM_setUsedForClassification(IntPtr obj, bool val);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void LM_initElements(IntPtr obj, int count);
    
    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void LM_initElementsTest(IntPtr obj, int count);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void LM_addElement(IntPtr obj /* + floats... */);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void LM_addElementArray(IntPtr obj, float[] array);
    
    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void LM_addElementTestArray(IntPtr obj, float[] array);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void LM_print(IntPtr obj, bool printX, bool printY, bool printW, bool printMSE);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void LM_train(IntPtr obj, int nb_iterations, float alpha, int mse_interval);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void LM_quickTrain(IntPtr obj);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern float LM_predict(IntPtr obj /* + floats... */);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern float LM_predictArray(IntPtr obj, float[] array);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern float LM_test(IntPtr obj);
    
    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern float LM_realTest(IntPtr obj);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int LM_getMSESize(IntPtr obj);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern float LM_MSE(IntPtr obj, int index);
    
    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int LM_getNbInputNeurons(IntPtr obj);
    
    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern float LM_getWeight(IntPtr obj, int index);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void LM_setWeights(IntPtr obj, float[] weights);
}