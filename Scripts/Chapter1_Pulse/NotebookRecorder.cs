using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PulseEntry
{
    public string experimentType;
    public string label;
    public int bpm;
    public bool passed;
}

public class NotebookRecorder : MonoBehaviour
{
    public static NotebookRecorder Instance;

    [Header("Notebook Display")]
    public TextMeshProUGUI notebookDisplay;

    [Header("Star Images")]
    public Image starImage;
    public Sprite star1;   // len_1bintang
    public Sprite star2;   // len_2bintang
    public Sprite star3;   // len_3bintang

    [Header("Experiment Titles")]
    public string title3A = "Pulse Rate based on Gender";
    public string title3B = "Pulse Rate based on Age";
    public string title3C = "Pulse Rate based on Physical Activity";

    private List<PulseEntry> entries = new List<PulseEntry>();

    private bool male3APassed = false;
    private bool female3APassed = false;

    private bool student3BPassed = false;
    private bool teacher3BPassed = false;
    private bool assistant3BPassed = false;

    private bool resting3CPassed = false;
    private bool walking3CPassed = false;
    private bool running3CPassed = false;

    private bool star1Earned = false;
    private bool star2Earned = false;
    private bool star3Earned = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Hide star image at start
        if (starImage != null)
            starImage.gameObject.SetActive(false);
    }

    public void RecordEntry(string expType, string label, int bpm, bool passed)
    {
        // Update existing entry if already recorded
        PulseEntry existing = entries.Find(e =>
            e.experimentType == expType && e.label == label);

        if (existing != null)
        {
            existing.bpm = bpm;
            existing.passed = passed;
        }
        else
        {
            entries.Add(new PulseEntry
            {
                experimentType = expType,
                label = label,
                bpm = bpm,
                passed = passed
            });
        }

        UpdatePassTracking(expType, label, passed);
        CheckStars();
        RefreshDisplay();
    }

    private void UpdatePassTracking(string expType, string label, bool passed)
    {
        if (expType == "3A")
        {
            if (label == "Male") male3APassed = passed;
            else if (label == "Female") female3APassed = passed;
        }
        else if (expType == "3B")
        {
            if (label == "Student") student3BPassed = passed;
            else if (label == "Teacher") teacher3BPassed = passed;
            else if (label == "Lab Assistant") assistant3BPassed = passed;
        }
        else if (expType == "3C")
        {
            if (label == "Resting") resting3CPassed = passed;
            else if (label == "Walking") walking3CPassed = passed;
            else if (label == "Running") running3CPassed = passed;
        }
    }

    private void CheckStars()
    {
        if (male3APassed && female3APassed)
            star1Earned = true;

        if (student3BPassed && teacher3BPassed && assistant3BPassed)
            star2Earned = true;

        if (resting3CPassed && walking3CPassed && running3CPassed)
            star3Earned = true;

        UpdateStarImage();
    }

    private void UpdateStarImage()
    {
        if (starImage == null) return;

        if (star3Earned && star3 != null)
        {
            starImage.gameObject.SetActive(true);
            starImage.sprite = star3;
        }
        else if (star2Earned && star2 != null)
        {
            starImage.gameObject.SetActive(true);
            starImage.sprite = star2;
        }
        else if (star1Earned && star1 != null)
        {
            starImage.gameObject.SetActive(true);
            starImage.sprite = star1;
        }
        else
        {
            // No stars yet — hide the image
            starImage.gameObject.SetActive(false);
        }
    }

    private void RefreshDisplay()
    {
        if (notebookDisplay == null) return;

        string text = "";

        string section3A = GetSectionText("3A");
        if (section3A != "")
            text += "[ " + title3A + " ]\n" + section3A + "\n";

        string section3B = GetSectionText("3B");
        if (section3B != "")
            text += "[ " + title3B + " ]\n" + section3B + "\n";

        string section3C = GetSectionText("3C");
        if (section3C != "")
            text += "[ " + title3C + " ]\n" + section3C + "\n";

        notebookDisplay.text = text;
    }

    private string GetSectionText(string expType)
    {
        string result = "";
        foreach (var e in entries)
        {
            if (e.experimentType == expType)
            {
                string status = e.passed ? "PASS" : "FAIL";
                result += status + "  " + e.label + ":  " + e.bpm + " BPM\n";
            }
        }
        return result;
    }
}
