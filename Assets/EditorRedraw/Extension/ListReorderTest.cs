using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubjectNerd.Utilities;

[System.Serializable]
public class AAA
{
    [SerializeField]
    private List<int> bbb = new List<int>();
}

public class ListReorderTest : MonoBehaviour
{
    [SerializeField]
    public List<AAA> aaa = new List<AAA>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
