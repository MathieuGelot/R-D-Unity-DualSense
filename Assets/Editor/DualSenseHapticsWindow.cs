using System;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DualSenseHapticsWindow : EditorWindow
{
    // Extern object
    WrapperDS5W_Handler controller;
    HapticPreset defaultPreset;
    HapticPreset currentPreset;

    // Path
    string texturesPath = "Assets/HapticsTool/Sprites/Btns/";
    string currentPresetPath = "Assets/HapticsTool/Presets/";
    string currentPresetName = "None";

    // Textures for UI
    Texture2D gamepadTex;
    Texture2D crossTex;
    Texture2D triangleTex;
    Texture2D circleTex;
    Texture2D squareTex;

    // Setters
    public enum TriggerEffectType
    {
        ContinuousResistance,
        SectionResistance,
        EffectEx
    }
    TriggerEffectType currentLeftTriggerType = TriggerEffectType.ContinuousResistance;
    TriggerEffectType currentRightTriggerType = TriggerEffectType.ContinuousResistance;
    byte[] currentLeftTriggerValues = new byte[5];
    bool currentLeftTriggerKeepEffect = false;
    byte[] currentRightTriggerValues = new byte[5];
    bool currentRightTriggerKeepEffect = false;
    WrapperDS5W_Native.Wrapper_TriggerEffect leftTriggerEffect= new WrapperDS5W_Native.Wrapper_TriggerEffect();
    WrapperDS5W_Native.Wrapper_TriggerEffect rightTriggerEffect = new WrapperDS5W_Native.Wrapper_TriggerEffect();
    byte leftRumble;
    byte rightRumble;

    // Bool for four buttons state
    bool crossPressed = false;
    bool trianglePressed = false;
    bool circlePressed = false;
    bool squarePressed = false;

    // Position in percent relative to the gamepad visuel 0 / 1
    Vector2 squareNorm = new Vector2(0.68f, 0.434f);
    Vector2 circleNorm = new Vector2(0.82f, 0.4309f);
    Vector2 triangleNorm = new Vector2(0.75f, 0.3372f);
    Vector2 crossNorm = new Vector2(0.74f, 0.5223f);

    // Default Window Rect
    static Rect windowRect = new Rect(300, 50, 1500, 1250);

    // Others
    bool playHaptics = true;

    [MenuItem("Tools/DualSense Haptics")]
    public static void Open()
    {
        // Window
        var window = CreateInstance<DualSenseHapticsWindow>();
        window.titleContent = new GUIContent("DualSense Haptics");
        window.position = windowRect;
        window.minSize = windowRect.size;
        window.maxSize = windowRect.size;
        window.ShowUtility(); // Undockable (used to avoid resize)
    }

    private void OnEnable()
    {
        defaultPreset = ScriptableObject.CreateInstance<HapticPreset>();
        currentPreset = defaultPreset;
        currentPresetName = currentPreset.presetName;

        controller = new WrapperDS5W_Handler(true);
        controller.CreateDevice(0, true);

        Vector2 size = new Vector2(windowRect.width, windowRect.height);
        minSize = size;
        maxSize = size;

        EditorApplication.update += EditorUpdate;

        // Loading Texture2D
        //gamepadTex = AssetDatabase.LoadAssetAtPath<Texture2D>(texturesPath + "PS5_Diagram" + ".png");
        gamepadTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/HapticsTool/Sprites/PS5_Diagram" + ".png");
        crossTex = AssetDatabase.LoadAssetAtPath<Texture2D>(texturesPath + "PS5_Cross" + ".png");
        triangleTex = AssetDatabase.LoadAssetAtPath<Texture2D>(texturesPath + "PS5_Triangle" + ".png");
        circleTex = AssetDatabase.LoadAssetAtPath<Texture2D>(texturesPath + "PS5_Circle" + ".png");
        squareTex = AssetDatabase.LoadAssetAtPath<Texture2D>(texturesPath + "PS5_Square" + ".png");
    }

    private void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
        controller?.Dispose();
    }

    private void EditorUpdate()
    {
        controller.Update();

        crossPressed = controller.GetButtonState(0, WrapperDS5W_Native.Wrapper_Buttons.CROSS);
        trianglePressed = controller.GetButtonState(0, WrapperDS5W_Native.Wrapper_Buttons.TRIANGLE);
        squarePressed = controller.GetButtonState(0, WrapperDS5W_Native.Wrapper_Buttons.SQUARE);
        circlePressed = controller.GetButtonState(0, WrapperDS5W_Native.Wrapper_Buttons.CIRCLE);

        if (playHaptics)
        {
            controller.SetRumbleEffect(0, WrapperDS5W_Native.Wrapper_Side.LEFT, leftRumble);
            controller.SetRumbleEffect(0, WrapperDS5W_Native.Wrapper_Side.RIGHT, rightRumble);

            controller.SetTriggerEffect(0, WrapperDS5W_Native.Wrapper_Side.LEFT, leftTriggerEffect);
            controller.SetTriggerEffect(0, WrapperDS5W_Native.Wrapper_Side.RIGHT, rightTriggerEffect);
        }
        else
        {
            StopAllHapticsOnController();
        }

        Repaint(); // Force OnGUI() to redraw
    }
     
    private void OnGUI()
    {
        GUILayout.Label("DualSense Haptics Tool", EditorStyles.boldLabel);
        ControllerUI();
        PresetSettingsUI();

        EditorGUILayout.BeginHorizontal(); // Ligne horizontale : tout ce qui est dedans sera côte à côte

        // ----- COLONNE GAUCHE -----
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width / 2)); // 50% de la fenêtre
        LeftSettingsUI();
        EditorGUILayout.EndVertical();

        // ----- COLONNE DROITE -----
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width / 2)); // 50% de la fenêtre
        RightSettingsUI();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal(); // Fin ligne horizontale
    }

    private void ControllerUI()
    {
        GUILayout.Label(gamepadTex);
        Rect gamepadRect = GUILayoutUtility.GetLastRect(); // Get Rect relative to window for gamepadTex
        Vector2 ButtonSize = new Vector2(gamepadRect.width * 0.0565f, gamepadRect.height * 0.0872f);
        if (crossPressed)
        {
            float X = gamepadRect.x + gamepadRect.width * crossNorm.x;
            float Y = gamepadRect.y + gamepadRect.height * crossNorm.y;
            Rect Rect = new Rect(X, Y, ButtonSize.x, ButtonSize.y);
            GUI.DrawTexture(Rect, crossTex);
        }
        if (trianglePressed)
        {
            float X = gamepadRect.x + gamepadRect.width * triangleNorm.x;
            float Y = gamepadRect.y + gamepadRect.height * triangleNorm.y;
            Rect Rect = new Rect(X, Y, ButtonSize.x, ButtonSize.y);
            GUI.DrawTexture(Rect, triangleTex);
        }
        if (squarePressed)
        {
            float X = gamepadRect.x + gamepadRect.width * squareNorm.x;
            float Y = gamepadRect.y + gamepadRect.height * squareNorm.y;
            Rect Rect = new Rect(X, Y, ButtonSize.x, ButtonSize.y);
            GUI.DrawTexture(Rect, squareTex);
        }
        if (circlePressed)
        {
            float X = gamepadRect.x + gamepadRect.width * circleNorm.x;
            float Y = gamepadRect.y + gamepadRect.height * circleNorm.y;
            Rect Rect = new Rect(X, Y, ButtonSize.x, ButtonSize.y);
            GUI.DrawTexture(Rect, circleTex);
        }

        Rect leftGrip = new Rect(
            gamepadRect.x + gamepadRect.width * 0.1f,
            gamepadRect.y + gamepadRect.height * 0.2f,
            gamepadRect.width * 0.15f,
            gamepadRect.height * 0.6f
        );
        Color leftColor = new Color(1, 0, 0, leftRumble / 255f);
        EditorGUI.DrawRect(leftGrip, leftColor);

        Rect rightGrip = new Rect(
          gamepadRect.x + gamepadRect.width * 0.7f,
          gamepadRect.y + gamepadRect.height * 0.2f,
          gamepadRect.width * 0.15f,
          gamepadRect.height * 0.6f
        );
        Color rightColor = new Color(1, 0, 0, rightRumble / 255f);
        EditorGUI.DrawRect(rightGrip, rightColor);
    }

    private void PresetSettingsUI()
    {
        playHaptics = EditorGUILayout.Toggle("Play Haptics", playHaptics);

        HapticPreset lastPreset = currentPreset;
        currentPreset = (HapticPreset)EditorGUILayout.ObjectField("Preset", currentPreset, typeof(HapticPreset), false);
        if(currentPreset != null)
        {
            if(currentPreset != lastPreset)
            {
                SetUIPresetValues();
            }

            leftTriggerEffect = currentPreset.leftTriggerEffect;
            currentPresetName = EditorGUILayout.TextField("Preset Name : ", currentPresetName);
            if (GUILayout.Button("Save"))
            {
                SavePreset();
            }
        }
    }

    private void LeftSettingsUI()
    {
        leftRumble = (byte)EditorGUILayout.IntSlider("Left Motor", (int)leftRumble, 0, 255);
        currentLeftTriggerType = (TriggerEffectType)EditorGUILayout.EnumPopup("Left Trigger Type", currentLeftTriggerType);
        switch (currentLeftTriggerType)
        {
            case TriggerEffectType.ContinuousResistance:
                currentLeftTriggerValues[0] = (byte)EditorGUILayout.IntSlider("Start Position", (int)currentLeftTriggerValues[0], 0, 255);
                currentLeftTriggerValues[1] = (byte)EditorGUILayout.IntSlider("Force", (int)currentLeftTriggerValues[1], 0, 255);
                leftTriggerEffect.effectType = WrapperDS5W_Native.Wrapper_TriggerEffectType.ContinuousResistance;
                leftTriggerEffect.Union.Continuous.startPosition = currentLeftTriggerValues[0];
                leftTriggerEffect.Union.Continuous.force = currentLeftTriggerValues[1];
                break;
            case TriggerEffectType.SectionResistance:
                currentLeftTriggerValues[0] = (byte)EditorGUILayout.IntSlider("Start Position", (int)currentLeftTriggerValues[0], 0, 255);
                currentLeftTriggerValues[1] = (byte)EditorGUILayout.IntSlider("End Position", (int)currentLeftTriggerValues[1], 0, 255);
                leftTriggerEffect.effectType = WrapperDS5W_Native.Wrapper_TriggerEffectType.SectionResistance;
                leftTriggerEffect.Union.Section.startPosition = currentLeftTriggerValues[0];
                leftTriggerEffect.Union.Section.endPosition = currentLeftTriggerValues[1];
                break;
            case TriggerEffectType.EffectEx:
                currentLeftTriggerKeepEffect = EditorGUILayout.Toggle("Keep Effect", currentLeftTriggerKeepEffect);
                currentLeftTriggerValues[0] = (byte)EditorGUILayout.IntSlider("Start Position", (int)currentLeftTriggerValues[0], 0, 255);
                currentLeftTriggerValues[1] = (byte)EditorGUILayout.IntSlider("Begin Force", (int)currentLeftTriggerValues[1], 0, 255);
                currentLeftTriggerValues[2] = (byte)EditorGUILayout.IntSlider("Middle Force", (int)currentLeftTriggerValues[2], 0, 255);
                currentLeftTriggerValues[3] = (byte)EditorGUILayout.IntSlider("End Force", (int)currentLeftTriggerValues[3], 0, 255);
                currentLeftTriggerValues[4] = (byte)EditorGUILayout.IntSlider("Frequency", (int)currentLeftTriggerValues[4], 0, 255);
                leftTriggerEffect.effectType = WrapperDS5W_Native.Wrapper_TriggerEffectType.EffectEx;
                leftTriggerEffect.Union.EffectEx.startPosition = currentLeftTriggerValues[0];
                leftTriggerEffect.Union.EffectEx.beginForce = currentLeftTriggerValues[1];
                leftTriggerEffect.Union.EffectEx.middleForce = currentLeftTriggerValues[2];
                leftTriggerEffect.Union.EffectEx.frequency = currentLeftTriggerValues[3];
                leftTriggerEffect.Union.EffectEx.beginForce = currentLeftTriggerValues[4];
                leftTriggerEffect.Union.EffectEx.keepEffect = currentLeftTriggerKeepEffect == false ? (byte)0x00 : (byte)0x01;
                break;
            default: break;
        }
    }

    private void RightSettingsUI()
    {
        rightRumble = (byte)EditorGUILayout.IntSlider("Right Motor", (int)rightRumble, 0, 255);
        currentRightTriggerType = (TriggerEffectType)EditorGUILayout.EnumPopup("Right Trigger Type", currentRightTriggerType);
        switch (currentRightTriggerType)
        {
            case TriggerEffectType.ContinuousResistance:
                currentRightTriggerValues[0] = (byte)EditorGUILayout.IntSlider("Start Position", (int)currentRightTriggerValues[0], 0, 255);
                currentRightTriggerValues[1] = (byte)EditorGUILayout.IntSlider("Force", (int)currentRightTriggerValues[1], 0, 255);
                rightTriggerEffect.effectType = WrapperDS5W_Native.Wrapper_TriggerEffectType.ContinuousResistance;
                rightTriggerEffect.Union.Continuous.startPosition = currentRightTriggerValues[0];
                rightTriggerEffect.Union.Continuous.force = currentRightTriggerValues[1];
                break;
            case TriggerEffectType.SectionResistance:
                currentRightTriggerValues[0] = (byte)EditorGUILayout.IntSlider("Start Position", (int)currentRightTriggerValues[0], 0, 255);
                currentRightTriggerValues[1] = (byte)EditorGUILayout.IntSlider("End Position", (int)currentRightTriggerValues[1], 0, 255);
                rightTriggerEffect.effectType = WrapperDS5W_Native.Wrapper_TriggerEffectType.SectionResistance;
                rightTriggerEffect.Union.Section.startPosition = currentRightTriggerValues[0];
                rightTriggerEffect.Union.Section.endPosition = currentRightTriggerValues[1];
                break;
            case TriggerEffectType.EffectEx:
                currentLeftTriggerKeepEffect = EditorGUILayout.Toggle("Keep Effect", currentLeftTriggerKeepEffect);
                currentRightTriggerValues[0] = (byte)EditorGUILayout.IntSlider("Start Position", (int)currentRightTriggerValues[0], 0, 255);
                currentRightTriggerValues[1] = (byte)EditorGUILayout.IntSlider("Begin Force", (int)currentRightTriggerValues[1], 0, 255);
                currentRightTriggerValues[2] = (byte)EditorGUILayout.IntSlider("Middle Force", (int)currentRightTriggerValues[2], 0, 255);
                currentRightTriggerValues[3] = (byte)EditorGUILayout.IntSlider("End Force", (int)currentRightTriggerValues[3], 0, 255);
                currentRightTriggerValues[4] = (byte)EditorGUILayout.IntSlider("Frequency", (int)currentRightTriggerValues[4], 0, 255);
                rightTriggerEffect.effectType = WrapperDS5W_Native.Wrapper_TriggerEffectType.EffectEx;
                rightTriggerEffect.Union.EffectEx.startPosition = currentRightTriggerValues[0];
                rightTriggerEffect.Union.EffectEx.beginForce = currentRightTriggerValues[1];
                rightTriggerEffect.Union.EffectEx.middleForce = currentRightTriggerValues[2];
                rightTriggerEffect.Union.EffectEx.frequency = currentRightTriggerValues[3];
                rightTriggerEffect.Union.EffectEx.beginForce = currentRightTriggerValues[4];
                rightTriggerEffect.Union.EffectEx.keepEffect = currentRightTriggerKeepEffect == false ? (byte)0x00 : (byte)0x01;
                break;
            default: break;
        }
    }

    private void StopAllHapticsOnController()
    {
        controller.SetRumbleEffect(0, WrapperDS5W_Native.Wrapper_Side.LEFT, 0x00);
        controller.SetRumbleEffect(0, WrapperDS5W_Native.Wrapper_Side.RIGHT, 0x00);

        WrapperDS5W_Native.Wrapper_TriggerEffect triggerEffect = new WrapperDS5W_Native.Wrapper_TriggerEffect();

        triggerEffect.effectType = 0x00; // No resistance

        controller.SetTriggerEffect(0, WrapperDS5W_Native.Wrapper_Side.LEFT, triggerEffect);
        controller.SetTriggerEffect(0, WrapperDS5W_Native.Wrapper_Side.RIGHT, triggerEffect);
    }

    private void SavePreset()
    {
        string path = $"{currentPresetPath}{currentPresetName}.asset";
        HapticPreset existing = AssetDatabase.LoadAssetAtPath<HapticPreset>(path);

        if (existing == null) // Create a new asset
        {
            HapticPreset newPreset = ScriptableObject.CreateInstance<HapticPreset>();
            SetPresetData(newPreset);
            AssetDatabase.CreateAsset(newPreset, path); 
            currentPreset = newPreset;
        }
        else // Overwrite an existing asset
        {
            if (EditorUtility.DisplayDialog("Overwrite Preset?", $"Preset '{currentPresetName}' already exists.\nOverwrite it?", "Confirm", "Cancel"))
            {
                Undo.RecordObject(existing, "Overwrite Preset");
                SetPresetData(existing);
                EditorUtility.SetDirty(existing);
                AssetDatabase.SaveAssets();
                currentPreset = existing;
                SetUIPresetValues();
            }
        }
    }

    private void SetUIPresetValues()
    {
        currentPresetName = currentPreset.presetName;

        leftRumble = currentPreset.leftRumble;
        rightRumble = currentPreset.rightRumble;

        leftTriggerEffect = currentPreset.leftTriggerEffect;
        rightTriggerEffect = currentPreset.rightTriggerEffect;
    }

    private void SetPresetData(HapticPreset _target)
    {
        _target.presetName = currentPresetName;

        _target.leftRumble = leftRumble;
        _target.rightRumble = rightRumble;

        _target.leftTriggerEffect = leftTriggerEffect;
        _target.rightTriggerEffect = rightTriggerEffect;


    }
}
