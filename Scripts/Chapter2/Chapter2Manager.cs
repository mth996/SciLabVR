using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Chapter2Manager : BaseExperimentManager
{
    public static Chapter2Manager Instance;

    [Header("Active Experiments — tick when ready")]
    public bool use2A = false;
    public bool use2B = true;

    public enum SubExperiment
    {
        None, Exp2A, Exp2B, AllComplete
    }

    private SubExperiment currentExperiment =
        SubExperiment.None;
    private SubExperiment _selectedExperiment =
        SubExperiment.None;

    [Header("Shared UI")]
    public TextMeshProUGUI statusText;
    public GameObject notebookButton;

    [Header("Begin Panels (EN / BM)")]
    public GameObject beginPanelEnglish;
    public GameObject beginPanelMalay;

    [Header("Ch2 Scene Begin Panels")]
    public GameObject sceneBeg2BEnglish;
    public GameObject sceneBeg2BMalay;
    public GameObject sceneBeg2AEnglish;
    public GameObject sceneBeg2AMalay;

    [Header("Experiment Selection Panels (EN / BM)")]
    public GameObject experimentSelectionPanelEnglish;
    public GameObject experimentSelectionPanelMalay;

    [Header("Intro Panel Controllers")]
    public IntroPanelController introPanelController2A;
    public IntroPanelController introPanelController2B;

    [Header("Intro Slide GameObjects 2A")]
    public GameObject[] introPanelSlides2A;

    [Header("Intro Slide GameObjects 2B")]
    public GameObject[] introPanelSlides2B;

    [Header("Retry Panels 2A (EN / BM)")]
    public GameObject retryPanel2AEnglish;
    public GameObject retryPanel2AMalay;

    [Header("Retry Panels 2B (EN / BM)")]
    public GameObject retryPanel2BEnglish;
    public GameObject retryPanel2BMalay;

    [Header("Experiment Panel Roots")]
    public GameObject panel2A;
    public GameObject ch2A;
    public GameObject panel2B;
    public GameObject ch2B;

    [Header("Quit Confirmation — Menu/Retry Area")]
    [Tooltip("Quit panel shown from retry/menu panels")]
    public GameObject quitConfirmPanelEN;
    public GameObject quitConfirmPanelBM;

    [Header("Quit Confirmation — Experiment Area")]
    [Tooltip("Quit panel shown from experiment UI")]
    public GameObject quitConfirmExpPanelEN;
    public GameObject quitConfirmExpPanelBM;

    [Header("Scene Controller")]
    public ChapterOverlayUI chapterOverlayUI;

    private bool _isEnglish = true;

    private void Awake()
    {
        Instance = this;
        chapterNumber = 2;
        chapterTitle = Chapter2BTexts.ChapterTitle();
    }

    protected override void Start()
    {
        base.Start();

        if (LanguageManager.Instance != null)
            _isEnglish = LanguageManager
                .Instance.IsEnglish();

        SetActive(beginPanelEnglish, false);
        SetActive(beginPanelMalay, false);
        SetActive(experimentSelectionPanelEnglish, false);
        SetActive(experimentSelectionPanelMalay, false);
        SetActive(retryPanel2AEnglish, false);
        SetActive(retryPanel2AMalay, false);
        SetActive(retryPanel2BEnglish, false);
        SetActive(retryPanel2BMalay, false);
        SetActive(panel2A, false);
        SetActive(panel2B, false);
        SetActive(ch2A, false);
        SetActive(ch2B, false);
        SetActive(notebookButton, false);
        SetActive(quitConfirmPanelEN, false);
        SetActive(quitConfirmPanelBM, false);
        SetActive(quitConfirmExpPanelEN, false);
        SetActive(quitConfirmExpPanelBM, false);

        if (introPanelController2A != null)
            introPanelController2A.gameObject
                .SetActive(false);
        if (introPanelController2B != null)
            introPanelController2B.gameObject
                .SetActive(false);

        if (statusText != null)
            statusText.text = "";
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
        SceneManager.LoadScene("Main Menu");
    }

    // ─── Begin Panel ──────────────────────────────────

    public void ShowBeginPanel()
    {
        _isEnglish = LanguageManager.Instance != null
            && LanguageManager.Instance.IsEnglish();
        SetActive(beginPanelEnglish, _isEnglish);
        SetActive(beginPanelMalay, !_isEnglish);
    }

    public void OnBeginPressed()
    {
        SetActive(beginPanelEnglish, false);
        SetActive(beginPanelMalay, false);
        SetActive(sceneBeg2BEnglish, false);
        SetActive(sceneBeg2BMalay, false);
        SetActive(sceneBeg2AEnglish, false);
        SetActive(sceneBeg2AMalay, false);

        if (_selectedExperiment ==
            SubExperiment.Exp2A)
        {
            _selectedExperiment = SubExperiment.None;
            BeginIntro2A();
            return;
        }
        if (_selectedExperiment ==
            SubExperiment.Exp2B)
        {
            _selectedExperiment = SubExperiment.None;
            BeginIntro2B();
            return;
        }

        // Always start 2A first when it's enabled —
        // NextAfter2A() -> GetNextExperiment() will
        // chain into 2B automatically once 2A finishes.
        if (use2A) BeginIntro2A();
        else if (use2B) BeginIntro2B();
    }

    // ─── Experiment Selection ─────────────────────────

    public void ShowExperimentSelection()
    {
        SetActive(beginPanelEnglish, false);
        SetActive(beginPanelMalay, false);
        SetActive(experimentSelectionPanelEnglish,
            _isEnglish);
        SetActive(experimentSelectionPanelMalay,
            !_isEnglish);
    }

    public void GoToChapterStart()
    {
        HideAllPanels();
        ShowBeginPanel();
    }

    public void SelectExperiment2A()
    {
        SetActive(experimentSelectionPanelEnglish, false);
        SetActive(experimentSelectionPanelMalay, false);
        _selectedExperiment = SubExperiment.Exp2A;
        SetActive(sceneBeg2AEnglish, _isEnglish);
        SetActive(sceneBeg2AMalay, !_isEnglish);
    }

    public void SelectExperiment2B()
    {
        SetActive(experimentSelectionPanelEnglish, false);
        SetActive(experimentSelectionPanelMalay, false);
        _selectedExperiment = SubExperiment.Exp2B;
        SetActive(sceneBeg2BEnglish, _isEnglish);
        SetActive(sceneBeg2BMalay, !_isEnglish);
    }

    // ─── Intro Panels ─────────────────────────────────

    private void BeginIntro2A()
    {
        if (introPanelController2A != null)
        {
            introPanelController2A.gameObject
                .SetActive(true);
            introPanelController2A.onIntroFinished
                .RemoveAllListeners();
            introPanelController2A.onIntroFinished
                .AddListener(StartExperiment2A);
            introPanelController2A.StartIntro();
        }
        else
        {
            Debug.LogWarning(
                "IntroPanelController2A not assigned!");
            StartExperiment2A();
        }
    }

    private void BeginIntro2B()
    {
        if (introPanelController2B != null)
        {
            introPanelController2B.gameObject
                .SetActive(true);
            introPanelController2B.onIntroFinished
                .RemoveAllListeners();
            introPanelController2B.onIntroFinished
                .AddListener(StartExperiment2B);
            introPanelController2B.StartIntro();
        }
        else
        {
            Debug.LogWarning(
                "IntroPanelController2B not assigned!");
            StartExperiment2B();
        }
    }

    // ─── OnExperimentUnlocked ─────────────────────────

    public void OnExperimentUnlocked()
    {
        _isEnglish = LanguageManager.Instance != null
            && LanguageManager.Instance.IsEnglish();
        SetActive(notebookButton, true);
        if (chapterOverlayUI != null)
            chapterOverlayUI.HideAllPanels();
        if (introPanelController2A != null)
            introPanelController2A.gameObject
                .SetActive(false);
        if (introPanelController2B != null)
            introPanelController2B.gameObject
                .SetActive(false);
        OnBeginPressed();
    }

    // ─── Start Methods ────────────────────────────────

    public void StartExperiment2A()
    {
        currentExperiment = SubExperiment.Exp2A;
        if (introPanelController2A != null)
            introPanelController2A.gameObject
                .SetActive(false);
        foreach (var slide in introPanelSlides2A)
            SetActive(slide, false);
        SetActive(panel2B, false);
        SetActive(ch2B, false);
        SetActive(panel2A, true);
        SetActive(ch2A, true);
        Chapter2AExperiment exp2A =
            GetComponent<Chapter2AExperiment>();
        if (exp2A != null) exp2A.OnUnlocked();
        Debug.Log("Chapter 2: Starting 2A");
    }

    public void StartExperiment2B()
    {
        currentExperiment = SubExperiment.Exp2B;
        if (introPanelController2B != null)
            introPanelController2B.gameObject
                .SetActive(false);
        foreach (var slide in introPanelSlides2B)
            SetActive(slide, false);
        SetActive(panel2A, false);
        SetActive(ch2A, false);
        SetActive(panel2B, true);
        SetActive(ch2B, true);
        SetActive(notebookButton, true);
        if (chapterOverlayUI != null)
            chapterOverlayUI
                .TeleportPlayerToExperiment();
        Chapter2BExperiment exp2B =
            GetComponent<Chapter2BExperiment>();
        if (exp2B != null) exp2B.OnUnlocked();
        Debug.Log("Chapter 2: Starting 2B");
    }

    // ─── Retry Panels ─────────────────────────────────

    private void ShowRetryPanel2A()
    {
        SetActive(retryPanel2AEnglish, _isEnglish);
        SetActive(retryPanel2AMalay, !_isEnglish);
    }

    private void ShowRetryPanel2B()
    {
        SetActive(retryPanel2BEnglish, _isEnglish);
        SetActive(retryPanel2BMalay, !_isEnglish);
    }

    public void RetryExperiment2A()
    {
        SetActive(retryPanel2AEnglish, false);
        SetActive(retryPanel2AMalay, false);
        BeginIntro2A();
    }

    public void RetryExperiment2B()
    {
        SetActive(retryPanel2BEnglish, false);
        SetActive(retryPanel2BMalay, false);
        Chapter2BExperiment exp2B =
            GetComponent<Chapter2BExperiment>();
        if (exp2B != null) exp2B.OnUnlocked();
        else BeginIntro2B();
    }

    public void NextAfter2A()
    {
        SetActive(retryPanel2AEnglish, false);
        SetActive(retryPanel2AMalay, false);
        SetActive(panel2A, false);
        SetActive(ch2A, false);
        StartNextExperiment(SubExperiment.Exp2A);
    }

    public void NextAfter2B()
    {
        SetActive(retryPanel2BEnglish, false);
        SetActive(retryPanel2BMalay, false);
        SetActive(panel2B, false);
        SetActive(ch2B, false);
        StartNextExperiment(SubExperiment.Exp2B);
    }

    public void On2AComplete()
    {
        Debug.Log("2A complete.");
        ShowRetryPanel2A();
    }

    public void On2BComplete()
    {
        Debug.Log("2B complete.");
        ShowRetryPanel2B();
    }

    private void StartNextExperiment(
        SubExperiment justCompleted)
    {
        SubExperiment next =
            GetNextExperiment(justCompleted);
        switch (next)
        {
            case SubExperiment.Exp2A:
                BeginIntro2A(); break;
            case SubExperiment.Exp2B:
                BeginIntro2B(); break;
            case SubExperiment.AllComplete:
                OnAllExperimentsComplete(); break;
        }
    }

    private SubExperiment GetNextExperiment(
        SubExperiment justCompleted)
    {
        SubExperiment[] order = {
            SubExperiment.Exp2A,
            SubExperiment.Exp2B };
        bool[] enabled = { use2A, use2B };
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

    private void OnAllExperimentsComplete()
    {
        Debug.Log(
            "All Chapter 2 experiments complete!");
        ExperimentSection finalSection =
            new ExperimentSection();
        finalSection.experimentTitle =
            Chapter2BTexts.ChapterTitle();
        finalSection.stars = 3;
        SendToNotebook(finalSection, true);
        if (statusText != null)
            statusText.text =
                Chapter2BTexts.ExperimentComplete();
        Invoke("ShowChapterCompletePanel", 2f);
    }

    private void ShowChapterCompletePanel()
    {
        if (chapterOverlayUI != null)
            chapterOverlayUI.ShowAchievementPanel();
        else
            Debug.LogWarning(
                "ChapterOverlayUI not assigned!");
    }

    public void OnNotebookButtonPressed()
    {
        ToggleNotebook();
    }

    private void HideAllPanels()
    {
        SetActive(beginPanelEnglish, false);
        SetActive(beginPanelMalay, false);
        SetActive(sceneBeg2BEnglish, false);
        SetActive(sceneBeg2BMalay, false);
        SetActive(sceneBeg2AEnglish, false);
        SetActive(sceneBeg2AMalay, false);
        SetActive(experimentSelectionPanelEnglish, false);
        SetActive(experimentSelectionPanelMalay, false);
        SetActive(retryPanel2AEnglish, false);
        SetActive(retryPanel2AMalay, false);
        SetActive(retryPanel2BEnglish, false);
        SetActive(retryPanel2BMalay, false);
        SetActive(panel2A, false);
        SetActive(panel2B, false);
        SetActive(ch2A, false);
        SetActive(ch2B, false);
        if (introPanelController2A != null)
            introPanelController2A.gameObject
                .SetActive(false);
        if (introPanelController2B != null)
            introPanelController2B.gameObject
                .SetActive(false);
        foreach (var slide in introPanelSlides2A)
            SetActive(slide, false);
        foreach (var slide in introPanelSlides2B)
            SetActive(slide, false);
    }

    private void SetActive(
        GameObject obj, bool active)
    {
        if (obj != null) obj.SetActive(active);
    }
}