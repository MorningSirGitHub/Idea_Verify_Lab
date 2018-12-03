using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyRangeAttribute : PropertyAttribute
{

    public float Min;
    public float Max;
    public string Label;

    public MyRangeAttribute(float min, float max, string label = "范围")
    {
        this.Min = min;
        this.Max = max;
        this.Label = label;
    }

}
