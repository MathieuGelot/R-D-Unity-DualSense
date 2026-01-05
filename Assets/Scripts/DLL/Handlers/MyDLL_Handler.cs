using UnityEngine;
using System;
using System.Runtime.InteropServices;

public sealed class MyDLL_Handler : IDisposable
{
    private bool _initialized = false;

    public MyDLL_Handler()
    {
        Debug.Log($"{Marshal.PtrToStringAnsi(MyDLL_Native.Init())}");
        _initialized = true;
    }
    
    public int AddInt(int _a, int _b)
    {
        EnsureInit();
        return MyDLL_Native.AddInt(_a, _b);
    }

    public float AddFloat(float _a, float _b)
    {
        EnsureInit();
        return MyDLL_Native.AddFloat(_a, _b);
    }

    private void EnsureInit()
    {
        if (!_initialized)
        {
            throw new ObjectDisposedException(nameof(MyDLL_Handler));
        }
    }

    // IDisposable function (called by the GC to clean native resources properly)
    public void Dispose()
    {
        if(_initialized)
        {
            Debug.Log($"{Marshal.PtrToStringAnsi(MyDLL_Native.Shutdown())}");
            _initialized = false;
        }
    }
}
