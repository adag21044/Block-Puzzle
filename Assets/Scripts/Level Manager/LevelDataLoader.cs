using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class LevelDataLoader
{
    private static JObject jsonData;

    static LevelDataLoader()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Game159Params.json");
        string content = File.ReadAllText(path);
        jsonData = JObject.Parse(content);
        Debug.Log("✅ Loaded Game159Params.json content: " + content);

    }

    public static int GetGridSize()
    {
        return (int)jsonData["gridSize"][LevelManager.LevelIndex];
    }

    public static int GetTime()
    {
        return (int)jsonData["time"][LevelManager.LevelIndex];
    }

    public static List<int> GetPieceIDs()
    {
        return jsonData["pieceIDs"][LevelManager.LevelIndex].ToObject<List<int>>();
    }

    public static List<float> GetPieceAngles()
    {
        return jsonData["pieceAngles"][LevelManager.LevelIndex].ToObject<List<float>>();
    }

}