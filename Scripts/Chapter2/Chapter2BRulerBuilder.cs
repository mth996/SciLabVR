#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class Chapter2BRulerBuilder : MonoBehaviour
{
    [MenuItem(
        "Tools/Build Chapter 2B Rulers")]
    public static void BuildRulers()
    {
        GameObject root = new GameObject(
            "SeedlingRulers");

        // Build one ruler per seed
        // Positions match Seed1/2/3
        // X positions from scene
        BuildRuler(root,
            "Ruler_Seed1",
            new Vector3(0f, 0f, 0f),
            "Seedling 1",
            new Color(0.4f, 0.9f,
                0.55f, 0.85f),
            new Color(0.06f, 0.1f,
                0.16f, 0.97f),
            new Color(0.4f, 0.95f,
                0.55f, 1f));

        BuildRuler(root,
            "Ruler_Seed2",
            new Vector3(0.266f, 0f, 0f),
            "Seedling 2",
            new Color(0.3f, 0.75f,
                0.95f, 0.85f),
            new Color(0.06f, 0.1f,
                0.16f, 0.97f),
            new Color(0.3f, 0.8f,
                1f, 1f));

        BuildRuler(root,
            "Ruler_Seed3",
            new Vector3(0.505f, 0f, 0f),
            "Seedling 3",
            new Color(1f, 0.72f,
                0.25f, 0.85f),
            new Color(0.06f, 0.1f,
                0.16f, 0.97f),
            new Color(1f, 0.8f,
                0.3f, 1f));

        Undo.RegisterCreatedObjectUndo(
            root,
            "Build Chapter 2B Rulers");
        Selection.activeGameObject = root;

        Debug.Log(
            "✅ 3 Seedling Rulers built!\n\n" +
            "Setup:\n" +
            "1. Place SeedlingRulers root\n" +
            "   beside your petri dishes\n" +
            "2. Each RulerPanel has a\n" +
            "   VerticalRulerUI component\n" +
            "3. Assign to SeedlingStage:\n" +
            "   seed1.rulerUI → Ruler_Seed1/RulerPanel\n"+
            "   seed2.rulerUI → Ruler_Seed2/RulerPanel\n"+
            "   seed3.rulerUI → Ruler_Seed3/RulerPanel");
    }

    static void BuildRuler(
        GameObject parent,
        string name,
        Vector3 localPos,
        string title,
        Color indicatorColor,
        Color bgColor,
        Color readingColor)
    {
        // Canvas — vertical, facing player
        // Scale 0.001 so 300px = 0.3m = 30cm
        // which matches real seedling height
        GameObject canvasGO =
            new GameObject(name);
        canvasGO.transform.SetParent(
            parent.transform, false);
        canvasGO.transform.localPosition =
            localPos;

        // Vertical ruler stands upright
        // No X rotation — faces forward
        canvasGO.transform.localRotation =
            Quaternion.identity;
        canvasGO.transform.localScale =
            Vector3.one * 0.001f;

        Canvas canvas =
            canvasGO.AddComponent<Canvas>();
        canvas.renderMode =
            RenderMode.WorldSpace;
        canvasGO.AddComponent<CanvasScaler>()
            .dynamicPixelsPerUnit = 10f;

        // Canvas size: 120 wide x 380 tall
        // 380px * 0.001 = 0.38m = 38cm height
        // covers full seedling range
        RectTransform canvasRT =
            canvasGO.GetComponent<RectTransform>();
        canvasRT.sizeDelta =
            new Vector2(120f, 380f);

        // ── Ruler Panel ───────────────────────────────────────────────────
        GameObject panelGO = new GameObject(
            "RulerPanel",
            typeof(RectTransform));
        panelGO.transform.SetParent(
            canvasGO.transform, false);
        RectTransform panelRT =
            panelGO.GetComponent<RectTransform>();
        panelRT.anchorMin = Vector2.zero;
        panelRT.anchorMax = Vector2.one;
        panelRT.offsetMin = Vector2.zero;
        panelRT.offsetMax = Vector2.zero;

        // Background
        CreateUIPanel(panelGO, "Background",
            new Vector2(0, 0),
            new Vector2(120f, 380f),
            bgColor);

        // Border
        GameObject border = CreateUIPanel(
            panelGO, "Border",
            new Vector2(0, 0),
            new Vector2(124f, 384f),
            new Color(0.3f, 0.75f,
                0.4f, 0.4f));
        border.transform.SetAsFirstSibling();

        // Title label at top
        // Assign to: VerticalRulerUI
        //            .seedlingLabel
        CreateTMPLabel(panelGO,
            "SeedlingLabel",
            new Vector2(0f, 163f),
            new Vector2(110f, 22f),
            title,
            8f, FontStyles.Bold,
            new Color(0.78f, 0.9f, 1f, 1f),
            TextAlignmentOptions.Center);

        // ── Ruler Track ───────────────────────────────────────────────────
        // Width = 30px, height = 300px
        // 300px * 0.001 = 0.3m = 30cm
        // but represents 0-80mm on labels
        GameObject rulerTrack = CreateUIPanel(
            panelGO, "RulerTrack",
            new Vector2(-20f, -20f),
            new Vector2(30f, 300f),
            new Color(0.04f, 0.07f,
                0.14f, 1f));

        // ── Height Indicator ──────────────────────────────────────────────
        // Coloured bar grows from bottom
        // Assign to: VerticalRulerUI
        //            .heightIndicator
        GameObject indicator =
            new GameObject("HeightIndicator",
            typeof(RectTransform),
            typeof(Image));
        indicator.transform.SetParent(
            rulerTrack.transform, false);

        RectTransform indRT =
            indicator.GetComponent
            <RectTransform>();
        // Anchor to bottom
        indRT.anchorMin =
            new Vector2(0f, 0f);
        indRT.anchorMax =
            new Vector2(1f, 0f);
        indRT.pivot =
            new Vector2(0.5f, 0f);
        // Start at 0 height
        indRT.offsetMin =
            new Vector2(2f, 10f);
        indRT.offsetMax =
            new Vector2(-2f, 10f);

        indicator.GetComponent<Image>()
            .color = indicatorColor;

        // ── Tick Marks Root ───────────────────────────────────────────────
        // Assign to: VerticalRulerUI
        //            .tickMarksParent
        GameObject tickRoot =
            new GameObject("TickMarksRoot",
            typeof(RectTransform));
        tickRoot.transform.SetParent(
            rulerTrack.transform, false);
        RectTransform tickRT =
            tickRoot.GetComponent
            <RectTransform>();
        tickRT.anchorMin = Vector2.zero;
        tickRT.anchorMax = Vector2.one;
        tickRT.offsetMin = Vector2.zero;
        tickRT.offsetMax = Vector2.zero;

        // ── Reading Label ─────────────────────────────────────────────────
        // Assign to: VerticalRulerUI
        //            .readingLabel
        CreateTMPLabel(panelGO,
            "ReadingLabel",
            new Vector2(15f, -150f),
            new Vector2(80f, 22f),
            "0 mm",
            10f, FontStyles.Bold,
            readingColor,
            TextAlignmentOptions.Center);

        // ── VerticalRulerUI Component ─────────────────────────────────────
        VerticalRulerUI rulerUI =
            panelGO.AddComponent<VerticalRulerUI>();

        // 300px total height = 80mm max
        rulerUI.rulerHeightUnits = 300f;
        rulerUI.maxHeightMm = 80f;
        rulerUI.rulerOffsetY = 10f;

        rulerUI.heightIndicator = indRT;

        rulerUI.readingLabel =
            panelGO.transform
            .Find("ReadingLabel")
            .GetComponent<TextMeshProUGUI>();

        rulerUI.seedlingLabel =
            panelGO.transform
            .Find("SeedlingLabel")
            .GetComponent<TextMeshProUGUI>();

        rulerUI.tickMarksParent =
            tickRoot.transform;
    }

    static GameObject CreateUIPanel(
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

    static TextMeshProUGUI CreateTMPLabel(
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
        tmp.enableWordWrapping = false;
        tmp.overflowMode =
            TextOverflowModes.Overflow;
        return tmp;
    }
}
#endif