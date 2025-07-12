using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GridCreator gridCreator; // Reference to the GridCreator component
    [SerializeField] private PieceSpawner pieceSpawner; // Reference to the PieceSpawner component
    [SerializeField] private Transform gridParent;
    [SerializeField] private TMP_Text levelNumberText; // UI Text to display the current level number

    private void Start()
    {
        UpdateLevelNumberText();
    }

    private void Update()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                LoadLevel(i - 1);
            }
        }
    }

    private void LoadLevel(int index)
    {
        if (index < 0 || index >= 13)
        {
            Debug.LogWarning($"❌ Invalid level index: {index}");
            return;
        }

        Debug.Log($"Loading level {index + 1}");

        ClearScene();

        LevelManager.LevelIndex = index;
        UpdateLevelNumberText();

        gridCreator.CreateGridFromLevel();
        pieceSpawner.SpawnPiecesFromJson();

        GameManager.Instance.ResetGameState();
        Timer.Instance.StopTimer();

        // fetch level data duration
        int duration = LevelDataLoader.GetTime();
        Timer.Instance.StartTimer(duration);
    }

    private void ClearScene()
    {
        // Clear existing grid cells
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        // Clear existing pieces
        foreach (Transform child in pieceSpawner.transform)
        {
            Destroy(child.gameObject);
        }

        GridManager.Instance.ClearAll();
    }

    private void UpdateLevelNumberText()
    {
        if (levelNumberText != null)
        {
            levelNumberText.text = "Level:" + (LevelManager.LevelIndex + 1).ToString();
        }
    }
}