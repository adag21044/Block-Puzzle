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

    private void Awake()
    {
        SpawnPiecesFromJson();
    }

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

        float startX = -((piecesArray.Count - 1) * spacing) / 2f;

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

            Vector3 targetPos = startPoint.position + new Vector3(i * spacing, 0f, 0f);
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
