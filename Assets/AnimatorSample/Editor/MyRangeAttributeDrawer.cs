using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(MyRangeAttribute))]
public class MyRangeAttributeDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        MyRangeAttribute range = this.attribute as MyRangeAttribute;

        if (property.propertyType == SerializedPropertyType.Float)
        {
            if (range.Label != string.Empty)
                label.text = range.Label;

            EditorGUI.Slider(position, property, range.Min, range.Max, label);
        }
        else if (property.propertyType == SerializedPropertyType.Integer)
        {
            if (range.Label != string.Empty)
                label.text = range.Label;

            EditorGUI.IntSlider(position, property, (int)range.Min, (int)range.Max, label);
        }

    }

}
