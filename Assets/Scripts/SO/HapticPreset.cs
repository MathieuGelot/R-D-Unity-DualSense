using UnityEngine;

[CreateAssetMenu(fileName = "HapticPreset", menuName = "Scriptable Objects/HapticPreset")]
public class HapticPreset : ScriptableObject
{
    [Header("Preset Infos")]
    public string presetName = "Default";

    [Header("Motors")]
    public byte leftRumble = 0x00;
    public byte rightRumble = 0x00;

    [Header("Triggers")]
    public WrapperDS5W_Handler.TriggerEffect leftTriggerEffect = new WrapperDS5W_Handler.TriggerEffect();
    public WrapperDS5W_Handler.TriggerEffect rightTriggerEffect = new WrapperDS5W_Handler.TriggerEffect();
}
