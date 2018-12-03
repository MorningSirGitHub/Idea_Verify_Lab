using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//实现代码如下：（这里我比上面多添加了一个点，最外层有5个点，依次4个，3个，2个）
//并添加了一个小球，没沿曲线上运动
public class DrawBezierLine : MonoBehaviour
{
    public List<Transform> gameOjbet_tran = new List<Transform>();
    private List<UnityEngine.Vector3> point = new List<UnityEngine.Vector3>();

    public GameObject ball;
    public float Speed = 1;
    public float Time1 = 2f;
    private float Timer = 0;
    int Index = 1;

    void Init()
    {
        point = new List<UnityEngine.Vector3>();
        for (int i = 0; i < 200; i++)
        {
            //一
            UnityEngine.Vector3 pos1 = UnityEngine.Vector3.Lerp(gameOjbet_tran[0].position, gameOjbet_tran[1].position, i / 100f);
            UnityEngine.Vector3 pos2 = UnityEngine.Vector3.Lerp(gameOjbet_tran[1].position, gameOjbet_tran[2].position, i / 100f);
            UnityEngine.Vector3 pos3 = UnityEngine.Vector3.Lerp(gameOjbet_tran[2].position, gameOjbet_tran[3].position, i / 100f);
            UnityEngine.Vector3 pos4 = UnityEngine.Vector3.Lerp(gameOjbet_tran[3].position, gameOjbet_tran[4].position, i / 100f);
            //二
            var pos1_0 = UnityEngine.Vector3.Lerp(pos1, pos2, i / 100f);
            var pos1_1 = UnityEngine.Vector3.Lerp(pos2, pos3, i / 100f);
            var pos1_2 = UnityEngine.Vector3.Lerp(pos3, pos4, i / 100f);
            //三
            var pos2_0 = UnityEngine.Vector3.Lerp(pos1_0, pos1_1, i / 100f);
            var pos2_1 = UnityEngine.Vector3.Lerp(pos1_1, pos1_2, i / 100f);
            //四
            UnityEngine.Vector3 find = UnityEngine.Vector3.Lerp(pos2_0, pos2_1, i / 100f);

            point.Add(find);
        }
    }

    void OnDrawGizmos()//画线
    {
        Init();
        Gizmos.color = Color.yellow;
        for (int i = 0; i < point.Count - 1; i++)
        {
            Gizmos.DrawLine(point[i], point[i + 1]);
        }
    }

    //------------------------------------------------------------------------------   
    //使小球没曲线运动
    //这里不能直接在for里以Point使用差值运算，看不到小球运算效果
    //定义一个计时器，在相隔时间内进行一次差值运算。
    void Awake()
    {
        Init();
    }

    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > Time1)
        {
            Timer = 0;
            ball.transform.localPosition = UnityEngine.Vector3.Lerp(point[Index - 1], point[Index], 1f);
            Index++;

            if (Index >= point.Count)
                Index = 1;
        }

    }

}
