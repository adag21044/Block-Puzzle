using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    private float currentTime;
    private float startTime = 10f;
    [SerializeField] private TextMeshProUGUI timerText; // UI element to display the timer

    public void StartTimer(float time)
    {
        currentTime = time;
        StartCoroutine(RunTimer());
    }


    private void UpdateTimerText()
    {
        // Format the time to show  seconds
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}", seconds);
    }

    private IEnumerator RunTimer()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();
            yield return null;
        }

        currentTime = 0;
        UpdateTimerText();
        Debug.Log("Timer finished!");
    }
}