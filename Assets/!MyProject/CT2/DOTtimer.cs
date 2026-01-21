using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DOTtimer : MonoBehaviour
{
    [SerializeField] private float timerDuration = 60f;
    [SerializeField] private bool autoStart = true;
    [SerializeField] private bool countdown = true;
    [SerializeField] private bool loopTimer = false;

    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI timerText;

    private float currentTime;
    private Tween timerTween;
    private bool isTimerRunning = false;

    void Start()
    {
        InitializeTimer();
        if (autoStart) StartTimer();
    }

    void OnDestroy()
    {
        timerTween?.Kill();
    }

    private void InitializeTimer()
    {
        if (progressSlider != null)
        {
            progressSlider.minValue = 0;
            progressSlider.maxValue = timerDuration;
            progressSlider.value = countdown ? timerDuration : 0;
        }

        if (timerText != null)
        {
            timerText.text = FormatTime(countdown ? timerDuration : 0);
        }

        currentTime = countdown ? timerDuration : 0;
    }

    public void StartTimer()
    {
        if (isTimerRunning) return;

        isTimerRunning = true;

        if (progressSlider != null)
        {
            float targetValue = countdown ? 0 : timerDuration;

            timerTween = progressSlider.DOValue(targetValue, timerDuration)
                .SetEase(Ease.Linear)
                .OnUpdate(UpdateTimerDisplay)
                .OnComplete(OnTimerComplete)
                .OnKill(() => isTimerRunning = false);
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerTween == null) return;

        currentTime = countdown ?
            timerDuration - timerTween.ElapsedPercentage() * timerDuration :
            timerTween.ElapsedPercentage() * timerDuration;

        if (timerText != null)
        {
            timerText.text = FormatTime(currentTime);
        }
    }

    private void OnTimerComplete()
    {
        isTimerRunning = false;

        if (timerText != null)
        {
            timerText.text = FormatTime(countdown ? 0 : timerDuration);
        }

        if (loopTimer)
        {
            DOVirtual.DelayedCall(1f, () => {
                InitializeTimer();
                StartTimer();
            });
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:00}:{seconds:00}";
    }

    public void PauseTimer()
    {
        if (!isTimerRunning) return;
        timerTween?.Pause();
    }

    public void ResumeTimer()
    {
        if (!isTimerRunning) return;
        timerTween?.Play();
    }

    public void StopTimer()
    {
        if (!isTimerRunning) return;
        isTimerRunning = false;
        timerTween?.Kill();
    }

    public void RestartTimer()
    {
        StopTimer();
        InitializeTimer();
        StartTimer();
    }

    public bool IsTimerRunning() => isTimerRunning;
}