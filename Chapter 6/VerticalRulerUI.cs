using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Attach to the RulerPanel inside
// each vertical ruler canvas
// Displays a vertical mm ruler
// with a height indicator
public class VerticalRulerUI : MonoBehaviour
{
    [Header("Ruler Settings")]
    // Total ruler height in UI units = maxMm
    public float rulerHeightUnits = 300f;
    public float maxHeightMm = 80f;
    // Bottom padding in pixels
    public float rulerOffsetY = 10f;

    [Header("UI References")]
    // The coloured bar showing seedling height
    public RectTransform heightIndicator;
    // Reading label e.g. "35 mm"
    public TextMeshProUGUI readingLabel;
    // Seedling name label
    public TextMeshProUGUI seedlingLabel;
    // Parent for tick marks
    public Transform tickMarksParent;

    [Header("Tick Colors")]
    public Color majorTickColor =
        new Color(0.9f, 0.95f, 1f, 1f);
    public Color minorTickColor =
        new Color(0.55f, 0.65f, 0.8f, 0.7f);

    private void Start()
    {
        GenerateTickMarks();
        SetSeedlingHeight(0f);
    }

    // ── Public API ────────────────────────────────────────────────────────

    public void SetSeedlingHeight(
        float heightMm)
    {
        heightMm = Mathf.Clamp(
            heightMm, 0f, maxHeightMm);

        UpdateIndicator(heightMm);
        UpdateReadingLabel(heightMm);
    }

    // ── Set Seedling Name (bilingual) ─────────────────────────────────────

    public void SetSeedlingName(string name)
    {
        if (seedlingLabel != null)
            seedlingLabel.text = name;
    }

    // ── Indicator Bar ─────────────────────────────────────────────────────

    private void UpdateIndicator(
        float heightMm)
    {
        if (heightIndicator == null) return;

        float ratio = heightMm / maxHeightMm;
        float pixelHeight =
            ratio * rulerHeightUnits;

        // Anchor bar from bottom
        heightIndicator.offsetMin =
            new Vector2(
                heightIndicator.offsetMin.x,
                rulerOffsetY);
        heightIndicator.offsetMax =
            new Vector2(
                heightIndicator.offsetMax.x,
                rulerOffsetY + pixelHeight);
    }

    // ── Reading Label ─────────────────────────────────────────────────────

    private void UpdateReadingLabel(
        float heightMm)
    {
        if (readingLabel == null) return;
        readingLabel.text =
            heightMm.ToString("F0") + " mm";
    }

    // ── Tick Mark Generation ──────────────────────────────────────────────

    // Major ticks every 10mm
    // Minor ticks every 5mm
    private void GenerateTickMarks()
    {
        if (tickMarksParent == null) return;

        // Clear existing
        for (int i =
            tickMarksParent.childCount - 1;
            i >= 0; i--)
            DestroyImmediate(
                tickMarksParent
                .GetChild(i).gameObject);

        float stepMm = 5f;
        int totalSteps = Mathf.RoundToInt(
            maxHeightMm / stepMm);

        for (int i = 0; i <= totalSteps; i++)
        {
            float mm = i * stepMm;
            bool isMajor = (i % 2 == 0);

            float yPos = rulerOffsetY
                + (mm / maxHeightMm)
                * rulerHeightUnits;

            GameObject tick = new GameObject(
                "Tick_" + mm + "mm",
                typeof(RectTransform),
                typeof(Image));
            tick.transform.SetParent(
                tickMarksParent, false);

            RectTransform tickRT =
                tick.GetComponent
                <RectTransform>();
            tickRT.anchorMin =
                new Vector2(0f, 0f);
            tickRT.anchorMax =
                new Vector2(0f, 0f);
            tickRT.pivot =
                new Vector2(0f, 0.5f);
            tickRT.anchoredPosition =
                new Vector2(0f, yPos);
            tickRT.sizeDelta = new Vector2(
                isMajor ? 20f : 12f,
                isMajor ? 2f : 1f);

            tick.GetComponent<Image>().color
                = isMajor
                ? majorTickColor
                : minorTickColor;

            // Label every 10mm
            if (isMajor)
            {
                GameObject label =
                    new GameObject("Label",
                    typeof(RectTransform),
                    typeof(TextMeshProUGUI));
                label.transform.SetParent(
                    tick.transform, false);

                RectTransform labelRT =
                    label.GetComponent
                    <RectTransform>();
                labelRT.anchoredPosition =
                    new Vector2(24f, 0f);
                labelRT.sizeDelta =
                    new Vector2(40f, 16f);

                TextMeshProUGUI tmp =
                    label.GetComponent
                    <TextMeshProUGUI>();
                tmp.text =
                    mm.ToString("F0");
                tmp.fontSize = 8f;
                tmp.color = new Color(
                    0.75f, 0.85f, 1f, 0.9f);
                tmp.alignment =
                    TextAlignmentOptions.Left;
                tmp.enableWordWrapping = false;
            }
        }
    }
}