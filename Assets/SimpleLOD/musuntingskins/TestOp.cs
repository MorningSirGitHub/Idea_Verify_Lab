using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestOp : MonoBehaviour
{

    Camera m_Camera;
    Camera MainCamera
    {
        get
        {
            return m_Camera ?? (m_Camera = Camera.main);
        }
    }

    private float speed = 50f;

    private void OnGUI()
    {
        //speed = GUILayout.HorizontalSlider(speed, 0f, 1000f);

        if (GUI.RepeatButton(new Rect(50, 100, 25, 25), ">"))
        {
            MainCamera.transform.Rotate(Vector3.up, Time.deltaTime * speed);
        }

        if (GUI.RepeatButton(new Rect(0, 100, 25, 25), "<"))
        {
            MainCamera.transform.Rotate(Vector3.down, Time.deltaTime * speed);
        }

        if (GUI.RepeatButton(new Rect(25, 75, 25, 25), "v"))
        {
            MainCamera.transform.Rotate(Vector3.right, Time.deltaTime * speed);
        }

        if (GUI.RepeatButton(new Rect(25, 125, 25, 25), "^"))
        {
            MainCamera.transform.Rotate(Vector3.left, Time.deltaTime * speed);
        }
    }

}
