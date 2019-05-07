using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PositionConvert : MonoBehaviour
{

    public Camera UICamera;
    public Camera V3Camera;

    public Transform Target;

    public RectTransform RootRect;

    private Vector2 m_ScreenPos;

    private Vector2 m_LocalPos;

    void Start()
    {

    }

    void Update()
    {
        if (UICamera == null || V3Camera == null)
            return;

        m_ScreenPos = RectTransformUtility.WorldToScreenPoint(V3Camera, Target.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(RootRect.root as RectTransform, m_ScreenPos, UICamera, out m_LocalPos);

        RootRect.anchoredPosition = m_LocalPos;

    }

}
