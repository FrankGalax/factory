using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler
{
    private RectTransform m_DragTransform;
    private Canvas m_Canvas;

    private void Awake()
    {
        m_DragTransform = transform.parent.GetComponent<RectTransform>();
        m_Canvas = UI.Instance.GetComponent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_DragTransform.anchoredPosition += eventData.delta / m_Canvas.scaleFactor;
    }

    public void CloseWindow()
    {
        Destroy(m_DragTransform.gameObject);
    }
}
