using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAgentObstacle : MonoBehaviour
{
    NavMeshAgent nav;
    NavMeshObstacle obs;

    // Use this for initialization
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        obs = GetComponent<NavMeshObstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (nav.enabled)
            {
                nav.enabled = false;
                //nav.updatePosition = false;
                //nav.updateRotation = false;
                //nav.updateUpAxis = false;
                return;
            }
            obs.enabled = true;
        }
        else
        {
            if (obs.enabled)
            {
                obs.enabled = false;
                return;
            }
            //nav.updateUpAxis = true;
            //nav.updateRotation = true;
            //nav.updatePosition = true;
            nav.enabled = true;
        }
    }

}
