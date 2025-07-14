using UnityEngine;
using TMPro;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GridCreator gridCreator;
    [SerializeField] private PieceSpawner pieceSpawner;
    [SerializeField] private Transform gridParent;
    [SerializeField] private TMP_Text levelNumberText;
    [SerializeField] private CountdownAnimator countdownAnimator;
    [SerializeField] private TapToPlayUI tapToPlayUI;

    private bool waitingForTap = true;

    private void Start()
    {
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

        LoadLevel(LevelManager.LevelIndex);

        countdownAnimator.StartCountdown(LevelDataLoader.GetTime(), OnCountdownFinished);
    }

    public void LoadLevel(int index)
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

        InputLocker.Instance.LockInput(); // Input lock

        countdownAnimator.StartCountdown(LevelDataLoader.GetTime(), OnCountdownFinished);
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
}
