using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotateSelf : MonoBehaviour
{
    private float speed = 5f;

    void Update()
    {
        transform.Rotate(Vector3.up, speed);
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        speed = GUILayout.HorizontalSlider(speed, 0.0f, 50.0f);
        GUILayout.EndVertical();
    }

}
