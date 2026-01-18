using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class UseCaseSample : MonoBehaviour
{
    // Ref to the DS5W Wrapper DLL
    WrapperDS5W_Handler wrapperDS5W;

    // Haptics presets list
    [SerializeField]
    List<HapticPreset> presets = new List<HapticPreset>();

    // Controller ID (if only one controller used 0)
    [SerializeField]
    private int controllerID = 0;

    void Start()
    {
        wrapperDS5W = new WrapperDS5W_Handler(); // Initialize obj, you can pass true in parameters to show DLL logs if needed
        wrapperDS5W.CreateDevice(controllerID); // Create device connection for ID 0
    }

    void Update()
    {
        wrapperDS5W.Update(controllerID); // Update DLL. You need to call this function at each frame so Update() func is appropriate for that)
        if (wrapperDS5W.GetButtonState(controllerID, WrapperDS5W_Native.Wrapper_Buttons.CROSS)) // Check if Cross btn is pressed on the controller
        {
            if (presets.Count > 0)
            {
                wrapperDS5W.PlayHapticsPreset(controllerID, presets[0]); // Play Haptic Preset is used to play haptic feedback on your controller.
            }
        }

        if (wrapperDS5W.GetButtonState(controllerID, WrapperDS5W_Native.Wrapper_Buttons.SQUARE))
        {
            if (presets.Count > 1)
            {
                wrapperDS5W.PlayHapticsPreset(controllerID, presets[1]);
            }
        }
    }

    // Always call Dispose() to shutdown the DLL properly
    private void OnDestroy()
    {
        wrapperDS5W?.Dispose();
    }
}


