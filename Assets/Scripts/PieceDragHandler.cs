using UnityEngine;
using UnityEngine.EventSystems;

public class PieceDragHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector3 _offset;
    [SerializeField] private LayerMask layerMask;
    private Vector3 homePosition;

    private void Start()
    {
        homePosition = transform.position; // Store the initial position as home
    }

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
        var hit = Hit();

        if (hit)
        {
            var target = hit.transform.position;
            target.z = 0; // Ensure z position is zero
            transform.position = target; // Snap to the hit position
        }
        else
        {
            BackHome();
        }
    }

    private void BackHome()
    {
        Debug.Log("Back Home");
        transform.position = homePosition; // Reset to the initial position
    }

    private RaycastHit2D Hit()
    {
        var origin = transform.position;
        return Physics2D.Raycast(origin, Vector3.forward, 10f, layerMask);
    }

    private void FixedUpdate()
    {
        var hit = Hit();
        Debug.Log(hit ? $"Hit: {hit.transform.name}" : "No Hit");
    }
}