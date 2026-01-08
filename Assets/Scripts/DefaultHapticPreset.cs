using UnityEngine;

[CreateAssetMenu(fileName = "DefaultHapticPreset", menuName = "Scriptable Objects/DefaultHapticPreset")]
public class DefaultHapticPreset : ScriptableObject
{
    [Header("Preset Infos")]
    public string presetName = "Default";

    [Header("Motors")]
    public byte leftRumble = 0x00;
    public byte rightRumble = 0x00;
}
