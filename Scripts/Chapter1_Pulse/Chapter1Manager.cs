using UnityEngine;
using TMPro;

public class Chapter1Manager : BaseExperimentManager
{
    public static Chapter1Manager Instance;

    [Header("Timer Pulse")]
    public GameObject TimerPulsePanel;

    [Header("NPC Groups")]
    public GameObject station1ANPCs;
    public GameObject station1BNPCs;
    public GameObject station1CNPCs;

    [Header("Pulse Targets — 1A")]
    public PulseTarget[] pulseTargets1A;

    [Header("Pulse Targets — 1B")]
    public PulseTarget[] pulseTargets1B;

    [Header("Pulse Targets — 1C")]
    public PulseTarget[] pulseTargets1C;

    [Header("Activity Selector (1C)")]
    public ActivitySelector activitySelector;

    [Header("Conclusion Panel")]
    public TextMeshProUGUI conclusionTitle;
    public TextMeshProUGUI conclusionText;
    public GameObject conclusionPanel;
    public GameObject conclusionNextButton;
    public TextMeshProUGUI conclusionNextButtonText;
    
   
    [Header("Begin Panels 1A")]
    public GameObject beginPanel1A_EN;
    public GameObject beginPanel1A_BM;

    [Header("Begin Panels 1B")]
    public GameObject beginPanel1B_EN;
    public GameObject beginPanel1B_BM;

    [Header("Intro Panels 1B")]
    public GameObject introPanel1B_EN;
    public GameObject introPanel1B_BM;

    [Header("Begin Panels 1C")]
    public GameObject beginPanel1C_EN;
    public GameObject beginPanel1C_BM;

    [Header("Intro Panels 1C")]
    public GameObject introPanel1C_EN;
    public GameObject introPanel1C_BM;

    [Header("Complete Panels")]
    public GameObject completePanelEN;
    public GameObject completePanelBM;

    [Header("Retry Panels — English")]
    public GameObject retryPanel1A_EN;
    public GameObject retryPanel1B_EN;
    public GameObject retryPanel1C_EN;

    [Header("Retry Panels — Malay")]
    public GameObject retryPanel1A_BM;
    public GameObject retryPanel1B_BM;
    public GameObject retryPanel1C_BM;

    [Header("Experiment Selection")]
    public GameObject experimentSelectionPanel;
    public GameObject experimentSelectionPanelBM;

    [Header("Quit Confirmation — Menu/Retry Area")]
    [Tooltip("Quit panel shown when pressing quit " +
        "from retry/menu panels")]
    public GameObject quitConfirmPanelEN;
    public GameObject quitConfirmPanelBM;

    [Header("Quit Confirmation — Experiment Area")]
    [Tooltip("Quit panel shown when pressing quit " +
        "from the experiment UI (X button)")]
    public GameObject quitConfirmExpPanelEN;
    public GameObject quitConfirmExpPanelBM;

    [Header("Scene Controller")]
    public ChapterOverlayUI chapterOverlayUI;

    [Header("UI")]
    public TextMeshProUGUI mainStatusText;
    public GameObject notebookButton;

    [Header("Notebook Button Text")]
    public TextMeshProUGUI notebookButtonText;
    public TextMeshProUGUI[] nextButtonText;

    private enum PendingAction
    {
        None,
        ShowBeginPanel1B,
        ShowBeginPanel1C,
        ShowFinalComplete
    }

    private PendingAction pendingAction =
        PendingAction.None;

    private enum CurrentExperiment
    {
        None,
        Experiment1A,
        Experiment1B,
        Experiment1C
    }

    private CurrentExperiment currentExperiment =
        CurrentExperiment.None;

    [HideInInspector] public bool male1ADone = false;
    [HideInInspector] public bool female1ADone = false;
    [HideInInspector] public bool student1BDone = false;
    [HideInInspector] public bool teacher1BDone = false;
    [HideInInspector] public bool assistant1BDone = false;
    [HideInInspector] public bool resting1CDone = false;
    [HideInInspector] public bool walking1CDone = false;
    [HideInInspector] public bool running1CDone = false;

