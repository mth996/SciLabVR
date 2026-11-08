using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleLineGraph : MonoBehaviour
{
    [Header("References")]
    public RectTransform plotArea;
    public GameObject dotPrefab;
    public GameObject linePrefab;
    public GameObject xLabelPrefab;
    public GameObject yLabelPrefab;
    public Transform xLabelsRoot;
    public Transform yLabelsRoot;

    [Header("Style")]
    public Color lineColor = new Color(0.2f, 0.5f, 0.3f);
    public Color dotColor = new Color(0.15f, 0.35f, 0.2f);
    public float dotSize = 12f;
    public float lineThickness = 3f;

    private List<GameObject> spawned = new List<GameObject>();

    public void Plot(float[] values, string[] xLabels, int yAxisDivisions = 5)
    {
        Clear();

        if (values == null || values.Length == 0 || plotArea == null)
            return;

        float maxValue = 0f;
        foreach (float v in values)
            if (v > maxValue) maxValue = v;

        float axisMax = RoundUpToNice(maxValue);

        float width = plotArea.rect.width;
        float height = plotArea.rect.height;

        int count = values.Length;
        Vector2[] points = new Vector2[count];

        for (int i = 0; i < count; i++)
        {
            float x = count > 1 ? (width * i / (count - 1)) : 0f;
            float normalizedY = axisMax > 0 ? (values[i] / axisMax) : 0f;
            float y = height * Mathf.Clamp01(normalizedY);
            points[i] = new Vector2(x, y);
        }

        for (int i = 0; i < count - 1; i++)
            DrawLine(points[i], points[i + 1]);

        for (int i = 0; i < count; i++)
            DrawDot(points[i]);

        if (xLabelsRoot != null && xLabelPrefab != null && xLabels != null)
        {
            for (int i = 0; i < xLabels.Length && i < count; i++)
            {
                GameObject label = Instantiate(xLabelPrefab, xLabelsRoot);
                RectTransform rt = label.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(points[i].x, 0f);
                TextMeshProUGUI text = label.GetComponent<TextMeshProUGUI>();
                if (text != null) text.text = xLabels[i];
                spawned.Add(label);
            }
        }

        if (yLabelsRoot != null && yLabelPrefab != null)
        {
            for (int i = 0; i <= yAxisDivisions; i++)
            {
                float value = axisMax * i / yAxisDivisions;
                GameObject label = Instantiate(yLabelPrefab, yLabelsRoot);
                RectTransform rt = label.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(0f, height * i / yAxisDivisions);
                TextMeshProUGUI text = label.GetComponent<TextMeshProUGUI>();
                if (text != null) text.text = value.ToString("F0");
                spawned.Add(label);
            }
        }
    }

    private void DrawDot(Vector2 point)
    {
        GameObject dot = dotPrefab != null
            ? Instantiate(dotPrefab, plotArea)
            : CreateDefaultRect(plotArea, dotColor);

        RectTransform rt = dot.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(dotSize, dotSize);
        rt.anchoredPosition = point;
        spawned.Add(dot);
    }

    private void DrawLine(Vector2 from, Vector2 to)
    {
        GameObject line = linePrefab != null
            ? Instantiate(linePrefab, plotArea)
            : CreateDefaultRect(plotArea, lineColor);

        RectTransform rt = line.GetComponent<RectTransform>();

        Vector2 diff = to - from;
        float length = diff.magnitude;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        rt.sizeDelta = new Vector2(length, lineThickness);
        rt.anchoredPosition = from + diff * 0.5f;
        rt.localRotation = Quaternion.Euler(0f, 0f, angle);

        spawned.Add(line);
    }

    private GameObject CreateDefaultRect(RectTransform parent, Color color)
    {
        GameObject go = new GameObject("GraphElement", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 0f);
        rt.anchorMax = new Vector2(0f, 0f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        Image img = go.GetComponent<Image>();
        img.color = color;

        return go;
    }

    private float RoundUpToNice(float value)
    {
        if (value <= 0f) return 10f;

        float magnitude = Mathf.Pow(10, Mathf.Floor(Mathf.Log10(value)));
        float normalized = value / magnitude;

        float niceNormalized;
        if (normalized <= 1f) niceNormalized = 1f;
        else if (normalized <= 2f) niceNormalized = 2f;
        else if (normalized <= 5f) niceNormalized = 5f;
        else niceNormalized = 10f;

        return niceNormalized * magnitude;
    }

    public void Clear()
    {
        foreach (var obj in spawned)
            if (obj != null) Destroy(obj);
        spawned.Clear();
    }
}