using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;
using System;

/// <summary>
/// 字符串的处理类，如果以后要对字符串进行多次处理，这里添加对应的方法
/// </summary>
public class StringUtility
{
    public static readonly string no_breaking_space = "\u00A0";

    public static StringBuilder sb = new StringBuilder("string");

    /// <summary>
    /// 链接字符串的方法
    /// 用法和 Debbug.LogFormat 一样
    /// </summary>
    /// <param name="content">文本</param>
    /// <param name="args">参数</param>
    /// <returns></returns>
    public static string GetString(string content,params object[] args)   
    {
        StringBuilder mark = new StringBuilder("{-1}");
        sb.Remove(0, sb.Length);
        sb.Append(content);
        for (int i = 0;i<args.Length;i++)
        {
            mark.Replace((i - 1).ToString(), i.ToString());
            if (!content.Contains(mark.ToString()))
            {
                Debug.LogErrorFormat("有多余的参数 ：{0}", args[i]);
                continue;
            }
            sb.Replace(mark.ToString(), args[i].ToString());
        }
        return sb.ToString();
    }

}
