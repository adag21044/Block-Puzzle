using UnityEngine;
using System.Collections;

/// <summary>
/// Moves the whole grid to a target position and locks input while moving.
/// </summary>
public class GridMover : MonoBehaviour
{
    [SerializeField] private GameObject gridParent;          // Parent that holds the grid
    [SerializeField] private float moveSpeed = 5f;           // Units per second
    [SerializeField] private Vector3 targetPosition = new Vector3(0f, 10f, 0f);

    private bool _isMoving;                                  // Prevent re-entrance

    private void Start()
    {
        if (gridParent == null)
        {
            Debug.LogError("Grid Parent is not assigned in the inspector.");
            return;
        }

        // Option-1 : otomatik başlatmak istiyorsan
        StartMove();                     
    }

    /// <summary>
    /// Public entry point – call this from başka bir script (örn: level start).
    /// </summary>
    public void StartMove()
    {
        if (_isMoving || gridParent == null) return;
        StartCoroutine(MoveGridRoutine());
    }

    /// <summary>
    /// Coroutine that moves the grid and handles input lock/unlock.
    /// </summary>
    private IEnumerator MoveGridRoutine()
    {
        _isMoving = true;
        InputLocker.Instance.LockInput();                     // Kilitle  :contentReference[oaicite:0]{index=0}

        while (gridParent.transform.position != targetPosition)
        {
            gridParent.transform.position = Vector3.MoveTowards(
                gridParent.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime);

            yield return null;                                // Wait a frame
        }


        InputLocker.Instance.UnlockInput();                   // Kilidi aç
        _isMoving = false;
    }
}
