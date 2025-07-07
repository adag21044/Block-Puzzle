using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public float CellSize = 1.64f; // Default cell size
    public float Spacing = 0.1f; // Spacing between cells

    private Dictionary<Vector2Int, Vector3> gridPositions = new Dictionary<Vector2Int, Vector3>();
    private HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();

    public int gridSize = 5; // Default grid size, can be set from JSON

    void Awake()
    {
        Instance = this;
    }

    public void RegisterCell(int x, int y, Vector3 position)
    {
        gridPositions[new Vector2Int(x, y)] = position;
    }

    public Vector3? GetClosestAvailableCell(Vector3 worldPos, float maxDistance = 0.8f)
    {
        Vector3? closest = null;
        float closestDist = float.MaxValue;

        foreach (var kvp in gridPositions)
        {
            if (occupiedCells.Contains(kvp.Key))
                continue;

            float distance = Vector3.Distance(worldPos, kvp.Value);
            if (distance <= maxDistance && distance < closestDist)
            {
                closestDist = distance;
                closest = kvp.Value;
            }
        }

        return closest;
    }


    public void MarkCellOccupied(Vector3 position)
    {
        foreach (var kvp in gridPositions)
        {
            if (kvp.Value == position)
            {
                occupiedCells.Add(kvp.Key);
                break; // Exit after marking the cell as occupied
            }
        }
    }

    public bool TryGetCellCoord(Vector3 worldPos, out Vector2Int coord, float maxDist = 0.8f)
    {
        coord = default;
        float best = float.MaxValue;

        foreach (var kvp in gridPositions)
        {
            float d = Vector3.Distance(worldPos, kvp.Value);
            if (d <= maxDist && d < best)
            {
                best  = d;
                coord = kvp.Key;
            }
        }
        return best < float.MaxValue;
    }

    public Vector3 GetCellWorldPos(Vector2Int coord) => gridPositions[coord];

    public bool IsCellOccupied(Vector2Int coord) => occupiedCells.Contains(coord);

    public void OccupyCell(Vector2Int coord) => occupiedCells.Add(coord);

    public void FreeCell(Vector2Int coord) => occupiedCells.Remove(coord);

    
}
