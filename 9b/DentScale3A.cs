using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DentScale3A : MonoBehaviour
{
    [Header("Scale Track")]
    [Tooltip("The vertical track the pointer " +
        "slides along. Its height in world/UI " +
        "units maps to 0 - maxDepthMm.")]
    public RectTransform scaleTrack;

    [Header("Pointer / Needle")]
    public RectTransform pointer;

    [Header("Tick Labels")]
    [Tooltip("Prefab for one tick label " +
        "(a small TMP text). Spawned once per " +
        "millimeter along the track.")]
    public GameObject tickLabelPrefab;
    public Transform tickLabelsRoot;

    [Header("Range")]
    [Tooltip("Top of the scale. Keep this small " +
        "(e.g. 10-20mm) so the scale reads as " +
        "zoomed-in, not a long ruler.")]
    public float maxDepthMm = 10f;
    public float tickIntervalMm = 1f;

    [Header("Style")]
    public Color pointerColor = new Color(
        0.85f, 0.2f, 0.2f);

    private void Awake()
    {
        Image pointerImage =
            pointer != null
                ? pointer.GetComponent<Image>()
                : null;

        if (pointerImage != null)
            pointerImage.color = pointerColor;

        BuildTicks();
        SetDepth(0f);
    }

    private void BuildTicks()
    {
        if (tickLabelsRoot == null
            || tickLabelPrefab == null
            || scaleTrack == null)
            return;

        foreach (Transform child in tickLabelsRoot)
            Destroy(child.gameObject);

        float trackHeight = scaleTrack.rect.height;
        int tickCount = Mathf.RoundToInt(
            maxDepthMm / tickIntervalMm);

        for (int i = 0; i <= tickCount; i++)
        {
            float depth = i * tickIntervalMm;
            float normalized = depth / maxDepthMm;
            float y = trackHeight * normalized;

            GameObject tick = Instantiate(
                tickLabelPrefab, tickLabelsRoot);

            RectTransform tickRT =
                tick.GetComponent<RectTransform>();
            if (tickRT != null)
                tickRT.anchoredPosition =
                    new Vector2(0f, y);

            TextMeshProUGUI text =
                tick.GetComponent<TextMeshProUGUI>();
            if (text != null)
                text.text = depth.ToString("F0");
        }
    }

    // Moves the pointer to the given depth (mm).
    // Call this once the ball has impacted and the
    // real dent depth is known.
    public void SetDepth(float depthMm)
    {
        if (pointer == null || scaleTrack == null)
            return;

        float clamped = Mathf.Clamp(
            depthMm, 0f, maxDepthMm);
        float normalized = clamped / maxDepthMm;
        float trackHeight = scaleTrack.rect.height;

        Vector2 pos = pointer.anchoredPosition;
        pos.y = trackHeight * normalized;
        pointer.anchoredPosition = pos;
    }
}