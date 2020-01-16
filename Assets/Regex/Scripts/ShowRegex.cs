using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ShowRegex : MonoBehaviour
{
    private Text InputContent;
    private Text RegexPattern;
    private Text OutputResult;

    //public string m_InputContent = "<size=24>{0x01##8AFF44#=3}</size>";
    //public string m_RegexPattern = "\\{([0-9A-Za-z]+)((\\|[0-9]+){0,2})(##[0-9a-fA-F]{6})?(#[^=\\}]+)?(=[^\\}]+)?\\}";
    private string m_LastInput = string.Empty;
    private string m_LastRegex = string.Empty;

    void OnEnable()
    {
        InputContent = InputContent ?? GameObject.Find("Input").GetComponent<Text>();
        RegexPattern = RegexPattern ?? GameObject.Find("Regex").GetComponent<Text>();
        OutputResult = OutputResult ?? GameObject.Find("Output").GetComponent<Text>();
    }

    private void Update()
    {
        if (m_LastInput != InputContent.text || m_LastRegex != RegexPattern.text)
            RegexContent();
    }

    private void RegexContent()
    {
        var title = @"";
        //title += InputContent.text = m_LastInput = m_InputContent;
        title += m_LastInput = InputContent.text;
        title += "\n";
        //title += RegexPattern.text = m_LastRegex = m_RegexPattern;
        title += m_LastRegex = RegexPattern.text;
        title += "\n";
        Debug.LogError(title);

        var result = string.Empty;
        var matches = Regex.Matches(m_LastInput, m_LastRegex);
        foreach (Match match in matches)
        {
            var index = 0;
            foreach (Group g in match.Groups)
            {
                result += "[" + index++ + "]: " + g.Value + "\n";
            }
        }

        result = string.IsNullOrEmpty(result) ? "Null" : result;
        OutputResult.text = result;
        Debug.LogError(result);
    }

}
