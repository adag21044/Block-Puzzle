using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = File.ReadAllText(fullPath);
                loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);

               
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load game data from {fullPath}: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"Save file not found at {fullPath}. Returning null.");
        }

        return loadedData;
    }


    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(dataDirPath);

            string dataToStore = JsonUtility.ToJson(gameData, true);

            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(dataToStore);
                }
            }

        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game data to {fullPath}: {e.Message}");
        }
    }
}
 