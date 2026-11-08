using UnityEngine;
using TMPro;

public class Chapter3CExperiment : MonoBehaviour
{
    public static Chapter3CExperiment Instance;

    [Header("3C Begin Panels")]
    public GameObject begin3CPanelEnglish;
    public GameObject begin3CPanelMalay;

    [Header("3C Intro Panels")]
    public GameObject intro3CPanelEnglish;
    public GameObject intro3CPanelMalay;

    [Header("Boiling Tubes")]
    public BoilingTube3C tubeA_Natural;
    public BoilingTube3C tubeB_Vulcanised;

    [Header("Rubber Strips")]
    public RubberStrip naturalStrip;
    public RubberStrip vulcanisedStrip;

    [Header("Bunsen Burners")]
    public BunsenBurnerController naturalBurner;
    public BunsenBurnerController vulcanisedBurner;

    [Header("Timer Settings")]
    public float timerMin3C = 20f;
    public float timerMax3C = 50f;

    [Header("Deformation Settings")]
    public float naturalMaxShrinkPercent = 0.30f;
    public float vulcanisedMaxShrinkPercent = 0.06f;
    public float fullHeatTime = 60f;

    [Header("Answer Settings")]
    public float acceptedMarginCm = 0.2f;

    [Header("Chapter 3C aparatus")]
    public GameObject Ch3C;

    [Header("UI — Shared")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI timerText;

    [Header("UI — Ruler")]
    public RulerUI naturalRulerUI;
    public RulerUI vulcanisedRulerUI;

    [Header("UI — Input Panel 3C")]
    public GameObject inputPanel3C;
    public TextMeshProUGUI naturalValueText;
    public TextMeshProUGUI vulcanisedValueText;

    [Header("UI — Conclusion")]
    public GameObject conclusionPanel3C;
    public TextMeshProUGUI conclusionText3C;
    public GameObject proceedButton3CEN;
    public GameObject proceedButton3CBM;

    [Header("3C — Labels")]
    public TextMeshProUGUI naturalRulerLabel;
    public TextMeshProUGUI vulcanisedRulerLabel;
    public TextMeshProUGUI inputPanelTitle3C;
    public TextMeshProUGUI naturalInputLabel;
    public TextMeshProUGUI vulcanisedInputLabel;
    public TextMeshProUGUI submitButton3CText;
    public TextMeshProUGUI conclusionTitleText3C;

    [Header("3C — Tube & Burner Labels")]
    [Tooltip("Parent with Tube A + B EN labels")]
    public GameObject engTubeLabels;
    [Tooltip("Parent with Tube A + B BM labels")]
    public GameObject malayTubeLabels;
    [Tooltip("Parent with ON/OFF EN labels")]
    public GameObject engBurnerLabels;
    [Tooltip("Parent with ON/OFF BM labels")]
    public GameObject malayBurnerLabels;

    private enum Phase3C
    {
        Locked,
        PlaceStrips,
        TimerRunning,
        Inspect,
        Complete
    }

    private Phase3C currentPhase = Phase3C.Locked;

    private bool naturalPlaced = false;
    private bool vulcanisedPlaced = false;

    private float timerDuration = 30f;
    private float timerElapsed = 0f;
    private bool timerRunning = false;

    private float actualNaturalShrink = 0f;
    private float actualVulcanisedShrink = 0f;

    private float actualNaturalLengthCm = 5f;
    private float actualVulcanisedLengthCm = 5f;

    private float naturalGuess = 5.0f;
    private float vulcanisedGuess = 5.0f;

    private bool _isEnglish = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (LanguageManager.Instance != null)
            _isEnglish = LanguageManager
                .Instance.IsEnglish();

        HideAll3CPanels();
        SetActive(inputPanel3C, false);
        SetActive(conclusionPanel3C, false);

        TurnOffBurners();

        // Ruler labels
        if (naturalRulerLabel != null)
            naturalRulerLabel.text =
                Chapter3Texts.NaturalRulerLabel();
        if (vulcanisedRulerLabel != null)
            vulcanisedRulerLabel.text =
                Chapter3Texts.VulcanisedRulerLabel();

        // Input panel labels
        if (inputPanelTitle3C != null)
            inputPanelTitle3C.text =
                Chapter3Texts.InputPanelTitle3C();
        if (naturalInputLabel != null)
            naturalInputLabel.text =
                Chapter3Texts.NaturalRubber();
        if (vulcanisedInputLabel != null)
            vulcanisedInputLabel.text =
                Chapter3Texts.VulcanisedRubber();
        if (submitButton3CText != null)
            submitButton3CText.text =
                Chapter3Texts.SubmitButton3C();
        if (conclusionTitleText3C != null)
            conclusionTitleText3C.text =
                Chapter3Texts.ConclusionTitle();
    }

