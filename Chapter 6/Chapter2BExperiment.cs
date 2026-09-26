using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chapter2BExperiment
    : MonoBehaviour
{
    public static Chapter2BExperiment Instance;

    // ─── Seedling Stages ──────────────────────────────────────────────────

    [Header("Seedlings")]
    public SeedlingStage seed1;
    public SeedlingStage seed2;
    public SeedlingStage seed3;

    // ─── Settings ─────────────────────────────────────────────────────────

    [Header("Settings")]
    public float acceptedMarginMm = 5f;

    // ─── Seedling Rulers Root ─────────────────────────────────────────────

    [Header("Seedling Rulers")]
    public GameObject seedlingRulersRoot;

    // ─── UI ───────────────────────────────────────────────────────────────

    [Header("UI — Status")]
    public TextMeshProUGUI headerLabel;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI dayLabel;
    public GameObject notebookButton;
    public TextMeshProUGUI notebookButtonText;

    [Header("UI — Day Navigation")]
    public GameObject prevDayButton;
    public GameObject nextDayButton;
    public TextMeshProUGUI prevDayButtonText;
    public TextMeshProUGUI nextDayButtonText;

    [Header("UI — Input Panel")]
    public GameObject inputPanel;
    public TextMeshProUGUI inputTitle;
    public TextMeshProUGUI seed1Label;
    public TextMeshProUGUI seed2Label;
    public TextMeshProUGUI seed3Label;
    public TextMeshProUGUI meanLabel;
    public TextMeshProUGUI seed1ValueText;
    public TextMeshProUGUI seed2ValueText;
    public TextMeshProUGUI seed3ValueText;
    public TextMeshProUGUI meanValueText;
    public TextMeshProUGUI submitButtonText;

    [Header("UI — Conclusion")]
    public GameObject conclusionPanel;
    public TextMeshProUGUI conclusionTitle;
    public TextMeshProUGUI conclusionText;
    public SimpleLineGraph growthGraph;
    
    
    [Header("UI Positioning")]
    public Transform canvasTransform;   // drag in "UI Canvas 2B" itself
    public Transform uiAnchor2B;        // empty object positioned near 2A's apparatus
    

    // completePanelEN/BM removed from flow — retry panel handled by Chapter2Manager

    // ─── Private State ────────────────────────────────────────────────────

    private enum Phase
    {
        Locked,
        Measuring,
        Complete
    }

    private Phase currentPhase = Phase.Locked;

    private int currentDay = 0;
    private const int totalDays = 7;

    private bool[] daySubmitted = new bool[7];
    private float[] recordedMeans = new float[7];

    private float seed1Guess = 0f;
    private float seed2Guess = 0f;
    private float seed3Guess = 0f;

    private bool _isEnglish = true;

    public TextMeshProUGUI proceedButtonText;

    // ─── Unity Lifecycle ──────────────────────────────────────────────────

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (LanguageManager.Instance != null)
            _isEnglish = LanguageManager.Instance.IsEnglish();

        SetActive(seedlingRulersRoot, false);
        SetActive(inputPanel, false);
        SetActive(conclusionPanel, false);
        SetActive(prevDayButton, false);
        SetActive(nextDayButton, false);
    }

    // ─── Unlock / Reset ───────────────────────────────────────────────────

    public void OnUnlocked()
    {
        if (canvasTransform != null && uiAnchor2B != null)
        {
            canvasTransform.position = uiAnchor2B.position;
            canvasTransform.rotation = uiAnchor2B.rotation;
        }

        
        if (LanguageManager.Instance != null)
            _isEnglish = LanguageManager.Instance.IsEnglish();

        SetActive(seedlingRulersRoot, true);
        SetActive(conclusionPanel, false);
        SetActive(inputPanel, false);

        currentPhase = Phase.Measuring;
        currentDay = 0;

        for (int i = 0; i < totalDays; i++)
        {
            daySubmitted[i] = false;
            recordedMeans[i] = 0f;
        }

        SetActive(nextDayButton, true);
        SetActive(prevDayButton, false);

        ShowDay(0);
        RefreshLanguage();

        Debug.Log("2B unlocked. Day 0.");
    }

    // ─── Day Navigation ───────────────────────────────────────────────────

    public void OnNextDayPressed()
    {
        if (currentPhase != Phase.Measuring)
            return;

        if (!daySubmitted[currentDay])
        {
            if (statusText != null)
                statusText.text =
                    IsEnglish()
                    ? "Submit Day " + currentDay + " first!"
                    : "Hantar Hari " + currentDay + " dahulu!";
            return;
        }

        if (currentDay < totalDays - 1)
        {
            currentDay++;
            ShowDay(currentDay);
        }
    }

    public void OnPrevDayPressed()
    {
        if (currentPhase != Phase.Measuring)
            return;

        if (currentDay > 0)
        {
            currentDay--;
            ShowDay(currentDay);
        }
    }

    private void ShowDay(int day)
    {
        currentDay = day;

        if (seed1 != null) seed1.ShowDay(day);
        if (seed2 != null) seed2.ShowDay(day);
        if (seed3 != null) seed3.ShowDay(day);

        if (dayLabel != null)
            dayLabel.text = Chapter2BTexts.DayLabel(day);

        if (statusText != null)
            statusText.text = Chapter2BTexts.MeasureInstruction();

        SetActive(prevDayButton, day > 0);
        SetActive(nextDayButton, day < totalDays - 1);

        if (daySubmitted[day])
        {
            seed1Guess = seed1 != null ? seed1.GetHeightMm(day) : 0f;
            seed2Guess = seed2 != null ? seed2.GetHeightMm(day) : 0f;
            seed3Guess = seed3 != null ? seed3.GetHeightMm(day) : 0f;
        }
        else if (day > 0 && daySubmitted[day - 1])
        {
            seed1Guess = seed1 != null ? seed1.GetHeightMm(day - 1) : 0f;
            seed2Guess = seed2 != null ? seed2.GetHeightMm(day - 1) : 0f;
            seed3Guess = seed3 != null ? seed3.GetHeightMm(day - 1) : 0f;
        }
        else
        {
            seed1Guess = 0f;
            seed2Guess = 0f;
            seed3Guess = 0f;
        }

        UpdateInputUI();
        SetActive(inputPanel, true);
    }

    // ─── Input Buttons ────────────────────────────────────────────────────

    public void OnSeed1Plus()
    {
        Debug.Log("OnSeed1Plus called. currentPhase=" + currentPhase);
        if (currentPhase != Phase.Measuring) return;
        seed1Guess = Mathf.Clamp(seed1Guess + 1f, 0f, 200f);
        UpdateInputUI();
    }

    public void OnSeed1Minus()
    {
        if (currentPhase != Phase.Measuring) return;
        seed1Guess = Mathf.Clamp(seed1Guess - 1f, 0f, 200f);
        UpdateInputUI();
    }

    public void OnSeed2Plus()
    {
        if (currentPhase != Phase.Measuring) return;
        seed2Guess = Mathf.Clamp(seed2Guess + 1f, 0f, 200f);
        UpdateInputUI();
    }

    public void OnSeed2Minus()
    {
        if (currentPhase != Phase.Measuring) return;
        seed2Guess = Mathf.Clamp(seed2Guess - 1f, 0f, 200f);
        UpdateInputUI();
    }

    public void OnSeed3Plus()
    {
        if (currentPhase != Phase.Measuring) return;
        seed3Guess = Mathf.Clamp(seed3Guess + 1f, 0f, 200f);
        UpdateInputUI();
    }

    public void OnSeed3Minus()
    {
        if (currentPhase != Phase.Measuring) return;
        seed3Guess = Mathf.Clamp(seed3Guess - 1f, 0f, 200f);
        UpdateInputUI();
    }

    private void UpdateInputUI()
    {
        if (seed1ValueText != null)
            seed1ValueText.text = seed1Guess.ToString("F0") + " mm";
        if (seed2ValueText != null)
            seed2ValueText.text = seed2Guess.ToString("F0") + " mm";
        if (seed3ValueText != null)
            seed3ValueText.text = seed3Guess.ToString("F0") + " mm";

        float mean = (seed1Guess + seed2Guess + seed3Guess) / 3f;

        if (meanValueText != null)
            meanValueText.text = Chapter2BTexts.MeanCalculated(mean);
    }

    // ─── Submit ───────────────────────────────────────────────────────────

    public void OnSubmitPressed()
    {
        if (currentPhase != Phase.Measuring) return;

        float correct1 = seed1 != null ? seed1.GetHeightMm(currentDay) : 0f;
        float correct2 = seed2 != null ? seed2.GetHeightMm(currentDay) : 0f;
        float correct3 = seed3 != null ? seed3.GetHeightMm(currentDay) : 0f;

        bool s1ok = Mathf.Abs(seed1Guess - correct1) <= acceptedMarginMm;
        bool s2ok = Mathf.Abs(seed2Guess - correct2) <= acceptedMarginMm;
        bool s3ok = Mathf.Abs(seed3Guess - correct3) <= acceptedMarginMm;

        bool passed = s1ok && s2ok && s3ok;

        if (!passed)
        {
            if (statusText != null)
                statusText.text = Chapter2BTexts.ResultWrong(currentDay);
            return;
        }

        // Player's guess is only used for display/notebook feedback.
        // The GRAPH always uses the exact expected values from the
        // hierarchy (seed heightsMm[] data), not the player's guess —
        // this guarantees the sigmoid shape is preserved even when the
        // player enters a slightly-off-but-still-passing value.
        float guessMean = (seed1Guess + seed2Guess + seed3Guess) / 3f;
        float exactMean = (correct1 + correct2 + correct3) / 3f;

        daySubmitted[currentDay] = true;
        recordedMeans[currentDay] = exactMean;   // <- changed from guessMean

        ExperimentSection section = new ExperimentSection();
        section.experimentTitle =
            Chapter2BTexts.ExperimentTitle() + " — " + Chapter2BTexts.DayLabel(currentDay);
        section.stars = 0;

        section.entries.Add(new ExperimentEntry
        {
            label = Chapter2BTexts.Seedling1Label(),
            result = seed1Guess.ToString("F0") + " mm",
            passed = true
        });
        section.entries.Add(new ExperimentEntry
        {
            label = Chapter2BTexts.Seedling2Label(),
            result = seed2Guess.ToString("F0") + " mm",
            passed = true
        });
        section.entries.Add(new ExperimentEntry
        {
            label = Chapter2BTexts.Seedling3Label(),
            result = seed3Guess.ToString("F0") + " mm",
            passed = true
        });
        section.entries.Add(new ExperimentEntry
        {
            label = Chapter2BTexts.MeanLabel(),
            result = guessMean.ToString("F1") + " mm",
            passed = true
        });

        bool allDone = CheckAllDaysSubmitted();

        Chapter2Manager.Instance.SendToNotebook(section, false);

        if (statusText != null)
            statusText.text = Chapter2BTexts.ResultCorrect(currentDay, guessMean);

        SetActive(inputPanel, false);

        if (allDone)
            Invoke("ShowConclusion", 1.5f);
        else if (currentDay < totalDays - 1)
            Invoke("AdvanceToNextDay", 1.5f);
    }

    private void AdvanceToNextDay()
    {
        currentDay++;
        ShowDay(currentDay);
    }

    private bool CheckAllDaysSubmitted()
    {
        for (int i = 0; i < totalDays; i++)
            if (!daySubmitted[i]) return false;
        return true;
    }

    // ─── Conclusion ───────────────────────────────────────────────────────

    private void ShowConclusion()
    {
        currentPhase = Phase.Complete;

        SetActive(conclusionPanel, true);
        SetActive(inputPanel, false);
        SetActive(nextDayButton, false);
        SetActive(prevDayButton, false);

        if (conclusionTitle != null)
            conclusionTitle.text = Chapter2BTexts.ConclusionTitle();

        if (conclusionText != null)
            conclusionText.text = Chapter2BTexts.Conclusion();

        if (growthGraph != null)
        {
            // Force the layout group to recalculate
            // sizes NOW, before we read PlotArea's
            // width/height — otherwise Plot() reads
            // stale (tiny) dimensions from before the
            // panel was activated.
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                conclusionPanel.GetComponent<RectTransform>());

            string[] dayLabels = new string[totalDays];
            for (int i = 0; i < totalDays; i++)
                dayLabels[i] = i.ToString();

            growthGraph.Plot(recordedMeans, dayLabels);
        }

        if (statusText != null)
            statusText.text = Chapter2BTexts.ExperimentComplete();
    }

    // Called by conclusion panel Proceed/Close button
    public void ShowCompletePanel()
    {
        SetActive(conclusionPanel, false);
        // Skip completePanelEN/BM — go straight to retry panel via manager
        Chapter2Manager.Instance.On2BComplete();
    }

    // ─── Language ─────────────────────────────────────────────────────────

    public void OnLanguageChanged()
    {
        if (LanguageManager.Instance != null)
            _isEnglish = LanguageManager.Instance.IsEnglish();
        RefreshLanguage();
    }

    private void RefreshLanguage()
    {
        if (headerLabel != null)
            headerLabel.text = Chapter2BTexts.ExperimentHeader();
        if (notebookButtonText != null)
            notebookButtonText.text = Chapter2BTexts.NotebookButton();
        if (prevDayButtonText != null)
            prevDayButtonText.text = Chapter2BTexts.PrevDayButton();
        if (nextDayButtonText != null)
            nextDayButtonText.text = Chapter2BTexts.NextDayButton();
        if (inputTitle != null)
            inputTitle.text = Chapter2BTexts.InputTitle();
        if (seed1Label != null)
            seed1Label.text = Chapter2BTexts.Seedling1Label();
        if (seed2Label != null)
            seed2Label.text = Chapter2BTexts.Seedling2Label();
        if (seed3Label != null)
            seed3Label.text = Chapter2BTexts.Seedling3Label();
        if (meanLabel != null)
            meanLabel.text = Chapter2BTexts.MeanLabel();
        if (submitButtonText != null)
            submitButtonText.text = Chapter2BTexts.SubmitButton();
        if (dayLabel != null)
            dayLabel.text = Chapter2BTexts.DayLabel(currentDay);
        if (proceedButtonText != null)
            proceedButtonText.text = Chapter2BTexts.ProceedButton();

        if (seed1 != null && seed1.rulerUI != null)
            seed1.rulerUI.SetSeedlingName(Chapter2BTexts.Seedling1Label());
        if (seed2 != null && seed2.rulerUI != null)
            seed2.rulerUI.SetSeedlingName(Chapter2BTexts.Seedling2Label());
        if (seed3 != null && seed3.rulerUI != null)
            seed3.rulerUI.SetSeedlingName(Chapter2BTexts.Seedling3Label());
    }

    // ─── Helpers ──────────────────────────────────────────────────────────

    private bool IsEnglish()
    {
        return LanguageManager.Instance != null
            ? LanguageManager.Instance.IsEnglish()
            : true;
    }

    private void SetActive(GameObject obj, bool active)
    {
        if (obj != null) obj.SetActive(active);
    }
    
}