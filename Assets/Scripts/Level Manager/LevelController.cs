using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GridCreator gridCreator;
    [SerializeField] private PieceSpawner pieceSpawner;
    [SerializeField] private Transform gridParent;
    [SerializeField] private TMP_Text levelNumberText;
    [SerializeField] private CountdownAnimator countdownAnimator;
    [SerializeField] private TapToPlayUI tapToPlayUI;
    [SerializeField] private GameObject levelSelectionUI;

    private bool waitingForTap = true;
    [SerializeField] private Button[] buttons;



    private void Start()
    {
        DataPersistenceManager.Instance.LoadGame();

        // 2) Kayıttan gelen düğme durumlarını uygula
        UpdateLevelButtons(); 

        UpdateLevelNumberText();

        tapToPlayUI.gameObject.SetActive(true);
        countdownAnimator.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (waitingForTap && Input.GetMouseButtonDown(0))
        {
            StartSequence();
            tapToPlayUI.ShowLevelAndTimer();
        }

        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                LoadLevel(i - 1);
            }
        }
    }



    private void StartSequence()
    {
        waitingForTap = false;

        tapToPlayUI.Hide();
        countdownAnimator.gameObject.SetActive(true);

        GameManager.Instance.ResetGameState();
        Timer.Instance.StopTimer();

        InputLocker.Instance.LockInput(); // 5 saniyelik countdown için input kilitle

        //LoadLevel(LevelManager.LevelIndex);

        countdownAnimator.StartCountdown(LevelDataLoader.GetTime(), OnCountdownFinished);
    }

    public void LoadLevel(int index)
    {
        levelSelectionUI.SetActive(false); // deactivate level selection UI

        if (index < 0 || index >= 13)
        {
            Debug.LogWarning($"❌ Invalid level index: {index}");
            return;
        }

        Debug.Log($"Loading level {index + 1}");

        ClearScene();


        UpdateLevelNumberText();

        gridCreator.CreateGridFromLevel();
        pieceSpawner.SpawnPiecesFromJson();

        GameManager.Instance.ResetGameState();
        Timer.Instance.StopTimer();

        InputLocker.Instance.LockInput(); // Input lock

        countdownAnimator.gameObject.SetActive(true);
        countdownAnimator.StartCountdown(LevelDataLoader.GetTime(), OnCountdownFinished);
    }

    public void OnLevelButtonClicked(int index)
    {
        LevelManager.LevelIndex = index;
        LoadLevel(index);
    }



    private void OnCountdownFinished()
    {
        InputLocker.Instance.UnlockInput(); // Countdown bitti → input açılır

        Timer.Instance.StartTimer(LevelDataLoader.GetTime());
    }



    private void ClearScene()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in pieceSpawner.transform)
        {
            Destroy(child.gameObject);
        }

        GridManager.Instance.ClearAll();
    }

    private void UpdateLevelNumberText()
    {
        levelNumberText.text = "Level: " + (LevelManager.LevelIndex + 1);
    }

    public void LoadData(GameData data)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = i <= data.maxUnlockedLevel;
        }

        LevelManager.LevelIndex = data.currentLevelIndex; // KAYDEDİLMİŞ SEVİYEYİ YÜKLE
        UpdateLevelNumberText();  
    }


    public void SaveData(ref GameData data)
    {
        // 1) Geçerli seviyeyi mutlaka kaydet
        data.currentLevelIndex = LevelManager.LevelIndex;

        
    }

    
    public void UpdateLevelButtons()
    {
        GameData data = DataPersistenceManager.Instance.GetGameData();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = i <= data.maxUnlockedLevel;
        }
    }


}
