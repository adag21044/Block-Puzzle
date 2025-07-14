using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles drag & drop of a single puzzle piece (parent GameObject).
/// Every child under this object represents a 1×1 block.
/// </summary>
public class PieceDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField] private LayerMask gridLayer;
    
    private readonly List<Transform> _cells = new();          // Child blocks
    private readonly List<Vector2Int> _placedCoords = new();  // Currently occupied cells

    private Vector3 _offset;          // Mouse offset
    private Vector3 _homePos;         // Spawn position
    private bool _canBeRotated = true; 
    public bool CanbeRotated
    {
        get => _canBeRotated;
    }

    #region Audio
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    #endregion

    #region Unity Events
    private void Awake()
    {
        foreach (Transform child in transform)
            _cells.Add(child);
    }

    private void Start() => _homePos = transform.position;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(InputLocker.Instance.IsInputLocked)
        {
            Debug.Log("Input is locked, cannot drag piece.");
            return;
        }

        _homePos = transform.position; // Update & Save home position

        // Serbest bırak (parça zaten ızgaradaysa)
        ReleaseCurrentCells();

        // Drag başlangıcındaki mouse-ofsetini sakla
        Vector3 world = Camera.main.ScreenToWorldPoint(eventData.position);
        _offset = transform.position - world;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (InputLocker.Instance.IsInputLocked)
        {
            return;
        }

        Vector3 world = Camera.main.ScreenToWorldPoint(eventData.position) + _offset;
        world.z = 0;
        transform.position = world;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (InputLocker.Instance.IsInputLocked)
        {
            return;
        }

        if (TryGetValidPlacement(out Vector3 snapOffset, out List<Vector2Int> coords))
        {
            // Yerleştir
            transform.position += snapOffset;

            foreach (var c in coords)
            {
                GridManager.Instance.OccupyCell(c);
            }

            _placedCoords.Clear();
            _placedCoords.AddRange(coords);
            _canBeRotated = false; // Flag to control rotation
        }
        else
        {
            bool isOverGrid = false;

            foreach (Transform cell in _cells)
            {
                if (GridManager.Instance.TryGetCellCoord(cell.position, out _))
                {
                    isOverGrid = true;
                    break;
                }
            }

            if (isOverGrid)
            {
                transform.position = _homePos; // piece is over grid but not valid placement
            }

            // Serbest bırak
            ReleaseCurrentCells();
        }
    }

    

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    #endregion

    #region Placement Logic
    /// <summary>
    /// Returns true if every child block can be placed,
    /// outputs common snap offset & target grid coords.
    /// </summary>
    private bool TryGetValidPlacement(out Vector3 offset, out List<Vector2Int> coords)
    {
        coords = new List<Vector2Int>();
        offset = Vector3.zero;

        // 1) İlk hücre → referans
        if (!GridManager.Instance.TryGetCellCoord(_cells[0].position, out Vector2Int first))
            return false;

        // Offset: world pos of first grid cell – current child pos
        offset = GridManager.Instance.GetCellWorldPos(first) - _cells[0].position;

        // 2) Diğer hücreler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(clickSound);
    }

    #endregion
}