    private bool star1Earned = false;
    private bool star2Earned = false;
    private bool star3Earned = false;

    [HideInInspector] public bool complete1AFired = false;
    [HideInInspector] public bool complete1BFired = false;
    [HideInInspector] public bool complete1CFired = false;

    private bool _isEnglish = true;

    private void Awake()
    {
        Instance = this;
        chapterNumber = 1;
        chapterTitle = Chapter1Texts.ChapterTitle();
        SetActive(TimerPulsePanel, false);
    }

    protected override void Start()
    {
        base.Start();

        if (LanguageManager.Instance != null)
            _isEnglish = LanguageManager
                .Instance.IsEnglish();

        SetActive(conclusionPanel, false);
        SetActive(notebookButton, false);
        SetActive(conclusionNextButton, false);
        HideAllRetryPanels();
        SetActive(experimentSelectionPanel, false);
        SetActive(experimentSelectionPanelBM, false);
        SetActive(station1ANPCs, false);
        SetActive(station1BNPCs, false);
        SetActive(station1CNPCs, false);
        Hide(beginPanel1B_EN);
        Hide(beginPanel1B_BM);
        Hide(introPanel1B_EN);
        Hide(introPanel1B_BM);
        Hide(beginPanel1C_EN);
        Hide(beginPanel1C_BM);
        Hide(introPanel1C_EN);
        Hide(introPanel1C_BM);
        Hide(completePanelEN);
        Hide(completePanelBM);

        SetActive(quitConfirmPanelEN, false);
        SetActive(quitConfirmPanelBM, false);
        SetActive(quitConfirmExpPanelEN, false);
        SetActive(quitConfirmExpPanelBM, false);
    }

    // ─── Quit Confirmation ────────────────────────────

    // Wire to quit button on retry/menu panels
    public void ShowQuitConfirmation()
    {
        bool isEng = LanguageManager.Instance == null
            || LanguageManager.Instance.IsEnglish();
        SetActive(quitConfirmPanelEN, isEng);
        SetActive(quitConfirmPanelBM, !isEng);
    }

    // Wire to quit button on experiment UI (X button)
    public void ShowQuitConfirmationExp()
    {
        bool isEng = LanguageManager.Instance == null
            || LanguageManager.Instance.IsEnglish();
        SetActive(quitConfirmExpPanelEN, isEng);
        SetActive(quitConfirmExpPanelBM, !isEng);
    }

    // Wire to BOTH No buttons
    public void HideQuitConfirmation()
    {
        SetActive(quitConfirmPanelEN, false);
        SetActive(quitConfirmPanelBM, false);
        SetActive(quitConfirmExpPanelEN, false);
        SetActive(quitConfirmExpPanelBM, false);
    }

    // Wire to BOTH Yes buttons
    public void BackToMenu()
    {
        PlayerPrefs.SetInt(
            "ReturnToChapterSelect", 1);
        UnityEngine.SceneManagement
            .SceneManager.LoadScene("Main Menu");
    }

    // ─── Timer Panel ──────────────────────────────────

    private void MoveTimerPanel(Transform target)
    {
        if (TimerPulsePanel == null)
        {
            Debug.LogWarning("TimerPulsePanel is missing.");
            return;
        }

        if (target == null)
        {
            Debug.LogWarning("Target UI spawn point is missing.");
            return;
        }

        // Move timer panel to NPC UI point
        TimerPulsePanel.transform.position =
            target.position;

        TimerPulsePanel.transform.rotation =
            target.rotation;

        Debug.Log("Timer panel moved to: " + target.name);

        // If notebook is already open,
        // refresh its world position too
        if (NotebookSystem.Instance != null &&
            NotebookSystem.Instance.IsNotebookOpen())
        {
            NotebookSystem.Instance.RefreshNotebookPosition();
        }
    }

