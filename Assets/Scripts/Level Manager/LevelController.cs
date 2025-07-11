using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GridCreator gridCreator; // Reference to the GridCreator component
    [SerializeField] private PieceSpawner pieceSpawner; // Reference to the PieceSpawner component
    [SerializeField] private Transform gridParent;

    private void Update()
    {
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                LoadLevel(i - 1);
            }
        }
    }

    private void LoadLevel(int index)
    {
        Debug.Log($"Loading level {index + 1}");

        ClearScene();

        LevelManager.LevelIndex = index;

        gridCreator.CreateGridFromLevel();
        pieceSpawner.SpawnPiecesFromJson();

        GameManager.Instance.ResetGameState();
        Timer.Instance.StopTimer();
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

}