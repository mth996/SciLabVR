using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RulerUI : MonoBehaviour
{
    [Header("Ruler Settings")]
    // Total ruler width in UI units = 5cm
    public float rulerWidthUnits = 400f;
    public float maxLengthCm = 5f;
    // Left offset of ruler track inside canvas
    public float rulerOffsetX = 10f;

    [Header("UI References")]
    // The coloured bar showing strip length
    public RectTransform stripIndicator;
    // The reading label e.g. "3.5 cm"
    public TextMeshProUGUI readingLabel;
    // Parent for generated tick marks
    public Transform tickMarksParent;
    // Parent for cm number labels
    // (already built by editor script,
    //  but can be regenerated if needed)
    public Transform labelsParent;

    [Header("Tick Settings")]
    public Color majorTickColor =
        new Color(0.9f, 0.9f, 1f, 1f);
    public Color minorTickColor =
        new Color(0.55f, 0.62f, 0.8f, 0.7f);

    private void Start()
    {
        GenerateTickMarks();
        // Start at full 5cm
        SetStripLength(5f);
    }

    // ── Public API ────────────────────────────────────────────────────────

    // Call this from Chapter3CExperiment
    // when strip snaps back after heating
    public void SetStripLength(float lengthCm)
    {
        lengthCm = Mathf.Clamp(
            lengthCm, 0f, maxLengthCm);

        UpdateIndicator(lengthCm);
        UpdateReadingLabel(lengthCm);
    }

    // ── Indicator Bar ─────────────────────────────────────────────────────

    private void UpdateIndicator(float lengthCm)
    {
        if (stripIndicator == null) return;

        // Calculate pixel width for this length
        float ratio = lengthCm / maxLengthCm;
        float pixelWidth =
            ratio * rulerWidthUnits;

        // Set indicator width
        // anchored from left edge of track
        stripIndicator.offsetMin =
            new Vector2(rulerOffsetX,
                stripIndicator.offsetMin.y);
        stripIndicator.offsetMax =
            new Vector2(
                rulerOffsetX + pixelWidth,
                stripIndicator.offsetMax.y);
    }

    // ── Reading Label ─────────────────────────────────────────────────────

    private void UpdateReadingLabel(
        float lengthCm)
    {
        if (readingLabel == null) return;
        readingLabel.text =
            lengthCm.ToString("F1") + " cm";
    }

    // ── Tick Mark Generation ──────────────────────────────────────────────

    // Generates major ticks every 1cm
    // and minor ticks every 0.5cm
    private void GenerateTickMarks()
    {
        if (tickMarksParent == null) return;

        // Clear any existing ticks
        for (int i =
            tickMarksParent.childCount - 1;
            i >= 0; i--)
        {
            DestroyImmediate(
                tickMarksParent
                .GetChild(i).gameObject);
        }

        float stepCm = 0.5f;
        int totalSteps = Mathf.RoundToInt(
            maxLengthCm / stepCm);

        // Pixels per cm
        float pxPerCm =
            rulerWidthUnits / maxLengthCm;

        for (int i = 0; i <= totalSteps; i++)
        {
            float cm = i * stepCm;
            bool isMajor = (i % 2 == 0);

            float xPos = rulerOffsetX
                + (cm / maxLengthCm)
                * rulerWidthUnits;

            // Tick mark image
            GameObject tick = new GameObject(
                "Tick_" + cm.ToString("F1"),
                typeof(RectTransform),
                typeof(Image));
            tick.transform.SetParent(
                tickMarksParent, false);

            RectTransform tickRT =
                tick.GetComponent<RectTransform>();

            // Anchor to bottom-left of track
            tickRT.anchorMin =
                new Vector2(0f, 0f);
            tickRT.anchorMax =
                new Vector2(0f, 0f);
            tickRT.pivot =
                new Vector2(0.5f, 0f);

            // Position along ruler
            tickRT.anchoredPosition =
                new Vector2(xPos, 0f);

            // Size — major ticks taller
            tickRT.sizeDelta = new Vector2(
                isMajor ? 2f : 1f,
                isMajor ? 22f : 13f);

            tick.GetComponent<Image>().color =
                isMajor
                ? majorTickColor
                : minorTickColor;
        }
    }
}
