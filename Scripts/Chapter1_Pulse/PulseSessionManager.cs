using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PulseSessionManager : MonoBehaviour
{
    public static PulseSessionManager Instance;

    [Header("Settings")]
    public float sessionDuration = 15f;
    public int passTolerance = 5;

    [Header("UI References")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI beatCountText;
    public TextMeshProUGUI resultFeedbackText;
    public GameObject startButton;
    public GameObject recordButton;
    public GameObject countPanel;
    public GameObject feedbackPanel;
    public GameObject retryButton;
    public GameObject nextButton;

    [Header("Hold Prompt UI")]
    public TextMeshProUGUI holdPromptText;
    public Image holdProgressBar;

    [Header("Button Text References")]
    public TextMeshProUGUI startButtonText;
    public TextMeshProUGUI recordButtonText;

    private PulseTarget activeTarget;
    private bool sessionRunning = false;
    private bool sessionCompleted = false;
    private int playerBeatCount = 0;

    private SubjectType lockedSubjectType;
    private string lockedSubjectName;
    private int lockedBPM;

    private int lastPlayerBPM;
    private int lastRealBPM;
    private int lastDifference;
    private bool lastPassed;

    // Store experiment type at detection time
    // so it's available even after
    // activeTarget is cleared
    private string lastExpType = "";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshButtonTexts();
        ResetUI();
    }

    private void RefreshButtonTexts()
    {
        if (startButtonText != null)
            startButtonText.text =
                Chapter1Texts.StartButton();
        if (recordButtonText != null)
            recordButtonText.text =
                Chapter1Texts.RecordButton();
    }

    private void ResetUI()
    {
        if (startButton != null)
            startButton.SetActive(false);
        if (countPanel != null)
            countPanel.SetActive(false);
        if (recordButton != null)
            recordButton.SetActive(false);
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);
        if (holdPromptText != null)
            holdPromptText.gameObject
                .SetActive(false);
        if (holdProgressBar != null)
            holdProgressBar.gameObject
                .SetActive(false);
        if (statusText != null)
            statusText.text =
                Chapter1Texts.PlaceHandOnWrist();
        if (timerText != null)
            timerText.text =
                Chapter1Texts.TimerDefault(
                    sessionDuration);
        if (beatCountText != null)
            beatCountText.text =
                Chapter1Texts.BeatsCount(0);
    }

    // ─── Hold Prompt ──────────────────────────────────

    public void ShowHoldPrompt(
        string subjectName)
    {
        if (holdPromptText != null)
        {
            holdPromptText.gameObject
                .SetActive(true);
            holdPromptText.text =
                Chapter1Texts.HoldStill(
                    subjectName);
        }

        if (holdProgressBar != null)
        {
            holdProgressBar.gameObject
                .SetActive(true);
            holdProgressBar.fillAmount = 0f;
        }
    }

    public void SetHoldProgress(float progress)
    {
        if (holdProgressBar != null)
            holdProgressBar.fillAmount =
                Mathf.Clamp01(progress);
    }

    // ─── Target Management ────────────────────────────

    public void SetActiveTarget(
        PulseTarget target)
    {
        activeTarget = target;
        sessionCompleted = false;

        lockedSubjectType = target.subjectType;
        lockedSubjectName =
            target.GetSubjectName();
        lockedBPM = target.bpm;

        // Store exp type immediately
        // so ProceedToNext can use it
        // even after activeTarget cleared
        lastExpType = target.experimentType;

        Debug.Log("SetActiveTarget: "
            + lockedSubjectName
            + " type=" + lockedSubjectType
            + " bpm=" + lockedBPM
            + " exp=" + lastExpType);

        if (holdPromptText != null)
            holdPromptText.gameObject
                .SetActive(false);
        if (holdProgressBar != null)
            holdProgressBar.gameObject
                .SetActive(false);

        if (statusText != null)
            statusText.text =
                Chapter1Texts.PulseDetected(
                    lockedSubjectName);

        if (PulseHapticPlayer.Instance != null)
            PulseHapticPlayer.Instance
                .StartPulse(target.bpm);

        if (PulseVisualIndicator.Instance
            != null)
            PulseVisualIndicator.Instance
                .StartVisualPulse(target.bpm);

        RefreshButtonTexts();

        if (startButton != null)
            startButton.SetActive(true);
        if (countPanel != null)
            countPanel.SetActive(false);
        if (recordButton != null)
            recordButton.SetActive(false);
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);
    }

    public void ClearActiveTarget()
    {
        if (sessionRunning) return;
        if (sessionCompleted) return;

        activeTarget = null;

        if (holdPromptText != null)
            holdPromptText.gameObject
                .SetActive(false);
        if (holdProgressBar != null)
            holdProgressBar.gameObject
                .SetActive(false);

        if (PulseHapticPlayer.Instance != null)
            PulseHapticPlayer.Instance
                .StopPulse();

        if (PulseVisualIndicator.Instance
            != null)
            PulseVisualIndicator.Instance
                .StopVisualPulse();

        if (statusText != null)
            statusText.text =
                Chapter1Texts.PlaceHandOnWrist();

        if (startButton != null)
            startButton.SetActive(false);
    }

    public void OnHandRemovedDuringSession()
    {
        if (!sessionRunning) return;

        StopAllCoroutines();
        sessionRunning = false;

        if (PulseHapticPlayer.Instance != null)
            PulseHapticPlayer.Instance
                .StopPulse();

        if (PulseVisualIndicator.Instance
            != null)
            PulseVisualIndicator.Instance
                .StopVisualPulse();

        if (statusText != null)
            statusText.text =
                Chapter1Texts.HandRemoved();

        if (countPanel != null)
            countPanel.SetActive(false);
        if (recordButton != null)
            recordButton.SetActive(false);
        if (feedbackPanel != null)
            feedbackPanel.SetActive(true);

        if (resultFeedbackText != null)
            resultFeedbackText.text =
                Chapter1Texts.HandRemoved();

        if (retryButton != null)
            retryButton.SetActive(true);
        if (nextButton != null)
            nextButton.SetActive(false);

        if (activeTarget != null)
            activeTarget.ResetForRetry();
    }

    // ─── Session ──────────────────────────────────────

    public void StartSession()
    {
        if (activeTarget == null)
        {
            if (statusText != null)
                statusText.text =
                    Chapter1Texts
                    .PlaceHandFirst();
            return;
        }
        
        // Tell WristDetector session started
        // so hand removal triggers retry
        // instead of just clearing
        if (activeTarget.wristDetector != null)
            activeTarget.wristDetector
                .OnSessionStarted();

        playerBeatCount = 0;
        UpdateBeatDisplay();

        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);

        StartCoroutine(RunSession());
    }

    private IEnumerator RunSession()
    {
        sessionRunning = true;

        if (startButton != null)
            startButton.SetActive(false);
        if (countPanel != null)
            countPanel.SetActive(false);
        if (recordButton != null)
            recordButton.SetActive(false);

        float timeLeft = sessionDuration;

        if (statusText != null)
            statusText.text =
                Chapter1Texts.CountBeats();

        while (timeLeft > 0)
        {
            if (timerText != null)
                timerText.text =
                    Chapter1Texts
                    .Timer(timeLeft);

            timeLeft -= Time.deltaTime;
            yield return null;
        }

        if (timerText != null)
            timerText.text =
                Chapter1Texts.TimesUp();

        if (statusText != null)
            statusText.text =
                Chapter1Texts.HowManyBeats();

        if (PulseVisualIndicator.Instance
            != null)
            PulseVisualIndicator.Instance
                .StopVisualPulse();

        if (countPanel != null)
            countPanel.SetActive(true);
        if (recordButton != null)
            recordButton.SetActive(true);

        RefreshButtonTexts();

        sessionRunning = false;
        sessionCompleted = true;

        if (PulseHapticPlayer.Instance != null)
            PulseHapticPlayer.Instance
                .StopPulse();
    }

    public void IncrementCount()
    {
        playerBeatCount++;
        UpdateBeatDisplay();
    }

    public void DecrementCount()
    {
        if (playerBeatCount > 0)
            playerBeatCount--;
        UpdateBeatDisplay();
    }

    private void UpdateBeatDisplay()
    {
        if (beatCountText != null)
            beatCountText.text =
                Chapter1Texts.BeatsCount(
                    playerBeatCount);
    }

    // ─── Record ───────────────────────────────────────

    public void RecordResult()
    {
        Debug.Log("RecordResult called");

        if (activeTarget == null)
        {
            Debug.LogError(
                "activeTarget is NULL!");
            return;
        }

        int playerBPM = playerBeatCount * 4;
        int realBPM = lockedBPM;
        int difference =
            Mathf.Abs(playerBPM - realBPM);
        bool passed =
            difference <= passTolerance;

        lastPlayerBPM = playerBPM;
        lastRealBPM = realBPM;
        lastDifference = difference;
        lastPassed = passed;

        Debug.Log("RecordResult: "
            + "playerBPM=" + playerBPM
            + " realBPM=" + realBPM
            + " passed=" + passed);

        if (passed)
            activeTarget.MarkMeasured();

        // ── Notify manager immediately ────────────
        // Do NOT wait for Next button press
        // Tracking must happen at record time
        // so data is saved even if player
        // walks away without pressing Next
        if (Chapter1Manager.Instance != null
            && activeTarget != null)
            Chapter1Manager.Instance
                .OnSubjectRecorded(
                    activeTarget,
                    lockedSubjectType,
                    lockedSubjectName,
                    playerBPM,
                    passed);

        if (countPanel != null)
            countPanel.SetActive(false);
        if (recordButton != null)
            recordButton.SetActive(false);

        ShowFeedback(
            playerBPM, realBPM,
            difference, passed);
    }

    private void ShowFeedback(
        int playerBPM, int realBPM,
        int difference, bool passed)
    {
        Debug.Log("ShowFeedback passed="
            + passed);

        if (feedbackPanel == null)
        {
            Debug.LogError(
                "feedbackPanel is NULL!");
            return;
        }

        feedbackPanel.SetActive(true);

        if (startButton != null)
            startButton.SetActive(false);

        string feedback =
            (passed
                ? Chapter1Texts.WellDone()
                : Chapter1Texts.NotQuite())
            + "\n\n"
            + Chapter1Texts
                .YourCount(playerBPM)
            + "\n"
            + Chapter1Texts
                .CorrectValue(realBPM)
            + "\n"
            + Chapter1Texts
                .Difference(difference)
            + "\n\n"
            + (passed
                ? Chapter1Texts
                    .ExperimentPassed()
                : Chapter1Texts.TryAgain(
                    passTolerance));

        if (resultFeedbackText != null)
            resultFeedbackText.text = feedback;

        if (retryButton != null)
            retryButton.SetActive(!passed);
        if (nextButton != null)
            nextButton.SetActive(passed);
    }

    // ─── Proceed To Next ──────────────────────────────
    // Wire to feedback panel Next button
    // OnClick → PulseSessionManager
    //           .ProceedToNext()

    public void ProceedToNext()
    {
        Debug.Log("ProceedToNext called "
            + "lastExpType=" + lastExpType);

        // 1. Close feedback panel
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);

        // OnSubjectRecorded already called
        // in RecordResult() — not called again

        // 2. Clear UI
        if (statusText != null)
            statusText.text =
                Chapter1Texts.MoveToNext();
        if (timerText != null)
            timerText.text = "";
        if (beatCountText != null)
            beatCountText.text = "";

        if (PulseHapticPlayer.Instance != null)
            PulseHapticPlayer.Instance
                .StopPulse();

        if (PulseVisualIndicator.Instance
            != null)
            PulseVisualIndicator.Instance
                .StopVisualPulse();

        // 3. Check if experiment group
        // is complete
        // Use lastExpType — NOT activeTarget
        // since it may already be null
        if (Chapter1Manager.Instance != null)
        {
            Chapter1Manager mgr =
                Chapter1Manager.Instance;

            Debug.Log("ProceedToNext check: "
                + "exp=" + lastExpType
                + " male=" + mgr.male1ADone
                + " female=" + mgr.female1ADone
                + " 1AFired="
                + mgr.complete1AFired
                + " student="
                + mgr.student1BDone
                + " teacher="
                + mgr.teacher1BDone
                + " assistant="
                + mgr.assistant1BDone
                + " 1BFired="
                + mgr.complete1BFired
                + " resting="
                + mgr.resting1CDone
                + " walking="
                + mgr.walking1CDone
                + " running="
                + mgr.running1CDone
                + " 1CFired="
                + mgr.complete1CFired);

            // 1A — male AND female done
            if (lastExpType == "1A"
                && mgr.male1ADone
                && mgr.female1ADone
                && !mgr.complete1AFired)
            {
                mgr.complete1AFired = true;
                activeTarget = null;
                sessionCompleted = false;
                mgr.TriggerComplete1A();
                return;
            }

            // 1B — all three done
            if (lastExpType == "1B"
                && mgr.student1BDone
                && mgr.teacher1BDone
                && mgr.assistant1BDone
                && !mgr.complete1BFired)
            {
                mgr.complete1BFired = true;
                activeTarget = null;
                sessionCompleted = false;
                mgr.TriggerComplete1B();
                return;
            }

            // 1C — all three done
            if (lastExpType == "1C"
                && mgr.resting1CDone
                && mgr.walking1CDone
                && mgr.running1CDone
                && !mgr.complete1CFired)
            {
                mgr.complete1CFired = true;
                activeTarget = null;
                sessionCompleted = false;
                mgr.TriggerComplete1C();
                return;
            }
        }

        // Group not complete yet
        activeTarget = null;
        sessionCompleted = false;

        Debug.Log("ProceedToNext — "
            + "group not complete yet, "
            + "waiting for next NPC.");
    }

    // ─── Retry ────────────────────────────────────────

    public void RetrySession()
    {
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);

        sessionCompleted = false;
        playerBeatCount = 0;
        UpdateBeatDisplay();

        if (activeTarget != null)
        {
            activeTarget.ResetForRetry();

            if (PulseHapticPlayer.Instance
                != null)
                PulseHapticPlayer.Instance
                    .StartPulse(
                        activeTarget.bpm);

            if (PulseVisualIndicator.Instance
                != null)
                PulseVisualIndicator.Instance
                    .StartVisualPulse(
                        activeTarget.bpm);
        }

        if (startButton != null)
            startButton.SetActive(true);
        if (timerText != null)
            timerText.text =
                Chapter1Texts.TimerDefault(
                    sessionDuration);
        if (statusText != null)
            statusText.text =
                Chapter1Texts.PlaceHandOnWrist();

        RefreshButtonTexts();
    }
}