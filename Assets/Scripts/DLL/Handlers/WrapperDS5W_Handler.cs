using System;
using System.Runtime.InteropServices;
using UnityEngine;
using static WrapperDS5W_Native;

public class WrapperDS5W_Handler : IDisposable
{
    [Serializable]
    public struct TriggerEffect
    {
        public Wrapper_TriggerEffectType effectType;
        public byte u1_0;
        public byte u1_1;
        public byte u1_2;
        public byte u1_3;
        public byte u1_4;
        public byte u1_5;
    }

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

    public WrapperDS5W_Native.Wrapper_TriggerEffect ToNative(TriggerEffect _effect)
    {
        WrapperDS5W_Native.Wrapper_TriggerEffect effect = new Wrapper_TriggerEffect();

        switch(_effect.effectType)
        {
            case Wrapper_TriggerEffectType.NoResistance:
                effect.effectType = Wrapper_TriggerEffectType.NoResistance;
                break;
            case Wrapper_TriggerEffectType.ContinuousResistance:
                effect.effectType = Wrapper_TriggerEffectType.ContinuousResistance;
                effect.Union.Continuous.startPosition = _effect.u1_0;
                effect.Union.Continuous.force = _effect.u1_1;
                break;
            case Wrapper_TriggerEffectType.SectionResistance:
                effect.effectType = Wrapper_TriggerEffectType.SectionResistance;
                effect.Union.Section.startPosition = _effect.u1_0;
                effect.Union.Section.endPosition = _effect.u1_1;
                break;
            case Wrapper_TriggerEffectType.EffectEx:
                effect.effectType = Wrapper_TriggerEffectType.EffectEx;
                effect.Union.EffectEx.startPosition = _effect.u1_0;
                effect.Union.EffectEx.keepEffect = _effect.u1_1;
                effect.Union.EffectEx.beginForce = _effect.u1_2;
                effect.Union.EffectEx.middleForce = _effect.u1_3;
                effect.Union.EffectEx.endForce = _effect.u1_4;
                effect.Union.EffectEx.frequency = _effect.u1_5;
                break;
        }
        return effect;
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
            TriggerEffect triggerEffect = new TriggerEffect();
            triggerEffect.effectType = Wrapper_TriggerEffectType.NoResistance;

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

    public void SetTriggerEffect(int _id, WrapperDS5W_Native.Wrapper_Side _trigger, TriggerEffect _effect, bool _showLogs = false)
    {
        EnsureInit();
        WrapperDS5W_Native.SetTriggerEffect(_id, _trigger, ToNative(_effect));
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
