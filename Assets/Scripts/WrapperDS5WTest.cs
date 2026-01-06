using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class WrapperDS5WTest : MonoBehaviour
{
    WrapperDS5W_Handler wrapperDS5W;
    IntPtr controllerCtx;

    void Start()
    {
        wrapperDS5W = new WrapperDS5W_Handler();
        IntPtr ctx = wrapperDS5W.GetControllersContext(0);
        if(ctx != IntPtr.Zero)
        {
            controllerCtx = ctx;
        }
    }

    void Update()
    {
        IntPtr a = wrapperDS5W.GetButtonState(controllerCtx, WrapperDS5W_Native.Wrapper_Buttons.CROSS);
        string state = Marshal.PtrToStringAnsi(a);
        Debug.Log(state);
    }

    private void OnDestroy()
    {
        if(controllerCtx != IntPtr.Zero)
        {
            wrapperDS5W.FreeControllersContext(controllerCtx);
        }
        wrapperDS5W?.Dispose();
    }
}