    public void MoveTimerToTarget(
        PulseTarget target)
    {
        if (target == null)
        {
            Debug.LogWarning("PulseTarget is null.");
            return;
        }
        if (target.uiSpawnPoint == null)
        {
            Debug.LogWarning(
                "UI spawn point missing on: "
                + target.name);
            return;
        }
        MoveTimerPanel(target.uiSpawnPoint);
    }

    // ─── Reset Pulse Targets ──────────────────────────

    private void ResetPulseTargets1A()
    {
        if (pulseTargets1A == null) return;
        foreach (var target in pulseTargets1A)
            if (target != null)
                target.ResetForRetry();
    }

    private void ResetPulseTargets1B()
    {
        if (pulseTargets1B == null) return;
        foreach (var target in pulseTargets1B)
            if (target != null)
                target.ResetForRetry();
    }

    private void ResetPulseTargets1C()
    {
        if (pulseTargets1C == null) return;
        foreach (var target in pulseTargets1C)
            if (target != null)
                target.ResetForRetry();
    }

    // ─── Notebook ─────────────────────────────────────

    private void ShowNotebook()
    {
        SetActive(notebookButton, true);
        if (notebookButtonText != null)
            notebookButtonText.text =
                Chapter1Texts.NotebookButton();
        if (chapterOverlayUI != null)
            chapterOverlayUI
                .SetLockedObjectsPublic(true);
    }

    // ─── Stations ─────────────────────────────────────

    private void HideAllStations()
    {
        SetActive(station1ANPCs, false);
        SetActive(station1BNPCs, false);
        SetActive(station1CNPCs, false);
        SetActive(TimerPulsePanel, false);
    }

    // ─── Unlock ───────────────────────────────────────

    public void OnExperimentUnlocked()
    {
        ShowNotebook();
        RefreshAllNextButtons();

        currentExperiment =
            CurrentExperiment.Experiment1A;

        SetActive(station1ANPCs, true);

        if (pulseTargets1A != null
            && pulseTargets1A.Length > 0)
        {
            MoveTimerToTarget(pulseTargets1A[0]);
        }

        HideTimerPanel();

        if (mainStatusText != null)
            mainStatusText.text =
                Chapter1Texts.PlaceHandOnWrist();
    }

    // ─── Subject Recorded ─────────────────────────────

    public void OnSubjectRecorded(
        PulseTarget target,
        SubjectType lockedType,
        string lockedName,
        int playerBPM,
        bool passed)
    {
        MoveTimerToTarget(target);
        ExperimentSection section =
            new ExperimentSection();
        switch (target.experimentType)
        {
            case "1A":
                section.experimentTitle =
                    Chapter1Texts.Experiment1ATitle();
                break;
            case "1B":
                section.experimentTitle =
                    Chapter1Texts.Experiment1BTitle();
                break;
            case "1C":
                section.experimentTitle =
                    Chapter1Texts.Experiment1CTitle();
                break;
            default:
                section.experimentTitle =
                    target.experimentType;
                break;
        }
        section.entries.Add(new ExperimentEntry
        {
            label = lockedName,
            result = Chapter1Texts.BPM(playerBPM),
            passed = passed
        });
        SubjectType completedType = target != null
            ? target.subjectType : lockedType;
        TrackCompletionByType(completedType, passed);

        if (passed)
        {
            OnSubjectPassed(completedType);
        }

        CheckStars();
        bool chapterComplete =
            star1Earned && star2Earned && star3Earned;
        SendToNotebook(section, chapterComplete);
    }

    // ─── Complete Triggers ────────────────────────────