    private void Update()
    {
        if (!timerRunning) return;

        timerElapsed += Time.deltaTime;

        float remaining = Mathf.Max(
            0, timerDuration - timerElapsed);

        if (timerText != null)
            timerText.text = FormatTime(remaining);

        float progress = Mathf.Clamp01(
            timerElapsed / fullHeatTime);

        if (naturalStrip != null)
            naturalStrip.DeformByPercent(
                actualNaturalShrink, progress);
        if (vulcanisedStrip != null)
            vulcanisedStrip.DeformByPercent(
                actualVulcanisedShrink, progress);

        if (timerElapsed >= timerDuration)
        {
            timerRunning = false;
            OnTimerComplete();
        }
    }

    public void OnUnlocked()
    {
        currentPhase = Phase3C.Locked;
        HideAll3CPanels();

        if (_isEnglish)
            SetActive(begin3CPanelEnglish, true);
        else
            SetActive(begin3CPanelMalay, true);

        Debug.Log("3C Begin panel shown.");
    }

    public void On3CBeginPressed()
    {
        HideAll3CPanels();

        if (_isEnglish)
            SetActive(intro3CPanelEnglish, true);
        else
            SetActive(intro3CPanelMalay, true);

        Debug.Log("3C Intro panel shown.");
    }

    public void On3CIntroNextPressed()
    {
        HideAll3CPanels();
        Chapter3Manager.Instance
            .StartExperiment3C();
        Debug.Log(
            "3C intro done. Manager activating 3C.");
    }

    public void StartExperiment()
    {
        ResetState();
        RefreshLabels();

        if (naturalStrip != null)
            naturalStrip.ResetStrip();
        if (vulcanisedStrip != null)
            vulcanisedStrip.ResetStrip();
        if (tubeA_Natural != null)
            tubeA_Natural.Reset();
        if (tubeB_Vulcanised != null)
            tubeB_Vulcanised.Reset();
        if (naturalRulerUI != null)
            naturalRulerUI.SetStripLength(5f);
        if (vulcanisedRulerUI != null)
            vulcanisedRulerUI.SetStripLength(5f);

        currentPhase = Phase3C.PlaceStrips;

        if (statusText != null)
            statusText.text =
                Chapter3Texts.PlaceStrips3C();

        Debug.Log(
            "3C experiment ready — place strips.");
    }

    public void OnStripPlaced(
        BoilingTube3C tube, RubberStrip strip)
    {
        if (currentPhase != Phase3C.PlaceStrips)
            return;

        if (strip.rubberType == RubberType.Natural)
        {
            naturalPlaced = true;
            if (!vulcanisedPlaced
                && statusText != null)
                statusText.text =
                    Chapter3Texts
                    .NaturalStripPlaced3C();
        }
        else if (strip.rubberType ==
            RubberType.Vulcanised)
        {
            vulcanisedPlaced = true;
            if (!naturalPlaced
                && statusText != null)
                statusText.text =
                    Chapter3Texts
                    .VulcanisedStripPlaced3C();
        }

        CheckStartTimerConditions();
    }

    public void OnWrongStrip(string tubeLabel)
    {
        if (statusText != null)
            statusText.text =
                Chapter3Texts.WrongStrip3C(
                    tubeLabel);
    }

    public void OnBurnerStateChanged()
    {
        CheckStartTimerConditions();
    }

    private void CheckStartTimerConditions()
    {
        if (currentPhase != Phase3C.PlaceStrips)
            return;

        bool naturalBurnerOn =
            naturalBurner != null
            && naturalBurner.IsOn();
        bool vulcanisedBurnerOn =
            vulcanisedBurner != null
            && vulcanisedBurner.IsOn();

        if (!naturalPlaced || !vulcanisedPlaced)
            return;

        if (!naturalBurnerOn || !vulcanisedBurnerOn)
        {
            if (statusText != null)
                statusText.text =
                    Chapter3Texts
                    .BothStripsPlaced3C();
            return;
        }

        StartTimer();
    }

