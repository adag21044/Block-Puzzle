using DG.Tweening;
using TMPro;
using UnityEngine;

public class TapToPlayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tapToPlayText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI timerText;

    private Tween blinkTween;

    private void Start()
    {
        StartBlink();
    }

    private void StartBlink()
    {
        Color startColor = tapToPlayText.color;
        startColor.a = 0f;
        tapToPlayText.color = startColor;

        blinkTween = tapToPlayText.DOFade(1f, 0.8f).SetLoops(-1, LoopType.Yoyo);
    }

    public void Hide()
    {
        if (blinkTween != null && blinkTween.IsActive())
        {
            blinkTween.Kill();
        }

        tapToPlayText.gameObject.SetActive(false);
    }

    public void ShowLevelAndTimer()
    {
        levelText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
    }
}
