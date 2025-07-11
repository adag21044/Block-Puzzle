using System.Collections.Generic;
using UnityEngine;

public class JsonDebugTest : MonoBehaviour
{
    private void Start()
    {
        int gridSize = LevelDataLoader.GetGridSize();
        int time = LevelDataLoader.GetTime();
        List<int> pieceIDs = LevelDataLoader.GetPieceIDs();
        List<float> pieceAngles = LevelDataLoader.GetPieceAngles();

        Debug.Log($"✅ Grid Size: {gridSize}");
        Debug.Log($"⏱️ Time: {time}");
        Debug.Log("🧩 Piece IDs and Angles:");

        int count = Mathf.Min(pieceIDs.Count, pieceAngles.Count);
        for (int i = 0; i < count; i++)
        {
            Debug.Log($"ID: {pieceIDs[i]} - Angle: {pieceAngles[i]}");
        }

        if (pieceIDs.Count != pieceAngles.Count)
        {
            Debug.LogWarning($"⚠️ Mismatch: pieceIDs.Count = {pieceIDs.Count}, pieceAngles.Count = {pieceAngles.Count}");
        }
    }
}
