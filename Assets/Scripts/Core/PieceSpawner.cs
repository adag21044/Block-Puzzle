using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> piecePrefabs; // Prefab list 
    [SerializeField] private Transform pieceParent; // Parent transform for spawned pieces
    [SerializeField] private float spacing = 2f; // Spacing between pieces
    [SerializeField] private int levelIndex = 0; // Index to select the level configuration

    private List<GameObject> spawnedPieces = new List<GameObject>();
    [SerializeField] private Transform startPoint; // Starting point for spawning pieces

    private void Start()
    {
        SpawnPiecesFromJson();
    }

    private void SpawnPiecesFromJson()
    {
        Debug.Log($"Spawning pieces for level index: {levelIndex}");
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "Assets/Data/Game159Params.json");
        string jsonText = File.ReadAllText(jsonPath);
        JObject data = JObject.Parse(jsonText);

        JArray piecesArray = (JArray)data["pieceIDs"][levelIndex]; // Get the pieces for the specified level
        JArray piecesAngles = (JArray)data["pieceAngles"][levelIndex]; // Get the angles for the specified level

        float startX = -((piecesArray.Count - 1) * spacing) / 2f; // Center the pieces horizontally

        for (int i = 0; i < piecesArray.Count; i++)
        {
            int id = (int)piecesArray[i];
            float angle = (float)piecesAngles[i];

            GameObject prefab = piecePrefabs[id - 1]; // Get the prefab based on the ID (assuming IDs start from 1)
            Vector3 position = startPoint.position + new Vector3(i * spacing, 0f, 0f);  // Calculate the position for the piece

            GameObject piece = Instantiate(prefab, position, Quaternion.Euler(0, 0, -angle), pieceParent);
            piece.name = $"Piece_{id}";
            spawnedPieces.Add(piece); // Add the spawned piece to the list

            // Assign color using PieceColorData model (MVC structure)
            var view = piece.GetComponent<PieceView>();
            if (view != null)
            {
                Color color = PieceColorData.Colors[i % PieceColorData.Colors.Length];
                view.SetColor(color);
            }
            else
            {
                Debug.LogWarning($"PieceView component not found on {piece.name}");
            }
        }
    }
}
