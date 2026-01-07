using System;
using System.Runtime.InteropServices;

public class WrapperDS5W_Native
{
    public enum Wrapper_Buttons : byte
    {
        // Buttons and DPAD
        SQUARE,
        CROSS,
        CIRCLE,
        TRIANGLE,
        DPAD_LEFT,
        DPAD_DOWN,
        DPAD_RIGHT,
        DPAD_UP,

        // Buttons set A
        LB,
        RB,
        LT,
        RT,
        SELECT,
        MENU,
        LEFT_STICK,
        RIGHT_STICK,

        // Buttons set B
        PLAYSTATION,
        PAD,
        MIC
    }

    public enum Wrapper_Side : byte
    {
        LEFT,
        RIGHT
    }

    public enum Wrapper_ReturnValue : byte
    {
        OK = 0,
        E_UNKNOWN = 1,
        E_INSUFFICIENT_BUFFER = 2,
        E_EXTERNAL_WINAPI = 3,
        E_STACK_OVERFLOW = 4,
        E_INVALID_ARGS = 5,
        E_CURRENTLY_NOT_SUPPORTED = 6,
        E_DEVICE_REMOVED = 7,
        E_BT_COM = 8
    }

    public enum Wrapper_DeviceConnection : byte
    {
        USB = 0,
        BT = 1
    }

    public enum Wrapper_MicLed : byte
    {
        OFF = 0x00,
        ON = 0x01,
        PULSE = 0x02
    }

    public enum Wrapper_TriggerEffectType : byte
    {
        NoResitance = 0x00,
        ContinuousResitance = 0x01,
        SectionResitance = 0x02,
        EffectEx = 0x26,
        Calibrate = 0xFC
    }

    public enum Wrapper_LedBrightness : byte
    {
        LOW = 0x02,
        MEDIUM = 0x01,
        HIGH = 0x00
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Wrapper_DeviceEnumInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
        public byte[] path;
        public Wrapper_DeviceConnection connection;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Wrapper_DeviceContext
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
        public byte[] devicePath;
        public IntPtr deviceHandle;
        Wrapper_DeviceConnection connection;
        [MarshalAs(UnmanagedType.I1)]
        public bool connected;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 547)]
        byte[] hidBuffer;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Wrapper_AnalogStick
    {
        public sbyte x;
        public sbyte y;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Wrapper_Vector3
    {
        public short x;
        public short y;
        public short z;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Wrapper_Color
    {
        public byte r;
        public byte g;
        public byte b;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Wrapper_Touch
    {
        public uint x;
        public uint y;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Wrapper_Battery
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool chargin;
        [MarshalAs(UnmanagedType.I1)]
        public bool fullyCharged;
        byte level;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Wrapper_TriggerEffect
    {
        public Wrapper_TriggerEffectType effectType;

        public TriggerEffectUnion Union;
        public ContinuousEffect Continuous => Union.Continuous;
        public SectionEffect Section => Union.Section;
        public EffectExEffect EffectEx => Union.EffectEx;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct TriggerEffectUnion
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] _u1_raw;

        [FieldOffset(0)]
        public ContinuousEffect Continuous;

        [FieldOffset(0)]
        public SectionEffect Section;

        [FieldOffset(0)]
        public EffectExEffect EffectEx;
    }

    // Sous-structs
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ContinuousEffect
    {
        public byte startPosition;
        public byte force;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] _pad;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SectionEffect
    {
        public byte startPosition;
        public byte endPosition;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] _pad;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EffectExEffect
    {
        public byte startPosition;

        [MarshalAs(UnmanagedType.I1)]
        public bool keepEffect;

        public byte beginForce;
        public byte middleForce;
        public byte endForce;
        public byte frequency;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Wrapper_PlayerLeds
    {
        public byte bitmask;
        [MarshalAs(UnmanagedType.I1)]
        public bool playerLedFade;
        public Wrapper_LedBrightness brightness;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Wrapper_InputState
    {
        public Wrapper_Touch touchPoint1;
        public Wrapper_Touch touchPoint2;
        public Wrapper_Vector3 accelerometer;
        public Wrapper_Vector3 gyroscope;
        public Wrapper_Battery battery;
        public Wrapper_AnalogStick leftStick;
        public Wrapper_AnalogStick rightStick;
        public byte leftTrigger;
        public byte rightTrigger;
        public byte buttonsAndDpad;
        public byte buttonsA;
        public byte buttonsB;
        public byte leftTriggerFeedback;
        public byte rightTriggerFeedback;
        [MarshalAs(UnmanagedType.I1)]
        public bool headPhoneConnected;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Wrapper_OutputState
    {
        public Wrapper_TriggerEffect leftTriggerEffect;
        public Wrapper_TriggerEffect rightTriggerEffect;
        public Wrapper_PlayerLeds playerLeds;
        public Wrapper_Color lightbar;
        public Wrapper_MicLed microphoneLed;
        public byte leftRumble;
        public byte rightRumble;
        [MarshalAs(UnmanagedType.I1)]
        public bool disableLeds;
    }

    const string DLL_NAME = "WrapperDS5W";

    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr InitControllersAPI();
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr ShutdownControllersAPI();
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static void CreateDevice(int _id);
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static void FreeDevice(int _id);
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static void Update();
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static bool GetButtonState(int _id, Wrapper_Buttons _btn);
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static byte GetTriggerValue(int _id, Wrapper_Side _trigger);
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static Wrapper_AnalogStick GetStickPosition(int _id, Wrapper_Side _stick);
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static Wrapper_Touch GetTouchPadPosition(int _id, int _fingerID);
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static Wrapper_Vector3 GetGyroscopeValue(int _id);
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static Wrapper_Vector3 GetAccelerometerValue(int _id);
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetRumbleEffect(int _id, Wrapper_Side _rumble, byte _rumbleStrength);
    [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetTriggerEffect(int _id, Wrapper_Side _rumble, Wrapper_TriggerEffect _effect);
}
