using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : ScriptableObject
{
    public enum Type
    {
        None,
        Attack,
        Defence,
        Poison,
        Boom
    }

    public Type SkillType = Type.None;
    public float Attack = 10f;
    public float Defence = 3f;
    public float Range = 1f;

}
