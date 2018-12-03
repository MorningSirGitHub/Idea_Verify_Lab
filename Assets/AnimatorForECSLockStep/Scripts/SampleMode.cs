using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class SampleMode : MonoBehaviour
{
    [Range(0, 1f)]
    public float SampleTime = 0f;

    public AnimationClip clip;

    void Update()
    {
        if (clip == null)
            return;

        SampleTime += Time.deltaTime;

        if (SampleTime <= 1f)
            clip.SampleAnimation(this.gameObject, SampleTime);

        if (Input.GetKeyDown(KeyCode.W))
            SampleTime = 0;
    }

}
