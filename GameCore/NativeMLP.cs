namespace Brick_Breaker;

using System;
using System.Runtime.InteropServices;

public class NativeMLP
{
    private const string DLL = "ML_Lib.dll";

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr MLP_new(int count /* + layer sizes */);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr MLP_new_array(int count, int[] array);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void MLP_delete(IntPtr obj);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void MLP_setUsedForClassification(IntPtr obj, bool val);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void MLP_initElements(IntPtr obj, int count);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void MLP_addElement(IntPtr obj /* + floats... */);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void MLP_addElementArray(IntPtr obj, float[] array);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void MLP_print(IntPtr obj, int nbElementsToPrint);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void MLP_train(IntPtr obj, int nb_iterations, float alpha, int mse_interval);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void MLP_quickTrain(IntPtr obj);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void MLP_generatePrediction(IntPtr obj /* + floats... */);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void MLP_generatePredictionArray(IntPtr obj, float[] array);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern float MLP_getPrediction(IntPtr obj, int index);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern float MLP_test(IntPtr obj);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MLP_getMSESize(IntPtr obj);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern float MLP_MSE(IntPtr obj, int index);
    
    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MLP_getNbInputNeurons(IntPtr obj);
    
    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MLP_getNbOutputNeurons(IntPtr obj);
    
    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MLP_getL(IntPtr obj);
    
    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MLP_getD(IntPtr obj, int index);
    
    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern float MLP_getW(IntPtr obj, int layer, int neuron_out, int neuron_in);
    
    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void MLP_setW(IntPtr obj, int layer, int neuron_out, int neuron_in, float weight);
}