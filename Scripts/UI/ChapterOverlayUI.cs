using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterOverlayUI : MonoBehaviour
{
    [Header("Language Panels")]
    public GameObject beginPanelEnglish;
    public GameObject beginPanelMalay;
    public GameObject introPanelEnglish;
    public GameObject introPanelMalay;
    public GameObject completePanelEnglish;
    public GameObject completePanelMalay;

    [Header("Chapter Start Panels")]
    public GameObject chapterStartPanelEnglish;
    public GameObject chapterStartPanelMalay;

    [Header("Tutorial Panels English")]
    [Tooltip("Drag all EN tutorial panels here " +
        "in order. Add as many as needed.")]
    public GameObject[] tutorialPanelsEN;

    [Header("Tutorial Panels Malay")]
    [Tooltip("Drag all BM tutorial panels here " +
        "in order. Add as many as needed.")]
    public GameObject[] tutorialPanelsBM;

    [Header("Objects To Lock Until Intro Done")]
    public GameObject[] lockedUntilIntro;

    [Header("Scene Settings")]
    public string mainMenuScene = "Main Menu";

    [Header("Teleport Settings")]
    [Tooltip("Spawn point near the experiment " +
        "area — used when experiment starts")]
    public Transform experimentSpawnPoint;
    [Tooltip("Spawn point in front of the " +
        "main UI/retry panel area — used when " +
        "pressing Proceed after conclusion")]
    public Transform uiSpawnPoint;

    [Header("Chapter Connections")]
    public Chapter4Manager chapter4Manager;
    public Chapter1Manager chapter1Manager;
    public Chapter3Manager chapter3Manager;
    public Chapter5Manager chapter5Manager;

    private bool _isEnglish = true;
    private int tutorialIndex = 0;

    private void Start()
    {
        if (LanguageManager.Instance != null)
            _isEnglish = LanguageManager.Instance
                .IsEnglish();
        else
            _isEnglish = true;

        HideAllPanels();
        SetLockedObjects(false);

        if (_isEnglish)
            SetActive(beginPanelEnglish, true);
        else
            SetActive(beginPanelMalay, true);
    }

    public void OnBeginPressed()
    {
        HideAllPanels();

        if (_isEnglish)
            SetActive(introPanelEnglish, true);
        else
            SetActive(introPanelMalay, true);
    }

    public void OnIntroNextPressed()
    {
        SetActive(introPanelEnglish, false);
        SetActive(introPanelMalay, false);
        StartTutorialFlow();
    }

    private void StartTutorialFlow()
    {
        tutorialIndex = 0;
        ShowTutorialPanel(tutorialIndex);
    }

    // Wire to Next button on tutorial panels
    public void OnTutorialNextPressed()
    {
        tutorialIndex++;

        GameObject[] panels = _isEnglish
            ? tutorialPanelsEN
            : tutorialPanelsBM;

        if (panels == null
            || tutorialIndex >= panels.Length)
        {
            HideAllTutorialPanels();
            ProceedAfterIntro();
            return;
        }

        ShowTutorialPanel(tutorialIndex);
    }

    // Wire to Back button on tutorial panels
    public void OnTutorialBackPressed()
    {
        tutorialIndex--;

        if (tutorialIndex < 0)
        {
            HideAllTutorialPanels();

            if (_isEnglish)
                SetActive(introPanelEnglish, true);
            else
                SetActive(introPanelMalay, true);

            tutorialIndex = 0;
            return;
        }

        ShowTutorialPanel(tutorialIndex);
    }

    private void ShowTutorialPanel(int index)
    {
        HideAllTutorialPanels();

        GameObject[] panels = _isEnglish
            ? tutorialPanelsEN
            : tutorialPanelsBM;

        if (panels == null
            || index < 0
            || index >= panels.Length)
        {
            Debug.LogWarning(
                "[ChapterOverlayUI] " +
                "Tutorial panel index "
                + index + " out of range.");
            return;
        }

        SetActive(panels[index], true);
    }

    private void HideAllTutorialPanels()
    {
        if (tutorialPanelsEN != null)
            foreach (var p in tutorialPanelsEN)
                SetActive(p, false);

        if (tutorialPanelsBM != null)
            foreach (var p in tutorialPanelsBM)
                SetActive(p, false);
    }

    private void ProceedAfterIntro()
    {
        SetLockedObjects(true);
        TeleportPlayerToExperiment();

        if (chapter4Manager != null)
            chapter4Manager.OnExperimentUnlocked();

        if (chapter1Manager != null)
            chapter1Manager.OnExperimentUnlocked();

        if (chapter3Manager != null)
            chapter3Manager.OnExperimentUnlocked();

        if (chapter5Manager != null)
            chapter5Manager.OnExperimentUnlocked();
    }

    public void ShowBeginPanel()
    {
        HideAllPanels();

        if (_isEnglish)
        {
            if (chapterStartPanelEnglish != null)
                SetActive(chapterStartPanelEnglish,
                    true);
            else
                SetActive(beginPanelEnglish, true);
        }
        else
        {
            if (chapterStartPanelMalay != null)
                SetActive(chapterStartPanelMalay,
                    true);
            else
                SetActive(beginPanelMalay, true);
        }
    }

    public void ShowAchievementPanel()
    {
        if (_isEnglish)
            SetActive(completePanelEnglish, true);
        else
            SetActive(completePanelMalay, true);
    }

    public void OnNotebookPressed()
    {
        ShowAchievementPanel();
    }

    public void BackToMenu()
    {
        PlayerPrefs.SetInt(
            "ReturnToChapterSelect", 1);
        SceneManager.LoadScene(mainMenuScene);
    }

    public void CloseCompletePanel()
    {
        SetActive(completePanelEnglish, false);
        SetActive(completePanelMalay, false);
    }

    public void HideAllPanels()
    {
        SetActive(beginPanelEnglish, false);
        SetActive(beginPanelMalay, false);
        SetActive(introPanelEnglish, false);
        SetActive(introPanelMalay, false);
        SetActive(completePanelEnglish, false);
        SetActive(completePanelMalay, false);
        SetActive(chapterStartPanelEnglish, false);
        SetActive(chapterStartPanelMalay, false);

        HideAllTutorialPanels();
    }

    // ─── Teleport ─────────────────────────────────────

    // Used when experiment starts
    public void TeleportPlayerToExperiment()
    {
        TeleportTo(experimentSpawnPoint,
            "experiment");
    }

    // Used when pressing Proceed in
    // conclusion panel (3B / 3C / 5B)
    public void TeleportPlayerToUI()
    {
        // Falls back to experimentSpawnPoint
        // if uiSpawnPoint not assigned
        Transform target = uiSpawnPoint != null
            ? uiSpawnPoint
            : experimentSpawnPoint;
        TeleportTo(target, "UI");
    }

    private void TeleportTo(
        Transform target, string label)
    {
        if (target == null)
        {
            Debug.LogWarning(
                "[ChapterOverlayUI] No "
                + label + " spawn point assigned!");
            return;
        }

        GameObject xrOrigin =
            GameObject.Find(
                "XR Origin Hands (XR Rig)");
        if (xrOrigin == null)
            xrOrigin = GameObject.Find(
                "XR Origin (XR Rig)");
        if (xrOrigin == null)
            xrOrigin = GameObject.Find("XR Origin");
        if (xrOrigin == null)
            xrOrigin = GameObject.Find("XRRig");

        if (xrOrigin == null)
        {
            Debug.LogWarning("XR Origin not found!");
            return;
        }

        CharacterController cc =
            xrOrigin.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        xrOrigin.transform.position =
            target.position;
        xrOrigin.transform.rotation =
            target.rotation;

        if (cc != null) cc.enabled = true;

        Debug.Log("[ChapterOverlayUI] Teleport to "
            + label + " complete!");
    }

    public void SetLockedObjectsPublic(bool active)
    {
        SetLockedObjects(active);
    }

    private void SetActive(
        GameObject obj, bool active)
    {
        if (obj != null)
            obj.SetActive(active);
    }

    private void SetLockedObjects(bool active)
    {
        foreach (var obj in lockedUntilIntro)
            if (obj != null)
                obj.SetActive(active);
    }
}