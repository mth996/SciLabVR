using UnityEngine;
using UnityEngine.Events;

public class IntroPanelController : MonoBehaviour
{
    [Header("Begin Panel")]
    [Tooltip("The begin panel to hide " +
        "when intro starts e.g. Begin 3b")]
    public GameObject beginPanelEnglish;
    public GameObject beginPanelMalay;

    [Header("Intro Panels — English")]
    [Tooltip("Drag intro panels in order " +
        "e.g. Intro 3b (1), Intro 3b (2)...")]
    public GameObject[] introPanelsEnglish;

    [Header("Intro Panels — Malay")]
    [Tooltip("Drag Malay intro panels " +
        "in same order")]
    public GameObject[] introPanelsMalay;

    [Header("On Finish")]
    [Tooltip("Called when player reaches " +
        "last panel and presses Next/Begin. " +
        "Connect to StartExperiment method.")]
    public UnityEvent onIntroFinished;

    private int currentIndex = 0;
    private bool _isEnglish = true;
    private GameObject[] _activePanels;

    private void Awake()
    {
        // Hide all panels at start
        HideAll(introPanelsEnglish);
        HideAll(introPanelsMalay);
    }

    // ─── Called by Begin Experiment button ────────

    public void StartIntro()
    {
        // Read language
        if (LanguageManager.Instance != null)
            _isEnglish = LanguageManager
                .Instance.IsEnglish();
        else
            _isEnglish = true;

        // Hide begin panel
        if (_isEnglish)
        {
            if (beginPanelEnglish != null)
                beginPanelEnglish
                    .SetActive(false);
        }
        else
        {
            if (beginPanelMalay != null)
                beginPanelMalay
                    .SetActive(false);
        }

        // Pick correct panel set
        _activePanels = _isEnglish
            ? introPanelsEnglish
            : introPanelsMalay;

        if (_activePanels == null
            || _activePanels.Length == 0)
        {
            Debug.LogWarning(
                "No intro panels assigned! " +
                "Skipping to experiment.");
            onIntroFinished?.Invoke();
            return;
        }

        currentIndex = 0;
        HideAll(introPanelsEnglish);
        HideAll(introPanelsMalay);
        ShowPanel(currentIndex);

        Debug.Log("Intro started. "
            + _activePanels.Length
            + " panels.");
    }

    // ─── Next button ──────────────────────────────

    public void OnNextPressed()
    {
        if (_activePanels == null) return;

        HidePanel(currentIndex);
        currentIndex++;

        // Last panel reached — start experiment
        if (currentIndex >= _activePanels.Length)
        {
            Debug.Log("Intro finished! " +
                "Starting experiment.");
            onIntroFinished?.Invoke();
            return;
        }

        ShowPanel(currentIndex);
    }

    // ─── Back button ──────────────────────────────

    public void OnBackPressed()
    {
        if (_activePanels == null) return;

        // If on first panel — show begin panel
        if (currentIndex <= 0)
        {
            HidePanel(currentIndex);

            if (_isEnglish)
            {
                if (beginPanelEnglish != null)
                    beginPanelEnglish
                        .SetActive(true);
            }
            else
            {
                if (beginPanelMalay != null)
                    beginPanelMalay
                        .SetActive(true);
            }
            return;
        }

        HidePanel(currentIndex);
        currentIndex--;
        ShowPanel(currentIndex);
    }

    // ─── Helpers ──────────────────────────────────

    private void ShowPanel(int index)
    {
        if (_activePanels == null) return;
        if (index < 0
            || index >= _activePanels.Length)
            return;
        if (_activePanels[index] != null)
            _activePanels[index].SetActive(true);
    }

    private void HidePanel(int index)
    {
        if (_activePanels == null) return;
        if (index < 0
            || index >= _activePanels.Length)
            return;
        if (_activePanels[index] != null)
            _activePanels[index]
                .SetActive(false);
    }

    private void HideAll(GameObject[] panels)
    {
        if (panels == null) return;
        foreach (var p in panels)
            if (p != null)
                p.SetActive(false);
    }
}