    private void StartTimer()
    {
        currentPhase = Phase3C.TimerRunning;

        timerDuration = Random.Range(
            timerMin3C, timerMax3C);
        timerElapsed = 0f;

        float heatRatio = Mathf.Clamp01(
            timerDuration / fullHeatTime);

        actualNaturalShrink =
            naturalMaxShrinkPercent * heatRatio;
        actualVulcanisedShrink =
            vulcanisedMaxShrinkPercent * heatRatio;

        actualNaturalLengthCm =
            Mathf.Round(
                naturalStrip.originalLengthCm *
                (1f - actualNaturalShrink) * 10f)
            / 10f;
        actualVulcanisedLengthCm =
            Mathf.Round(
                vulcanisedStrip.originalLengthCm *
                (1f - actualVulcanisedShrink) * 10f)
            / 10f;

        if (statusText != null)
            statusText.text =
                Chapter3Texts.HeatingInProgress();
        if (timerText != null)
            timerText.text =
                FormatTime(timerDuration);

        timerRunning = true;
    }

    private void OnTimerComplete()
    {
        TurnOffBurners();
        currentPhase = Phase3C.Inspect;

        if (naturalStrip != null)
            naturalStrip.SetFinalDeformation(
                actualNaturalShrink);
        if (vulcanisedStrip != null)
            vulcanisedStrip.SetFinalDeformation(
                actualVulcanisedShrink);
        if (naturalStrip != null)
            naturalStrip.SnapBackToStart();
        if (vulcanisedStrip != null)
            vulcanisedStrip.SnapBackToStart();
        if (naturalRulerUI != null)
            naturalRulerUI.SetStripLength(
                actualNaturalLengthCm);
        if (vulcanisedRulerUI != null)
            vulcanisedRulerUI.SetStripLength(
                actualVulcanisedLengthCm);

        naturalGuess = 5.0f;
        vulcanisedGuess = 5.0f;
        UpdateInputUI();

        if (timerText != null)
            timerText.text = "00:00";
        if (statusText != null)
            statusText.text =
                Chapter3Texts.InspectStrips3C();

        SetActive(inputPanel3C, true);
    }

    public void OnNaturalPlus()
    {
        if (currentPhase != Phase3C.Inspect) return;
        naturalGuess = Mathf.Clamp(
            Mathf.Round(
                (naturalGuess + 0.1f) * 10f) / 10f,
            0f, 5f);
        UpdateInputUI();
    }

    public void OnNaturalMinus()
    {
        if (currentPhase != Phase3C.Inspect) return;
        naturalGuess = Mathf.Clamp(
            Mathf.Round(
                (naturalGuess - 0.1f) * 10f) / 10f,
            0f, 5f);
        UpdateInputUI();
    }

    public void OnVulcanisedPlus()
    {
        if (currentPhase != Phase3C.Inspect) return;
        vulcanisedGuess = Mathf.Clamp(
            Mathf.Round(
                (vulcanisedGuess + 0.1f) * 10f)
            / 10f, 0f, 5f);
        UpdateInputUI();
    }

    public void OnVulcanisedMinus()
    {
        if (currentPhase != Phase3C.Inspect) return;
        vulcanisedGuess = Mathf.Clamp(
            Mathf.Round(
                (vulcanisedGuess - 0.1f) * 10f)
            / 10f, 0f, 5f);
        UpdateInputUI();
    }

    private void UpdateInputUI()
    {
        if (naturalValueText != null)
            naturalValueText.text =
                naturalGuess.ToString("F1") + " cm";
        if (vulcanisedValueText != null)
            vulcanisedValueText.text =
                vulcanisedGuess.ToString("F1")
                + " cm";
    }

