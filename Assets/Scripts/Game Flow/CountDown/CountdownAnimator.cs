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


    

    public void StartCountdown(float newDuration, System.Action onComplete)
    {
        StopAllCoroutines();
        timer.StopTimer();
        gameDuration = newDuration;
        StartCoroutine(CountdownRoutine(onComplete));
    }

    private IEnumerator CountdownRoutine(System.Action onComplete)
    {
        isCountdownFinished = false;

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
        isCountdownFinished = true;

        onComplete?.Invoke();
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

    //public void RestartCountDown()
    //{
      //  StopAllCoroutines(); // Stop any ongoing countdown
        //StartCoroutine(CountdownRoutine()); // Restart the countdown
    //}
    
    public void Restart(float newDuration)
    {
        StartCountdown(newDuration, null);
    }

        
}

   
    
