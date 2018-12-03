using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 根据表格，动态配置动作的动画事件，目前默认动作的时间是 24FPS
/// </summary>
public class AddModelAnimationEvent : Editor
{
    const int FPS = 30;

    #region Tip
    /// 要优化速度的话可以用上下面2个参数保存当前变量，如果下一个文件和当前文件相同，下个文件就不需要加载直接用保存好的文件就行
    /// 但是编辑器代码不考虑效率
    //string lastPath;
    //ModelImporter lastModel;
    #endregion

    [MenuItem("配置/配置动画事件")]
    static void AddAnimationEvent()
    {
        string configurePath = Application.dataPath + "/AutoAddAnimationEvent/Configure/_animationEvent.csv";
        CsvFile csv = new CsvFile(configurePath, 1);
        Debug.LogFormat("一共有：{0} 条数据", csv.Lines.Count);
        for (int i = 0; i < csv.Lines.Count; i++)
        {
            string path = csv.Lines[i].rowValues[0];
            Debug.Log(path);
            string animationName = csv.Lines[i].rowValues[1];
            string eventName = csv.Lines[i].rowValues[2];
            string eventParameter = csv.Lines[i].rowValues[3];
            string eventOdds = csv.Lines[i].rowValues[4];
            string eventTimeStr = csv.Lines[i].rowValues[5];

            string[] eventNames = eventName.Split(';');
            string[] eventParameters = eventParameter.Split(';');
            string[] eventTimeStrs = eventTimeStr.Split(';');
            string[] eventOddses = eventOdds.Split(';');

            int eventNamesCount = eventNames.Length;
            List<string> eventNamesList = new List<string>();
            eventNamesList.AddRange(eventNames);
            if (eventNamesList.Contains("_Attack"))
            {
                if (eventNamesList.IndexOf("_Attack") != eventNamesList.Count - 1)
                {
                    Debug.LogErrorFormat("_Attack参数必须放在最后一个 ：行数:{0}", i);
                    return;
                }
                else
                {
                    eventNamesCount--;
                }
            }


            List<float> eventTimes = new List<float>();
            for (int m = 0; m < eventNamesCount; m++)
            {
                string time = eventTimeStrs[m];
                if (time.StartsWith("s"))
                {
                    var temp = time.Remove(0, 1);
                    eventTimes.Add(float.Parse(temp));
                }
                else if (time.StartsWith("f"))
                {
                    var temp = time.Remove(0, 1);
                    eventTimes.Add(float.Parse(temp) / FPS);
                }
            }


            if (eventNamesCount != eventParameters.Length || eventNamesCount != eventTimes.Count)
            {
                Debug.LogErrorFormat("事件名称，事件参数，事件时间，个数必须一致！！！ 行数: {0},{1},{2},{2}", i + 1, eventNamesCount, eventParameters.Length, eventTimes.Count);
                return;
            }

            int eventCount = eventNamesCount;
            if (eventNamesCount < eventNames.Length)     //如果有_Attack事件
            {
                string attackTime = eventTimeStrs[eventNames.Length - 1];    //获取Attack的时间列表
                var attackTimes = attackTime.Split(',');                 //分解时间
                eventCount += attackTimes.Length;       //事件个数要加上攻击事件的个数
            }

            ModelImporter model = ModelImporter.GetAtPath(path) as ModelImporter;
            var clips = model.clipAnimations;

            // 复制现有模型动画的数据到新的一个数组中
            // model.clipAnimations = clips Unity崩溃
            // Unity 2017.2 BUG: 资产导入：在设置属性ModelImporter.clipAnimations时崩溃时，剪辑头像掩码设置为“从此模型创建”。（960595）
            //ModelImporterClipAnimation[] clips2 = new ModelImporterClipAnimation[model.clipAnimations.Length];
            //for (int p = 0; p < model.clipAnimations.Length; p++)
            //{
            //    clips2[p] = new ModelImporterClipAnimation();
            //    clips2[p].additiveReferencePoseFrame = model.clipAnimations[p].additiveReferencePoseFrame;
            //    clips2[p].curves = model.clipAnimations[p].curves;
            //    clips2[p].cycleOffset = model.clipAnimations[p].cycleOffset;
            //    clips2[p].events = model.clipAnimations[p].events;
            //    clips2[p].firstFrame = model.clipAnimations[p].firstFrame;
            //    clips2[p].hasAdditiveReferencePose = model.clipAnimations[p].hasAdditiveReferencePose;
            //    clips2[p].heightFromFeet = model.clipAnimations[p].heightFromFeet;
            //    clips2[p].heightOffset = model.clipAnimations[p].heightOffset;
            //    clips2[p].keepOriginalOrientation = model.clipAnimations[p].keepOriginalOrientation;
            //    clips2[p].keepOriginalPositionXZ = model.clipAnimations[p].keepOriginalPositionXZ;
            //    clips2[p].keepOriginalPositionY = model.clipAnimations[p].keepOriginalPositionY;
            //    clips2[p].lastFrame = model.clipAnimations[p].lastFrame;
            //    clips2[p].lockRootHeightY = model.clipAnimations[p].lockRootHeightY;
            //    clips2[p].lockRootPositionXZ = model.clipAnimations[p].lockRootPositionXZ;
            //    clips2[p].lockRootRotation = model.clipAnimations[p].lockRootRotation;
            //    clips2[p].loop = model.clipAnimations[p].loop;
            //    clips2[p].loopPose = model.clipAnimations[p].loopPose;
            //    clips2[p].loopTime = model.clipAnimations[p].loopTime;
            //    clips2[p].maskType = model.clipAnimations[p].maskType;
            //    //clips2[p].maskSource = model.clipAnimations[p].maskSource;
            //    //var mask = new AvatarMask();
            //    //model.clipAnimations[p].ConfigureMaskFromClip(ref mask);
            //    //mask.SetTransformActive(0, true);
            //    //model.clipAnimations[p].ConfigureClipFromMask(mask);
            //    //Object.DestroyImmediate(mask);
            //    clips2[p].mirror = model.clipAnimations[p].mirror;
            //    clips2[p].name = model.clipAnimations[p].name;
            //    clips2[p].rotationOffset = model.clipAnimations[p].rotationOffset;
            //    clips2[p].takeName = model.clipAnimations[p].takeName;
            //    clips2[p].wrapMode = model.clipAnimations[p].wrapMode;
            //}

            for (int x = 0; x < clips.Length; x++)
            {
                var clip = clips[x];
                if (clip.name != animationName)  //找到指定的动画名字的clip
                {
                    continue;
                }
                float start = clip.firstFrame;
                float end = clip.lastFrame;
                float totleFrame = end - start;
                float totleTime = totleFrame / FPS;

                AnimationEvent[] events = new AnimationEvent[eventCount];
                for (int j = 0; j < eventNamesCount; j++)       //这里不是eventCount,只要先添加不是_Attack的事件
                {
                    events[j] = new AnimationEvent();
                    events[j].functionName = eventNames[j];
                    events[j].stringParameter = eventParameters[j] + "?" + eventOddses[j];
                    events[j].time = eventTimes[j] / totleTime;

                }
                if (eventNamesCount < eventNames.Length)     //如果有_Attack事件
                {
                    string attackTime = eventTimeStrs[eventNames.Length - 1];    //获取Attack的时间列表
                    var attackTimes = attackTime.Split(',');                 //分解时间
                    for (int a = 0; a < attackTimes.Length; a++)
                    {
                        float target = 0;
                        string time = attackTimes[a];
                        if (time.StartsWith("s"))
                        {
                            var temp = time.Remove(0, 1);
                            target = float.Parse(temp);
                        }
                        else if (time.StartsWith("f"))
                        {
                            var temp = time.Remove(0, 1);
                            target = float.Parse(temp) / FPS;
                        }
                        events[a + eventNamesCount] = new AnimationEvent();
                        events[a + eventNamesCount].functionName = "_Attack";
                        events[a + eventNamesCount].time = target / totleTime;
                    }
                }
                clip.events = events;
                break;
            }

            try
            {
                //for (int x = 0; x < model.clipAnimations.Length; x++)
                //{
                //    model.clipAnimations[x] = clips2[x];
                //    if (model.clipAnimations[x].name.Equals("attack_01"))
                //    {
                //        Debug.Log(clips2[x].events.Length);
                //        Debug.Log(model.clipAnimations[x].events.Length);
                //    }
                //    if (model.clipAnimations[x].name.Equals("dead_01"))
                //        Debug.Log(model.clipAnimations[x].events.Length);
                //}
                model.clipAnimations = clips;
            }
            catch (System.Exception e)
            {
                Debug.LogError(path.Split('/')[1] + "动画导入 出错啦！！！");
                Debug.Log(e);
            }

            model.SaveAndReimport();
        }
        AssetDatabase.Refresh();
    }

}