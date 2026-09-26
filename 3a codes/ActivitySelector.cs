using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActivitySelector : MonoBehaviour
{
    [Header("Managers")]
    public ActivityNPCRestingSwapManager restingManager;
    public ActivityNPCSwapManager walkingManager;
    public ActivityNPCSwapManager runningManager;

    [Header("Activity Canvas Roots")]
    public GameObject activityCanvasEN;
    public GameObject activityCanvasBM;

    [Header("Button Labels — English")]
    public TextMeshProUGUI restingButtonText;
    public TextMeshProUGUI walkingButtonText;
    public TextMeshProUGUI runningButtonText;

    [Header("Button Labels — Malay")]
    public TextMeshProUGUI restingButtonTextBM;
    public TextMeshProUGUI walkingButtonTextBM;
    public TextMeshProUGUI runningButtonTextBM;

    [Header("Buttons — English")]
    public Button restingButton;
    public Button walkingButton;
    public Button runningButton;

    [Header("Buttons — Malay")]
    public Button restingButtonBM;
    public Button walkingButtonBM;
    public Button runningButtonBM;

    [Header("Status Text")]
    public TextMeshProUGUI statusText;

    [Header("Activity Sounds")]
    public AudioClip walkingSound;
    public AudioClip runningSound;
    [Range(0f, 1f)]
    public float activityVolume = 1f;

    [Header("Timer Panel")]
    public GameObject TimerPulsePanel;

    [Header("UI Follow Points")]
    public Transform restingUIPoint;
    public Transform walkingUIPoint;
    public Transform runningUIPoint;

    [Header("Retry Button")]
    public GameObject retryButton;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 0f;
        _audioSource.volume = activityVolume;

        UpdateStatus("");

        Invoke("RefreshButtonLabels", 0.2f);
    }

    public void RefreshButtonLabels()
    {
        bool isEN = IsEnglish();

        if (activityCanvasEN != null)
            activityCanvasEN.SetActive(isEN);

        if (activityCanvasBM != null)
            activityCanvasBM.SetActive(!isEN);

        if (restingButtonText != null)
            restingButtonText.text = Chapter1Texts.RestingButton();

        if (walkingButtonText != null)
            walkingButtonText.text = Chapter1Texts.WalkingButton();

        if (runningButtonText != null)
            runningButtonText.text = Chapter1Texts.RunningButton();

        if (restingButtonTextBM != null)
            restingButtonTextBM.text = Chapter1Texts.RestingButton();

        if (walkingButtonTextBM != null)
            walkingButtonTextBM.text = Chapter1Texts.WalkingButton();

        if (runningButtonTextBM != null)
            runningButtonTextBM.text = Chapter1Texts.RunningButton();

        Debug.Log("Activity canvas refreshed. EN=" + isEN);
    }

    public void ResetButtons()
    {
        if (restingManager != null)
            restingManager.ResetForRetry();

        if (walkingManager != null)
            walkingManager.ResetForRetry();

        if (runningManager != null)
            runningManager.ResetForRetry();

        EnableButton(restingButton);
        EnableButton(restingButtonBM);

        DisableButton(walkingButton);
        DisableButton(walkingButtonBM);

        DisableButton(runningButton);
        DisableButton(runningButtonBM);

        UpdateStatus("");
        StopActivitySound();

        MoveTimerPanel(restingUIPoint);

        if (TimerPulsePanel != null)
            TimerPulsePanel.SetActive(false);

        if (retryButton != null)
            retryButton.SetActive(true);

        Debug.Log("ActivitySelector reset for retry.");
    }

    public void SetResting()
    {
        MoveTimerPanel(restingUIPoint);

        if (restingManager != null)
            restingManager.PlayFullSequence();

        StopActivitySound();

        if (retryButton != null)
            retryButton.SetActive(false);

        DisableButton(restingButton);
        DisableButton(restingButtonBM);

        DisableButton(walkingButton);
        DisableButton(walkingButtonBM);

        DisableButton(runningButton);
        DisableButton(runningButtonBM);

        UpdateStatus(Chapter1Texts.Resting());

        if (TimerPulsePanel != null)
            TimerPulsePanel.SetActive(false);

        Debug.Log("Resting task started");
    }

    public void SetWalking()
    {
        MoveTimerPanel(walkingUIPoint);

        if (walkingManager != null)
            walkingManager.PlayFullSequence();

        PlayActivitySound(walkingSound);

        if (retryButton != null)
            retryButton.SetActive(false);

        DisableButton(walkingButton);
        DisableButton(walkingButtonBM);

        DisableButton(runningButton);
        DisableButton(runningButtonBM);

        UpdateStatus(Chapter1Texts.Walking());

        if (TimerPulsePanel != null)
            TimerPulsePanel.SetActive(false);

        Debug.Log("Walking task started");
    }

    public void SetRunning()
    {
        MoveTimerPanel(runningUIPoint);

        if (runningManager != null)
            runningManager.PlayFullSequence();

        PlayActivitySound(runningSound);

        if (retryButton != null)
            retryButton.SetActive(false);

        DisableButton(runningButton);
        DisableButton(runningButtonBM);

        UpdateStatus(Chapter1Texts.Running());

        if (TimerPulsePanel != null)
            TimerPulsePanel.SetActive(false);

        Debug.Log("Running task started");
    }

    public void UnlockWalkingButton()
    {
        EnableButton(walkingButton);
        EnableButton(walkingButtonBM);

        Debug.Log("Walking button unlocked after Resting passed.");
    }

    public void UnlockRunningButton()
    {
        EnableButton(runningButton);
        EnableButton(runningButtonBM);

        Debug.Log("Running button unlocked after Walking passed.");
    }

    public void OnActivitySoundStop()
    {
        StopActivitySound();

        if (retryButton != null)
            retryButton.SetActive(true);

        Debug.Log("Activity finished — retry button shown.");
    }

    private void PlayActivitySound(AudioClip clip)
    {
        if (_audioSource == null || clip == null)
            return;

        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private void StopActivitySound()
    {
        if (_audioSource != null && _audioSource.isPlaying)
            _audioSource.Stop();
    }

    private void MoveTimerPanel(Transform target)
    {
        if (TimerPulsePanel == null || target == null)
            return;

        TimerPulsePanel.transform.position = target.position;
        TimerPulsePanel.transform.rotation = target.rotation;
    }

    private void EnableButton(Button btn)
    {
        if (btn != null)
        {
            btn.interactable = true;
            btn.gameObject.SetActive(true);
        }
    }

    private void DisableButton(Button btn)
    {
        if (btn != null)
        {
            btn.interactable = false;
            btn.gameObject.SetActive(false);
        }
    }

    private void UpdateStatus(string activity)
    {
        if (statusText == null)
            return;

        if (string.IsNullOrEmpty(activity))
        {
            statusText.text = "";
            return;
        }

        statusText.text = IsEnglish()
            ? "Subject is " + activity
            : "Subjek sedang " + activity;
    }

    private bool IsEnglish()
    {
        if (LanguageManager.Instance != null)
            return LanguageManager.Instance.IsEnglish();

        return true;
    }
}