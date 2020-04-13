using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FloatRange))]
class FloatRangeDrawer : PropertyDrawer
{
    const float minMaxLabelWidth = 32.0f;
    const float gap = 2.0f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty minProp = property.FindPropertyRelative("_min");
        SerializedProperty maxProp = property.FindPropertyRelative("_max");
        
        var serializedObject = minProp.serializedObject;
        float min = minProp.floatValue;
        float max = maxProp.floatValue;

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label);

        float totalWidth = position.width * 0.5f;
        Rect minTotalPosition = position;
        minTotalPosition.width = totalWidth;
        minTotalPosition.height = EditorGUIUtility.singleLineHeight;

        Rect minLabelPosition = minTotalPosition;
        minLabelPosition.width = minMaxLabelWidth;
        EditorGUI.HandlePrefixLabel(minTotalPosition, minLabelPosition, new GUIContent("Min"));

        Rect minFloatPosition = minTotalPosition;
        minFloatPosition.x += minMaxLabelWidth;
        minFloatPosition.width -= minMaxLabelWidth + gap;
        EditorGUI.BeginChangeCheck();
        min = EditorGUI.DelayedFloatField(minFloatPosition, min);
        if (EditorGUI.EndChangeCheck())
        {
            minProp.floatValue = min;
            if (maxProp.floatValue < min)
                maxProp.floatValue = min;
            serializedObject.ApplyModifiedProperties();
        }

        Rect maxTotalPosition = position;
        maxTotalPosition.x += totalWidth + gap;
        maxTotalPosition.height = EditorGUIUtility.singleLineHeight;

        maxTotalPosition.width = totalWidth;
        Rect maxLabelPosition = maxTotalPosition;
        maxLabelPosition.width = minMaxLabelWidth;
        EditorGUI.HandlePrefixLabel(maxTotalPosition, maxLabelPosition, new GUIContent("Max"));

        Rect maxFloatPosition = maxTotalPosition;
        maxFloatPosition.x += minMaxLabelWidth;
        maxFloatPosition.width -= minMaxLabelWidth + gap;
        EditorGUI.BeginChangeCheck();
        max = EditorGUI.DelayedFloatField(maxFloatPosition, max);
        if (EditorGUI.EndChangeCheck())
        {
            maxProp.floatValue = max;
            if (minProp.floatValue > max)
                minProp.floatValue = max;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
