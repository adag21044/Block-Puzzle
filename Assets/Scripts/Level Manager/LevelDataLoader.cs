using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class LevelDataLoader
{
    private static JObject jsonData;

    static LevelDataLoader()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Game159Params.json");

        if (!File.Exists(path))
        {
            Debug.LogError($"❌ JSON file not found at path: {path}");
            return;
        }

        try
        {
            string content = File.ReadAllText(path);
            jsonData = JObject.Parse(content);
            Debug.Log("✅ Loaded Game159Params.json content.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Failed to parse JSON: {e.Message}");
        }
    }

    public static int GetGridSize()
    {
        return TryGetArrayValue("gridSize");
    }

    public static int GetTime()
    {
        return TryGetArrayValue("time");
    }

    public static List<int> GetPieceIDs()
    {
        return TryGetArrayList<int>("pieceIDs");
    }

    public static List<float> GetPieceAngles()
    {
        return TryGetArrayList<float>("pieceAngles");
    }

    private static int TryGetArrayValue(string key)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("⚠️ JSON data not loaded.");
            return 0;
        }

        JArray array = jsonData[key] as JArray;

        if (array == null || LevelManager.LevelIndex >= array.Count)
        {
            Debug.LogWarning($"⚠️ Key '{key}' not found or index out of range.");
            return 0;
        }

        return (int)array[LevelManager.LevelIndex];
    }

    private static List<T> TryGetArrayList<T>(string key)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("⚠️ JSON data not loaded.");
            return new List<T>();
        }

        JArray array = jsonData[key] as JArray;

        if (array == null || LevelManager.LevelIndex >= array.Count)
        {
            Debug.LogWarning($"⚠️ Key '{key}' not found or index out of range.");
            return new List<T>();
        }

        return array[LevelManager.LevelIndex].ToObject<List<T>>();
    }
}
