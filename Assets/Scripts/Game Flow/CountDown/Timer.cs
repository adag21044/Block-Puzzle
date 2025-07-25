using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }
    private float currentTime;
    [SerializeField] private TextMeshProUGUI timerText; // UI element to display the timer
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartTimer(float time)
    {
        currentTime = time;
        StartCoroutine(RunTimer());
    }


    private void UpdateTimerText()
    {
        // Format the time to show  seconds
        int seconds = Mathf.FloorToInt(currentTime);
        timerText.text = seconds.ToString("00"); 
    }

    private IEnumerator RunTimer()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                currentTime = 0;
            }

            UpdateTimerText();

            yield return null;
        }

        currentTime = 0;
        UpdateTimerText();
        Debug.Log("Timer finished!");
        InputLocker.Instance.LockInput(); // Lock input when timer finishes
        timerText.text = "00"; // Update the UI text to show 00
        GameManager.Instance.LoseGame(); // Notify GameManager that the timer has finished
    }

    public void FinishTimer()
    {
        StopAllCoroutines(); // Stop the timer coroutine
        currentTime = 0; // Reset the timer
        UpdateTimerText(); // Update the UI text to show 00
        Debug.Log("Timer finished early!");
        InputLocker.Instance.LockInput(); // Lock input when timer finishes
        GameManager.Instance.LoseGame(); // Notify GameManager that the timer has finished
    }

    public void StopTimer()
    {
        StopAllCoroutines(); // Stop the timer coroutine
        currentTime = 0; // Reset the timer
        UpdateTimerText(); // Update the UI text to show 00
    }
}