using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void OnNewGame()
    {
        DataPersistenceManager.Instance.NewGame();
        Debug.Log("New game started.");
    }

    public void OnLoadGame()
    {
        DataPersistenceManager.Instance.LoadGame();
        Debug.Log("Game loaded.");
    } 

    public void OnSaveGame()
    {
        DataPersistenceManager.Instance.SaveGame();
        Debug.Log("Game saved.");
    }  
}