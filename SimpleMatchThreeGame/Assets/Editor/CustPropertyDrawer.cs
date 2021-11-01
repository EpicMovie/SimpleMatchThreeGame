using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(ArrayLayout))]
public class CustPropertyDrawer : PropertyDrawer {

    private const int height = 14;
    private const int width = 9;
    private const float defaultHeight = 18f;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.PrefixLabel(position,label);

		Rect newposition = position;

		newposition.y += defaultHeight;

        SerializedProperty data = property.FindPropertyRelative("rows");

        if (data.arraySize != height)
        {
			data.arraySize = height;
		}
            
		//data.rows[0][]

		for(int i = 0; i < height; i++)
        {
            SerializedProperty row = data.GetArrayElementAtIndex(i).FindPropertyRelative("row");

            newposition.height = defaultHeight;

            if (row.arraySize != width)
            {
                row.arraySize = width;
            }
                
            newposition.width = position.width / width;

            for (int j = 0; j < width; j++)
            {
                EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(j), GUIContent.none);
                newposition.x += newposition.width;
            }

            newposition.x = position.x;
            newposition.y += defaultHeight;
        }
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return defaultHeight * 15;
	}
}
