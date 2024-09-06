using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SlotsCombination))]
public class SlotsCombinationDrawer : PropertyDrawer
{
    private float oneLineHeight, verticalSpacing;
    
    private const float toggleWidth = 15, toggleSpacing = 5;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

         oneLineHeight = EditorGUIUtility.singleLineHeight;
        float currentY = position.y;
        verticalSpacing = EditorGUIUtility.standardVerticalSpacing;

        Rect foldRect = new Rect(position.x, currentY, position.width, oneLineHeight);

        string title = "Combination";
        string titleIndexString = GetStringTitleIndex(property);
        if (!string.IsNullOrEmpty(titleIndexString))
        {
            title += " " + titleIndexString;
        }

        property.isExpanded = EditorGUI.Foldout(foldRect, property.isExpanded, new GUIContent(title));
        if (!property.isExpanded)
        {
            EditorGUI.EndProperty();
            return;
        }

        SerializedProperty columnsProperty = property.FindPropertyRelative("columns");

        int columnsArraySize = columnsProperty.arraySize;
        float columnX = position.x;
        for (int i = 0; i < SlotsCombination.columnsCount; i++)
        {
            if(i >= columnsArraySize)
            {
                columnsProperty.InsertArrayElementAtIndex(i);
                columnsArraySize = i + 1;
            }

            SerializedProperty columnProperty = columnsProperty.GetArrayElementAtIndex(i);

            DrawColumnCombination(columnProperty, currentY, columnX);

            columnX += toggleWidth + toggleSpacing;
        }

        property.serializedObject.ApplyModifiedProperties();
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    private void DrawColumnCombination(SerializedProperty columnProperty, float startY, float x)
    {
        SerializedProperty overlapIndexesProperty = columnProperty.FindPropertyRelative("overlapIndexes");

        List<int> overlapIndexes = new List<int>();
        for (int i = 0; i < overlapIndexesProperty.arraySize; i++)
        {
            overlapIndexes.Add(overlapIndexesProperty.GetArrayElementAtIndex(i).intValue);
        }
        List<int> newLightsIndexes = new List<int>();

        float currentY = startY + oneLineHeight + verticalSpacing;

        int toggleIndex = 0;
        for (int lineIndex = 0; lineIndex < SlotsCombination.columnSlotsCount; lineIndex++)
        {
            Rect toggleRect = new Rect(x, currentY, toggleWidth, oneLineHeight);

            bool toggleValue = overlapIndexes.Contains(toggleIndex);
            toggleValue = EditorGUI.Toggle(toggleRect, toggleValue);
            if (toggleValue)
            {
                newLightsIndexes.Add(toggleIndex);
            }

            toggleRect.x += toggleWidth + toggleSpacing;
            toggleIndex++;

            currentY += oneLineHeight + verticalSpacing;
        }

        overlapIndexesProperty.ClearArray();
        for (int i = 0; i < newLightsIndexes.Count; i++)
        {
            overlapIndexesProperty.InsertArrayElementAtIndex(i);
            overlapIndexesProperty.GetArrayElementAtIndex(i).intValue = newLightsIndexes[i];
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorUtility.GetExpandingPropertyHeight(property, 5);
    }

    string GetStringTitleIndex(SerializedProperty property)
    {
        return EditorUtility.GetStringTitleIndex(property);
    }
}
