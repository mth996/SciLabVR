#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class Chapter2BUIBuilder : MonoBehaviour
{
    [MenuItem("Tools/Build Chapter 2B UI Canvas")]
    public static void BuildUI()
    {
        // ── Canvas ────────────────────────────────────────────────────────
        GameObject canvasGO =
            new GameObject("UI Canvas 2B");
        Canvas canvas =
            canvasGO.AddComponent<Canvas>();
        canvas.renderMode =
            RenderMode.WorldSpace;
        canvasGO.AddComponent<CanvasScaler>()
            .dynamicPixelsPerUnit = 10f;
        canvasGO.AddComponent<GraphicRaycaster>();

        RectTransform canvasRT =
            canvasGO.GetComponent<RectTransform>();
        canvasRT.sizeDelta =
            new Vector2(800f, 650f);
        canvasRT.localScale =
            Vector3.one * 0.001f;

        // ── Background ────────────────────────────────────────────────────
        GameObject bg = CreatePanel(canvasGO,
            "BackgroundPanel",
            new Vector2(0, 0),
            new Vector2(780, 630),
            new Color(0.05f, 0.08f,
                0.13f, 0.93f));

        // Border
        GameObject border = CreatePanel(bg,
            "Border",
            new Vector2(0, 0),
            new Vector2(784, 634),
            new Color(0.3f, 0.7f,
                0.4f, 0.35f));
        border.transform.SetAsFirstSibling();

        // ── Header ────────────────────────────────────────────────────────
        // Assign to: Chapter2BManager.headerLabel
        CreateTMP(bg, "HeaderLabel",
            new Vector2(0, 285),
            new Vector2(720, 38),
            "MUNG BEAN SEEDLING " +
            "GROWTH EXPERIMENT",
            16, FontStyles.Bold,
            new Color(0.4f, 0.9f, 0.5f, 1f),
            TextAlignmentOptions.Center);

        CreatePanel(bg, "HeaderDivider",
            new Vector2(0, 258),
            new Vector2(700, 2),
            new Color(0.3f, 0.8f,
                0.4f, 0.4f));

        // ── Day Label ─────────────────────────────────────────────────────
        // Assign to: Chapter2BManager.dayLabel
        CreateTMP(bg, "DayLabel",
            new Vector2(-200, 228),
            new Vector2(200, 35),
            "Day 0",
            18, FontStyles.Bold,
            new Color(1f, 0.85f, 0.2f, 1f),
            TextAlignmentOptions.Center);

        // ── Status Text ───────────────────────────────────────────────────
        // Assign to: Chapter2BManager.statusText
        CreateTMP(bg, "StatusText",
            new Vector2(100, 228),
            new Vector2(400, 50),
            "Read the ruler beside each\n" +
            "seedling and enter the height.",
            12, FontStyles.Normal,
            new Color(0.85f, 0.92f, 1f, 1f),
            TextAlignmentOptions.Center);

        CreatePanel(bg, "Divider2",
            new Vector2(0, 198),
            new Vector2(700, 2),
            new Color(0.3f, 0.6f,
                0.4f, 0.25f));

        // ── Input Panel ───────────────────────────────────────────────────
        // Assign to: Chapter2BManager.inputPanel
        GameObject inputPanel = CreatePanel(bg,
            "InputPanel",
            new Vector2(0, 30),
            new Vector2(720, 320),
            new Color(0.04f, 0.08f,
                0.13f, 0.96f));

        // Title
        // Assign to: Chapter2BManager.inputTitle
        CreateTMP(inputPanel, "InputTitle",
            new Vector2(0, 135),
            new Vector2(680, 28),
            "Enter seedling heights (mm)",
            13, FontStyles.Bold,
            new Color(0.4f, 0.85f,
                0.55f, 1f),
            TextAlignmentOptions.Center);

        CreatePanel(inputPanel,
            "InputDivider",
            new Vector2(0, 110),
            new Vector2(660, 2),
            new Color(0.3f, 0.7f,
                0.4f, 0.3f));

        // Row positions
        float[] rowY = new float[]
            { 70f, 10f, -50f };
        string[] seedNames = new string[]
            { "Seed1", "Seed2", "Seed3" };
        string[] seedLabels = new string[]
        {
            "Seedling 1",
            "Seedling 2",
            "Seedling 3"
        };
        Color[] seedColors = new Color[]
        {
            new Color(0.4f, 0.85f, 0.55f, 1f),
            new Color(0.3f, 0.75f, 0.95f, 1f),
            new Color(1f, 0.75f, 0.3f, 1f)
        };

        for (int i = 0; i < 3; i++)
        {
            float y = rowY[i];
            string n = seedNames[i];
            Color c = seedColors[i];

            // Label
            // Assign to: seed1Label/2/3
            CreateTMP(inputPanel,
                n + "Label",
                new Vector2(-220, y),
                new Vector2(160, 28),
                seedLabels[i],
                11, FontStyles.Bold, c,
                TextAlignmentOptions.Right);

            // Minus button
            // OnClick → OnSeed1/2/3Minus()
            CreateButton(inputPanel,
                n + "MinusButton",
                new Vector2(-30, y),
                new Vector2(44, 44),
                "−",
                new Color(0.15f, 0.22f,
                    0.32f, 1f),
                new Color(0.9f, 0.95f,
                    1f, 1f), 18);

            // Value text
            // Assign to: seed1/2/3ValueText
            CreateTMP(inputPanel,
                n + "ValueText",
                new Vector2(55, y),
                new Vector2(80, 44),
                "0 mm",
                14, FontStyles.Bold, c,
                TextAlignmentOptions.Center);

            // Plus button
            // OnClick → OnSeed1/2/3Plus()
            CreateButton(inputPanel,
                n + "PlusButton",
                new Vector2(140, y),
                new Vector2(44, 44),
                "+",
                new Color(0.08f, 0.45f,
                    0.3f, 1f),
                new Color(0.9f, 0.97f,
                    1f, 1f), 18);
        }

        // Mean display
        // Assign to: Chapter2BManager.meanLabel
        CreateTMP(inputPanel, "MeanLabel",
            new Vector2(-160, -110),
            new Vector2(140, 28),
            "Mean",
            11, FontStyles.Bold,
            new Color(1f, 0.85f, 0.2f, 1f),
            TextAlignmentOptions.Right);

        // Assign to: meanValueText
        CreateTMP(inputPanel, "MeanValueText",
            new Vector2(55, -110),
            new Vector2(200, 28),
            "0.0 mm",
            13, FontStyles.Bold,
            new Color(1f, 0.9f, 0.3f, 1f),
            TextAlignmentOptions.Center);

        CreatePanel(inputPanel,
            "SubmitDivider",
            new Vector2(0, -138),
            new Vector2(660, 2),
            new Color(0.3f, 0.7f,
                0.4f, 0.3f));

        // Submit button
        // OnClick → Chapter2BManager
        //           .OnSubmitPressed()
        CreateButton(inputPanel,
            "SubmitButton",
            new Vector2(0, -155),
            new Vector2(280, 48),
            "SUBMIT MEASUREMENT",
            new Color(0.08f, 0.5f,
                0.3f, 1f),
            new Color(0.92f, 0.97f,
                1f, 1f), 13);

        inputPanel.SetActive(false);

        // ── Day Navigation ────────────────────────────────────────────────
        // Prev button
        // OnClick → Chapter2BManager
        //           .OnPrevDayPressed()
        CreateButton(bg, "PrevDayButton",
            new Vector2(-300, -240),
            new Vector2(160, 48),
            "◀ PREV DAY",
            new Color(0.12f, 0.2f,
                0.3f, 1f),
            new Color(0.75f, 0.85f,
                1f, 1f), 11);

        // Next button
        // OnClick → Chapter2BManager
        //           .OnNextDayPressed()
        CreateButton(bg, "NextDayButton",
            new Vector2(300, -240),
            new Vector2(160, 48),
            "NEXT DAY ▶",
            new Color(0.08f, 0.45f,
                0.3f, 1f),
            new Color(0.9f, 0.97f,
                1f, 1f), 11);

        // ── Notebook Button ───────────────────────────────────────────────
        // OnClick → Chapter2BManager
        //           .OnNotebookButtonPressed()
        CreateButton(bg, "NotebookButton",
            new Vector2(0, -240),
            new Vector2(150, 44),
            "📓  NOTEBOOK",
            new Color(0.1f, 0.2f,
                0.3f, 1f),
            new Color(0.7f, 0.85f,
                1f, 1f), 11);

        // ── Conclusion Panel ──────────────────────────────────────────────
        // Assign to:
        // Chapter2BManager.conclusionPanel
        GameObject conclusionPanel =
            CreatePanel(bg,
            "ConclusionPanel",
            new Vector2(0, 0),
            new Vector2(720, 340),
            new Color(0.03f, 0.06f,
                0.10f, 0.97f));

        CreatePanel(conclusionPanel,
            "LeftAccent",
            new Vector2(-348, 0),
            new Vector2(4, 320),
            new Color(0.3f, 0.85f,
                0.45f, 1f));

        // Assign to:
        // Chapter2BManager.conclusionTitle
        CreateTMP(conclusionPanel,
            "ConclusionTitle",
            new Vector2(0, 140),
            new Vector2(660, 32),
            "CONCLUSION",
            15, FontStyles.Bold,
            new Color(0.35f, 0.92f,
                0.5f, 1f),
            TextAlignmentOptions.Center);

        // Assign to:
        // Chapter2BManager.conclusionText
        CreateTMP(conclusionPanel,
            "ConclusionText",
            new Vector2(10, -10),
            new Vector2(640, 240),
            "",
            12, FontStyles.Normal,
            new Color(0.85f, 0.95f,
                0.88f, 1f),
            TextAlignmentOptions.Center);

        conclusionPanel.SetActive(false);

        // ── Complete Panel EN ─────────────────────────────────────────────
        // Assign to:
        // Chapter2BManager.completePanelEN
        GameObject completePanelEN =
            CreatePanel(bg,
            "CompletePanel_EN",
            new Vector2(0, 0),
            new Vector2(740, 560),
            new Color(0.04f, 0.07f,
                0.11f, 0.98f));

        CreateTMP(completePanelEN,
            "CompleteTitleEN",
            new Vector2(0, 210),
            new Vector2(680, 48),
            "✓  EXPERIMENT COMPLETE",
            20, FontStyles.Bold,
            new Color(0.3f, 0.95f,
                0.55f, 1f),
            TextAlignmentOptions.Center);

        CreateTMP(completePanelEN,
            "CompleteStarsEN",
            new Vector2(0, 160),
            new Vector2(400, 35),
            "★ ★ ★",
            22, FontStyles.Bold,
            new Color(1f, 0.85f, 0.2f, 1f),
            TextAlignmentOptions.Center);

        CreateTMP(completePanelEN,
            "CompleteBodyEN",
            new Vector2(0, 30),
            new Vector2(640, 180),
            "6B: Growth Pattern of\n" +
            "Mung Bean Seedlings\n\n" +
            "All 7 days recorded.\n" +
            "Growth follows a sigmoid pattern.",
            13, FontStyles.Normal,
            new Color(0.82f, 0.9f, 1f, 1f),
            TextAlignmentOptions.Center);

        // OnClick → Chapter2BManager
        //           .BackToMenu()
        CreateButton(completePanelEN,
            "BackToMenuButton_EN",
            new Vector2(-105, -195),
            new Vector2(200, 48),
            "BACK TO MENU",
            new Color(0.15f, 0.2f,
                0.28f, 1f),
            new Color(0.7f, 0.82f,
                1f, 1f));

        // OnClick → Chapter2BManager
        //           .CloseCompletePanel()
        CreateButton(completePanelEN,
            "CloseButton_EN",
            new Vector2(115, -195),
            new Vector2(200, 48),
            "CLOSE",
            new Color(0.08f, 0.45f,
                0.3f, 1f),
            new Color(0.9f, 0.97f,
                1f, 1f));

        completePanelEN.SetActive(false);

        // ── Complete Panel BM ─────────────────────────────────────────────
        // Assign to:
        // Chapter2BManager.completePanelBM
        GameObject completePanelBM =
            CreatePanel(bg,
            "CompletePanel_BM",
            new Vector2(0, 0),
            new Vector2(740, 560),
            new Color(0.04f, 0.07f,
                0.11f, 0.98f));

        CreateTMP(completePanelBM,
            "CompleteTitleBM",
            new Vector2(0, 210),
            new Vector2(680, 48),
            "✓  EKSPERIMEN SELESAI",
            20, FontStyles.Bold,
            new Color(0.3f, 0.95f,
                0.55f, 1f),
            TextAlignmentOptions.Center);

        CreateTMP(completePanelBM,
            "CompleteStarsBM",
            new Vector2(0, 160),
            new Vector2(400, 35),
            "★ ★ ★",
            22, FontStyles.Bold,
            new Color(1f, 0.85f, 0.2f, 1f),
            TextAlignmentOptions.Center);

        CreateTMP(completePanelBM,
            "CompleteBodyBM",
            new Vector2(0, 30),
            new Vector2(640, 180),
            "6B: Corak Pertumbuhan\n" +
            "Anak Benih Kacang Hijau\n\n" +
            "Semua 7 hari direkodkan.\n" +
            "Pertumbuhan mengikuti\n" +
            "corak sigmoid.",
            13, FontStyles.Normal,
            new Color(0.82f, 0.9f, 1f, 1f),
            TextAlignmentOptions.Center);

        // OnClick → Chapter2BManager.BackToMenu()
        CreateButton(completePanelBM,
            "BackToMenuButton_BM",
            new Vector2(-105, -195),
            new Vector2(200, 48),
            "KEMBALI KE MENU",
            new Color(0.15f, 0.2f,
                0.28f, 1f),
            new Color(0.7f, 0.82f,
                1f, 1f));

        // OnClick → Chapter2BManager
        //           .CloseCompletePanel()
        CreateButton(completePanelBM,
            "CloseButton_BM",
            new Vector2(115, -195),
            new Vector2(200, 48),
            "TUTUP",
            new Color(0.08f, 0.45f,
                0.3f, 1f),
            new Color(0.9f, 0.97f,
                1f, 1f));

        completePanelBM.SetActive(false);

        // ── Done ──────────────────────────────────────────────────────────
        Undo.RegisterCreatedObjectUndo(
            canvasGO,
            "Build Chapter 2B UI Canvas");
        Selection.activeGameObject = canvasGO;

        Debug.Log(
            "✅ Chapter 2B UI Canvas built!\n\n" +
            "Assign in Chapter2BManager:\n" +
            "  headerLabel    → HeaderLabel\n" +
            "  statusText     → StatusText\n" +
            "  dayLabel       → DayLabel\n" +
            "  inputPanel     → InputPanel\n" +
            "  inputTitle     → InputTitle\n" +
            "  seed1Label     → Seed1Label\n" +
            "  seed2Label     → Seed2Label\n" +
            "  seed3Label     → Seed3Label\n" +
            "  meanLabel      → MeanLabel\n" +
            "  seed1ValueText → Seed1ValueText\n"+
            "  seed2ValueText → Seed2ValueText\n"+
            "  seed3ValueText → Seed3ValueText\n"+
            "  meanValueText  → MeanValueText\n" +
            "  submitButtonText → SubmitButton/Label\n"+
            "  prevDayButton  → PrevDayButton\n" +
            "  nextDayButton  → NextDayButton\n" +
            "  notebookButton → NotebookButton\n" +
            "  conclusionPanel → ConclusionPanel\n"+
            "  conclusionTitle → ConclusionTitle\n"+
            "  conclusionText → ConclusionText\n" +
            "  completePanelEN → CompletePanel_EN\n"+
            "  completePanelBM → CompletePanel_BM");
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    static GameObject CreatePanel(
        GameObject parent, string name,
        Vector2 pos, Vector2 size,
        Color color)
    {
        GameObject go = new GameObject(name,
            typeof(RectTransform),
            typeof(Image));
        go.transform.SetParent(
            parent.transform, false);
        RectTransform rt =
            go.GetComponent<RectTransform>();
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
        rt.anchorMin =
            new Vector2(0.5f, 0.5f);
        rt.anchorMax =
            new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        go.GetComponent<Image>().color = color;
        return go;
    }

    static TextMeshProUGUI CreateTMP(
        GameObject parent, string name,
        Vector2 pos, Vector2 size,
        string text, float fontSize,
        FontStyles style, Color color,
        TextAlignmentOptions align)
    {
        GameObject go = new GameObject(name,
            typeof(RectTransform),
            typeof(TextMeshProUGUI));
        go.transform.SetParent(
            parent.transform, false);
        RectTransform rt =
            go.GetComponent<RectTransform>();
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
        rt.anchorMin =
            new Vector2(0.5f, 0.5f);
        rt.anchorMax =
            new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        TextMeshProUGUI tmp =
            go.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontStyle = style;
        tmp.color = color;
        tmp.alignment = align;
        tmp.enableWordWrapping = true;
        tmp.overflowMode =
            TextOverflowModes.Overflow;
        return tmp;
    }

    static GameObject CreateButton(
        GameObject parent, string name,
        Vector2 pos, Vector2 size,
        string label, Color bgColor,
        Color textColor,
        float fontSize = 13f)
    {
        GameObject go = new GameObject(name,
            typeof(RectTransform),
            typeof(Image),
            typeof(Button));
        go.transform.SetParent(
            parent.transform, false);
        RectTransform rt =
            go.GetComponent<RectTransform>();
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
        rt.anchorMin =
            new Vector2(0.5f, 0.5f);
        rt.anchorMax =
            new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        go.GetComponent<Image>().color =
            bgColor;
        Button btn =
            go.GetComponent<Button>();
        ColorBlock cb = btn.colors;
        cb.normalColor = bgColor;
        cb.highlightedColor = bgColor * 1.3f;
        cb.pressedColor = bgColor * 0.75f;
        btn.colors = cb;
        GameObject labelGO =
            new GameObject("Label",
            typeof(RectTransform),
            typeof(TextMeshProUGUI));
        labelGO.transform.SetParent(
            go.transform, false);
        RectTransform lrt =
            labelGO.GetComponent
            <RectTransform>();
        lrt.anchorMin = Vector2.zero;
        lrt.anchorMax = Vector2.one;
        lrt.offsetMin = Vector2.zero;
        lrt.offsetMax = Vector2.zero;
        TextMeshProUGUI tmp =
            labelGO.GetComponent
            <TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = fontSize;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = textColor;
        tmp.alignment =
            TextAlignmentOptions.Center;
        tmp.enableWordWrapping = false;
        return go;
    }
}
#endif