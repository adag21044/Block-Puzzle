using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using DG.Tweening;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> piecePrefabs; // Prefab list 
    [SerializeField] private Transform pieceParent; // Parent transform for spawned pieces
    [SerializeField] private float spacing = 2f; // Spacing between pieces
    [SerializeField] private int levelIndex = 0; // Index to select the level configuration

    private List<GameObject> spawnedPieces = new();
    [SerializeField] private Transform startPoint; // Starting point for spawning pieces

    [SerializeField] private float spacingX = 2f; // Horizontal spacing between pieces
    [SerializeField] private float spacingY = 5f; // Vertical spacing between pieces

    /*private void Awake()
    {
        SpawnPiecesFromJson();
    }*/

    public void SpawnPiecesFromJson()
    {
        int levelIndex = LevelManager.LevelIndex;
        Debug.Log($"Spawning pieces for level index: {levelIndex}");

        // Load JSON data
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "Game159Params.json");
        string jsonText = File.ReadAllText(jsonPath);
        JObject data = JObject.Parse(jsonText);

        JArray piecesArray = (JArray)data["pieceIDs"][levelIndex];
        JArray piecesAngles = (JArray)data["pieceAngles"][levelIndex];

        // Shuffle color pool
        List<Color> colorPool = new(PieceColorData.Colors);
        System.Random rng = new();
        colorPool.Sort((a, b) => rng.Next(-1, 2));

        List<Color> assignedColors = new();
        int colorIndex = 0;
        for (int i = 0; i < piecesArray.Count; i++)
        {
            if (colorIndex >= colorPool.Count)
            {
                colorIndex = 0;
                colorPool.Sort((a, b) => rng.Next(-1, 2));
            }
            assignedColors.Add(colorPool[colorIndex++]);
        }

        // Define layout parameters
        int piecesPerRow = 3;
        float offsetX = spacingX; // horizontal spacing
        float offsetY = spacingY; // vertical spacing

        // Calculate start point (centered)
        int totalRows = Mathf.CeilToInt(piecesArray.Count / (float)piecesPerRow);
        for (int i = 0; i < piecesArray.Count; i++)
        {
            int id = (int)piecesArray[i];
            float angle = (float)piecesAngles[i];
            GameObject prefab = piecePrefabs[id - 1];

            int row = i / piecesPerRow;
            int col = i % piecesPerRow;

            Vector3 rowStart = new Vector3(startPoint.position.x, startPoint.position.y - (row * offsetY), startPoint.position.z);

            // Instantiate temporarily to get bounds
            GameObject tempPiece = Instantiate(prefab);
            Renderer renderer = tempPiece.GetComponentInChildren<Renderer>();

            float pieceWidth = renderer.bounds.size.x;
            Destroy(tempPiece);

            // Calculate cumulative offset for the row
            float xOffset = 0f;

            // Sum widths of previous pieces in this row
            for (int j = 0; j < col; j++)
            {
                int prevIndex = row * piecesPerRow + j;
                if (prevIndex < i)
                {
                    int prevId = (int)piecesArray[prevIndex];
                    GameObject prevPrefab = piecePrefabs[prevId - 1];
                    GameObject tempPrev = Instantiate(prevPrefab);
                    Renderer prevRenderer = tempPrev.GetComponentInChildren<Renderer>();

                    xOffset += prevRenderer.bounds.size.x + spacing;

                    Destroy(tempPrev);
                }
            }

            Vector3 targetPos = rowStart + new Vector3(xOffset, 0f, 0f);

            // Camera bounds check (opsiyonel aynı kalabilir)
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(targetPos);
            if (viewportPos.x < 0f || viewportPos.x > 1f || viewportPos.y < 0f || viewportPos.y > 1f)
            {
                Vector3 clampedViewportPos = new Vector3(
                    Mathf.Clamp(viewportPos.x, 0.05f, 0.95f),
                    Mathf.Clamp(viewportPos.y, 0.05f, 0.95f),
                    viewportPos.z
                );
                targetPos = Camera.main.ViewportToWorldPoint(clampedViewportPos);
            }

            Vector3 startPos = targetPos - new Vector3(0f, 5f, 0f);

            GameObject piece = Instantiate(prefab, startPos, Quaternion.Euler(0, 0, -angle), pieceParent);
            piece.name = $"Piece_{id}";
            spawnedPieces.Add(piece);

            var view = piece.GetComponent<PieceView>();
            if (view != null)
            {
                view.SetColor(assignedColors[i]);
            }

            piece.transform.DOMoveY(targetPos.y, 1f).SetEase(Ease.OutBack).SetDelay(0.2f * i);
        }

    }

}
