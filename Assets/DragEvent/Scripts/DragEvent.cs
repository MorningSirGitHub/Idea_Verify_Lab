using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragEvent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{

    public RectTransform rectTransform;
    public RectTransform rectTransformSlot;
    public Transform draggedItemBox;
    private Vector2 pointerOffset;
    public CanvasGroup canvasGroup;
    public Transform grid;

    /// <summary>  
    /// 开始拖拽！  
    /// </summary>  
    /// <param name="eventData"></param>  
    public void OnBeginDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }
    /// <summary>  
    /// 设置拖拽开始和结束的位置  
    /// </summary>  
    /// <param name="eventData"></param>  
    private void SetDraggedPosition(PointerEventData eventData)
    {
        if (rectTransform == null)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //把世界坐标转换为局部坐标  
            rectTransform.SetAsLastSibling();

            transform.SetParent(draggedItemBox);
            Vector2 localPointerPosition;
            //画布是否接受射线  
            canvasGroup.blocksRaycasts = false;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransformSlot, Input.mousePosition,
                eventData.pressEventCamera, out localPointerPosition))
            {
                rectTransform.localPosition = localPointerPosition - pointerOffset;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerCurrentRaycast.gameObject.name == "skill")
        {
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "Text99")
        {
            this.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;
            eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(grid);
        }
        else
        {
            transform.SetParent(grid);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localPosition = Vector3.one * 0.5f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localPosition = Vector3.one;
    }
}
