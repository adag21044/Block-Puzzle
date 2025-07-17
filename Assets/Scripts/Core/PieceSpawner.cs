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

    public void SpawnPiecesFromJson()
    {
        int levelIndex = LevelManager.LevelIndex;
        Debug.Log($"Spawning pieces for level index: {levelIndex}");

        string jsonPath = Path.Combine(Application.streamingAssetsPath, "Game159Params.json");
        string jsonText = File.ReadAllText(jsonPath);
        JObject data = JObject.Parse(jsonText);

        JArray piecesArray = (JArray)data["pieceIDs"][levelIndex];
        JArray piecesAngles = (JArray)data["pieceAngles"][levelIndex];

        Camera mainCamera = Camera.main;

        float camZ = startPoint.position.z - mainCamera.transform.position.z;

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0.05f, 0.1f, camZ));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(0.95f, 0.4f, camZ));

        float minX = bottomLeft.x;
        float maxX = topRight.x;
        float minY = bottomLeft.y;
        float maxY = topRight.y;

        Debug.Log($"Spawn area X: {minX} to {maxX}, Y: {minY} to {maxY}");

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

        for (int i = 0; i < piecesArray.Count; i++)
        {
            int id = (int)piecesArray[i];
            float angle = (float)piecesAngles[i];

            GameObject prefab = piecePrefabs[id - 1];

            // Generate random position in camera bounds
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            Vector3 targetPos = new Vector3(randomX, randomY, startPoint.position.z);
            Vector3 startPos = targetPos - new Vector3(0f, 5f, 0f);

            GameObject piece = Instantiate(prefab, startPos, Quaternion.Euler(0, 0, -angle), pieceParent);
            piece.name = $"Piece_{id}";
            spawnedPieces.Add(piece);

            var view = piece.GetComponent<PieceView>();
            if (view != null)
            {
                view.SetColor(assignedColors[i]);
            }

            piece.transform.DOMoveY(targetPos.y, 1f).SetEase(Ease.OutBack).SetDelay(0.1f * i);
        }
    }

}
