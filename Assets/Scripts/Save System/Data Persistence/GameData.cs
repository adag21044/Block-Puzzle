[System.Serializable]
public class GameData
{
    public int currentLevelIndex;
    public int maxUnlockedLevel;

    public GameData()
    {
        this.currentLevelIndex = 0;
        this.maxUnlockedLevel = 0; 
    }
}
