using UnityEngine;
using UnityEngine.EventSystems;

public class PieceDragHandler : MonoBehaviour, IPointerDownHandler,IPointerUpHandler, IDragHandler
{
    private Vector3 _offset;
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");

        var target = Camera.main.ScreenToWorldPoint(eventData.position);
        _offset = transform.position - target;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");

        var target = Camera.main.ScreenToWorldPoint(eventData.position);
        target += _offset;
        target.z = 0; 
        transform.position = target;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("PieceDragHandler  : Drag ended for: " + gameObject.name);
    }
}