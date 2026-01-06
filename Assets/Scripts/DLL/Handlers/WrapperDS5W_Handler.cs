using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class WrapperDS5W_Handler : IDisposable
{
    private bool _initialized = false;

    public WrapperDS5W_Handler()
    {
        Debug.Log($"{Marshal.PtrToStringAnsi(WrapperDS5W_Native.Wrapper_InitControllersAPI())}");
        _initialized = true;
    }

    public IntPtr GetControllersContext(int _id)
    {
        EnsureInit();
        return WrapperDS5W_Native.Wrapper_GetControllersContext(_id);
    }

    public IntPtr FreeControllersContext(IntPtr _controllerContext)
    {
        EnsureInit();
        return WrapperDS5W_Native.Wrapper_FreeControllerContext(_controllerContext);
    }

    public void SetOutputState(IntPtr _controllerContext, WrapperDS5W_Native.Wrapper_OutputState _outputState)
    {
        EnsureInit();
        WrapperDS5W_Native.Wrapper_SetOutputState(_controllerContext, _outputState);
    }

    public IntPtr GetButtonState(IntPtr _controllerContext, WrapperDS5W_Native.Wrapper_Buttons _btn)
    {
        EnsureInit();
        return WrapperDS5W_Native.Wrapper_GetButtonState(_controllerContext, _btn);
    }

    public byte GetTriggerValue(IntPtr _controllerContext, WrapperDS5W_Native.Wrapper_Side _trigger)
    {
        EnsureInit();
        return WrapperDS5W_Native.Wrapper_GetTriggerValue(_controllerContext, _trigger);
    }

    public WrapperDS5W_Native.Wrapper_AnalogStick GetStickPosition(IntPtr _controllerContext, WrapperDS5W_Native.Wrapper_Side _stick)
    {
        EnsureInit();
        return WrapperDS5W_Native.Wrapper_GetStickPosition(_controllerContext, _stick);
    }

    public WrapperDS5W_Native.Wrapper_Touch GetTouchPadPosition(IntPtr _controllerContext, int _fingerID)
    {
        EnsureInit();
        return WrapperDS5W_Native.Wrapper_GetTouchPadPosition(_controllerContext, _fingerID);
    }

    public WrapperDS5W_Native.Wrapper_Vector3 GetGyroscopeValue(IntPtr _controllerContext)
    {
        EnsureInit();
        return WrapperDS5W_Native.Wrapper_GetGyroscopeValue(_controllerContext);
    }

    public WrapperDS5W_Native.Wrapper_Vector3 GetAccelerometerValue(IntPtr _controllerContext)
    {
        EnsureInit();
        return WrapperDS5W_Native.Wrapper_GetAccelerometerValue(_controllerContext);
    }

    public void SetRumble(WrapperDS5W_Native.Wrapper_OutputState _outputState, WrapperDS5W_Native.Wrapper_Side _rumble, byte _rumbleStrength)
    {
        EnsureInit();
        WrapperDS5W_Native.Wrapper_SetRumble(_outputState, _rumble, _rumbleStrength);
    }

    public void SetTriggerEffect(WrapperDS5W_Native.Wrapper_OutputState _outputState, WrapperDS5W_Native.Wrapper_Side _trigger, WrapperDS5W_Native.Wrapper_TriggerEffect _triggerEffect)
    {
        EnsureInit();
        WrapperDS5W_Native.Wrapper_SetTriggerEffects(_outputState, _trigger, _triggerEffect);
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
            Debug.Log($"{Marshal.PtrToStringAnsi(WrapperDS5W_Native.Wrapper_ShutdownControllersAPI())}");
            _initialized = false;
        }
    }
}
