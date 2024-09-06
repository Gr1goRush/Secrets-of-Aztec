using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public static class EditorUtility
{
    public static float GetExpandingPropertyHeight(SerializedProperty property, int linesCount)
    {
        float height = 0;
        float oneLineHeight = EditorGUIUtility.singleLineHeight;
        float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;
        height += oneLineHeight + verticalSpacing;
        if (!property.isExpanded)
        {
            return height;
        }

        height += ((oneLineHeight + verticalSpacing) * linesCount);

        return height;
    }

    public static string GetStringTitleIndex(SerializedProperty property, int increaseAmount = 0)
    {
        string[] strs = property.propertyPath.Split('.');
        for (int i = 0; i < strs.Length; i++)
        {
            string str = strs[i];
            if (str.Contains("data"))
            {
                int startIndex = str.IndexOf('[');
                if (startIndex < 0)
                {
                    return null;
                }

                int endIndex = str.LastIndexOf(']');
                if (endIndex < 0)
                {
                    return null;
                }

                string cut = str.Substring(startIndex + 1, endIndex - startIndex - 1);
                if(int.TryParse(cut, out int index))
                {
                    index += increaseAmount;
                    return index.ToString();
                }

                return null;
            }
        }

        return null;
    }
}
