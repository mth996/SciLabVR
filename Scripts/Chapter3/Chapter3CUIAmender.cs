#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class Chapter3CUIAmender : MonoBehaviour
{
    [MenuItem("Tools/Amend Chapter 3C UI")]
    public static void AmendUI()
    {
        GameObject canvasGO =
            GameObject.Find("UI Canvas");
        if (canvasGO == null)
        {
            Debug.LogError(
                "Cannot find 'UI Canvas'.");
            return;
        }

        Transform bgPanel =
            canvasGO.transform.Find(
                "BackgroundPanel");
        if (bgPanel == null)
        {
            Debug.LogError(
                "Cannot find 'BackgroundPanel'.");
            return;
        }

        GameObject bg = bgPanel.gameObject;

        // ── Panel3C root ───────────────────────
        if (bg.transform.Find("Panel3C") != null)
        {
            Debug.Log(
                "Panel3C already exists, skipped.");
            return;
        }

        GameObject panel3C = CreatePanel(bg,
            "Panel3C",
            new Vector2(0, 0),
            new Vector2(780, 560),
            new Color(0f, 0f, 0f, 0f));
        panel3C.GetComponent<Image>().enabled
            = false;

        // ── Ruler Panel ────────────────────────
        // Assign to: Chapter3CExperiment
        //            .rulerPanel
        GameObject rulerPanel = CreatePanel(
            panel3C, "RulerPanel",
            new Vector2(0, 120),
            new Vector2(720, 160),
            new Color(0.04f, 0.07f, 0.12f, 0.95f));
        rulerPanel.SetActive(false);

        CreateTMP(rulerPanel, "RulerTitle",
            new Vector2(0, 58),
            new Vector2(680, 28),
            "MEASUREMENT — After Heating",
            12, FontStyles.Bold,
            new Color(0.4f, 0.78f, 1f, 1f),
            TextAlignmentOptions.Center);

        // ── Natural Ruler ──────────────────────
        GameObject naturalRulerGO = CreatePanel(
            rulerPanel, "NaturalRulerUI",
            new Vector2(-170, 10),
            new Vector2(300, 80),
            new Color(0.06f, 0.1f, 0.18f, 0.9f));

        CreateTMP(naturalRulerGO,
            "NaturalLabel",
            new Vector2(0, 28),
            new Vector2(280, 22),
            "Natural Rubber (Tube A)",
            10, FontStyles.Bold,
            new Color(0.95f, 0.6f, 0.2f, 1f),
            TextAlignmentOptions.Center);

        // Ruler bar background
        GameObject naturalRulerBar = CreatePanel(
            naturalRulerGO, "RulerBar",
            new Vector2(0, -5),
            new Vector2(260, 24),
            new Color(0.1f, 0.15f, 0.25f, 1f));

        // Strip indicator — assign to
        // RulerUI.stripIndicator
        GameObject naturalIndicator =
            CreatePanel(naturalRulerBar,
            "StripIndicator",
            new Vector2(0, 0),
            new Vector2(260, 20),
            new Color(0.9f, 0.5f, 0.15f, 0.85f));
        RectTransform natIndRT =
            naturalIndicator
            .GetComponent<RectTransform>();
        natIndRT.anchorMin =
            new Vector2(0f, 0.5f);
        natIndRT.anchorMax =
            new Vector2(0f, 0.5f);
        natIndRT.pivot = new Vector2(0f, 0.5f);
        natIndRT.anchoredPosition = Vector2.zero;

        // Tick marks parent
        GameObject naturalTicks = new GameObject(
            "TickMarks",
            typeof(RectTransform));
        naturalTicks.transform.SetParent(
            naturalRulerBar.transform, false);
        RectTransform natTicksRT =
            naturalTicks
            .GetComponent<RectTransform>();
        natTicksRT.anchorMin = Vector2.zero;
        natTicksRT.anchorMax = Vector2.one;
        natTicksRT.offsetMin = Vector2.zero;
        natTicksRT.offsetMax = Vector2.zero;

        // Reading label — assign to
        // RulerUI.readingLabel
        CreateTMP(naturalRulerGO,
            "ReadingLabel",
            new Vector2(0, -28),
            new Vector2(200, 20),
            "5.0 cm",
            11, FontStyles.Bold,
            new Color(0.95f, 0.75f, 0.3f, 1f),
            TextAlignmentOptions.Center);

        // Add RulerUI component
        RulerUI naturalRulerComp =
            naturalRulerGO.AddComponent<RulerUI>();
        naturalRulerComp.rulerWidthUnits = 260f;
        naturalRulerComp.stripIndicator =
            natIndRT;
        naturalRulerComp.readingLabel =
            naturalRulerGO.transform
            .Find("ReadingLabel")
            .GetComponent<TextMeshProUGUI>();
        naturalRulerComp.tickMarksParent =
            naturalTicks.transform;

        // ── Vulcanised Ruler ───────────────────
        GameObject vulcRulerGO = CreatePanel(
            rulerPanel, "VulcanisedRulerUI",
            new Vector2(170, 10),
            new Vector2(300, 80),
            new Color(0.06f, 0.1f, 0.18f, 0.9f));

        CreateTMP(vulcRulerGO,
            "VulcanisedLabel",
            new Vector2(0, 28),
            new Vector2(280, 22),
            "Vulcanised Rubber (Tube B)",
            10, FontStyles.Bold,
            new Color(0.3f, 0.88f, 0.6f, 1f),
            TextAlignmentOptions.Center);

        GameObject vulcRulerBar = CreatePanel(
            vulcRulerGO, "RulerBar",
            new Vector2(0, -5),
            new Vector2(260, 24),
            new Color(0.1f, 0.15f, 0.25f, 1f));

        GameObject vulcIndicator = CreatePanel(
            vulcRulerBar, "StripIndicator",
            new Vector2(0, 0),
            new Vector2(260, 20),
            new Color(0.2f, 0.85f, 0.5f, 0.85f));
        RectTransform vulcIndRT =
            vulcIndicator
            .GetComponent<RectTransform>();
        vulcIndRT.anchorMin =
            new Vector2(0f, 0.5f);
        vulcIndRT.anchorMax =
            new Vector2(0f, 0.5f);
        vulcIndRT.pivot = new Vector2(0f, 0.5f);
        vulcIndRT.anchoredPosition = Vector2.zero;

        GameObject vulcTicks = new GameObject(
            "TickMarks",
            typeof(RectTransform));
        vulcTicks.transform.SetParent(
            vulcRulerBar.transform, false);
        RectTransform vulcTicksRT =
            vulcTicks.GetComponent<RectTransform>();
        vulcTicksRT.anchorMin = Vector2.zero;
        vulcTicksRT.anchorMax = Vector2.one;
        vulcTicksRT.offsetMin = Vector2.zero;
        vulcTicksRT.offsetMax = Vector2.zero;

        CreateTMP(vulcRulerGO, "ReadingLabel",
            new Vector2(0, -28),
            new Vector2(200, 20),
            "5.0 cm",
            11, FontStyles.Bold,
            new Color(0.3f, 0.92f, 0.6f, 1f),
            TextAlignmentOptions.Center);

        RulerUI vulcRulerComp =
            vulcRulerGO.AddComponent<RulerUI>();
        vulcRulerComp.rulerWidthUnits = 260f;
        vulcRulerComp.stripIndicator = vulcIndRT;
        vulcRulerComp.readingLabel =
            vulcRulerGO.transform
            .Find("ReadingLabel")
            .GetComponent<TextMeshProUGUI>();
        vulcRulerComp.tickMarksParent =
            vulcTicks.transform;

        // ── Input Panel 3C ─────────────────────
        // Assign to: Chapter3CExperiment
        //            .inputPanel3C
        GameObject inputPanel3C = CreatePanel(
            panel3C, "InputPanel3C",
            new Vector2(0, -80),
            new Vector2(700, 200),
            new Color(0.04f, 0.08f, 0.14f, 0.96f));
        inputPanel3C.SetActive(false);

        CreateTMP(inputPanel3C, "InputTitle",
            new Vector2(0, 78),
            new Vector2(640, 28),
            "Enter the length of each rubber " +
            "strip after heating",
            12, FontStyles.Bold,
            new Color(0.5f, 0.82f, 1f, 1f),
            TextAlignmentOptions.Center);

        // Natural row
        CreateTMP(inputPanel3C,
            "NaturalInputLabel",
            new Vector2(-210, 28),
            new Vector2(200, 28),
            "Natural Rubber (Tube A)",
            11, FontStyles.Bold,
            new Color(0.95f, 0.6f, 0.2f, 1f),
            TextAlignmentOptions.Right);

        // OnClick → Chapter3CExperiment
        //           .OnNaturalMinus()
        CreateButton(inputPanel3C,
            "NaturalMinusButton",
            new Vector2(20, 28),
            new Vector2(44, 44), "−",
            new Color(0.15f, 0.22f, 0.32f, 1f),
            new Color(0.9f, 0.95f, 1f, 1f), 18);

        // Assign to: Chapter3CExperiment
        //            .naturalValueText
        CreateTMP(inputPanel3C,
            "NaturalValueText",
            new Vector2(95, 28),
            new Vector2(90, 44),
            "5.0 cm",
            14, FontStyles.Bold,
            new Color(0.95f, 0.7f, 0.3f, 1f),
            TextAlignmentOptions.Center);

        // OnClick → Chapter3CExperiment
        //           .OnNaturalPlus()
        CreateButton(inputPanel3C,
            "NaturalPlusButton",
            new Vector2(170, 28),
            new Vector2(44, 44), "+",
            new Color(0.08f, 0.45f, 0.75f, 1f),
            new Color(0.9f, 0.97f, 1f, 1f), 18);

        // Vulcanised row
        CreateTMP(inputPanel3C,
            "VulcanisedInputLabel",
            new Vector2(-210, -28),
            new Vector2(200, 28),
            "Vulcanised Rubber (Tube B)",
            11, FontStyles.Bold,
            new Color(0.3f, 0.88f, 0.6f, 1f),
            TextAlignmentOptions.Right);

        // OnClick → Chapter3CExperiment
        //           .OnVulcanisedMinus()
        CreateButton(inputPanel3C,
            "VulcanisedMinusButton",
            new Vector2(20, -28),
            new Vector2(44, 44), "−",
            new Color(0.15f, 0.22f, 0.32f, 1f),
            new Color(0.9f, 0.95f, 1f, 1f), 18);

        // Assign to: Chapter3CExperiment
        //            .vulcanisedValueText
        CreateTMP(inputPanel3C,
            "VulcanisedValueText",
            new Vector2(95, -28),
            new Vector2(90, 44),
            "5.0 cm",
            14, FontStyles.Bold,
            new Color(0.3f, 0.9f, 0.6f, 1f),
            TextAlignmentOptions.Center);

        // OnClick → Chapter3CExperiment
        //           .OnVulcanisedPlus()
        CreateButton(inputPanel3C,
            "VulcanisedPlusButton",
            new Vector2(170, -28),
            new Vector2(44, 44), "+",
            new Color(0.08f, 0.45f, 0.75f, 1f),
            new Color(0.9f, 0.97f, 1f, 1f), 18);

        // Divider
        CreatePanel(inputPanel3C, "Divider",
            new Vector2(0, -68),
            new Vector2(620, 2),
            new Color(0.3f, 0.55f, 0.9f, 0.3f));

        // Submit button
        // OnClick → Chapter3CExperiment
        //           .OnSubmit3CPressed()
        CreateButton(inputPanel3C,
            "SubmitButton3C",
            new Vector2(0, -90),
            new Vector2(280, 48),
            "SUBMIT OBSERVATION",
            new Color(0.08f, 0.5f, 0.82f, 1f),
            new Color(0.92f, 0.97f, 1f, 1f), 13);

        // ── Conclusion Panel 3C ────────────────
        // Assign to: Chapter3CExperiment
        //            .conclusionPanel3C
        GameObject conclusionPanel3C =
            CreatePanel(panel3C,
            "ConclusionPanel3C",
            new Vector2(0, -5),
            new Vector2(720, 300),
            new Color(0.03f, 0.06f, 0.10f,
                0.97f));

        CreatePanel(conclusionPanel3C,
            "LeftAccent",
            new Vector2(-348, 0),
            new Vector2(4, 280),
            new Color(0.2f, 0.75f, 0.4f, 1f));

        CreateTMP(conclusionPanel3C,
            "ConclusionTitle",
            new Vector2(0, 120),
            new Vector2(660, 30),
            "CONCLUSION",
            14, FontStyles.Bold,
            new Color(0.3f, 0.9f, 0.5f, 1f),
            TextAlignmentOptions.Center);

        // Assign to: Chapter3CExperiment
        //            .conclusionText3C
        CreateTMP(conclusionPanel3C,
            "ConclusionText3C",
            new Vector2(10, -10),
            new Vector2(640, 200),
            "",
            12, FontStyles.Normal,
            new Color(0.85f, 0.95f, 0.88f, 1f),
            TextAlignmentOptions.Center);

        conclusionPanel3C.SetActive(false);

        panel3C.SetActive(false);

        // ── Done ──────────────────────────────
        Undo.RegisterCreatedObjectUndo(
            panel3C, "Amend Chapter 3C UI");
        EditorUtility.SetDirty(canvasGO);

        Debug.Log(
            "✅ Panel3C built!\n\n" +
            "Assign in Chapter3Manager:\n" +
            "  panel3C → Panel3C\n\n" +
            "Assign in Chapter3CExperiment:\n" +
            "  rulerPanel → RulerPanel\n" +
            "  naturalRulerUI → NaturalRulerUI\n" +
            "  vulcanisedRulerUI → VulcanisedRulerUI\n"+
            "  inputPanel3C → InputPanel3C\n" +
            "  naturalValueText → NaturalValueText\n"+
            "  vulcanisedValueText → VulcanisedValueText\n"+
            "  conclusionPanel3C → ConclusionPanel3C\n"+
            "  conclusionText3C → ConclusionText3C\n\n"+
            "Wire OnClick:\n" +
            "  NaturalMinusButton → OnNaturalMinus()\n"+
            "  NaturalPlusButton → OnNaturalPlus()\n" +
            "  VulcanisedMinusButton → OnVulcanisedMinus()\n"+
            "  VulcanisedPlusButton → OnVulcanisedPlus()\n"+
            "  SubmitButton3C → OnSubmit3CPressed()");
    }

    // ── Helpers ───────────────────────────────

    static GameObject CreatePanel(
        GameObject parent, string name,
        Vector2 pos, Vector2 size, Color color)
    {
        GameObject go = new GameObject(name,
            typeof(RectTransform), typeof(Image));
        go.transform.SetParent(
            parent.transform, false);
        RectTransform rt =
            go.GetComponent<RectTransform>();
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        go.GetComponent<Image>().color = color;
        return go;
    }

    static TextMeshProUGUI CreateTMP(
        GameObject parent, string name,
        Vector2 pos, Vector2 size, string text,
        float fontSize, FontStyles style,
        Color color, TextAlignmentOptions align)
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
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
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
        Vector2 pos, Vector2 size, string label,
        Color bgColor, Color textColor,
        float fontSize = 13f)
    {
        GameObject go = new GameObject(name,
            typeof(RectTransform),
            typeof(Image), typeof(Button));
        go.transform.SetParent(
            parent.transform, false);
        RectTransform rt =
            go.GetComponent<RectTransform>();
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        go.GetComponent<Image>().color = bgColor;
        Button btn = go.GetComponent<Button>();
        ColorBlock cb = btn.colors;
        cb.normalColor = bgColor;
        cb.highlightedColor = bgColor * 1.3f;
        cb.pressedColor = bgColor * 0.75f;
        btn.colors = cb;
        GameObject labelGO = new GameObject(
            "Label", typeof(RectTransform),
            typeof(TextMeshProUGUI));
        labelGO.transform.SetParent(
            go.transform, false);
        RectTransform labelRT =
            labelGO.GetComponent<RectTransform>();
        labelRT.anchorMin = Vector2.zero;
        labelRT.anchorMax = Vector2.one;
        labelRT.offsetMin = Vector2.zero;
        labelRT.offsetMax = Vector2.zero;
        TextMeshProUGUI tmp =
            labelGO.GetComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = fontSize;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = textColor;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableWordWrapping = false;
        return go;
    }
}
#endif