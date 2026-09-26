// ─────────────────────────────────────────────────────────────────────────────
// Chapter3UIBuilder.cs
// Place this file in any Editor folder in your Unity project.
// Menu: Tools > Build Chapter 3 UI Canvas
//
// What it builds:
//   UI Canvas (World Space, 800x600, scale 0.001)
//   └── Background Panel (dark glass)
//       ├── StatusText
//       ├── IronRustText
//       ├── CopperRustText
//       ├── ConfirmButton
//       ├── NotebookButton
//       ├── ConclusionPanel
//       │   └── ConclusionText
//       ├── CompletePanel_EN
//       │   ├── CompleteTitleEN
//       │   ├── CompleteBodyEN
//       │   ├── BackToMenuButton_EN
//       │   └── CloseButton_EN
//       └── CompletePanel_BM
//           ├── CompleteTitleBM
//           ├── CompleteBodyBM
//           ├── BackToMenuButton_BM
//           └── CloseButton_BM
//
// After running:
//   1. Assign the canvas GameObject a position in world space in front of the player
//   2. Drag each UI element into Chapter3Manager inspector slots
//   3. Wire button OnClick events (OnConfirmPressed, OnNotebookButtonPressed etc.)
// ─────────────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class Chapter3UIBuilder : MonoBehaviour
{
    [MenuItem("Tools/Build Chapter 3 UI Canvas")]
    public static void BuildUI()
    {
        // ── Root Canvas ────────────────────────────────────────────────────
        GameObject canvasGO = new GameObject("UI Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.dynamicPixelsPerUnit = 10;

        canvasGO.AddComponent<GraphicRaycaster>();

        RectTransform canvasRect = canvasGO.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(800, 600);
        canvasRect.localScale = Vector3.one * 0.001f;

        // ── Background Panel ───────────────────────────────────────────────
        GameObject bgPanel = CreatePanel(canvasGO, "BackgroundPanel",
            new Vector2(0, 0), new Vector2(780, 580),
            new Color(0.05f, 0.08f, 0.12f, 0.92f));

        // Thin border effect via outline image
        GameObject border = CreatePanel(bgPanel, "Border",
            new Vector2(0, 0), new Vector2(784, 584),
            new Color(0.2f, 0.6f, 0.9f, 0.35f));
        border.GetComponent<RectTransform>().SetAsFirstSibling();

        // ── Header Label ───────────────────────────────────────────────────
        CreateTMP(bgPanel, "HeaderLabel",
            new Vector2(0, 255), new Vector2(720, 40),
            "CORROSION RESISTANCE EXPERIMENT",
            18, FontStyles.Bold,
            new Color(0.4f, 0.75f, 1f, 1f),
            TextAlignmentOptions.Center);

        // Divider line
        GameObject divider = CreatePanel(bgPanel, "Divider",
            new Vector2(0, 228), new Vector2(700, 2),
            new Color(0.3f, 0.6f, 1f, 0.4f));

        // ── StatusText ─────────────────────────────────────────────────────
        // Mapped to: Chapter3Manager.statusText
        CreateTMP(bgPanel, "StatusText",
            new Vector2(0, 170), new Vector2(680, 80),
            "Place the iron nail in tube P\nand copper nail in tube Q.",
            16, FontStyles.Normal,
            new Color(0.9f, 0.95f, 1f, 1f),
            TextAlignmentOptions.Center);

        // ── Nail Status Row ────────────────────────────────────────────────
        // Tube P label
        CreateTMP(bgPanel, "TubePLabel",
            new Vector2(-180, 95), new Vector2(280, 30),
            "TUBE P  —  Iron Nail",
            12, FontStyles.Bold,
            new Color(0.9f, 0.5f, 0.2f, 1f),
            TextAlignmentOptions.Center);

        // Tube Q label
        CreateTMP(bgPanel, "TubeQLabel",
            new Vector2(180, 95), new Vector2(280, 30),
            "TUBE Q  —  Copper Nail",
            12, FontStyles.Bold,
            new Color(0.3f, 0.85f, 0.6f, 1f),
            TextAlignmentOptions.Center);

        // ── IronRustText ───────────────────────────────────────────────────
        // Mapped to: Chapter3Manager.ironRustText
        CreateTMP(bgPanel, "IronRustText",
            new Vector2(-180, 65), new Vector2(280, 35),
            "",
            14, FontStyles.Bold,
            new Color(0.95f, 0.55f, 0.15f, 1f),
            TextAlignmentOptions.Center);

        // ── CopperRustText ─────────────────────────────────────────────────
        // Mapped to: Chapter3Manager.copperRustText
        CreateTMP(bgPanel, "CopperRustText",
            new Vector2(180, 65), new Vector2(280, 35),
            "",
            14, FontStyles.Bold,
            new Color(0.3f, 0.9f, 0.6f, 1f),
            TextAlignmentOptions.Center);

        // Divider 2
        CreatePanel(bgPanel, "Divider2",
            new Vector2(0, 30), new Vector2(700, 2),
            new Color(0.3f, 0.6f, 1f, 0.25f));

        // ── ConfirmButton ──────────────────────────────────────────────────
        // Mapped to: Chapter3Manager.confirmButton
        // OnClick → Chapter3Manager.OnConfirmPressed()
        GameObject confirmBtn = CreateButton(bgPanel, "ConfirmButton",
            new Vector2(0, -30), new Vector2(320, 55),
            "CONFIRM RESULTS",
            new Color(0.1f, 0.55f, 0.95f, 1f),
            new Color(0.9f, 0.97f, 1f, 1f));
        confirmBtn.SetActive(false); // hidden by default

        // ── NotebookButton ─────────────────────────────────────────────────
        // Mapped to: Chapter3Manager.notebookButton
        // OnClick → Chapter3Manager.OnNotebookButtonPressed()
        GameObject notebookBtn = CreateButton(bgPanel, "NotebookButton",
            new Vector2(0, -100), new Vector2(260, 45),
            "📓  NOTEBOOK",
            new Color(0.15f, 0.22f, 0.32f, 1f),
            new Color(0.7f, 0.85f, 1f, 1f));
        // Add border tint to distinguish
        notebookBtn.GetComponent<Image>().color =
            new Color(0.12f, 0.28f, 0.48f, 0.9f);
        notebookBtn.SetActive(false); // hidden until unlocked

        // ── ConclusionPanel ────────────────────────────────────────────────
        // Mapped to: Chapter3Manager.conclusionPanel
        GameObject conclusionPanel = CreatePanel(bgPanel,
            "ConclusionPanel",
            new Vector2(0, -5), new Vector2(720, 320),
            new Color(0.03f, 0.06f, 0.10f, 0.97f));

        // Coloured left border accent
        GameObject conclusionAccent = CreatePanel(conclusionPanel,
            "LeftAccent",
            new Vector2(-348, 0), new Vector2(4, 300),
            new Color(0.2f, 0.75f, 0.4f, 1f));

        CreateTMP(conclusionPanel, "ConclusionTitle",
            new Vector2(0, 130), new Vector2(660, 35),
            "CONCLUSION",
            15, FontStyles.Bold,
            new Color(0.3f, 0.9f, 0.5f, 1f),
            TextAlignmentOptions.Center);

        // Mapped to: Chapter3Manager.conclusionText
        CreateTMP(conclusionPanel, "ConclusionText",
            new Vector2(10, -20), new Vector2(640, 220),
            "The hypothesis is accepted.\n\nIron nail corroded when exposed to water and air.\nCopper nail showed no corrosion,\nproving alloys resist corrosion better than pure metals.",
            13, FontStyles.Normal,
            new Color(0.85f, 0.95f, 0.88f, 1f),
            TextAlignmentOptions.Center);

        conclusionPanel.SetActive(false); // hidden by default

        // ── CompletePanel_EN ───────────────────────────────────────────────
        // Mapped to: Chapter3Manager — driven by ChapterOverlayUI
        GameObject completePanelEN = CreatePanel(bgPanel,
            "CompletePanel_EN",
            new Vector2(0, 0), new Vector2(740, 540),
            new Color(0.04f, 0.07f, 0.11f, 0.98f));

        CreateTMP(completePanelEN, "CompleteTitleEN",
            new Vector2(0, 195), new Vector2(680, 50),
            "✓  EXPERIMENT COMPLETE",
            20, FontStyles.Bold,
            new Color(0.3f, 0.95f, 0.55f, 1f),
            TextAlignmentOptions.Center);

        CreateTMP(completePanelEN, "CompleteStarsEN",
            new Vector2(0, 145), new Vector2(400, 35),
            "★ ★ ★",
            22, FontStyles.Bold,
            new Color(1f, 0.85f, 0.2f, 1f),
            TextAlignmentOptions.Center);

        CreateTMP(completePanelEN, "CompleteBodyEN",
            new Vector2(0, 30), new Vector2(640, 180),
            "9B: Corrosion Resistance\n\nIron nail (pure metal) → Corroded\nCopper nail → No corrosion\n\nAlloys resist corrosion better than pure metals.",
            13, FontStyles.Normal,
            new Color(0.82f, 0.9f, 1f, 1f),
            TextAlignmentOptions.Center);

        // Back to Menu button
        // OnClick → ChapterOverlayUI.BackToMenu()
        CreateButton(completePanelEN, "BackToMenuButton_EN",
            new Vector2(-105, -185), new Vector2(200, 48),
            "BACK TO MENU",
            new Color(0.15f, 0.2f, 0.28f, 1f),
            new Color(0.7f, 0.82f, 1f, 1f));

        // Close button
        // OnClick → ChapterOverlayUI.CloseCompletePanel()
        CreateButton(completePanelEN, "CloseButton_EN",
            new Vector2(115, -185), new Vector2(200, 48),
            "CLOSE",
            new Color(0.08f, 0.45f, 0.75f, 1f),
            new Color(0.9f, 0.97f, 1f, 1f));

        completePanelEN.SetActive(false);

        // ── CompletePanel_BM ───────────────────────────────────────────────
        GameObject completePanelBM = CreatePanel(bgPanel,
            "CompletePanel_BM",
            new Vector2(0, 0), new Vector2(740, 540),
            new Color(0.04f, 0.07f, 0.11f, 0.98f));

        CreateTMP(completePanelBM, "CompleteTitleBM",
            new Vector2(0, 195), new Vector2(680, 50),
            "✓  EKSPERIMEN SELESAI",
            20, FontStyles.Bold,
            new Color(0.3f, 0.95f, 0.55f, 1f),
            TextAlignmentOptions.Center);

        CreateTMP(completePanelBM, "CompleteStarsBM",
            new Vector2(0, 145), new Vector2(400, 35),
            "★ ★ ★",
            22, FontStyles.Bold,
            new Color(1f, 0.85f, 0.2f, 1f),
            TextAlignmentOptions.Center);

        CreateTMP(completePanelBM, "CompleteBodyBM",
            new Vector2(0, 30), new Vector2(640, 180),
            "9B: Ketahanan Kakisan\n\nPaku besi (logam tulen) → Berkarat\nPaku tembaga → Tiada karat\n\nAloi lebih tahan kakisan berbanding logam tulen.",
            13, FontStyles.Normal,
            new Color(0.82f, 0.9f, 1f, 1f),
            TextAlignmentOptions.Center);

        // OnClick → ChapterOverlayUI.BackToMenu()
        CreateButton(completePanelBM, "BackToMenuButton_BM",
            new Vector2(-105, -185), new Vector2(200, 48),
            "KEMBALI KE MENU",
            new Color(0.15f, 0.2f, 0.28f, 1f),
            new Color(0.7f, 0.82f, 1f, 1f));

        // OnClick → ChapterOverlayUI.CloseCompletePanel()
        CreateButton(completePanelBM, "CloseButton_BM",
            new Vector2(115, -185), new Vector2(200, 48),
            "TUTUP",
            new Color(0.08f, 0.45f, 0.75f, 1f),
            new Color(0.9f, 0.97f, 1f, 1f));

        completePanelBM.SetActive(false);

        // ── Finalise ───────────────────────────────────────────────────────
        Undo.RegisterCreatedObjectUndo(canvasGO, "Build Chapter 3 UI");
        Selection.activeGameObject = canvasGO;

        Debug.Log("Chapter 3 UI Canvas built successfully!\n" +
                  "Now assign each child to Chapter3Manager inspector slots.");
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    static GameObject CreatePanel(GameObject parent, string name,
        Vector2 anchoredPos, Vector2 size, Color color)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
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
        GameObject go = new GameObject(name, typeof(RectTransform),
            typeof(TextMeshProUGUI));
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
        Color bgColor, Color textColor)
    {
        // Button background
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

        // Hover tint via ColorBlock
        Button btn = go.GetComponent<Button>();
        ColorBlock cb = btn.colors;
        cb.normalColor = bgColor;
        cb.highlightedColor = bgColor * 1.25f;
        cb.pressedColor = bgColor * 0.8f;
        cb.selectedColor = bgColor;
        btn.colors = cb;

        // Label child
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
        tmp.fontSize = 13;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = textColor;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableWordWrapping = false;

        return go;
    }
}
#endif
