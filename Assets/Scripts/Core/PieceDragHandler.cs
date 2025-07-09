using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// Handles drag & drop of a single puzzle piece (parent GameObject).
/// Every child under this object represents a 1×1 block.
/// </summary>
public class PieceDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private LayerMask gridLayer;

    private readonly List<Transform> _cells = new();          // Child blocks
    private readonly List<Vector2Int> _placedCoords = new();  // Currently occupied cells

    private Vector3 _offset;          // Mouse offset
    private Vector3 _homePos;         // Spawn position
    private bool _canBeRotated = true; 
    public bool CanBeRotated => _canBeRotated;

    [SerializeField] private float _snapDuration = 0.08f; // Snap animation duration
    [SerializeField] private AnimationCurve _snapEase = AnimationCurve.EaseInOut(0, 0, 1, 1); // Snap animation curve

    private Tween _snapTween; // DOTween tween reference

    [Header("Feedback")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _lockClip;

    #region Unity Events
    private void Awake()
    {
        foreach (Transform child in transform)
            _cells.Add(child);
    }

    private void Start() => _homePos = transform.position;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (InputLocker.Instance.IsInputLocked)
            return;

        // Release occupied cells if the piece was already on grid
        ReleaseCurrentCells();

        // Cache mouse offset at drag start
        Vector3 world = Camera.main.ScreenToWorldPoint(eventData.position);
        _offset = transform.position - world;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (InputLocker.Instance.IsInputLocked)
            return;

        Vector3 world = Camera.main.ScreenToWorldPoint(eventData.position) + _offset;
        world.z = 0;
        transform.position = world;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (InputLocker.Instance.IsInputLocked)
            return;

        if (TryGetValidPlacement(out Vector3 snapOffset, out List<Vector2Int> coords))
        {
            // Kill any running tween
            _snapTween?.Kill();
            SnapToGridDOTween(snapOffset, coords);
        }
        else
        {
            // Return or leave piece at current position
            ReleaseCurrentCells();
        }
    }
    #endregion

    /// <summary>
    /// Smoothly snaps the piece to the grid using DOTween.
    /// </summary>
    private void SnapToGridDOTween(Vector3 offset, List<Vector2Int> coords)
    {
        Vector3 target = transform.position + offset;

        // Optional: lock input during snap
        InputLocker.Instance.LockInput();

        _snapTween = transform.DOMove(target, _snapDuration)
                               .SetEase(_snapEase)
                               .OnComplete(() =>
                               {
                                   transform.position = target.Round(2); // ensure exact alignment

                                   // Mark cells as occupied
                                   foreach (var c in coords)
                                       GridManager.Instance.OccupyCell(c);

                                   _placedCoords.Clear();
                                   _placedCoords.AddRange(coords);

                                   _canBeRotated = false;

                                   PlaySnapFeedback();
                                   InputLocker.Instance.UnlockInput();
                               });
    }

    /// <summary>
    /// Plays punch scale, sound and optional haptic feedback after snapping.
    /// </summary>
    private void PlaySnapFeedback()
    {
        const float scalePunch = 0.1f;
        const float punchTime = 0.12f;

        transform.DOPunchScale(Vector3.one * scalePunch, punchTime, 5, 0.7f)
                 .SetEase(Ease.OutExpo)
                 .OnComplete(() => transform.localScale = Vector3.one);

        if (_audioSource && _lockClip)
            _audioSource.PlayOneShot(_lockClip);

#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#endif
    }


    #region Placement Logic
    /// <summary>
    /// Returns true if every child block can be placed,
    /// outputs common snap offset & target grid coords.
    /// </summary>
    private bool TryGetValidPlacement(out Vector3 offset, out List<Vector2Int> coords)
    {
        coords = new List<Vector2Int>();
        offset = Vector3.zero;

        // 1) First child → reference cell
        if (!GridManager.Instance.TryGetCellCoord(_cells[0].position, out Vector2Int first))
            return false;

        // Offset: world pos of first grid cell – current child pos
        offset = GridManager.Instance.GetCellWorldPos(first) - _cells[0].position;

        // 2) Other child blocks
        foreach (Transform t in _cells)
        {
            Vector3 targetWorld = t.position + offset;

            if (!GridManager.Instance.TryGetCellCoord(targetWorld, out Vector2Int coord))
                return false;

            if (GridManager.Instance.IsCellOccupied(coord))
                return false;

            coords.Add(coord);
        }

        return true;
    }

    private void ReleaseCurrentCells()
    {
        foreach (var c in _placedCoords)
            GridManager.Instance.FreeCell(c);

        _placedCoords.Clear();
        _canBeRotated = true; // allow rotation again when piece is lifted
    }
    #endregion
}
