using System;
using System.Runtime.InteropServices;

public class MyDLL_Native
{
    const string DLL_NAME = "DllUnityTest";

    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)] 
    public extern static IntPtr Init();
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)] 
    public extern static IntPtr Shutdown();
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)] 
    public extern static int AddInt(int _a, int _b);
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)] 
    public extern static float AddFloat(float _a, float _b);
}
