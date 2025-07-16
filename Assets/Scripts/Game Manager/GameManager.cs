using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int totalCellCount;
    private int filledCellCount;
    private bool gameEnded = false;

    private void Awake()
    {
        Instance = this;
    }

    public void SetTotalCells(int gridSize)
    {
        totalCellCount = gridSize * gridSize;
    }

    public void OnCellFilled()
    {
        if (gameEnded)
        {
            return;
        }

        filledCellCount++;

        if (filledCellCount >= totalCellCount)
        {
            EndGame();
        }
    }

    public void OnCellEmptied()
    {
        if (gameEnded)
        {
            return;
        }

        filledCellCount--;
    }

    public void LoseGame()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;
        Debug.Log("You lost the game!");
        Debug.Log("Score: " + Mathf.RoundToInt(100f * filledCellCount / (float)totalCellCount));
        InputLocker.Instance.LockInput(); // Lock input when the game is lost
    }

    private void WinGame()
    {
        gameEnded = true;
        Debug.Log("You won the game!");
        Debug.Log("Score: " + Mathf.RoundToInt(100f * filledCellCount / (float)totalCellCount));
        Timer.Instance.StopTimer(); // Stop the timer when the game is won
        InputLocker.Instance.LockInput(); // Unlock input when the game is won
        Debug.Log("You won the game and Input is locked!");

        DataPersistenceManager dataManager = DataPersistenceManager.Instance;
        GameData data = dataManager.GetGameData();

        if (LevelManager.LevelIndex >= data.maxUnlockedLevel)
        {
            data.maxUnlockedLevel = LevelManager.LevelIndex + 1;
            DataPersistenceManager.Instance.SaveGame();
        }

        LevelController levelController = FindObjectOfType<LevelController>();
        if (levelController != null)
        {
            Debug.Log("Updating level buttons after winning the game.");
            levelController.UpdateLevelButtons();
        }


        Debug.Log("Unlocked level updated and saved!");
    }

    private void EndGame()
    {
        gameEnded = true;
        Debug.Log("Game ended!");
        if (filledCellCount >= totalCellCount)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
        Debug.Log("Final Score: " + GetScore());
    }

    public float GetScore()
    {
        if (100 * filledCellCount / totalCellCount == 100)
        {
            return 150; // Bonus score for perfect completion
        }

        return 100f * (float)filledCellCount / (float)totalCellCount;
    }

    public void ResetGameState()
    {
        filledCellCount = 0;
        gameEnded = false;
        InputLocker.Instance.UnlockInput(); // Unlock input when the game state is reset
    }

}