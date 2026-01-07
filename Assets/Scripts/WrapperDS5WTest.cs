using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class WrapperDS5WTest : MonoBehaviour
{
    WrapperDS5W_Handler wrapperDS5W;

    void Start()
    {
        wrapperDS5W = new WrapperDS5W_Handler(true);
        wrapperDS5W.CreateDevice(0, true);
    }

    void Update()
    {
        wrapperDS5W.Update();
        if (wrapperDS5W.GetButtonState(0, WrapperDS5W_Native.Wrapper_Buttons.CROSS, true))
        {
            Debug.Log("Cross has been pressed");
            wrapperDS5W.SetRumbleEffect(0, WrapperDS5W_Native.Wrapper_Side.LEFT, 0xCF);
        }
        else
        {
            wrapperDS5W.SetRumbleEffect(0, WrapperDS5W_Native.Wrapper_Side.LEFT, 0x00);
        }
        wrapperDS5W.SetRumbleEffect(0, WrapperDS5W_Native.Wrapper_Side.RIGHT, 0x10);
    }

    private void OnDestroy()
    {
        wrapperDS5W?.Dispose();
    }
}
