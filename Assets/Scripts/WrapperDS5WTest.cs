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
        wrapperDS5W.CreateDevice(0);
    }

    void Update()
    {
        wrapperDS5W.Update();
        if(wrapperDS5W.GetButtonState(0, WrapperDS5W_Native.Wrapper_Buttons.CROSS))
        {
            Debug.Log("Cross has been pressed");
        }
    }

    private void OnDestroy()
    {
        wrapperDS5W?.Dispose();
    }
}
