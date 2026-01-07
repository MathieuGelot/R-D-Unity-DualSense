using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class WrapperDS5W_Handler : IDisposable
{
    private bool _initialized = false;

    public WrapperDS5W_Handler()
    {
        Debug.Log($"{Marshal.PtrToStringAnsi(WrapperDS5W_Native.InitControllersAPI())}");
        _initialized = true;
    }

    public void CreateDevice(int _id)
    {
        EnsureInit();
        WrapperDS5W_Native.CreateDevice(_id);
    }

    public void FreeDevice(int _id)
    {
        EnsureInit();
        WrapperDS5W_Native.FreeDevice(_id);
    }

    public void Update()
    {
        EnsureInit();
        WrapperDS5W_Native.Update();
    }

    public bool GetButtonState(int _id, WrapperDS5W_Native.Wrapper_Buttons _btn)
    {
        EnsureInit();
        return WrapperDS5W_Native.GetButtonState(_id, _btn);
    }

    public byte GetTriggerValue(int _id, WrapperDS5W_Native.Wrapper_Side _trigger)
    {
        EnsureInit();
        return WrapperDS5W_Native.GetTriggerValue(_id, _trigger);
    }

    public WrapperDS5W_Native.Wrapper_AnalogStick GetStickPosition(int _id, WrapperDS5W_Native.Wrapper_Side _stick)
    {
        EnsureInit();
        return WrapperDS5W_Native.GetStickPosition(_id, _stick);
    }

    public WrapperDS5W_Native.Wrapper_Touch GetTouchPadPosition(int _id, int _fingerID)
    {
        EnsureInit();
        return WrapperDS5W_Native.GetTouchPadPosition(_id, _fingerID);
    }

    public WrapperDS5W_Native.Wrapper_Vector3 GetGyroscopeValue(int _id)
    {
        EnsureInit();
        return WrapperDS5W_Native.GetGyroscopeValue(_id);
    }

    public WrapperDS5W_Native.Wrapper_Vector3 GetAccelerometerValue(int _id)
    {
        EnsureInit();
        return WrapperDS5W_Native.GetAccelerometerValue(_id);
    }

    public void SetRumbleEffect(int _id, WrapperDS5W_Native.Wrapper_Side _rumble, byte _rumbleStrength)
    {
        EnsureInit();
        WrapperDS5W_Native.SetRumbleEffect(_id, _rumble, _rumbleStrength);
    }

    public void SetTriggerEffect(int _id, WrapperDS5W_Native.Wrapper_Side _trigger, WrapperDS5W_Native.Wrapper_TriggerEffect _effect)
    {
        EnsureInit();
        WrapperDS5W_Native.SetTriggerEffect(_id, _trigger, _effect);
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
            Debug.Log($"{Marshal.PtrToStringAnsi(WrapperDS5W_Native.ShutdownControllersAPI())}");
            _initialized = false;
        }
    }
}
