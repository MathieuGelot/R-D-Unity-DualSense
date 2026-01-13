using UnityEngine;
using System.Collections.Generic;

public class WrapperDS5WTest : MonoBehaviour
{
    [SerializeField]
    List<HapticPreset> presets = new List<HapticPreset>();
    WrapperDS5W_Handler wrapperDS5W;
    [SerializeField]
    private int controllerID = 0;

    private void Awake()
    {
        Debug.Log("0");
    }

    void Start()
    {
        wrapperDS5W = new WrapperDS5W_Handler(true);
        wrapperDS5W.CreateDevice(controllerID, true);
    }

    void Update()
    {
        wrapperDS5W.Update(controllerID);
        if (wrapperDS5W.GetButtonState(controllerID, WrapperDS5W_Native.Wrapper_Buttons.CROSS, true))
        {
            if(presets.Count > 0)
            {
                wrapperDS5W.PlayHapticsPreset(controllerID, presets[0]);
            }
        }
    }

    private void OnDestroy()
    {
        wrapperDS5W?.Dispose();
    }
}
