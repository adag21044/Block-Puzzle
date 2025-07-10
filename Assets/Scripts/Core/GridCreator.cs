using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using DG.Tweening;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab; // Tek bir kareyi temsil eden prefab
    [SerializeField] private float cellSize = 1.64f;
    [SerializeField] private int levelIndex = 0; // JSON'daki hangi seviyeyi kullanacağını belirtir
    [SerializeField] private float spacing = 0.1f;

    [SerializeField] private int gridSize;

    void Start()
    {
        Debug.Log(cellPrefab.GetComponent<SpriteRenderer>().bounds.size);

        LoadGridSizeFromJson();
        CreateCenteredGrid();

        // Grid parent'ı önce aşağıya konumla
        Vector3 startPos = transform.position - new Vector3(0f, 10f, 0f);
        transform.position = startPos;

        // 10 saniye sonra sahneye girsin
        Invoke(nameof(AnimateGridEntry), 1f);
        
    }
    

    public void AnimateGridEntry()
    {
        transform.DOMoveY(transform.position.y + 10f, 1f).SetEase(Ease.OutBack);
    }

    /// <summary>

    void LoadGridSizeFromJson()
    {
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "Assets/Data/Game159Params.json");
        string jsonText = File.ReadAllText(jsonPath);
        JObject data = JObject.Parse(jsonText);
        gridSize = (int)data["gridSize"][levelIndex]; // Örn: 5x5 için gridSize = 5
        GameManager.Instance.SetTotalCells(gridSize);
    }

    void CreateCenteredGrid()
    {
        float totalCellSize = cellSize + spacing;
        Vector2 centerOffset = new Vector2((gridSize - 1) * totalCellSize / 2f, (gridSize - 1) * totalCellSize / 2f);

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector2 position = new Vector2(x * totalCellSize, y * totalCellSize) - centerOffset;
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cell.name = $"Cell_{x}_{y}";
                GridManager.Instance.RegisterCell(x, y, position);
            }
        }
    }
}
