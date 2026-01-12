using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomPropertyDrawer(typeof(WrapperDS5W_Native.Wrapper_TriggerEffect))]
public class WrapperTriggerEffectDrawer : PropertyDrawer
{
    float lineHeight = 18f;
    float y = 0f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, 16), property.isExpanded, label);

        if(property.isExpanded)
        {
            y = position.y + lineHeight;

            WrapperDS5W_Native.Wrapper_TriggerEffect effect = GetStructValue(property);
            effect.effectType = (WrapperDS5W_Native.Wrapper_TriggerEffectType)EditorGUI.EnumPopup(
                new Rect(position.x, y, position.width, lineHeight), 
                "Effect Type", 
                effect.effectType);

            y += lineHeight;

            switch (effect.effectType)
            {
                case WrapperDS5W_Native.Wrapper_TriggerEffectType.NoResistance:
                    break;
                case WrapperDS5W_Native.Wrapper_TriggerEffectType.ContinuousResistance:
                    DrawContinuousResistanceEffect(new Rect(position.x, y, position.width, lineHeight), ref effect);
                    break;
                case WrapperDS5W_Native.Wrapper_TriggerEffectType.SectionResistance:
                    DrawSectionResistanceEffect(new Rect(position.x, y, position.width, lineHeight), ref effect);
                    break;
                case WrapperDS5W_Native.Wrapper_TriggerEffectType.EffectEx:
                    DrawEffectExEffect(new Rect(position.x, y, position.width, lineHeight), ref effect);
                    break;
            }
            SetStructValue(property, effect);
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float h = lineHeight; // foldout

        if (!property.isExpanded)
            return h;

        h += lineHeight; // effect type

        var effect = GetStructValue(property);

        switch (effect.effectType)
        {
            case WrapperDS5W_Native.Wrapper_TriggerEffectType.ContinuousResistance:
                h += 2 * lineHeight;
                break;

            case WrapperDS5W_Native.Wrapper_TriggerEffectType.SectionResistance:
                h += 2 * lineHeight;
                break;

            case WrapperDS5W_Native.Wrapper_TriggerEffectType.EffectEx:
                h += 6 * lineHeight;
                break;
        }

        h += lineHeight;
        return h;
    }

    WrapperDS5W_Native.Wrapper_TriggerEffect GetStructValue(SerializedProperty property)
    {
        object target = property.serializedObject.targetObject;
        var field = target.GetType().GetField(property.propertyPath, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (field != null)
        {
            return (WrapperDS5W_Native.Wrapper_TriggerEffect)field.GetValue(target);
        }
        return default;
    }

    void SetStructValue(SerializedProperty property, WrapperDS5W_Native.Wrapper_TriggerEffect value)
    {
        UnityEngine.Object target = property.serializedObject.targetObject;
        var field = target.GetType().GetField(property.propertyPath, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (field != null)
        {
            field.SetValue(target, value);
            EditorUtility.SetDirty(target);
        }
    }

    void DrawContinuousResistanceEffect(Rect _rect, ref WrapperDS5W_Native.Wrapper_TriggerEffect _effect)
    {
        _effect.Union.Continuous.startPosition = (byte)EditorGUI.IntField(new Rect(_rect.x, y, _rect.width, 16), "Start Position", _effect.Union.Continuous.startPosition);
        y += lineHeight;
        _effect.Union.Continuous.force = (byte)EditorGUI.IntField(new Rect(_rect.x, y, _rect.width, 16), "Force", _effect.Union.Continuous.force);
    }

    void DrawSectionResistanceEffect(Rect _rect, ref WrapperDS5W_Native.Wrapper_TriggerEffect _effect)
    {
        _effect.Union.Section.startPosition = (byte)EditorGUI.IntField(new Rect(_rect.x, y, _rect.width, 16), "Start Position", _effect.Union.Section.startPosition);
        y += lineHeight;
        _effect.Union.Section.endPosition = (byte)EditorGUI.IntField(new Rect(_rect.x, y, _rect.width, 16), "End Position", _effect.Union.Section.endPosition);
    }

    void DrawEffectExEffect(Rect _rect, ref WrapperDS5W_Native.Wrapper_TriggerEffect _effect)
    {
        bool keepEffect = _effect.Union.EffectEx.keepEffect == 0x00 ? false : true;
        keepEffect = EditorGUI.Toggle(new Rect(_rect.x, y, _rect.width, 16), "Keep Effect", keepEffect);
        _effect.Union.EffectEx.keepEffect = keepEffect ? (byte)0x01 : (byte)0x00;
        y += lineHeight;
        _effect.Union.EffectEx.startPosition = (byte)EditorGUI.IntField(new Rect(_rect.x, y, _rect.width, 16), "End Position", _effect.Union.EffectEx.startPosition);
        y += lineHeight;
        _effect.Union.EffectEx.beginForce = (byte)EditorGUI.IntField(new Rect(_rect.x, y, _rect.width, 16), "Begin Force", _effect.Union.EffectEx.beginForce);
        y += lineHeight;
        _effect.Union.EffectEx.middleForce = (byte)EditorGUI.IntField(new Rect(_rect.x, y, _rect.width, 16), "Middle Force", _effect.Union.EffectEx.middleForce);
        y += lineHeight;
        _effect.Union.EffectEx.endForce = (byte)EditorGUI.IntField(new Rect(_rect.x, y, _rect.width, 16), "End Force", _effect.Union.EffectEx.endForce);
        y += lineHeight;
        _effect.Union.EffectEx.frequency = (byte)EditorGUI.IntField(new Rect(_rect.x, y, _rect.width, 16), "Frequency", _effect.Union.EffectEx.frequency);
    }
}
