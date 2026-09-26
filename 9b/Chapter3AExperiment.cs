using UnityEngine;
using TMPro;

public class Chapter3AExperiment : MonoBehaviour
{
    // ─── Release Button ────────────────────────────────

    [Header("Release Button")]
    public GameObject releaseButton;
    public TextMeshProUGUI releaseButtonText;

    // ─── UI — Status ──────────────────────────────────

    [Header("UI — Status")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI headerLabel;
    public GameObject notebookButton;

    // ─── UI — Input Panel ─────────────────────────────

    [Header("UI — Input Panel")]
    public GameObject inputPanel3A;
    public TextMeshProUGUI inputTitle;
    public TextMeshProUGUI copperDentLabel;
    public TextMeshProUGUI bronzeDentLabel;
    public TextMeshProUGUI copperDentValueText;
    public TextMeshProUGUI bronzeDentValueText;
    public TextMeshProUGUI submitButtonText;

    // ─── UI — Conclusion ──────────────────────────────

    [Header("UI — Conclusion")]
    public GameObject conclusionPanel;
    public TextMeshProUGUI conclusionTitle;
    public TextMeshProUGUI conclusionText;
    public GameObject proceedButtonEN;
    public GameObject proceedButtonBM;

    // ─── Scene Controller ──────────────────────────────

    [Header("Scene Controller")]
    public ChapterOverlayUI chapterOverlayUI;

    // ─── Settings ─────────────────────────────────────

    [Header("Settings")]
    public float correctDentCopperMm = 4.5f;
    public float correctDentBronzeMm = 2.0f;
    public float acceptedMarginMm = 0.5f;

    // ─── Scales ─────────────────────────────────────

    [Header("Dent Scales")]
    public DentScale3A copperScale;
    public DentScale3A bronzeScale;

    // ─── State ────────────────────────────────────────

    private enum Phase
    {
        Locked, ReadyToRelease, Falling,
        Measuring, Complete
    }

    private Phase currentPhase = Phase.Locked;

    private float copperGuess = 5f;
    private float bronzeGuess = 5f;

    private bool _isEnglish = true;

    private void Start()
    {
        if (LanguageManager.Instance != null)
            _isEnglish = LanguageManager
                .Instance.IsEnglish();

        SetActive(releaseButton, false);
        SetActive(inputPanel3A, false);
        SetActive(conclusionPanel, false);

        if (headerLabel != null)
            headerLabel.text =
                Chapter3Texts.ExperimentHeader3A();
        if (inputTitle != null)
            inputTitle.text =
                Chapter3Texts.InputTitleBoth3A();
        if (copperDentLabel != null)
            copperDentLabel.text =
                Chapter3Texts.Copper3A();
        if (bronzeDentLabel != null)
            bronzeDentLabel.text =
                Chapter3Texts.Bronze3A();
        if (submitButtonText != null)
            submitButtonText.text =
                Chapter3Texts.SubmitButton3A();
        if (releaseButtonText != null)
            releaseButtonText.text =
                Chapter3Texts.ReleaseButton3A();
    }

    private void Update()
    {
        if (currentPhase != Phase.Falling) return;
        if (HardnessExperimentController.Instance
            == null) return;

        if (HardnessExperimentController.Instance
            .IsExperimentComplete())
        {
            ShowMeasurementPanel();
        }
    }

    // ─── Start ────────────────────────────────────────

    // Called by Chapter3Manager.StartExperiment3A()
    public void StartExperiment()
    {
        _isEnglish = LanguageManager.Instance != null
            && LanguageManager.Instance.IsEnglish();

        currentPhase = Phase.ReadyToRelease;

        SetActive(inputPanel3A, false);
        SetActive(conclusionPanel, false);
        SetActive(releaseButton, true);

        SetActive(proceedButtonEN, _isEnglish);
        SetActive(proceedButtonBM, !_isEnglish);

        if (HardnessExperimentController.Instance
            != null)
            HardnessExperimentController.Instance
                .ResetCompleteExperiment();

        // Reset both dent-scale pointers back to
        // zero so they don't still show last
        // attempt's depth while the new ball falls.
        if (copperScale != null)
            copperScale.SetDepth(0f);
        if (bronzeScale != null)
            bronzeScale.SetDepth(0f);

        if (statusText != null)
            statusText.text =
                Chapter3Texts.ReadyToReleaseBoth3A();

        Debug.Log("Chapter3AExperiment: Started.");
    }

    // ─── Release ──────────────────────────────────────

    public void OnReleasePressed()
    {
        if (currentPhase != Phase.ReadyToRelease)
            return;

        if (HardnessExperimentController.Instance
            != null)
            HardnessExperimentController.Instance
                .ReleaseBothWeights();

        SetActive(releaseButton, false);
        currentPhase = Phase.Falling;

        if (statusText != null)
            statusText.text =
                Chapter3Texts.BallFalling3A();
    }

    // ─── Measurement ──────────────────────────────────

    private void ShowMeasurementPanel()
    {
        currentPhase = Phase.Measuring;

        copperGuess = 5f;
        bronzeGuess = 5f;
        UpdateInputUI();

        if (copperScale != null)
            copperScale.SetDepth(correctDentCopperMm);
        if (bronzeScale != null)
            bronzeScale.SetDepth(correctDentBronzeMm);

        if (statusText != null)
            statusText.text =
                Chapter3Texts.MeasureDentBoth3A();

        SetActive(inputPanel3A, true);
    }

    // ─── Copper Steppers ──────────────────────────────

    public void OnCopperDentPlus()
    {
        if (currentPhase != Phase.Measuring) return;
        copperGuess = Mathf.Clamp(
            Mathf.Round((copperGuess + 0.1f) * 10f)
            / 10f, 0f, 20f);
        UpdateInputUI();
    }

    public void OnCopperDentMinus()
    {
        if (currentPhase != Phase.Measuring) return;
        copperGuess = Mathf.Clamp(
            Mathf.Round((copperGuess - 0.1f) * 10f)
            / 10f, 0f, 20f);
        UpdateInputUI();
    }

    // ─── Bronze Steppers ──────────────────────────────

    public void OnBronzeDentPlus()
    {
        if (currentPhase != Phase.Measuring) return;
        bronzeGuess = Mathf.Clamp(
            Mathf.Round((bronzeGuess + 0.1f) * 10f)
            / 10f, 0f, 20f);
        UpdateInputUI();
    }

    public void OnBronzeDentMinus()
    {
        if (currentPhase != Phase.Measuring) return;
        bronzeGuess = Mathf.Clamp(
            Mathf.Round((bronzeGuess - 0.1f) * 10f)
            / 10f, 0f, 20f);
        UpdateInputUI();
    }

    private void UpdateInputUI()
    {
        if (copperDentValueText != null)
            copperDentValueText.text =
                copperGuess.ToString("F1") + " mm";
        if (bronzeDentValueText != null)
            bronzeDentValueText.text =
                bronzeGuess.ToString("F1") + " mm";
    }

    // ─── Submit ───────────────────────────────────────

    public void OnSubmitPressed()
    {
        if (currentPhase != Phase.Measuring) return;

        SetActive(inputPanel3A, false);

        SendResultsToNotebook();
        ShowConclusion();
    }

    // ─── Notebook ─────────────────────────────────────

    private void SendResultsToNotebook()
    {
        bool copperCorrect = Mathf.Abs(
            copperGuess - correctDentCopperMm)
            <= acceptedMarginMm;
        bool bronzeCorrect = Mathf.Abs(
            bronzeGuess - correctDentBronzeMm)
            <= acceptedMarginMm;
        bool bronzeHarder =
            bronzeGuess < copperGuess;

        ExperimentSection section =
            new ExperimentSection();
        section.experimentTitle =
            Chapter3Texts.ExperimentTitle3A();
        section.stars =
            (copperCorrect && bronzeCorrect) ? 1 : 0;

        section.entries.Add(new ExperimentEntry
        {
            label = Chapter3Texts.Copper3A(),
            result = copperGuess
                .ToString("F1") + " mm",
            passed = copperCorrect
        });

        section.entries.Add(new ExperimentEntry
        {
            label = Chapter3Texts.Bronze3A(),
            result = bronzeGuess
                .ToString("F1") + " mm",
            passed = bronzeCorrect
        });

        section.entries.Add(new ExperimentEntry
        {
            label = bronzeHarder
                ? Chapter3Texts.ComparisonCloser3A()
                : Chapter3Texts.ComparisonFar3A(),
            result = "",
            passed = bronzeHarder
        });

        if (Chapter3Manager.Instance != null)
            Chapter3Manager.Instance
                .SendToNotebook(section, false);
    }

    // ─── Conclusion ───────────────────────────────────

    private void ShowConclusion()
    {
        currentPhase = Phase.Complete;

        SetActive(conclusionPanel, true);

        if (conclusionTitle != null)
            conclusionTitle.text =
                Chapter3Texts.ConclusionTitle3A();
        if (conclusionText != null)
            conclusionText.text =
                Chapter3Texts.Conclusion3A(
                    copperGuess, bronzeGuess);

        if (statusText != null)
            statusText.text = "";
    }

    public void OnConclude3APressed()
    {
        SetActive(conclusionPanel, false);

        if (chapterOverlayUI != null)
            chapterOverlayUI.TeleportPlayerToUI();

        if (Chapter3Manager.Instance != null)
            Chapter3Manager.Instance.On3AComplete();
    }

    private void SetActive(
        GameObject obj, bool active)
    {
        if (obj != null) obj.SetActive(active);
    }
}