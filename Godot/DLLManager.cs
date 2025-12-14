using Godot;
using System;
using System.IO;
using System.Runtime.InteropServices;

public partial class DLLManager : Node
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LoadLibrary(string lpFileName);

    public override void _EnterTree()
    {
        string dllPath =
            Path.Combine(
                ProjectSettings.GlobalizePath("res://"),
                ".godot/mono/temp/bin/Debug/ML_Lib.dll"
            );

        GD.Print("Loading native DLL: ", dllPath);

        IntPtr handle = LoadLibrary(dllPath);

        if (handle == IntPtr.Zero)
        {
            int err = Marshal.GetLastWin32Error();
            GD.PushError($"LoadLibrary failed ({err})");
        }
        else
        {
            GD.Print("Native DLL loaded OK");
        }

        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            GD.PushError(e.ExceptionObject.ToString());
        };
    }

}
