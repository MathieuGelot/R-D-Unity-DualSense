using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class WrapperDS5W_Handler : IDisposable
{
    private bool _initialized = false;

    public WrapperDS5W_Handler()
    {
        Debug.Log($"{Marshal.PtrToStringAnsi(WrapperDS5W_Native.Init())}");
        _initialized = true;
    }

    private void EnsureInit()
    {
        if (!_initialized)
        {
            throw new ObjectDisposedException(nameof(WrapperDS5W_Handler));
        }
    }

    // IDisposable function (called by the GC to clean native resources properly)
    public void Dispose()
    {
        if (_initialized)
        {
            Debug.Log($"{Marshal.PtrToStringAnsi(WrapperDS5W_Native.Shutdown())}");
            _initialized = false;
        }
    }
}
