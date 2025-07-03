using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab; // Tek bir kareyi temsil eden prefab
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private int levelIndex = 0; // JSON'daki hangi seviyeyi kullanacağını belirtir

    private int gridSize;

    void Start()
    {
        LoadGridSizeFromJson();
        CreateCenteredGrid();
    }

    void LoadGridSizeFromJson()
    {
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "Game159Params.json");
        string jsonText = File.ReadAllText(jsonPath);
        JObject data = JObject.Parse(jsonText);
        gridSize = (int)data["gridSize"][levelIndex]; // Örn: 5x5 için gridSize = 5
    }

    void CreateCenteredGrid()
    {
        Vector2 centerOffset = new Vector2((gridSize - 1) * cellSize / 2f, (gridSize - 1) * cellSize / 2f);

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector2 position = new Vector2(x * cellSize, y * cellSize) - centerOffset;
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cell.name = $"Cell_{x}_{y}";
            }
        }
    }
}
