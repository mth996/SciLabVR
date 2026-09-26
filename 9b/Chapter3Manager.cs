using System.Collections;
using UnityEngine;
using TMPro;

public class Chapter3Manager : BaseExperimentManager
{
    public static Chapter3Manager Instance;

    [Header("Active Experiments — tick when ready")]
    public bool use3A = true;
    public bool use3B = true;
    public bool use3C = true;
    public bool use3D = true;

    public enum SubExperiment
    {
        None, Exp3A, Exp3B, Exp3C, Exp3D,
        AllComplete
    }

    private SubExperiment currentExperiment =
        SubExperiment.None;

    [Header("Shared UI")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI fullCorrosionText;
    public GameObject notebookButton;

    [Header("Retry Panels — English")]
    public GameObject retryPanel3A_EN;
    public GameObject retryPanel3B_EN;
    public GameObject retryPanel3C_EN;
    public GameObject retryPanel3D_EN;

    [Header("Retry Panels — Malay")]
    public GameObject retryPanel3A_BM;
    public GameObject retryPanel3B_BM;
    public GameObject retryPanel3C_BM;
    public GameObject retryPanel3D_BM;

    [Header("Begin Panels — English")]
    public GameObject beginPanel3A_EN;
    public GameObject beginPanel3B_EN;
    public GameObject beginPanel3C_EN;
    public GameObject beginPanel3D_EN;

    [Header("Begin Panels — Malay")]
    public GameObject beginPanel3A_BM;
    public GameObject beginPanel3B_BM;
    public GameObject beginPanel3C_BM;
    public GameObject beginPanel3D_BM;

    [Header("Experiment Selection")]
    public GameObject experimentSelectionPanel;
    public GameObject experimentSelectionPanelBM;

    [Header("Quit Confirmation — Menu/Retry Area")]
    [Tooltip("Quit panel shown from retry/menu panels")]
    public GameObject quitConfirmPanelEN;
    public GameObject quitConfirmPanelBM;

    [Header("Quit Confirmation — Experiment Area")]
    [Tooltip("Quit panel shown from experiment UI")]
    public GameObject quitConfirmExpPanelEN;
    public GameObject quitConfirmExpPanelBM;

    [Header("3A — Panel Root")]
    public GameObject panel3A;
    public GameObject ch3A;

    [Header("3B — Panel Root")]
    public GameObject panel3B;
    public GameObject ch3B;

    [Header("3B — Test Tubes")]
    public TestTube tubePIron;
    public TestTube tubeQCopper;

    [Header("3B — Nails")]
    public NailItem ironNail;
    public NailItem copperNail;

    [Header("3B — Settings")]
    public float timerMin3B = 20f;
    public float timerMax3B = 50f;
    public float fullCorrosionTime3B = 60f;
    public int acceptedMargin3B = 10;

    [Header("3B — Input UI")]
    public GameObject inputPanel3B;
    public TextMeshProUGUI ironValueText;
    public TextMeshProUGUI copperValueText;

    [Header("3B — Conclusion")]
    public TextMeshProUGUI conclusionText3B;
    public GameObject conclusionPanel3B;
    public GameObject proceedButton3BEN;
    public GameObject proceedButton3BBM;

    [Header("3B — Language Labels")]
    public GameObject engLabels;
    public GameObject malayLabels;

    [Header("3C — Panel Root")]
    public GameObject panel3C;
    public GameObject ch3C;

    [Header("3D — Panel Root")]
    public GameObject panel3D;
    public GameObject ch3D;

    [Header("Scene Controller")]
    public ChapterOverlayUI chapterOverlayUI;

    [Header("UI Canvas")]
    [Tooltip("The main UI Canvas — hidden until " +
        "experiment starts")]
    public GameObject uiCanvas;

    private enum Phase3B
    {
        PlaceNails, TimerRunning, Inspect, Complete
    }

    private Phase3B current3BPhase =
        Phase3B.PlaceNails;
    private bool ironPlaced = false;
    private bool copperPlaced = false;
    private float randomTimerDuration3B = 30f;
    private float timerElapsed3B = 0f;
    private bool timer3BRunning = false;
    private int ironGuess = 0;
    private int copperGuess = 0;
    private int correctIronPercent = 0;
    private bool _isEnglish = true;

    [Header("3B — Labels")]
    public TextMeshProUGUI headerLabel3B;
    public TextMeshProUGUI tubePLabel;
    public TextMeshProUGUI tubeQLabel;
    public TextMeshProUGUI ironInputLabel;
    public TextMeshProUGUI copperInputLabel;
    public TextMeshProUGUI submitButton3BText;
    public TextMeshProUGUI notebookButtonText;
    public TextMeshProUGUI conclusionTitleText3B;

    private void Awake()
    {
        Instance = this;
        chapterNumber = 3;
        chapterTitle = Chapter3Texts.ChapterTitle();
    }

    protected override void Start()
    {
        base.Start();

        if (LanguageManager.Instance != null)
            _isEnglish = LanguageManager
                .Instance.IsEnglish();

        if (engLabels != null)
            engLabels.SetActive(_isEnglish);
        if (malayLabels != null)
            malayLabels.SetActive(!_isEnglish);

        SetActive(panel3A, false);
        SetActive(panel3B, false);
        SetActive(panel3C, false);
        SetActive(panel3D, false);
        SetActive(ch3A, false);
        SetActive(ch3B, false);
        SetActive(ch3C, false);
        SetActive(ch3D, false);
        SetActive(notebookButton, false);
        SetActive(inputPanel3B, false);
        SetActive(conclusionPanel3B, false);
        HideAllRetryPanels();
        SetActive(experimentSelectionPanel, false);
        SetActive(experimentSelectionPanelBM, false);
        SetActive(quitConfirmPanelEN, false);
        SetActive(quitConfirmPanelBM, false);
        SetActive(quitConfirmExpPanelEN, false);
        SetActive(quitConfirmExpPanelBM, false);
        SetActive(uiCanvas, false);

        if (statusText != null) statusText.text = "";
        if (timerText != null) timerText.text = "";
        if (fullCorrosionText != null) fullCorrosionText.text = "";
        if (tubePLabel != null) tubePLabel.text = Chapter3Texts.TubePLabel();
        if (tubeQLabel != null) tubeQLabel.text = Chapter3Texts.TubeQLabel();
        if (ironInputLabel != null) ironInputLabel.text = Chapter3Texts.IronInputLabel();
        if (copperInputLabel != null) copperInputLabel.text = Chapter3Texts.CopperInputLabel();
        if (submitButton3BText != null) submitButton3BText.text = Chapter3Texts.SubmitButton3B();
        if (notebookButtonText != null) notebookButtonText.text = Chapter3Texts.NotebookButton();
        if (conclusionTitleText3B != null) conclusionTitleText3B.text = Chapter3Texts.ConclusionTitle();
        if (headerLabel3B != null) headerLabel3B.text = Chapter3Texts.ExperimentHeader3B();
        SetActive(proceedButton3BEN, _isEnglish);
        SetActive(proceedButton3BBM, !_isEnglish);
    }

    private void Update()
    {
        if (currentExperiment == SubExperiment.Exp3B)
            Update3B();
    }

    // ─── Quit Confirmation ────────────────────────────

    public void ShowQuitConfirmation()
    {
        bool isEng = LanguageManager.Instance == null
            || LanguageManager.Instance.IsEnglish();
        SetActive(quitConfirmPanelEN, isEng);
        SetActive(quitConfirmPanelBM, !isEng);
    }

    public void ShowQuitConfirmationExp()
    {
        bool isEng = LanguageManager.Instance == null
            || LanguageManager.Instance.IsEnglish();
        SetActive(quitConfirmExpPanelEN, isEng);
        SetActive(quitConfirmExpPanelBM, !isEng);
    }

    public void HideQuitConfirmation()
    {
        SetActive(quitConfirmPanelEN, false);
        SetActive(quitConfirmPanelBM, false);
        SetActive(quitConfirmExpPanelEN, false);
        SetActive(quitConfirmExpPanelBM, false);
    }

    public void BackToMenu()
    {
        PlayerPrefs.SetInt(
            "ReturnToChapterSelect", 1);
        UnityEngine.SceneManagement
            .SceneManager.LoadScene("Main Menu");
    }

    // ─── Flow Control ─────────────────────────────────

    public void OnExperimentUnlocked()
    {
        SetActive(notebookButton, true);
        StartNextExperiment(SubExperiment.None);
    }

    private void StartNextExperiment(
        SubExperiment justCompleted)
    {
        SubExperiment next =
            GetNextExperiment(justCompleted);
        switch (next)
        {
            case SubExperiment.Exp3A:
                ShowBeginPanelFor(SubExperiment.Exp3A);
                break;
            case SubExperiment.Exp3B:
                StartExperiment3B(); break;
            case SubExperiment.Exp3C:
                Trigger3CBegin(); break;
            case SubExperiment.Exp3D:
                StartExperiment3D(); break;
            case SubExperiment.AllComplete:
                OnAllExperimentsComplete(); break;
        }
    }

    private SubExperiment GetNextExperiment(
        SubExperiment justCompleted)
    {
        SubExperiment[] order = {
            SubExperiment.Exp3A,
            SubExperiment.Exp3B,
            SubExperiment.Exp3C,
            SubExperiment.Exp3D
        };
        bool[] enabled = {
            use3A, use3B, use3C, use3D };
        bool found =
            (justCompleted == SubExperiment.None);
        for (int i = 0; i < order.Length; i++)
        {
            if (!found)
            {
                if (order[i] == justCompleted)
                    found = true;
                continue;
            }
            if (enabled[i]) return order[i];
        }
        return SubExperiment.AllComplete;
    }

    private void OnExperimentStarted()
    {
        SetActive(notebookButton, true);
        SetActive(uiCanvas, true);
        if (chapterOverlayUI != null)
        {
            chapterOverlayUI
                .TeleportPlayerToExperiment();
            chapterOverlayUI
                .SetLockedObjectsPublic(true);
        }
    }

    // ─── Retry Wrappers ───────────────────────────────

    public void RetryExperiment3A()
    {
        HideAllRetryPanels();
        StartExperiment3A();
    }

    public void RetryExperiment3B()
    {
        HideAllRetryPanels();
        StartExperiment3B();
    }

    public void RetryExperiment3C()
    {
        HideAllRetryPanels();
        StartExperiment3C();
    }

    public void RetryExperiment3D()
    {
        HideAllRetryPanels();
        StartExperiment3D();
    }

    // ─── Next Wrappers ────────────────────────────────

    public void NextAfter3A()
    {
        HideAllRetryPanels();
        StartNextExperiment(SubExperiment.Exp3A);
    }

    public void NextAfter3B()
    {
        HideAllRetryPanels();
        StartNextExperiment(SubExperiment.Exp3B);
    }

    public void NextAfter3C()
    {
        HideAllRetryPanels();
        StartNextExperiment(SubExperiment.Exp3C);
    }

    public void NextAfter3D()
    {
        HideAllRetryPanels();
        StartNextExperiment(SubExperiment.Exp3D);
    }

    public void GoToChapterStart()
    {
        HideAllRetryPanels();
        HideAllExperiments();
        HideAllBeginPanels();
        SetActive(experimentSelectionPanel, false);
        SetActive(experimentSelectionPanelBM, false);
        if (chapterOverlayUI != null)
            chapterOverlayUI.ShowBeginPanel();
    }

    // ─── Experiment Selection ─────────────────────────

    public void ShowExperimentSelection()
    {
        HideAllRetryPanels();
        HideAllExperiments();
        HideAllBeginPanels();
        if (chapterOverlayUI != null)
            chapterOverlayUI.HideAllPanels();
        if (_isEnglish)
        {
            SetActive(experimentSelectionPanel, true);
            SetActive(experimentSelectionPanelBM, false);
        }
        else
        {
            SetActive(experimentSelectionPanel, false);
            SetActive(experimentSelectionPanelBM, true);
        }
    }

    public void SelectExperiment3A()
    {
        HideSelectionPanels();
        ShowBeginPanelFor(SubExperiment.Exp3A);
    }

    public void SelectExperiment3B()
    {
        HideSelectionPanels();
        ShowBeginPanelFor(SubExperiment.Exp3B);
    }

    public void SelectExperiment3C()
    {
        HideSelectionPanels();
        ShowBeginPanelFor(SubExperiment.Exp3C);
    }

    public void SelectExperiment3D()
    {
        HideSelectionPanels();
        ShowBeginPanelFor(SubExperiment.Exp3D);
    }

    private void ShowBeginPanelFor(
        SubExperiment exp)
    {
        HideAllBeginPanels();
        if (_isEnglish)
        {
            switch (exp)
            {
                case SubExperiment.Exp3A:
                    SetActive(beginPanel3A_EN, true); break;
                case SubExperiment.Exp3B:
                    SetActive(beginPanel3B_EN, true); break;
                case SubExperiment.Exp3C:
                    SetActive(beginPanel3C_EN, true); break;
                case SubExperiment.Exp3D:
                    SetActive(beginPanel3D_EN, true); break;
            }
        }
        else
        {
            switch (exp)
            {
                case SubExperiment.Exp3A:
                    SetActive(beginPanel3A_BM, true); break;
                case SubExperiment.Exp3B:
                    SetActive(beginPanel3B_BM, true); break;
                case SubExperiment.Exp3C:
                    SetActive(beginPanel3C_BM, true); break;
                case SubExperiment.Exp3D:
                    SetActive(beginPanel3D_BM, true); break;
            }
        }
    }

    private void ShowRetryPanel(SubExperiment exp)
    {
        HideAllRetryPanels();
        if (_isEnglish)
        {
            switch (exp)
            {
                case SubExperiment.Exp3A:
                    SetActive(retryPanel3A_EN, true); break;
                case SubExperiment.Exp3B:
                    SetActive(retryPanel3B_EN, true); break;
                case SubExperiment.Exp3C:
                    SetActive(retryPanel3C_EN, true); break;
                case SubExperiment.Exp3D:
                    SetActive(retryPanel3D_EN, true); break;
            }
        }
        else
        {
            switch (exp)
            {
                case SubExperiment.Exp3A:
                    SetActive(retryPanel3A_BM, true); break;
                case SubExperiment.Exp3B:
                    SetActive(retryPanel3B_BM, true); break;
                case SubExperiment.Exp3C:
                    SetActive(retryPanel3C_BM, true); break;
                case SubExperiment.Exp3D:
                    SetActive(retryPanel3D_BM, true); break;
            }
        }
    }

    private void HideSelectionPanels()
    {
        SetActive(experimentSelectionPanel, false);
        SetActive(experimentSelectionPanelBM, false);
    }

    private void HideAllRetryPanels()
    {
        SetActive(retryPanel3A_EN, false);
        SetActive(retryPanel3B_EN, false);
        SetActive(retryPanel3C_EN, false);
        SetActive(retryPanel3D_EN, false);
        SetActive(retryPanel3A_BM, false);
        SetActive(retryPanel3B_BM, false);
        SetActive(retryPanel3C_BM, false);
        SetActive(retryPanel3D_BM, false);
    }

    private void HideAllBeginPanels()
    {
        SetActive(beginPanel3A_EN, false);
        SetActive(beginPanel3B_EN, false);
        SetActive(beginPanel3C_EN, false);
        SetActive(beginPanel3D_EN, false);
        SetActive(beginPanel3A_BM, false);
        SetActive(beginPanel3B_BM, false);
        SetActive(beginPanel3C_BM, false);
        SetActive(beginPanel3D_BM, false);
    }

    private void HideAllExperiments()
    {
        SetActive(panel3A, false);
        SetActive(panel3B, false);
        SetActive(panel3C, false);
        SetActive(panel3D, false);
        SetActive(ch3A, false);
        SetActive(ch3B, false);
        SetActive(ch3C, false);
        SetActive(ch3D, false);
        SetActive(inputPanel3B, false);
        SetActive(conclusionPanel3B, false);
    }

    // ─── Start Methods ────────────────────────────────

    public void StartExperiment3A()
    {
        currentExperiment = SubExperiment.Exp3A;
        HideAllExperiments();
        HideAllBeginPanels();
        SetActive(panel3A, true);
        SetActive(ch3A, true);
        OnExperimentStarted();
        Chapter3AExperiment exp3A =
            GetComponent<Chapter3AExperiment>();
        if (exp3A != null) exp3A.StartExperiment();
        else Debug.LogWarning(
            "Chapter3AExperiment not found!");
        Debug.Log("Chapter 3: Starting 3A");
    }

    public void StartExperiment3B()
    {
        currentExperiment = SubExperiment.Exp3B;
        HideAllExperiments();
        HideAllBeginPanels();
        SetActive(panel3B, true);
        SetActive(ch3B, true);
        OnExperimentStarted();
        Reset3BState();
        if (statusText != null)
            statusText.text =
                Chapter3Texts.PlaceNails();
        Debug.Log("Chapter 3: Starting 3B");
    }

    public void StartExperiment3C()
    {
        currentExperiment = SubExperiment.Exp3C;
        HideAllExperiments();
        HideAllBeginPanels();
        SetActive(panel3C, true);
        SetActive(ch3C, true);
        OnExperimentStarted();
        if (statusText != null)
            statusText.text = "";
        Chapter3CExperiment exp3C =
            GetComponent<Chapter3CExperiment>();
        if (exp3C != null) exp3C.StartExperiment();
        SetActive(ch3B, false);
        SetActive(panel3B, false);
        Debug.Log("Chapter 3: 3C started.");
    }

    // ─── Complete Callbacks ───────────────────────────

    private void On3BComplete()
    {
        // Just show conclusion panel —
        // player presses button to proceed
        // (panel stays open until then)
    }

    // Wire to the Proceed/Next button
    // inside ConclusionPanel
    public void OnConclude3BPressed()
    {
        SetActive(conclusionPanel3B, false);
        SetActive(panel3B, false);
        SetActive(ch3B, false);

        // Teleport back to main UI area
        if (chapterOverlayUI != null)
            chapterOverlayUI
                .TeleportPlayerToUI();

        Invoke("ShowRetryPanel3B", 0.5f);
    }

    private void ShowRetryPanel3B()
    {
        ShowRetryPanel(SubExperiment.Exp3B);
    }

    private void Trigger3CBegin()
    {
        Chapter3CExperiment exp3C =
            GetComponent<Chapter3CExperiment>();
        if (exp3C != null) exp3C.OnUnlocked();
        else Debug.LogWarning(
            "Chapter3CExperiment not found!");
    }

    public void On3AComplete()
    {
        Debug.Log($"On3AComplete called — ch3A: {(ch3A != null ? ch3A.name : "NULL")}, active before: {(ch3A != null ? ch3A.activeSelf.ToString() : "n/a")}");

        SetActive(panel3A, false);
        SetActive(ch3A, false);

        Debug.Log($"ch3A active after SetActive(false): {(ch3A != null ? ch3A.activeSelf.ToString() : "n/a")}");

        Invoke("ShowRetryPanel3A", 1f);
    }

    private void ShowRetryPanel3A()
    {
        ShowRetryPanel(SubExperiment.Exp3A);
    }

    public void On3CComplete()
    {
        SetActive(panel3C, false);
        SetActive(ch3C, false);
        Invoke("ShowRetryPanel3C", 1f);
    }

    private void ShowRetryPanel3C()
    {
        ShowRetryPanel(SubExperiment.Exp3C);
    }

    private void ShowRetryPanel3D()
    {
        ShowRetryPanel(SubExperiment.Exp3D);
    }

    private void OnAllExperimentsComplete()
    {
        ExperimentSection finalSection =
            new ExperimentSection();
        finalSection.experimentTitle =
            Chapter3Texts.ChapterTitle();
        finalSection.stars = 3;
        SendToNotebook(finalSection, true);
        if (statusText != null)
            statusText.text =
                Chapter3Texts.ExperimentComplete();
        Invoke("ShowChapterCompletePanel", 2f);
    }

    private void ShowChapterCompletePanel()
    {
        if (chapterOverlayUI != null)
            chapterOverlayUI.ShowAchievementPanel();
    }

    // ─── 3B Logic ─────────────────────────────────────

    private void Update3B()
    {
        if (!timer3BRunning) return;
        timerElapsed3B += Time.deltaTime;
        float remaining = Mathf.Max(0,
            randomTimerDuration3B - timerElapsed3B);
        if (timerText != null)
            timerText.text = FormatTime(remaining);
        float rustProgress = Mathf.Clamp01(
            timerElapsed3B / fullCorrosionTime3B);
        if (ironNail != null)
            ironNail.SetRustAmount(rustProgress);
        if (copperNail != null)
            copperNail.SetRustAmount(0f);
        if (timerElapsed3B >= randomTimerDuration3B)
        {
            timer3BRunning = false;
            OnTimer3BComplete();
        }
    }

    public void OnNailPlaced(TestTube tube,
        NailItem nail)
    {
        if (current3BPhase != Phase3B.PlaceNails)
            return;
        if (nail.nailType == NailType.Iron)
        {
            ironPlaced = true;
            if (!copperPlaced && statusText != null)
                statusText.text =
                    Chapter3Texts.IronNailPlaced();
        }
        else if (nail.nailType == NailType.Copper)
        {
            copperPlaced = true;
            if (!ironPlaced && statusText != null)
                statusText.text =
                    Chapter3Texts.CopperNailPlaced();
        }
        if (ironPlaced && copperPlaced)
            Start3BTimer();
    }

    private void Start3BTimer()
    {
        current3BPhase = Phase3B.TimerRunning;
        randomTimerDuration3B = Random.Range(
            timerMin3B, timerMax3B);
        timerElapsed3B = 0f;
        float rawPercent =
            (randomTimerDuration3B
            / fullCorrosionTime3B) * 100f;
        correctIronPercent = Mathf.Clamp(
            RoundToNearest5((int)rawPercent), 0, 100);
        if (fullCorrosionText != null)
            fullCorrosionText.text =
                Chapter3Texts.FullCorrosionTime(
                    (int)fullCorrosionTime3B);
        if (statusText != null)
            statusText.text =
                Chapter3Texts.TimerRunning();
        if (timerText != null)
            timerText.text =
                FormatTime(randomTimerDuration3B);
        timer3BRunning = true;
    }

    private void OnTimer3BComplete()
    {
        current3BPhase = Phase3B.Inspect;
        float finalRust = Mathf.Clamp01(
            randomTimerDuration3B
            / fullCorrosionTime3B);
        if (ironNail != null)
            ironNail.SetRustAmount(finalRust);
        if (ironNail != null) ironNail.AllowPickup();
        if (copperNail != null)
            copperNail.AllowPickup();
        ironGuess = 0; copperGuess = 0;
        Update3BInputUI();
        if (timerText != null)
            timerText.text = "00:00";
        if (fullCorrosionText != null)
            fullCorrosionText.text = "";
        if (statusText != null)
            statusText.text =
                Chapter3Texts.InspectNails();
        SetActive(inputPanel3B, true);
    }

    public void OnIronPlus()
    {
        if (current3BPhase != Phase3B.Inspect) return;
        ironGuess = Mathf.Clamp(ironGuess + 5, 0, 100);
        Update3BInputUI();
    }

    public void OnIronMinus()
    {
        if (current3BPhase != Phase3B.Inspect) return;
        ironGuess = Mathf.Clamp(ironGuess - 5, 0, 100);
        Update3BInputUI();
    }

    public void OnCopperPlus()
    {
        if (current3BPhase != Phase3B.Inspect) return;
        copperGuess = Mathf.Clamp(
            copperGuess + 5, 0, 100);
        Update3BInputUI();
    }

    public void OnCopperMinus()
    {
        if (current3BPhase != Phase3B.Inspect) return;
        copperGuess = Mathf.Clamp(
            copperGuess - 5, 0, 100);
        Update3BInputUI();
    }

    private void Update3BInputUI()
    {
        if (ironValueText != null)
            ironValueText.text = ironGuess + "%";
        if (copperValueText != null)
            copperValueText.text = copperGuess + "%";
    }

    public void OnSubmitPressed()
    {
        if (current3BPhase != Phase3B.Inspect) return;
        current3BPhase = Phase3B.Complete;
        SetActive(inputPanel3B, false);
        bool ironCorrect = Mathf.Abs(
            ironGuess - correctIronPercent)
            <= acceptedMargin3B;
        bool copperCorrect = copperGuess == 0;
        bool passed = ironCorrect && copperCorrect;
        ExperimentSection section =
            new ExperimentSection();
        section.experimentTitle =
            Chapter3Texts.ExperimentTitle3B();
        section.stars = passed ? 1 : 0;
        section.entries.Add(new ExperimentEntry
        {
            label = Chapter3Texts.IronNail(),
            result = Chapter3Texts.RustPresent(
                correctIronPercent),
            passed = ironCorrect
        });
        section.entries.Add(new ExperimentEntry
        {
            label = Chapter3Texts.CopperNail(),
            result = Chapter3Texts.NoRust(),
            passed = copperCorrect
        });
        SendToNotebook(section, false);
        if (statusText != null)
            statusText.text = passed
                ? Chapter3Texts.ResultCorrect(
                    ironGuess, correctIronPercent)
                : Chapter3Texts.ResultWrong(
                    ironGuess, correctIronPercent,
                    copperGuess);
        Show3BConclusion(Chapter3Texts.Conclusion(
            (int)randomTimerDuration3B,
            (int)fullCorrosionTime3B,
            correctIronPercent));
        if (passed) On3BComplete();
        else Invoke("Retry3B", 3f);
    }

    private void Show3BConclusion(string text)
    {
        SetActive(conclusionPanel3B, true);
        if (conclusionText3B != null)
            conclusionText3B.text = text;
    }

    private void Retry3B()
    {
        SetActive(conclusionPanel3B, false);
        Reset3BState();
        if (ironNail != null) ironNail.AllowPickup();
        if (copperNail != null)
            copperNail.AllowPickup();
        if (statusText != null)
            statusText.text =
                Chapter3Texts.PlaceNails();
    }

    private void Reset3BState()
    {
        current3BPhase = Phase3B.PlaceNails;
        ironPlaced = false; copperPlaced = false;
        timerElapsed3B = 0f; timer3BRunning = false;
        ironGuess = 0; copperGuess = 0;
        if (ironNail != null)
            ironNail.ResetForRetry();
        if (copperNail != null)
            copperNail.ResetForRetry();
        if (tubePIron != null)
        {
            tubePIron.hasNail = false;
            tubePIron.nailInside = null;
        }
        if (tubeQCopper != null)
        {
            tubeQCopper.hasNail = false;
            tubeQCopper.nailInside = null;
        }
        if (timerText != null) timerText.text = "";
        if (fullCorrosionText != null)
            fullCorrosionText.text = "";
        SetActive(inputPanel3B, false);
        SetActive(conclusionPanel3B, false);
        Update3BInputUI();
        SetActive(proceedButton3BEN, _isEnglish);
        SetActive(proceedButton3BBM, !_isEnglish);
    }

    public void OnNotebookButtonPressed()
    {
        ToggleNotebook();
    }

    private string FormatTime(float seconds)
    {
        int s = Mathf.CeilToInt(seconds);
        return (s / 60).ToString("00") + ":"
            + (s % 60).ToString("00");
    }

    private int RoundToNearest5(int value)
    {
        return Mathf.RoundToInt(value / 5f) * 5;
    }

    private void SetActive(
        GameObject obj, bool active)
    {
        if (obj != null) obj.SetActive(active);
    }

    // ─── 3D Public Methods ────────────────────────────────────────────────

    public void SendNotebook3D(
        ExperimentSection section)
    {
        SendToNotebook(section, false);
    }

    public void StartExperiment3D()
    {
        currentExperiment = SubExperiment.Exp3D;
        HideAllExperiments();
        HideAllBeginPanels();
        SetActive(panel3D, true);
        SetActive(ch3D, true);
        OnExperimentStarted();

        if (statusText != null)
            statusText.text = "";

        Chapter3DExperiment exp3D =
            GetComponent<Chapter3DExperiment>();
        if (exp3D != null)
            exp3D.StartExperiment();
        else
            Debug.LogWarning(
                "Chapter3DExperiment not found!");

        SetActive(ch3B, false);
        SetActive(panel3B, false);
        SetActive(ch3C, false);
        SetActive(panel3C, false);

        Debug.Log("Chapter 3: Starting 3D");
    }

    public void On3DComplete()
    {
        SetActive(panel3D, false);
        SetActive(ch3D, false);

        if (chapterOverlayUI != null)
            chapterOverlayUI.TeleportPlayerToUI();

        Invoke("TriggerAllComplete", 0.5f);
    }

    private void TriggerAllComplete()
    {
        OnAllExperimentsComplete();
    }
}