    public void OnSubmit3CPressed()
    {
        if (currentPhase != Phase3C.Inspect) return;

        currentPhase = Phase3C.Complete;
        SetActive(inputPanel3C, false);

        bool naturalCorrect =
            Mathf.Abs(naturalGuess
                - actualNaturalLengthCm)
            <= acceptedMarginCm;
        bool vulcanisedCorrect =
            Mathf.Abs(vulcanisedGuess
                - actualVulcanisedLengthCm)
            <= acceptedMarginCm;
        bool passed =
            naturalCorrect && vulcanisedCorrect;

        ExperimentSection section =
            new ExperimentSection();
        section.experimentTitle =
            Chapter3Texts.ExperimentTitle3C();
        section.stars = passed ? 1 : 0;
        section.entries.Add(new ExperimentEntry
        {
            label = Chapter3Texts.NaturalRubber(),
            result = actualNaturalLengthCm
                .ToString("F1") + " cm",
            passed = naturalCorrect
        });
        section.entries.Add(new ExperimentEntry
        {
            label =
                Chapter3Texts.VulcanisedRubber(),
            result = actualVulcanisedLengthCm
                .ToString("F1") + " cm",
            passed = vulcanisedCorrect
        });

        Chapter3Manager.Instance
            .SendToNotebook(section, false);

        if (statusText != null)
            statusText.text = passed
                ? Chapter3Texts.ResultCorrect3C(
                    naturalGuess,
                    actualNaturalLengthCm,
                    vulcanisedGuess,
                    actualVulcanisedLengthCm)
                : Chapter3Texts.ResultWrong3C(
                    naturalGuess,
                    actualNaturalLengthCm,
                    vulcanisedGuess,
                    actualVulcanisedLengthCm);

        ShowConclusion(
            Chapter3Texts.Conclusion3C(
                actualNaturalLengthCm,
                actualVulcanisedLengthCm));

        if (!passed)
            Invoke("Retry3C", 3f);

        // If passed — conclusion panel stays open
        // until player presses confirm button
    }

    // Wire to Confirm/Proceed button
    // inside conclusionPanel3C
    public void OnConclude3CPressed()
    {
        SetActive(conclusionPanel3C, false);
        TurnOffBurners();

        // Teleport back to main UI area
        if (Chapter3Manager.Instance != null
            && Chapter3Manager.Instance
                .chapterOverlayUI != null)
            Chapter3Manager.Instance
                .chapterOverlayUI
                .TeleportPlayerToUI();

        Chapter3Manager.Instance.On3CComplete();
    }

    private void Retry3C()
    {
        SetActive(conclusionPanel3C, false);
        TurnOffBurners();
        StartExperiment();
    }

    private void ShowConclusion(string text)
    {
        SetActive(conclusionPanel3C, true);
        if (conclusionText3C != null)
            conclusionText3C.text = text;
    }

    private void TurnOffBurners()
    {
        if (naturalBurner != null)
            naturalBurner.ForceFlameOff();
        if (vulcanisedBurner != null)
            vulcanisedBurner.ForceFlameOff();
    }

    private void HideAll3CPanels()
    {
        SetActive(begin3CPanelEnglish, false);
        SetActive(begin3CPanelMalay, false);
        SetActive(intro3CPanelEnglish, false);
        SetActive(intro3CPanelMalay, false);
    }

    private void RefreshLabels()
    {
        _isEnglish = LanguageManager.Instance != null
            && LanguageManager.Instance.IsEnglish();

        if (engTubeLabels != null)
            engTubeLabels.SetActive(_isEnglish);
        if (malayTubeLabels != null)
            malayTubeLabels.SetActive(!_isEnglish);
        if (engBurnerLabels != null)
            engBurnerLabels.SetActive(_isEnglish);
        if (malayBurnerLabels != null)
            malayBurnerLabels.SetActive(!_isEnglish);
        if (proceedButton3CEN != null)
            proceedButton3CEN.SetActive(_isEnglish);
        if (proceedButton3CBM != null)
            proceedButton3CBM.SetActive(!_isEnglish);
    }

    private void ResetState()
    {
        currentPhase = Phase3C.PlaceStrips;
        naturalPlaced = false;
        vulcanisedPlaced = false;
        timerElapsed = 0f;
        timerRunning = false;
        naturalGuess = 5.0f;
        vulcanisedGuess = 5.0f;
        actualNaturalShrink = 0f;
        actualVulcanisedShrink = 0f;
        actualNaturalLengthCm = 5f;
        actualVulcanisedLengthCm = 5f;
        TurnOffBurners();
        SetActive(inputPanel3C, false);
        SetActive(conclusionPanel3C, false);
        if (proceedButton3CEN != null)
            proceedButton3CEN.SetActive(false);
        if (proceedButton3CBM != null)
            proceedButton3CBM.SetActive(false);
        UpdateInputUI();
        if (timerText != null)
            timerText.text = "";
    }

    private string FormatTime(float seconds)
    {
        int s = Mathf.CeilToInt(seconds);
        return (s / 60).ToString("00") + ":"
            + (s % 60).ToString("00");
    }

    private void SetActive(
        GameObject obj, bool active)
    {
        if (obj != null) obj.SetActive(active);
    }
}