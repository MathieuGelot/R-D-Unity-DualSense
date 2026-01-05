using System;
using System.Runtime.InteropServices;

public class WrapperDS5W_Native
{
    const string DLL_NAME = "WrapperDS5W";

    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr Init();
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr Shutdown();
}