    public void TriggerComplete1A()
    {
        Debug.Log("TriggerComplete1A");

        if (mainStatusText != null)
            mainStatusText.text =
                Chapter1Texts.Experiment1AComplete();

        SetActive(station1ANPCs, false);
        SetActive(TimerPulsePanel, false);
        ForceCloseNotebook();

        ShowConclusionPanel(
            Chapter1Texts.ConclusionTitle1A(),
            Chapter1Texts.Conclusion1A(),
            PendingAction.ShowBeginPanel1B);
    }

    public void TriggerComplete1B()
    {
        Debug.Log("TriggerComplete1B");

        if (mainStatusText != null)
            mainStatusText.text =
                Chapter1Texts.Experiment1BComplete();

        SetActive(station1BNPCs, false);
        SetActive(TimerPulsePanel, false);
        ForceCloseNotebook();

        ShowConclusionPanel(
            Chapter1Texts.ConclusionTitle1B(),
            Chapter1Texts.Conclusion1B(),
            PendingAction.ShowBeginPanel1C);
    }

    public void TriggerComplete1C()
    {
        Debug.Log("TriggerComplete1C");

        if (mainStatusText != null)
            mainStatusText.text =
                Chapter1Texts.Experiment1CComplete();

        SetActive(station1CNPCs, false);
        SetActive(TimerPulsePanel, false);
        ForceCloseNotebook();

        ShowConclusionPanel(
            Chapter1Texts.ConclusionTitle1C(),
            Chapter1Texts.Conclusion1C(),
            PendingAction.ShowFinalComplete);
    }

    // ─── Retry ────────────────────────────────────────

    public void RetryExperiment1A()
    {
        ForceCloseNotebook();
        HideAllRetryPanels();
        HideAllStations();

        currentExperiment =
            CurrentExperiment.Experiment1A;

        ResetPulseTargets1A();

        male1ADone = false;
        female1ADone = false;
        complete1AFired = false;

        SetActive(station1ANPCs, true);

        if (pulseTargets1A != null
            && pulseTargets1A.Length > 0)
        {
            MoveTimerToTarget(pulseTargets1A[0]);
        }

        HideTimerPanel();

        if (mainStatusText != null)
            mainStatusText.text =
                Chapter1Texts.PlaceHandOnWrist();
    }

    public void RetryExperiment1B()
    {
        ForceCloseNotebook();
        HideAllRetryPanels();
        HideAllStations();

        currentExperiment =
            CurrentExperiment.Experiment1B;

        ResetPulseTargets1B();

        student1BDone = false;
        teacher1BDone = false;
        assistant1BDone = false;
        complete1BFired = false;

        SetActive(station1BNPCs, true);

        if (pulseTargets1B != null
            && pulseTargets1B.Length > 0)
        {
            MoveTimerToTarget(pulseTargets1B[0]);
        }

        HideTimerPanel();

        if (mainStatusText != null)
            mainStatusText.text =
                Chapter1Texts.PlaceHandOnWrist();
    }

    public void RetryExperiment1C()
    {
        ForceCloseNotebook();
        HideAllRetryPanels();
        HideAll1BPanels();
        HideAll1CPanels();
        HideAllStations();

        currentExperiment =
            CurrentExperiment.Experiment1C;

        ResetPulseTargets1C();

        resting1CDone = false;
        walking1CDone = false;
        running1CDone = false;
        complete1CFired = false;

        SetActive(station1CNPCs, true);

        if (pulseTargets1C != null
            && pulseTargets1C.Length > 0)
        {
            MoveTimerToTarget(pulseTargets1C[0]);
        }

        HideTimerPanel();

        if (activitySelector != null)
        {
            activitySelector.ResetButtons();
            activitySelector.RefreshButtonLabels();
        }

        if (mainStatusText != null)
            mainStatusText.text =
                Chapter1Texts.PlaceHandOnWrist();
    }

    public void RetryCurrentExperiment()
    {
        switch (currentExperiment)
        {
            case CurrentExperiment.Experiment1A:
                RetryExperiment1A();
                break;

            case CurrentExperiment.Experiment1B:
                RetryExperiment1B();
                break;

            case CurrentExperiment.Experiment1C:
                RetryExperiment1C();
                break;

            default:
                Debug.LogWarning(
                    "No current experiment selected.");
                break;
        }
    }

