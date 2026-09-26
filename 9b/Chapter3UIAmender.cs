// ─────────────────────────────────────────────────────────────────────────────
// Chapter3UIAmender.cs
// Place in any Editor folder in your Unity project.
// Menu: Tools > Amend Chapter 3 UI Canvas
//
// This script ONLY adds missing elements to your existing UI Canvas.
// It will NOT touch or move anything already in the scene.
//
// New elements added:
//   BackgroundPanel
//   ├── TimerText           (countdown display)
//   ├── FullCorrosionText   ("Full corrosion time: 60s")
//   └── InputPanel
//       ├── InputPanelBG
//       ├── InputTitle
//       ├── IronInputLabel
//       ├── IronMinusButton
//       ├── IronValueText
//       ├── IronPlusButton
//       ├── CopperInputLabel
//       ├── CopperMinusButton
//       ├── CopperValueText
//       └── CopperPlusButton
//       └── SubmitButton
//
// After running:
//   1. Assign new elements to Chapter3Manager inspector slots
//   2. Wire button OnClick events as listed below each button
// ─────────────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class Chapter3UIAmender : MonoBehaviour
{
    [MenuItem("Tools/Amend Chapter 3 UI Canvas")]
    public static void AmendUI()
    {
        // ── Find existing canvas ───────────────────────────────────────────
        GameObject canvasGO = GameObject.Find("UI Canvas");
        if (canvasGO == null)
        {
            Debug.LogError(
                "Could not find 'UI Canvas' in scene. " +
                "Make sure your canvas GameObject is named exactly 'UI Canvas'.");
            return;
        }

        // ── Find BackgroundPanel ───────────────────────────────────────────
        Transform bgPanel = canvasGO.transform.Find("BackgroundPanel");
        if (bgPanel == null)
        {
            Debug.LogError(
                "Could not find 'BackgroundPanel' inside 'UI Canvas'. " +
                "Make sure the panel is named exactly 'BackgroundPanel'.");
            return;
        }

        GameObject bg = bgPanel.gameObject;

        // ── Add TimerText ──────────────────────────────────────────────────
        // Assign to: Chapter3Manager.timerText
        if (bg.transform.Find("TimerText") == null)
        {
            CreateTMP(bg, "TimerText",
                new Vector2(0, 210), new Vector2(400, 50),
                "00:30",
                28, FontStyles.Bold,
                new Color(0.2f, 0.85f, 1f, 1f),
                TextAlignmentOptions.Center);

            Debug.Log("Added: TimerText");
        }
        else
            Debug.Log("Skipped (already exists): TimerText");

        // ── Add FullCorrosionText ──────────────────────────────────────────
        // Assign to: Chapter3Manager.fullCorrosionText
        if (bg.transform.Find("FullCorrosionText") == null)
        {
            CreateTMP(bg, "FullCorrosionText",
                new Vector2(0, 175), new Vector2(600, 30),
                "Full corrosion time for this nail: 60s",
                12, FontStyles.Normal,
                new Color(0.7f, 0.8f, 0.95f, 0.85f),
                TextAlignmentOptions.Center);

            Debug.Log("Added: FullCorrosionText");
        }
        else
            Debug.Log("Skipped (already exists): FullCorrosionText");

        // ── Add InputPanel ─────────────────────────────────────────────────
        // This whole panel is shown only during Phase 3 (inspection phase)
        // Assign panel GO to: Chapter3Manager.inputPanel
        if (bg.transform.Find("InputPanel") == null)
        {
            // Panel background
            GameObject inputPanel = CreatePanel(bg, "InputPanel",
                new Vector2(0, -20), new Vector2(700, 260),
                new Color(0.04f, 0.08f, 0.14f, 0.96f));

            // Title
            // ─ "Inspect both nails and estimate corrosion %"
            CreateTMP(inputPanel, "InputTitle",
                new Vector2(0, 100), new Vector2(640, 32),
                "Inspect both nails and estimate corrosion %",
                13, FontStyles.Bold,
                new Color(0.5f, 0.82f, 1f, 1f),
                TextAlignmentOptions.Center);

            // ── Iron row ───────────────────────────────────────────────────
            // Label
            CreateTMP(inputPanel, "IronInputLabel",
                new Vector2(-200, 48), new Vector2(220, 28),
                "Iron Nail (Tube P)",
                12, FontStyles.Bold,
                new Color(0.95f, 0.6f, 0.2f, 1f),
                TextAlignmentOptions.Right);

            // Minus button
            // OnClick → Chapter3Manager.OnIronMinus()
            CreateButton(inputPanel, "IronMinusButton",
                new Vector2(30, 48), new Vector2(44, 44),
                "−",
                new Color(0.15f, 0.22f, 0.32f, 1f),
                new Color(0.9f, 0.95f, 1f, 1f),
                18);

            // Value display
            // Assign to: Chapter3Manager.ironValueText
            CreateTMP(inputPanel, "IronValueText",
                new Vector2(100, 48), new Vector2(80, 44),
                "0%",
                16, FontStyles.Bold,
                new Color(0.95f, 0.7f, 0.3f, 1f),
                TextAlignmentOptions.Center);

            // Plus button
            // OnClick → Chapter3Manager.OnIronPlus()
            CreateButton(inputPanel, "IronPlusButton",
                new Vector2(170, 48), new Vector2(44, 44),
                "+",
                new Color(0.08f, 0.45f, 0.75f, 1f),
                new Color(0.9f, 0.97f, 1f, 1f),
                18);

            // ── Copper row ─────────────────────────────────────────────────
            // Label
            CreateTMP(inputPanel, "CopperInputLabel",
                new Vector2(-200, -10), new Vector2(220, 28),
                "Copper Nail (Tube Q)",
                12, FontStyles.Bold,
                new Color(0.3f, 0.88f, 0.6f, 1f),
                TextAlignmentOptions.Right);

            // Minus button
            // OnClick → Chapter3Manager.OnCopperMinus()
            CreateButton(inputPanel, "CopperMinusButton",
                new Vector2(30, -10), new Vector2(44, 44),
                "−",
                new Color(0.15f, 0.22f, 0.32f, 1f),
                new Color(0.9f, 0.95f, 1f, 1f),
                18);

            // Value display
            // Assign to: Chapter3Manager.copperValueText
            CreateTMP(inputPanel, "CopperValueText",
                new Vector2(100, -10), new Vector2(80, 44),
                "0%",
                16, FontStyles.Bold,
                new Color(0.3f, 0.9f, 0.6f, 1f),
                TextAlignmentOptions.Center);

            // Plus button
            // OnClick → Chapter3Manager.OnCopperPlus()
            CreateButton(inputPanel, "CopperPlusButton",
                new Vector2(170, -10), new Vector2(44, 44),
                "+",
                new Color(0.08f, 0.45f, 0.75f, 1f),
                new Color(0.9f, 0.97f, 1f, 1f),
                18);

            // Divider
            CreatePanel(inputPanel, "InputDivider",
                new Vector2(0, -52), new Vector2(620, 2),
                new Color(0.3f, 0.55f, 0.9f, 0.3f));

            // ── Submit button ──────────────────────────────────────────────
            // OnClick → Chapter3Manager.OnSubmitPressed()
            CreateButton(inputPanel, "SubmitButton",
                new Vector2(0, -95), new Vector2(280, 52),
                "SUBMIT OBSERVATION",
                new Color(0.08f, 0.5f, 0.82f, 1f),
                new Color(0.92f, 0.97f, 1f, 1f),
                13);

            inputPanel.SetActive(false); // hidden until Phase 3

            Debug.Log("Added: InputPanel with all children");
        }
        else
            Debug.Log("Skipped (already exists): InputPanel");

        // ── Done ───────────────────────────────────────────────────────────
        Undo.RegisterFullObjectHierarchyUndo(
            canvasGO, "Amend Chapter 3 UI");

        EditorUtility.SetDirty(canvasGO);

        Debug.Log(
            "✅ Chapter 3 UI amend complete!\n" +
            "Now assign in Chapter3Manager inspector:\n" +
            "  timerText         → TimerText\n" +
            "  fullCorrosionText → FullCorrosionText\n" +
            "  inputPanel        → InputPanel\n" +
            "  ironValueText     → InputPanel/IronValueText\n" +
            "  copperValueText   → InputPanel/CopperValueText\n\n" +
            "Wire OnClick events:\n" +
            "  IronMinusButton   → Chapter3Manager.OnIronMinus()\n" +
            "  IronPlusButton    → Chapter3Manager.OnIronPlus()\n" +
            "  CopperMinusButton → Chapter3Manager.OnCopperMinus()\n" +
            "  CopperPlusButton  → Chapter3Manager.OnCopperPlus()\n" +
            "  SubmitButton      → Chapter3Manager.OnSubmitPressed()");
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    static GameObject CreatePanel(GameObject parent, string name,
        Vector2 anchoredPos, Vector2 size, Color color)
    {
        GameObject go = new GameObject(name,
            typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent.transform, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = size;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        go.GetComponent<Image>().color = color;
        return go;
    }

    static TextMeshProUGUI CreateTMP(GameObject parent, string name,
        Vector2 anchoredPos, Vector2 size, string text,
        float fontSize, FontStyles style, Color color,
        TextAlignmentOptions alignment)
    {
        GameObject go = new GameObject(name,
            typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent.transform, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = size;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontStyle = style;
        tmp.color = color;
        tmp.alignment = alignment;
        tmp.enableWordWrapping = true;
        tmp.overflowMode = TextOverflowModes.Overflow;
        return tmp;
    }

    static GameObject CreateButton(GameObject parent, string name,
        Vector2 anchoredPos, Vector2 size, string label,
        Color bgColor, Color textColor, float fontSize = 13f)
    {
        GameObject go = new GameObject(name,
            typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(parent.transform, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = size;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        Image img = go.GetComponent<Image>();
        img.color = bgColor;

        Button btn = go.GetComponent<Button>();
        ColorBlock cb = btn.colors;
        cb.normalColor = bgColor;
        cb.highlightedColor = bgColor * 1.3f;
        cb.pressedColor = bgColor * 0.75f;
        cb.selectedColor = bgColor;
        btn.colors = cb;

        GameObject labelGO = new GameObject("Label",
            typeof(RectTransform), typeof(TextMeshProUGUI));
        labelGO.transform.SetParent(go.transform, false);

        RectTransform labelRT = labelGO.GetComponent<RectTransform>();
        labelRT.anchorMin = Vector2.zero;
        labelRT.anchorMax = Vector2.one;
        labelRT.offsetMin = Vector2.zero;
        labelRT.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = labelGO.GetComponent<TextMeshProUGUI>();
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
