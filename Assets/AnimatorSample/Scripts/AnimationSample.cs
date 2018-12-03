using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class AnimationSample : MonoBehaviour
{
    public bool IsUseClipOrAnimation = true;

    public AnimationClip clip;
    [Range(0, 1f)]
    public float sampleTime;

    public Animation anim;
    [Range(0, 1f)]
    public float curValue = 0f;

    float deltaTime = 0f;
    float lastFrameTime = 0f;
    float progressTime = 0f;
    string clipName = "run_04";

    void Start()
    {
        anim.enabled = false;
        AnimationState state = anim[clipName];
        state.enabled = true;
        state.weight = 1;
        state.normalizedTime = 0;
        anim.Sample();
        state.enabled = false;
    }

    void Update()
    {
        deltaTime = Time.realtimeSinceStartup;
        progressTime += deltaTime - lastFrameTime;

        if (IsUseClipOrAnimation)
            clip.SampleAnimation(this.gameObject, sampleTime);
        else
            Example();

        lastFrameTime = deltaTime;
    }

    void Example()
    {
        AnimationState animState = anim[clipName];
        animState.enabled = true;
        animState.speed = 1f;
        animState.weight = 1;
        if (curValue > 1f)
        {
            curValue = 0f;
        }
        animState.normalizedTime = curValue;
        anim.Sample();
        animState.enabled = false;
    }

}