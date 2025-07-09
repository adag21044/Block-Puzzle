using UnityEngine;
using UnityEngine.EventSystems;

public class PieceRotater : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 _pointerDownPos;
    private float _dragThreshold = 10f; 
    private float _rotationAngle = 90f;

    

    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDownPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (InputLocker.Instance.IsInputLocked)
        {
            Debug.Log("Input is locked, cannot rotate piece.");
            return;
        }
        
        float distance = Vector2.Distance(_pointerDownPos, eventData.position);

        if (distance < _dragThreshold && GetComponent<PieceDragHandler>().CanbeRotated)
        {
            RotatePiece();
        }
        
    }

    private void RotatePiece()
    {
        Vector3 center = GetCenterOfChildren();
        transform.RotateAround(center, Vector3.forward, _rotationAngle);
    }
    
    private Vector3 GetCenterOfChildren()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (Transform child in transform)
        {
            sum += child.position;
            count++;
        }

        return count > 0 ? sum / count : transform.position;
    }

}