    // ─── Next ─────────────────────────────────────────

    public void NextAfter1A()
    {
        Debug.Log("NextAfter1A called");
        Debug.Log("_isEnglish = " + _isEnglish);

        ForceCloseNotebook();
        HideAllRetryPanels();

        if (_isEnglish)
        {
            Debug.Log("Showing EN Begin Panel 1B");
            SetActive(beginPanel1B_EN, true);
        }
        else
        {
            Debug.Log("Showing BM Begin Panel 1B");
            SetActive(beginPanel1B_BM, true);
        }
    }

    public void NextAfter1B()
    {
        ForceCloseNotebook();
        HideAllRetryPanels();
        if (_isEnglish)
            SetActive(beginPanel1C_EN, true);
        else
            SetActive(beginPanel1C_BM, true);
    }

    public void NextAfter1C()
    {
        ForceCloseNotebook();
        HideAllRetryPanels();
        if (_isEnglish)
            SetActive(completePanelEN, true);
        else
            SetActive(completePanelBM, true);
    }

    // ─── Intro Next ───────────────────────────────────

    public void OnIntroNext1BPressed()
    {
        HideAll1BPanels();
        HideAllStations();
        ForceCloseNotebook();

        currentExperiment =
            CurrentExperiment.Experiment1B;

        ShowNotebook();

        SetActive(station1BNPCs, true);

        if (pulseTargets1B != null
            && pulseTargets1B.Length > 0)
        {
            MoveTimerToTarget(pulseTargets1B[0]);
        }

        HideTimerPanel();

        if (mainStatusText != null)
            mainStatusText.text =
                Chapter1Texts.PlaceHandOnWrist();
    }

    public void OnIntroNext1CPressed()
    {
        HideAll1CPanels();
        HideAllStations();
        ForceCloseNotebook();

        currentExperiment =
            CurrentExperiment.Experiment1C;

        ShowNotebook();

        SetActive(station1CNPCs, true);

        if (pulseTargets1C != null
            && pulseTargets1C.Length > 0)
        {
            MoveTimerToTarget(pulseTargets1C[0]);
        }

        HideTimerPanel();

        if (activitySelector != null)
        {
            activitySelector.ResetButtons();
            activitySelector.RefreshButtonLabels();
        }

        if (mainStatusText != null)
            mainStatusText.text =
                Chapter1Texts.PlaceHandOnWrist();
    }

    // ─── Begin Buttons ────────────────────────────────

    public void OnBegin1BPressed()
    {
        ForceCloseNotebook();
        Hide(beginPanel1B_EN);
        Hide(beginPanel1B_BM);
        if (_isEnglish)
        {
            SetActive(introPanel1B_EN, true);
            SetActive(introPanel1B_BM, false);
        }
        else
        {
            SetActive(introPanel1B_EN, false);
            SetActive(introPanel1B_BM, true);
        }
    }

    public void OnBegin1CPressed()
    {
        
        ForceCloseNotebook();
        Hide(beginPanel1C_EN);
        Hide(beginPanel1C_BM);
        if (_isEnglish)
        {
            SetActive(introPanel1C_EN, true);
            SetActive(introPanel1C_BM, false);
        }
        else
        {
            SetActive(introPanel1C_EN, false);
            SetActive(introPanel1C_BM, true);
        }
    }

    // ─── Experiment Selection ─────────────────────────

