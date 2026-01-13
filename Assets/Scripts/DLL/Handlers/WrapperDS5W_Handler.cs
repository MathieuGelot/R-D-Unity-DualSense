using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class WrapperDS5W_Handler : IDisposable
{
    private bool _initialized = false;

    public WrapperDS5W_Handler(bool _showLogs = false)
    {
        WrapperDS5W_Native.InitControllersAPI();
        _initialized = true;
        if (_showLogs)
        {
            ShowLogs();
        }
    }

    public void ShowLogs()
    {
        EnsureInit();
        Debug.Log($"WrapperDS5W Log :\n{Marshal.PtrToStringAnsi(WrapperDS5W_Native.ShowLogs())}");
    }

    public void CreateDevice(int _id, bool _showLogs = false)
    {
        EnsureInit();
        WrapperDS5W_Native.CreateDevice(_id);
        if (_showLogs)
        {
            ShowLogs();
        }
    }

    public void FreeDevice(int _id, bool _showLogs = false)
    {
        EnsureInit();
        WrapperDS5W_Native.FreeDevice(_id);
        if (_showLogs)
        {
            ShowLogs();
        }
    }

    public void Update(int _id, bool _autoResetEffect = true, bool _showLogs = false)
    {
        EnsureInit();
        WrapperDS5W_Native.Update();

        if (_showLogs)
        {
            ShowLogs();
        }

        if (_autoResetEffect)
        {
            WrapperDS5W_Native.Wrapper_TriggerEffect triggerEffect = new WrapperDS5W_Native.Wrapper_TriggerEffect();
            triggerEffect.effectType = 0x00; // No resistance

            SetTriggerEffect(_id, WrapperDS5W_Native.Wrapper_Side.LEFT, triggerEffect, _showLogs);
            SetTriggerEffect(_id, WrapperDS5W_Native.Wrapper_Side.RIGHT, triggerEffect, _showLogs);
            SetRumbleEffect(_id, WrapperDS5W_Native.Wrapper_Side.LEFT, 0x00, _showLogs);
            SetRumbleEffect(_id, WrapperDS5W_Native.Wrapper_Side.RIGHT, 0x00, _showLogs);
        }
    }

    public bool GetButtonState(int _id, WrapperDS5W_Native.Wrapper_Buttons _btn, bool _showLogs = false)
    {
        EnsureInit();
        bool value = WrapperDS5W_Native.GetButtonState(_id, _btn);
        if (_showLogs)
        {
            ShowLogs();
        }
        return value;
    }

    public byte GetTriggerValue(int _id, WrapperDS5W_Native.Wrapper_Side _trigger, bool _showLogs = false)
    {
        EnsureInit();
        byte value = WrapperDS5W_Native.GetTriggerValue(_id, _trigger);
        if (_showLogs)
        {
            ShowLogs();
        }
        return value;
    }

    public WrapperDS5W_Native.Wrapper_AnalogStick GetStickPosition(int _id, WrapperDS5W_Native.Wrapper_Side _stick, bool _showLogs = false)
    {
        EnsureInit();
        WrapperDS5W_Native.Wrapper_AnalogStick value = WrapperDS5W_Native.GetStickPosition(_id, _stick);
        if (_showLogs)
        {
            ShowLogs();
        }
        return value;
    }

    public WrapperDS5W_Native.Wrapper_Touch GetTouchPadPosition(int _id, int _fingerID, bool _showLogs = false)
    {
        EnsureInit();
        WrapperDS5W_Native.Wrapper_Touch value = WrapperDS5W_Native.GetTouchPadPosition(_id, _fingerID);
        if (_showLogs)
        {
            ShowLogs();
        }
        return value;
    }

    public WrapperDS5W_Native.Wrapper_Vector3 GetGyroscopeValue(int _id, bool _showLogs = false)
    {
        EnsureInit();
        WrapperDS5W_Native.Wrapper_Vector3 value = WrapperDS5W_Native.GetGyroscopeValue(_id);
        if (_showLogs)
        {
            ShowLogs();
        }
        return value;
    }

    public WrapperDS5W_Native.Wrapper_Vector3 GetAccelerometerValue(int _id, bool _showLogs = false)
    {
        EnsureInit();
        WrapperDS5W_Native.Wrapper_Vector3 value = WrapperDS5W_Native.GetAccelerometerValue(_id);
        if (_showLogs)
        { 
            ShowLogs();
        }
        return value;
    }

    public void SetRumbleEffect(int _id, WrapperDS5W_Native.Wrapper_Side _rumble, byte _rumbleStrength, bool _showLogs = false)
    {
        EnsureInit();
        WrapperDS5W_Native.SetRumbleEffect(_id, _rumble, _rumbleStrength);
        if (_showLogs)
        {
            ShowLogs();
        }
    }

    public void SetTriggerEffect(int _id, WrapperDS5W_Native.Wrapper_Side _trigger, WrapperDS5W_Native.Wrapper_TriggerEffect _effect, bool _showLogs = false)
    {
        EnsureInit();
        WrapperDS5W_Native.SetTriggerEffect(_id, _trigger, _effect);
        if (_showLogs)
        {
            ShowLogs();
        }
    }

    public void PlayHapticsPreset(int _id, HapticPreset _preset, bool _showLogs = false)
    {
        SetTriggerEffect(_id, WrapperDS5W_Native.Wrapper_Side.LEFT, _preset.leftTriggerEffect);
        SetTriggerEffect(_id, WrapperDS5W_Native.Wrapper_Side.RIGHT, _preset.rightTriggerEffect);
        SetRumbleEffect(_id, WrapperDS5W_Native.Wrapper_Side.LEFT, _preset.leftRumble);
        SetRumbleEffect(_id, WrapperDS5W_Native.Wrapper_Side.RIGHT, _preset.rightRumble);
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
            WrapperDS5W_Native.ShutdownControllersAPI();
            ShowLogs();
            _initialized = false;
        }
    }
}
