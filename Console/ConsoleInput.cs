using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Console;

public class ConsoleInput : Form
{
    private struct MSG
    {
        IntPtr hwnd;
        uint message;
        IntPtr wParam;
        IntPtr lParam;
        uint time;
        int pt_x;
        int pt_y;
    }

    [DllImport("user32.dll")]
    private static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
    
    private int key_mask = 0;
    private const int left_key_mask = 2;
    private const int right_key_mask = 1;

    public bool pressingLeft{ get => key_mask == left_key_mask; }
    public bool pressingRight{ get => key_mask == right_key_mask; }
    
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    private static LowLevelKeyboardProc _proc = new LowLevelKeyboardProc(HookCallback);
    private static IntPtr _hookId = IntPtr.Zero;
    private static bool _running = true;
    private static Thread? _listenerThread;

    public static void beginListening()
    {
        StartListener();
    }

    public static void stopListening()
    {
        StopListener();
    }

    // --- Starts the listener thread ---
    private static void StartListener()
    {
        _listenerThread = new Thread(ListenerLoop);
        _listenerThread.IsBackground = true; // So it exits automatically with the program
        _listenerThread.Start();
    }

    // --- The background thread loop ---
    private static void ListenerLoop()
    {
        _hookId = SetHook(_proc);

        // Create a simple Windows message loop so the hook can receive events
        MSG msg;
        while (GetMessage(out msg, IntPtr.Zero, 0, 0))
        {
            // Do nothing with the message; just pump it
        }

        UnhookWindowsHookEx(_hookId);
    }

    // --- Stop the listener from anywhere ---
    private static void StopListener()
    {
        _running = false;
        _listenerThread?.Join();
    }

    // --- Windows hook setup ---
    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using var curProcess = Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule!;
        return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
            GetModuleHandle(curModule.ModuleName), 0);
    }

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);

            switch (vkCode)
            {
                case 0x25: OnArrowPressed("Left"); break;
                case 0x26: OnArrowPressed("Up"); break;
                case 0x27: OnArrowPressed("Right"); break;
                case 0x28: OnArrowPressed("Down"); break;
            }
        }

        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    private static void OnArrowPressed(string direction)
    {
        System.Console.WriteLine($"[{DateTime.Now:T}] Arrow pressed: {direction}");
    }

    // --- WinAPI constants & imports ---
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk,
        int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
}