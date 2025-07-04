using UnityEngine;
using UnityEngine.EventSystems;

public class PieceDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 _startPosition;
    private Transform _dragParent;
    private CanvasGroup _canvasGroup;
    private bool _isDragging;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag started on: " + gameObject.name);
        _startPosition = transform.position;
        _isDragging = true;

        if(_canvasGroup != null)
        {
            _canvasGroup.blocksRaycasts = false; // Disable raycasts to allow dragging
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
        {
            return;
        }

        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 10f; // Set a distance from the camera
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
        transform.position = new Vector3(worldPoint.x, worldPoint.y, 0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;

        if (_canvasGroup != null)
        {
            _canvasGroup.blocksRaycasts = true; // Re-enable raycasts after dragging
        }
    }
}