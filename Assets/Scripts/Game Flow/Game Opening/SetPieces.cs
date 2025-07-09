using UnityEngine;
using System.Collections;

public class SetPieces : MonoBehaviour
{
    [SerializeField] private GameObject[] pieces; // Array of pieces to set
    [SerializeField] private float moveSpeed = 5f; // Speed at which pieces move
    [SerializeField] private Vector3 baseTargetPosition = new Vector3(-10f, 10f, 0f); // Start target position

    private bool _isMoving; // Flag to prevent re-entrance

    private void Start()
    {
        if (pieces == null || pieces.Length == 0)
        {
            Debug.LogError("No pieces assigned in the inspector.");
            return;
        }

        // Option-1: Automatically start moving pieces
        StartMove();
    }

    private void StartMove()
    {
        if (_isMoving)
        {
            return;
        }

        StartCoroutine(MovePiecesRoutine());
    }

    private IEnumerator MovePiecesRoutine()
    {
        _isMoving = true;

        // Lock input if 
        InputLocker.Instance.LockInput();

        for (int i = 0; i < pieces.Length; i++)
        {
            GameObject piece = pieces[i];
            if (piece == null) continue;

            Vector3 target = baseTargetPosition + new Vector3(i * 5f, 0f, 0f); // x += 5f each time

            while (piece.transform.position != target)
            {
                piece.transform.position = Vector3.MoveTowards(
                    piece.transform.position,
                    target,
                    moveSpeed * Time.deltaTime);

                yield return null;
            }
        }

        
        InputLocker.Instance.UnlockInput();
        _isMoving = false;
    }
}