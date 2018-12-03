using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class AnimatorSample : MonoBehaviour
{
    [Range(0, 1f)]
    public float SampleTime = 0f;

    public AnimationClip clip;

    Animator m_Animator;

    AnimationClip[] clips;

    void Start()
    {
        m_Animator = transform.GetComponent<Animator>();

        //m_Animator.enabled = false;

        clips = m_Animator.runtimeAnimatorController.animationClips;
        Debug.LogError(clips.Length);
    }

    void Update()
    {
        m_Animator.runtimeAnimatorController.animationClips[0].SampleAnimation(this.gameObject, SampleTime);

        if (Input.GetKeyDown(KeyCode.Space))
            ChangeAnimationClipDynamic();
    }

    void ChangeAnimationClipDynamic()
    {
        UnityEditor.EditorGUIUtility.PingObject(clip);
        AnimatorOverrideController overrideController = new AnimatorOverrideController();

        Debug.LogError("Before____________" + m_Animator.runtimeAnimatorController.animationClips[0].name);
        overrideController.runtimeAnimatorController = m_Animator.runtimeAnimatorController;

        Debug.LogError("__override________" + overrideController["die_03"].name + "________Source______" + clip.name);
        overrideController["die_03"] = clip;
        Debug.LogError("__override________" + overrideController["die_03"].name + "________Source______" + clip.name);

        m_Animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        Debug.LogError("After:____________" + m_Animator.runtimeAnimatorController.animationClips[0].name);

        //m_Animator.enabled = false;
    }

}
