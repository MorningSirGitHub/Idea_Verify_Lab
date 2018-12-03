using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class AnimationClipMode : MonoBehaviour
{

    Animator Ani;

    void Start()
    {
        Ani = GetComponent<Animator>();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //    Ani.CrossFade("hit_01", 0, 0);

        //Ani.CrossFade("idle_01", 0, 0);
    }

}