    public void ShowExperimentSelection()
    {
        HideAllRetryPanels();
        HideAllStations();
        Hide(beginPanel1B_EN);
        Hide(beginPanel1B_BM);
        Hide(introPanel1B_EN);
        Hide(introPanel1B_BM);
        Hide(beginPanel1C_EN);
        Hide(beginPanel1C_BM);
        Hide(introPanel1C_EN);
        Hide(introPanel1C_BM);
        Hide(completePanelEN);
        Hide(completePanelBM);
        if (chapterOverlayUI != null)
            chapterOverlayUI.HideAllPanels();
        if (_isEnglish)
        {
            SetActive(experimentSelectionPanel,
                true);
            SetActive(experimentSelectionPanelBM,
                false);
        }
        else
        {
            SetActive(experimentSelectionPanel,
                false);
            SetActive(experimentSelectionPanelBM,
                true);
        }
    }

    public void SelectExperiment1A()
    {
        SetActive(experimentSelectionPanel, false);
        SetActive(experimentSelectionPanelBM, false);
        HideAllStations();
        HideAllRetryPanels();
        currentExperiment =
            CurrentExperiment.Experiment1A;
        SetActive(station1ANPCs, true);
        if (_isEnglish)
            SetActive(beginPanel1A_EN, true);
        else
            SetActive(beginPanel1A_BM, true);
    }

    public void SelectExperiment1B()
    {
        SetActive(experimentSelectionPanel, false);
        SetActive(experimentSelectionPanelBM, false);
        HideAllStations();
        HideAllRetryPanels();
        currentExperiment =
            CurrentExperiment.Experiment1B;
        if (_isEnglish)
            SetActive(beginPanel1B_EN, true);
        else
            SetActive(beginPanel1B_BM, true);
    }

    public void SelectExperiment1C()
    {
        SetActive(experimentSelectionPanel, false);
        SetActive(experimentSelectionPanelBM, false);
        HideAllStations();
        HideAllRetryPanels();
        currentExperiment =
            CurrentExperiment.Experiment1C;
        if (_isEnglish)
            SetActive(beginPanel1C_EN, true);
        else
            SetActive(beginPanel1C_BM, true);
    }

    // ─── Tracking ─────────────────────────────────────

    private void TrackCompletionByType(
        SubjectType lockedType, bool passed)
    {
        if (!passed) return;
        switch (lockedType)
        {
            case SubjectType.Male:
                male1ADone = true; break;
            case SubjectType.Female:
                female1ADone = true; break;
            case SubjectType.Student:
                student1BDone = true; break;
            case SubjectType.Teacher:
                teacher1BDone = true; break;
            case SubjectType.LabAssistant:
                assistant1BDone = true; break;
            case SubjectType.Resting:
                resting1CDone = true; break;
            case SubjectType.Walking:
                walking1CDone = true; break;
            case SubjectType.Running:
                running1CDone = true; break;
        }
    }

    private void CheckStars()
    {
        if (male1ADone && female1ADone
            && !star1Earned)
        {
            star1Earned = true;
            ExperimentSection s =
                new ExperimentSection();
            s.experimentTitle =
                Chapter1Texts.Experiment1ATitle();
            s.stars = 1;
            SendToNotebook(s, false);
        }
        if (student1BDone && teacher1BDone
            && assistant1BDone && !star2Earned)
        {
            star2Earned = true;
            ExperimentSection s =
                new ExperimentSection();
            s.experimentTitle =
                Chapter1Texts.Experiment1BTitle();
            s.stars = 2;
            SendToNotebook(s, false);
        }
        if (resting1CDone && walking1CDone
            && running1CDone && !star3Earned)
        {
            star3Earned = true;
            ExperimentSection s =
                new ExperimentSection();
            s.experimentTitle =
                Chapter1Texts.Experiment1CTitle();
            s.stars = 3;
            SendToNotebook(s, true);
        }
    }

    // ─── Language ─────────────────────────────────────

    private void RefreshAllNextButtons()
    {
        foreach (var btn in nextButtonText)
            if (btn != null)
                btn.text = Chapter1Texts.NextButton();
    }

    // ─── Hide Helpers ─────────────────────────────────

