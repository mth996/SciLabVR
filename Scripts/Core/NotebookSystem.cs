using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotebookSystem : MonoBehaviour
{
    public static NotebookSystem Instance;

    [Header("UI References")]
    public GameObject notebookUI;
    public TextMeshProUGUI chapterTitleText;
    public TextMeshProUGUI pageNumberText;
    public TextMeshProUGUI pageContentText;

    [Header("Star Sprites (optional)")]
    public Image starImage;
    public Sprite star0;
    public Sprite star1;
    public Sprite star2;
    public Sprite star3;

    [Header("Star GameObjects")]
    public GameObject star1Object;
    public GameObject star2Object;
    public GameObject star3Object;

    [Header("Navigation")]
    public GameObject prevButton;
    public GameObject nextButton;

    [Header("Navigation Button Text")]
    public TextMeshProUGUI prevButtonText;
    public TextMeshProUGUI nextButtonText;

    [Header("Spawn Settings")]
    public Transform spawnPoint;

    [Header("Page Settings")]
    [Tooltip("Max characters per sub-page")]
    public int maxCharsPerPage = 300;

    private List<NotebookPageData> pages =
        new List<NotebookPageData>();

    private int currentChapterIndex = 0;
    private int currentSubPageIndex = 0;
    private List<string> currentSubPages =
        new List<string>();

    private bool isOpen = false;
    private bool resetScrollOnNextUpdate = false;

    [Header("Scroll Buttons")]
    public ScrollRect scrollRect;
    public float scrollAmount = 0.15f;
    public GameObject scrollUpButton;
    public GameObject scrollDownButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePages();
    }

    private void Start()
    {
        if (notebookUI != null)
            notebookUI.SetActive(false);

        if (scrollUpButton != null)
            scrollUpButton.SetActive(false);
        if (scrollDownButton != null)
            scrollDownButton.SetActive(false);
    }

    // ─── Language ─────────────────────────────────────

    private bool IsEnglish()
    {
        if (LanguageManager.Instance != null)
            return LanguageManager.Instance
                .IsEnglish();
        return true;
    }

    private string GetChapterTitle(
        int chapterNumber)
    {
        bool eng = IsEnglish();
        switch (chapterNumber)
        {
            case 1:
                return eng
                ? "Chapter 1: Body Health Parameters"
                : "Bab 1: Parameter Kesihatan Badan";
            case 2:
                return eng
                ? "Chapter 2: Support & Growth"
                : "Bab 2: Sokongan & Pertumbuhan";
            case 3:
                return eng
                ? "Chapter 3: Industrial Chemistry"
                : "Bab 3: Kimia Industri";
            case 4:
                return eng
                ? "Chapter 4: Medicine & Health"
                : "Bab 4: Perubatan & Kesihatan";
            case 5:
                return eng
                ? "Chapter 5: Force & Motion"
                : "Bab 5: Daya dan Gerakan";
            default:
                return eng
                    ? "Chapter " + chapterNumber
                    : "Bab " + chapterNumber;
        }
    }

    // ─── Initialize ───────────────────────────────────

    private void InitializePages()
    {
        pages.Clear();
        for (int i = 1; i <= 5; i++)
        {
            pages.Add(new NotebookPageData
            {
                chapterNumber = i,
                chapterTitle = GetChapterTitle(i),
                isComplete = false
            });
        }
    }

    // ─── Build full content string ────────────────────

    private string BuildContent(
        NotebookPageData page)
    {
        if (!page.isComplete
            && page.sections.Count == 0)
        {
            return IsEnglish()
                ? "This chapter has not been\ncompleted yet."
                : "Bab ini belum\ndiselesaikan lagi.";
        }

        string content = "";
        foreach (var section in page.sections)
        {
            content += "[ "
                + section.experimentTitle
                + " ]\n";

            foreach (var entry in section.entries)
            {
                string status = entry.passed
                    ? (IsEnglish()
                        ? "PASS" : "LULUS")
                    : (IsEnglish()
                        ? "FAIL" : "GAGAL");

                content += status + "  "
                    + entry.label + ":  "
                    + entry.result + "\n";
            }

            content += "\n";
        }

        return content;
    }

    // ─── Split content into sub-pages ─────────────────

    private List<string> SplitIntoSubPages(
        string fullContent)
    {
        List<string> subPages =
            new List<string>();

        if (string.IsNullOrEmpty(fullContent))
        {
            subPages.Add("");
            return subPages;
        }

        string[] lines =
            fullContent.Split('\n');

        string currentPage = "";

        foreach (string line in lines)
        {
            string testPage = string.IsNullOrEmpty(
                currentPage)
                ? line
                : currentPage + "\n" + line;

            if (testPage.Length > maxCharsPerPage
                && !string.IsNullOrEmpty(
                    currentPage))
            {
                subPages.Add(currentPage.Trim());
                currentPage = line;
            }
            else
            {
                currentPage = testPage;
            }
        }

        if (!string.IsNullOrEmpty(
            currentPage.Trim()))
            subPages.Add(currentPage.Trim());

        if (subPages.Count == 0)
            subPages.Add(fullContent);

        return subPages;
    }

    // ─── Spawn ────────────────────────────────────────

    public void SetSpawnPoint(Transform point)
    {
        spawnPoint = point;
    }

    private void PositionNotebook()
    {
        if (spawnPoint != null)
        {
            notebookUI.transform.position =
                spawnPoint.position;
            notebookUI.transform.rotation =
                spawnPoint.rotation;
        }
        else
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                notebookUI.transform.position =
                    cam.transform.position
                    + cam.transform.forward * 1.5f
                    + Vector3.up * -0.2f;

                notebookUI.transform.rotation =
                    Quaternion.LookRotation(
                        notebookUI.transform.position
                        - cam.transform.position);
            }
        }
    }

    // ─── Update Page ──────────────────────────────────

    public void UpdatePage(
        int chapterNumber,
        ExperimentSection section,
        bool isComplete)
    {
        NotebookPageData page = pages.Find(
            p => p.chapterNumber == chapterNumber);

        if (page == null)
        {
            Debug.LogError(
                "Page not found chapter="
                + chapterNumber);
            return;
        }

        ExperimentSection existing =
            page.sections.Find(s =>
                s.experimentTitle ==
                section.experimentTitle);

        if (existing != null)
        {
            foreach (var entry in section.entries)
            {
                ExperimentEntry existingEntry =
                    existing.entries.Find(
                        e => e.label == entry.label);

                if (existingEntry != null)
                {
                    existingEntry.result =
                        entry.result;
                    existingEntry.passed =
                        entry.passed;
                }
                else
                {
                    existing.entries.Add(entry);
                }
            }

            if (section.stars > existing.stars)
                existing.stars = section.stars;
        }
        else
        {
            page.sections.Add(section);
        }

        page.isComplete = isComplete;
        page.chapterTitle =
            GetChapterTitle(chapterNumber);

        if (pages[currentChapterIndex]
            .chapterNumber == chapterNumber)
            RefreshDisplay();
    }

    // ─── Open / Close ─────────────────────────────────

    public void OpenToChapter(int chapterNumber)
    {
        int index = pages.FindIndex(
            p => p.chapterNumber == chapterNumber);
        if (index >= 0)
            currentChapterIndex = index;

        currentSubPageIndex = 0;

        PositionNotebook();
        notebookUI.SetActive(true);
        isOpen = true;
        RefreshDisplay();
    }

    public void CloseNotebook()
    {
        if (notebookUI != null)
            notebookUI.SetActive(false);
        isOpen = false;
    }

    public void ToggleNotebook(int currentChapter)
    {
        if (isOpen)
            CloseNotebook();
        else
            OpenToChapter(currentChapter);
    }

    // ─── Navigation ───────────────────────────────────

    public void NextPage()
    {
        if (currentSubPageIndex <
            currentSubPages.Count - 1)
        {
            currentSubPageIndex++;
            DisplayCurrentSubPage();
            return;
        }

        if (currentChapterIndex <
            pages.Count - 1)
        {
            currentChapterIndex++;
            currentSubPageIndex = 0;
            RefreshDisplay();
        }
    }

    public void PreviousPage()
    {
        if (currentSubPageIndex > 0)
        {
            currentSubPageIndex--;
            DisplayCurrentSubPage();
            return;
        }

        if (currentChapterIndex > 0)
        {
            currentChapterIndex--;
            currentSubPageIndex = 0;
            RefreshDisplay();
            currentSubPageIndex =
                currentSubPages.Count - 1;
            DisplayCurrentSubPage();
        }
    }

    public void ScrollUp()
    {
        if (scrollRect == null) return;
        scrollRect.verticalNormalizedPosition =
            Mathf.Clamp01(
                scrollRect
                    .verticalNormalizedPosition
                + scrollAmount);
    }

    public void ScrollDown()
    {
        if (scrollRect == null) return;
        scrollRect.verticalNormalizedPosition =
            Mathf.Clamp01(
                scrollRect
                    .verticalNormalizedPosition
                - scrollAmount);
    }

    // ─── Refresh for language change ──────────────────

    public void RefreshFromLanguage()
    {
        RefreshDisplay();
    }

    // ─── Display ──────────────────────────────────────

    private void RefreshDisplay()
    {
        NotebookPageData page =
            pages[currentChapterIndex];

        page.chapterTitle =
            GetChapterTitle(page.chapterNumber);

        string fullContent = BuildContent(page);
        currentSubPages =
            SplitIntoSubPages(fullContent);

        currentSubPageIndex = Mathf.Clamp(
            currentSubPageIndex,
            0,
            currentSubPages.Count - 1);

        DisplayCurrentSubPage();
    }

    private void DisplayCurrentSubPage()
    {
        NotebookPageData page =
            pages[currentChapterIndex];

        chapterTitleText.text =
            GetChapterTitle(page.chapterNumber);

        bool isMultiSubPage =
            currentSubPages.Count > 1;

        if (IsEnglish())
        {
            pageNumberText.text = isMultiSubPage
                ? "Chapter "
                  + page.chapterNumber
                  + "  ·  Page "
                  + (currentSubPageIndex + 1)
                  + " / "
                  + currentSubPages.Count
                : "Page "
                  + (currentChapterIndex + 1)
                  + " of " + pages.Count;
        }
        else
        {
            pageNumberText.text = isMultiSubPage
                ? "Bab "
                  + page.chapterNumber
                  + "  ·  Halaman "
                  + (currentSubPageIndex + 1)
                  + " / "
                  + currentSubPages.Count
                : "Halaman "
                  + (currentChapterIndex + 1)
                  + " daripada " + pages.Count;
        }

        pageContentText.text =
            currentSubPages[currentSubPageIndex];

        bool hasPrev =
            currentChapterIndex > 0
            || currentSubPageIndex > 0;

        if (prevButton != null)
            prevButton.SetActive(hasPrev);

        bool hasNext =
            currentChapterIndex < pages.Count - 1
            || currentSubPageIndex <
               currentSubPages.Count - 1;

        if (nextButton != null)
            nextButton.SetActive(hasNext);

        if (prevButtonText != null)
            prevButtonText.text = IsEnglish()
                ? " PREV" : " SEBELUMNYA";

        if (nextButtonText != null)
            nextButtonText.text = IsEnglish()
                ? "NEXT " : "SETERUSNYA ";

        int maxStars = 0;
        foreach (var section in page.sections)
            if (section.stars > maxStars)
                maxStars = section.stars;

        UpdateStarDisplay(maxStars);
    }

    // ─── Stars ────────────────────────────────────────

    private void UpdateStarDisplay(int stars)
    {
        if (star1Object != null)
            star1Object.SetActive(stars >= 1);
        if (star2Object != null)
            star2Object.SetActive(stars >= 2);
        if (star3Object != null)
            star3Object.SetActive(stars >= 3);

        if (starImage == null) return;

        if (stars >= 3 && star3 != null)
        {
            starImage.gameObject.SetActive(true);
            starImage.sprite = star3;
        }
        else if (stars == 2 && star2 != null)
        {
            starImage.gameObject.SetActive(true);
            starImage.sprite = star2;
        }
        else if (stars == 1 && star1 != null)
        {
            starImage.gameObject.SetActive(true);
            starImage.sprite = star1;
        }
        else if (stars == 0 && star0 != null)
        {
            starImage.gameObject.SetActive(true);
            starImage.sprite = star0;
        }
        else
        {
            starImage.gameObject.SetActive(false);
        }
    }

    public bool IsNotebookOpen()
    {
        return isOpen;
    }
    public void RefreshNotebookPosition()
    {
        if (isOpen)
        {
            PositionNotebook();
        }
    }
}