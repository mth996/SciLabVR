// ─────────────────────────────────────────────────────────────────────────────
// Chapter3CMeasurementBuilder.cs
// Place in any Editor folder.
// Menu: Tools > Build Chapter 3C Measurement Table
//
// Calibrated for rubber strip scale:
//   X 0.0696, Y 0.0519, Z 0.0500
//   Rotation Y 90 (length runs along world X)
//   Z = 0.05 world units = 5cm
//
// Canvas scale: 0.0002
//   So 1px = 0.0002m = 0.02cm
//   5cm = 0.05m / 0.0002 = 250px ruler width
//
// What it builds:
//   MeasurementTable (root)
//   ├── NaturalSnapPoint       ← right side, natural rubber rests here
//   ├── VulcanisedSnapPoint    ← left side, vulcanised rubber rests here
//   ├── NaturalRulerCanvas     ← world space, flat on table (right)
//   │   └── RulerPanel         ← assign to Chapter3CExperiment.naturalRulerUI
//   └── VulcanisedRulerCanvas  ← world space, flat on table (left)
//       └── RulerPanel         ← assign to Chapter3CExperiment.vulcanisedRulerUI
//
// After building:
//   1. Place MeasurementTable on your lab table
//   2. Position NaturalSnapPoint and VulcanisedSnapPoint
//      so strips sit between the two rulers
//   3. NaturalSnapPoint   → NaturalRubberStrip.startSnapPoint
//   4. VulcanisedSnapPoint → VulcanisedRubberStrip.startSnapPoint
//   5. NaturalRulerCanvas/RulerPanel   → Chapter3CExperiment.naturalRulerUI
//   6. VulcanisedRulerCanvas/RulerPanel → Chapter3CExperiment.vulcanisedRulerUI
// ─────────────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class Chapter3CMeasurementBuilder : MonoBehaviour
{
    [MenuItem("Tools/Build Chapter 3C Measurement Table")]
    public static void BuildMeasurementTable()
    {
        // ── Root ──────────────────────────────────────────────────────────
        GameObject root = new GameObject(
            "MeasurementTable");

        // ── Snap Points ───────────────────────────────────────────────────
        // Natural = right side (+X)
        GameObject naturalSnap = new GameObject(
            "NaturalSnapPoint");
        naturalSnap.transform.SetParent(
            root.transform, false);
        naturalSnap.transform.localPosition =
            new Vector3(0.03f, 0f, 0f);
        naturalSnap.transform.localRotation =
            Quaternion.Euler(0f, 90f, 0f);

        // Vulcanised = left side (-X)
        GameObject vulcSnap = new GameObject(
            "VulcanisedSnapPoint");
        vulcSnap.transform.SetParent(
            root.transform, false);
        vulcSnap.transform.localPosition =
            new Vector3(-0.03f, 0f, 0f);
        vulcSnap.transform.localRotation =
            Quaternion.Euler(0f, 90f, 0f);

        // ── Natural Ruler Canvas (right side) ─────────────────────────────
        GameObject naturalCanvas = BuildRulerCanvas(
            root,
            "NaturalRulerCanvas",
            new Vector3(0.17f, 0.001f, 0f),
            "Natural Rubber  (Tube A)",
            new Color(0.95f, 0.55f, 0.15f, 1f),
            new Color(0.06f, 0.10f, 0.18f, 0.97f),
            new Color(1f, 0.72f, 0.2f, 1f));

        // ── Vulcanised Ruler Canvas (left side) ───────────────────────────
        GameObject vulcCanvas = BuildRulerCanvas(
            root,
            "VulcanisedRulerCanvas",
            new Vector3(-0.17f, 0.001f, 0f),
            "Vulcanised Rubber  (Tube B)",
            new Color(0.2f, 0.88f, 0.5f, 1f),
            new Color(0.06f, 0.10f, 0.18f, 0.97f),
            new Color(0.25f, 0.95f, 0.55f, 1f));

        // ── Finalise ──────────────────────────────────────────────────────
        Undo.RegisterCreatedObjectUndo(
            root, "Build Chapter 3C Measurement Table");
        Selection.activeGameObject = root;

        Debug.Log(
            "✅ Measurement Table built!\n\n" +
            "Set rubber strip scale to:\n" +
            "  X 0.0696  Y 0.0519  Z 0.0500\n" +
            "  Rotation Y 90\n\n" +
            "Inspector assignments:\n" +
            "  NaturalSnapPoint → NaturalRubberStrip.startSnapPoint\n" +
            "  VulcanisedSnapPoint → VulcanisedRubberStrip.startSnapPoint\n" +
            "  NaturalRulerCanvas/RulerPanel → Chapter3CExperiment.naturalRulerUI\n" +
            "  VulcanisedRulerCanvas/RulerPanel → Chapter3CExperiment.vulcanisedRulerUI\n\n" +
            "RubberStrip inspector:\n" +
            "  originalLengthCm = 5\n" +
            "  scalePerCm = 0.01");
    }

    // ─────────────────────────────────────────────────────────────────────
    // Builds one complete ruler canvas
    // Canvas scale: 0.0002 (1px = 0.0002m)
    // Ruler inner width: 250px = 0.05m = 5cm exactly
    // ─────────────────────────────────────────────────────────────────────
    static GameObject BuildRulerCanvas(
        GameObject parent,
        string name,
        Vector3 localPos,
        string title,
        Color indicatorColor,
        Color bgColor,
        Color readingColor)
    {
        // ── Canvas ────────────────────────────────────────────────────────
        GameObject canvasGO =
            new GameObject(name);
        canvasGO.transform.SetParent(
            parent.transform, false);
        canvasGO.transform.localPosition =
            localPos;

        // Flat on table facing upward
        canvasGO.transform.localRotation =
            Quaternion.Euler(90f, 0f, 0f);

        // 0.0002 per pixel
        // 250px * 0.0002 = 0.05m = 5cm
        canvasGO.transform.localScale =
            Vector3.one * 0.0002f;

        Canvas canvas =
            canvasGO.AddComponent<Canvas>();
        canvas.renderMode =
            RenderMode.WorldSpace;

        CanvasScaler scaler =
            canvasGO.AddComponent<CanvasScaler>();
        scaler.dynamicPixelsPerUnit = 10f;

        // Canvas: 320 wide x 110 tall
        RectTransform canvasRT =
            canvasGO.GetComponent<RectTransform>();
        canvasRT.sizeDelta =
            new Vector2(320f, 110f);

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

        // ── Background ────────────────────────────────────────────────────
        CreateUIPanel(panelGO,
            "Background",
            new Vector2(0f, 0f),
            new Vector2(320f, 110f),
            bgColor);

        // Border
        GameObject border = CreateUIPanel(
            panelGO, "Border",
            new Vector2(0f, 0f),
            new Vector2(324f, 114f),
            new Color(0.3f, 0.6f, 1f, 0.35f));
        border.transform.SetAsFirstSibling();

        // ── Title ─────────────────────────────────────────────────────────
        CreateTMPLabel(panelGO, "TitleLabel",
            new Vector2(0f, 38f),
            new Vector2(300f, 20f),
            title,
            9f, FontStyles.Bold,
            new Color(0.78f, 0.88f, 1f, 1f),
            TextAlignmentOptions.Center);

        // ── Ruler Track ───────────────────────────────────────────────────
        // 270px wide container
        // 10px padding each side = 250px inner
        GameObject rulerTrack = CreateUIPanel(
            panelGO, "RulerTrack",
            new Vector2(0f, 8f),
            new Vector2(270f, 26f),
            new Color(0.04f, 0.07f, 0.14f, 1f));

        // ── Strip Indicator ───────────────────────────────────────────────
        // Starts at full 250px (5cm)
        // Shrinks when SetStripLength() called
        // Assign to: RulerUI.stripIndicator
        GameObject indicator = new GameObject(
            "StripIndicator",
            typeof(RectTransform),
            typeof(Image));
        indicator.transform.SetParent(
            rulerTrack.transform, false);

        RectTransform indRT =
            indicator.GetComponent<RectTransform>();
        indRT.anchorMin = new Vector2(0f, 0f);
        indRT.anchorMax = new Vector2(0f, 1f);
        indRT.pivot = new Vector2(0f, 0.5f);
        // Left padding 10px,
        // right = 10 + 250 = 260px = full 5cm
        indRT.offsetMin = new Vector2(10f, 3f);
        indRT.offsetMax = new Vector2(260f, -3f);

        indicator.GetComponent<Image>().color =
            indicatorColor;

        // ── Tick Marks Root ───────────────────────────────────────────────
        // Assign to: RulerUI.tickMarksParent
        GameObject tickRoot = new GameObject(
            "TickMarksRoot",
            typeof(RectTransform));
        tickRoot.transform.SetParent(
            rulerTrack.transform, false);
        RectTransform tickRT =
            tickRoot.GetComponent<RectTransform>();
        tickRT.anchorMin = Vector2.zero;
        tickRT.anchorMax = Vector2.one;
        tickRT.offsetMin = Vector2.zero;
        tickRT.offsetMax = Vector2.zero;

        // ── Cm Number Labels ──────────────────────────────────────────────
        // 0–5 labels spaced 50px apart
        // Starting at x = -125 + 10 = -115px
        GameObject labelRoot = new GameObject(
            "LabelRoot",
            typeof(RectTransform));
        labelRoot.transform.SetParent(
            panelGO.transform, false);

        RectTransform labelRootRT =
            labelRoot.GetComponent<RectTransform>();
        labelRootRT.anchoredPosition =
            new Vector2(0f, -10f);
        labelRootRT.sizeDelta =
            new Vector2(270f, 16f);
        labelRootRT.anchorMin =
            new Vector2(0.5f, 0.5f);
        labelRootRT.anchorMax =
            new Vector2(0.5f, 0.5f);
        labelRootRT.pivot =
            new Vector2(0.5f, 0.5f);

        for (int i = 0; i <= 5; i++)
        {
            float xPos = -125f + 10f
                + (i * 50f);
            CreateTMPLabel(labelRoot,
                "CmLabel_" + i,
                new Vector2(xPos, 0f),
                new Vector2(24f, 16f),
                i.ToString(),
                8f, FontStyles.Normal,
                new Color(0.72f, 0.82f,
                    1f, 0.9f),
                TextAlignmentOptions.Center);
        }

        // cm unit
        CreateTMPLabel(labelRoot, "CmUnit",
            new Vector2(148f, 0f),
            new Vector2(24f, 16f),
            "cm",
            7f, FontStyles.Normal,
            new Color(0.6f, 0.72f, 0.9f, 0.7f),
            TextAlignmentOptions.Left);

        // ── Reading Label ─────────────────────────────────────────────────
        // e.g. "3.5 cm"
        // Assign to: RulerUI.readingLabel
        CreateTMPLabel(panelGO,
            "ReadingLabel",
            new Vector2(0f, -38f),
            new Vector2(200f, 20f),
            "5.0 cm",
            11f, FontStyles.Bold,
            readingColor,
            TextAlignmentOptions.Center);

        // ── RulerUI Component ─────────────────────────────────────────────
        // rulerWidthUnits = 250px = 5cm
        // rulerOffsetX    = 10px left padding
        RulerUI rulerUI =
            panelGO.AddComponent<RulerUI>();

        rulerUI.rulerWidthUnits = 250f;
        rulerUI.maxLengthCm = 5f;
        rulerUI.rulerOffsetX = 10f;

        rulerUI.stripIndicator = indRT;

        rulerUI.readingLabel =
            panelGO.transform
            .Find("ReadingLabel")
            .GetComponent<TextMeshProUGUI>();

        rulerUI.tickMarksParent =
            tickRoot.transform;

        rulerUI.labelsParent =
            labelRoot.transform;

        return canvasGO;
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    static GameObject CreateUIPanel(
        GameObject parent, string name,
        Vector2 pos, Vector2 size, Color color)
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
        rt.pivot =
            new Vector2(0.5f, 0.5f);

        go.GetComponent<Image>().color = color;
        return go;
    }

    static TextMeshProUGUI CreateTMPLabel(
        GameObject parent, string name,
        Vector2 pos, Vector2 size, string text,
        float fontSize, FontStyles style,
        Color color,
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
        rt.pivot =
            new Vector2(0.5f, 0.5f);

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