    private void HideAllRetryPanels()
    {
        SetActive(retryPanel1A_EN, false);
        SetActive(retryPanel1B_EN, false);
        SetActive(retryPanel1C_EN, false);
        SetActive(retryPanel1A_BM, false);
        SetActive(retryPanel1B_BM, false);
        SetActive(retryPanel1C_BM, false);
    }

    private void HideAll1BPanels()
    {
        Hide(beginPanel1B_EN);
        Hide(beginPanel1B_BM);
        Hide(introPanel1B_EN);
        Hide(introPanel1B_BM);
    }

    private void HideAll1CPanels()
    {
        Hide(beginPanel1C_EN);
        Hide(beginPanel1C_BM);
        Hide(introPanel1C_EN);
        Hide(introPanel1C_BM);
    }

    private void ShowRetryPanel1A()
    {
        HideAllRetryPanels();
        SetActive(_isEnglish
            ? retryPanel1A_EN
            : retryPanel1A_BM, true);
    }

    private void ShowRetryPanel1B()
    {
        HideAllRetryPanels();
        SetActive(_isEnglish
            ? retryPanel1B_EN
            : retryPanel1B_BM, true);
    }

    private void ShowRetryPanel1C()
    {
        HideAllRetryPanels();
        SetActive(_isEnglish
            ? retryPanel1C_EN
            : retryPanel1C_BM, true);
    }

    // ─── Menu / Chapter ───────────────────────────────

    public void GoToChapterStart()
    {
        UnityEngine.SceneManagement
            .SceneManager.LoadScene(
                UnityEngine.SceneManagement
                    .SceneManager
                    .GetActiveScene()
                    .buildIndex);
    }

    // ─── Helpers ──────────────────────────────────────

    private void Hide(GameObject obj)
    {
        if (obj != null) obj.SetActive(false);
    }

    private void SetActive(
        GameObject obj, bool active)
    {
        if (obj != null) obj.SetActive(active);
    }

    public void OnNotebookButtonPressed()
    {
        ToggleNotebook();
    }
    
    public void ShowTimerPanel()
    {
        SetActive(TimerPulsePanel, true);
    }

    public void HideTimerPanel()
    {
        SetActive(TimerPulsePanel, false);
    }

    private void CloseNotebookIfOpen()
    {
        if (NotebookSystem.Instance != null &&
            NotebookSystem.Instance.IsNotebookOpen())
        {
            NotebookSystem.Instance.CloseNotebook();
        }
    }

    public void OnSubjectPassed(SubjectType subjectType)
    {
        if (activitySelector == null) return;

        if (subjectType == SubjectType.Resting)
            activitySelector.UnlockWalkingButton();

        if (subjectType == SubjectType.Walking)
            activitySelector.UnlockRunningButton();
    }
    
    private void ForceCloseNotebook()
    {
        if (NotebookSystem.Instance != null)
        {
            NotebookSystem.Instance.CloseNotebook();
        }
    }
    
    private void ShowConclusionPanel(
        string title,
        string conclusion,
        PendingAction action)
    {
        HideAllRetryPanels();

        pendingAction = action;

        if (conclusionTitle != null)
            conclusionTitle.text = title;

        if (conclusionText != null)
            conclusionText.text = conclusion;

        if (conclusionNextButtonText != null)
            conclusionNextButtonText.text =
                Chapter1Texts.NextButton();

        SetActive(conclusionPanel, true);
        SetActive(conclusionNextButton, true);
    }

    public void OnConclusionNextPressed()
    {
        SetActive(conclusionPanel, false);
        SetActive(conclusionNextButton, false);

        switch (pendingAction)
        {
            case PendingAction.ShowBeginPanel1B:
                NextAfter1A();
                break;

            case PendingAction.ShowBeginPanel1C:
                NextAfter1B();
                break;

            case PendingAction.ShowFinalComplete:
                NextAfter1C();
                break;
        }

        pendingAction = PendingAction.None;
    }
}