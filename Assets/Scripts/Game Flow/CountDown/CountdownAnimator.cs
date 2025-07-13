using TMPro;
using UnityEngine;
using System.Collections;

public class CountdownAnimator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText; // countdown text UI element
    [SerializeField] private float countdownTime = 5f; // total countdown time in seconds
    [SerializeField] private AnimationCurve scaleCurve; // curve for scaling animation
    [SerializeField] private AnimationCurve alphaCurve; // curve for alpha animation
    [SerializeField] private static bool isCountdownFinished = false; // flag to check if countdown is finished
    public static bool IsCountdownFinished => isCountdownFinished; // public getter for the countdown finished flag
    [SerializeField] private Timer timer; // reference to the Timer 
    [SerializeField] private float gameDuration; // duration of the game in seconds


    private void Start()
    {
        gameDuration = LevelDataLoader.GetTime(); // fetch game duration from LevelDataLoader

        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        int count = (int)countdownTime;

        while (count > 0)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = count.ToString();

            yield return StartCoroutine(AnimateText());

            count--;
        }

        countdownText.text = "GO!";
        yield return StartCoroutine(AnimateText());

        countdownText.gameObject.SetActive(false);
        InputLocker.Instance.UnlockInput();
        isCountdownFinished = true;

        // Geri sayım bitti → grid ve parçaları sahneye al
        //gridCreator.AnimateGridEntry();
        //pieceSpawner.SpawnPiecesFromJson();

        timer.StartTimer(gameDuration);
    }

    private IEnumerator AnimateText()
    {
        float duration = 1f; // duration of the animation
        float elapsed = 0f; // time elapsed since the start of the animation
        Color baseColor = countdownText.color; // original color of the text
        Vector3 originalScale = Vector3.one; // original scale of the text

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // Animate scale
            float scale = scaleCurve.Evaluate(t);
            countdownText.transform.localScale = originalScale * scale;

            // Animate alpha
            float alpha = alphaCurve.Evaluate(t);
            countdownText.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        countdownText.transform.localScale = originalScale;
        countdownText.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
    }

    public void RestartCountDown()
    {
        StopAllCoroutines(); // Stop any ongoing countdown
        StartCoroutine(CountdownRoutine()); // Restart the countdown
    }
        
}

   
    
