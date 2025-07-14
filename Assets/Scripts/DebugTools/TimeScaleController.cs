using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
    public static TimeScaleController Instance;

    [Range(0.1f, 5f)]
    [SerializeField] private float timeScale = 1f;
    private bool isTimeFreeze = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Seviye geçişinde yok olmasın
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus)) // Zamanı yavaşlat
        {
            SetTimeScale(timeScale - 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.Plus)) // Zamanı hızlandır
        {
            SetTimeScale(timeScale + 0.5f);
        }
        else if (!isTimeFreeze && Input.GetKeyDown(KeyCode.I)) // Zamanı sıfırla (dondur)
        {
            SetTimeScale(0f);
            isTimeFreeze = true;
        }
        else if (isTimeFreeze && Input.GetKeyDown(KeyCode.I)) // Zamanı eski haline getir
        {
            SetTimeScale(1f);
            isTimeFreeze = false;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (!InputLocker.Instance.IsInputLocked)
            {
                Timer.Instance.FinishTimer(); // Timer'ı bitir ve LoseGame çağır
                Debug.Log("⛔ Timer finished manually with X key!");
            }
        }
    }

    public void SetTimeScale(float scale)
    {
        timeScale = Mathf.Clamp(scale, 0f, 5f);
        Time.timeScale = timeScale;
        Debug.Log($"⏱️ Time scale set to {timeScale}");
    }

    public float GetTimeScale() => timeScale;
}
