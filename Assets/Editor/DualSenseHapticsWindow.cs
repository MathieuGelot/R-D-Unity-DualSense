using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

public class DualSenseHapticsWindow : EditorWindow
{
    // Extern object
    WrapperDS5W_Handler controller;
    DefaultHapticPreset defaultPreset;
    DefaultHapticPreset currentPreset;

    // Path
    string texturesPath = "Assets/HapticsTool/Sprites/Btns/";
    string defaultPresetPath = "Assets/HapticsTool/DefaultHapticPreset.asset"; 

    // Textures for UI
    Texture2D gamepadTex;
    Texture2D crossTex;
    Texture2D triangleTex;
    Texture2D circleTex;
    Texture2D squareTex;
    
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

    private void LoadDefaultPreset()
    {
        defaultPreset = Instantiate(AssetDatabase.LoadAssetAtPath<DefaultHapticPreset>(defaultPresetPath));
        currentPreset = defaultPreset;
    }

    private void OnEnable()
    {
        LoadDefaultPreset();
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

        Repaint(); // Force OnGUI() to redraw
    }
     
    private void OnGUI()
    {
        GUILayout.Label("DualSense Haptics Tool", EditorStyles.boldLabel);
        ControllerUI();
        PresetSettingsUI();

        if (playHaptics)
        {
            controller.SetRumbleEffect(0, WrapperDS5W_Native.Wrapper_Side.LEFT, currentPreset.leftRumble);
            controller.SetRumbleEffect(0, WrapperDS5W_Native.Wrapper_Side.RIGHT, currentPreset.leftRumble);
        }
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
        Color leftColor = new Color(1, 0, 0, currentPreset.leftRumble / 255f);
        EditorGUI.DrawRect(leftGrip, leftColor);

        Rect rightGrip = new Rect(
          gamepadRect.x + gamepadRect.width * 0.7f,
          gamepadRect.y + gamepadRect.height * 0.2f,
          gamepadRect.width * 0.15f,
          gamepadRect.height * 0.6f
        );
        Color rightColor = new Color(1, 0, 0, currentPreset.rightRumble / 255f);
        EditorGUI.DrawRect(rightGrip, rightColor);
    }

    private void PresetSettingsUI()
    {
        playHaptics = EditorGUILayout.Toggle("Play Haptics", playHaptics);
        if(!playHaptics)
        {

        }

        currentPreset = (DefaultHapticPreset)EditorGUILayout.ObjectField("Preset", currentPreset,  typeof(DefaultHapticPreset), false);

        currentPreset.leftRumble = (byte)EditorGUILayout.IntSlider("Left Motor", (int)currentPreset.leftRumble, 0, 255);
        currentPreset.rightRumble = (byte)EditorGUILayout.IntSlider("Left Motor", (int)currentPreset.rightRumble, 0, 255);


    }

    private void StopAllHapticsOnController()
    {
        controller.SetRumbleEffect(0, WrapperDS5W_Native.Wrapper_Side.LEFT, 0x00);
        controller.SetRumbleEffect(0, WrapperDS5W_Native.Wrapper_Side.RIGHT, 0x00);

        WrapperDS5W_Native.Wrapper_TriggerEffect triggerEffect = new WrapperDS5W_Native.Wrapper_TriggerEffect();

        triggerEffect.effectType = WrapperDS5W_Native.Wrapper_TriggerEffectType.NoResitance;

        controller.SetTriggerEffect(0, WrapperDS5W_Native.Wrapper_Side.LEFT, triggerEffect);
    }
}
