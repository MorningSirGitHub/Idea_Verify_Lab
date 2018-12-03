using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class ParameterMode : MonoBehaviour
{

    Animator Ani;

    void Start()
    {
        Ani = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Ani.SetTrigger("PPP");
    }

}
