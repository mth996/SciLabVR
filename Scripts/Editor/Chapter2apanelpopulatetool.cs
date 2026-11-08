using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

// ─────────────────────────────────────────────────────────────
// Chapter2APanelPopulateTool
//
// Companion to Chapter2UIRestructureTool. Run THAT one first.
// This one builds the two UI elements 2A needs that don't
// exist yet, places them inside "Panel_2A_Only", styles them
// to match your existing UI where possible, and automatically
// drags them into Chapter2AExperiment's Inspector fields.
//
// Creates:
//   Panel_2A_Only
//   ├── BookCountText           (TextMeshProUGUI)
//   └── ProceedToSolidButton    (Button)
//       └── ProceedToSolidButtonText  (TextMeshProUGUI)
//
// HOW TO USE:
// 1. Make sure Chapter2UIRestructureTool.cs has already been
//    run once (so "Panel_2A_Only" exists under BackgroundPanel).
// 2. Put this file in the same "Editor" folder as the other
//    tool (e.g. Assets/Editor/Chapter2APanelPopulateTool.cs).
// 3. Select "UI Canvas 2B" in the Hierarchy (same as before).
// 4. Menu bar → Tools > SciLab VR > Populate 2A Panel Elements
// 5. Check the Console for what was created and what got
//    auto-wired into Chapter2AExperiment. Anything it could
//    NOT auto-wire (e.g. if it can't find the component in
//    the scene) will say so — drag those two fields manually.
// 6. Reposition/restyle the new Text/Button by hand afterward
//    if the auto-placed position or copied style doesn't suit
//    your layout — this tool gets you working defaults, not
//    final polish.
//
// Safe to run more than once — if BookCountText or
// ProceedToSolidButton already exist under Panel_2A_Only, it
// skips recreating them instead of duplicating.
// ─────────────────────────────────────────────────────────────

public static class Chapter2APanelPopulateTool
{
    [MenuItem("Tools/SciLab VR/Populate 2A Panel Elements")]
    private static void Populate()
    {
        Transform panel2AOnly = FindPanel2AOnly();

        if (panel2AOnly == null)
        {
            EditorUtility.DisplayDialog(
                "Populate 2A Panel",
                "Could not find 'Panel_2A_Only'.\n\n" +
                "Run 'Tools > SciLab VR > Restructure " +
                "Chapter 2 UI' first — that creates this " +
                "group — then run this tool again.",
                "OK");
            return;
        }

        // Look for something to copy styling from.
        TMP_Text styleSourceText = FindStyleSourceText();
        Button styleSourceButton = FindStyleSourceButton();

        TMP_Text bookCountText =
            GetOrCreateBookCountText(
                panel2AOnly, styleSourceText);

        Button proceedButton;
        TMP_Text proceedButtonText;
        GetOrCreateProceedButton(
            panel2AOnly, styleSourceButton, styleSourceText,
            out proceedButton, out proceedButtonText);

        int wiredCount = WireIntoExperimentScript(
            bookCountText, proceedButton, proceedButtonText);

        EditorUtility.SetDirty(panel2AOnly.gameObject);

        Debug.Log(
            "[Chapter2APanelPopulateTool] Done.\n" +
            "  Created/found BookCountText: " +
            (bookCountText != null) + "\n" +
            "  Created/found ProceedToSolidButton: " +
            (proceedButton != null) + "\n" +
            $"  Auto-wired {wiredCount}/3 fields on " +
            "Chapter2AExperiment (bookCountText, " +
            "proceedToSolidButton, " +
            "proceedToSolidButtonText).");

        EditorUtility.DisplayDialog(
            "Populate 2A Panel",
            "Created the 2A book count text and Proceed " +
            "to Solid button inside Panel_2A_Only, and " +
            $"auto-wired {wiredCount}/3 fields on " +
            "Chapter2AExperiment.\n\n" +
            "Reposition/restyle by hand if needed, then " +
            "save the scene.",
            "OK");
    }

    // ─── Finding existing objects ───────────────────────────

    private static Transform FindPanel2AOnly()
    {
        GameObject[] all =
            UnityEngine.Object.FindObjectsByType<GameObject>(
                FindObjectsSortMode.None);
        foreach (var go in all)
        {
            if (go.name == "Panel_2A_Only")
                return go.transform;
        }
        return null;
    }

    // Tries to find StatusText (inside Shared_2A2B, or
    // directly under BackgroundPanel if restructure hasn't
    // run) to copy font/size/color from.
    private static TMP_Text FindStyleSourceText()
    {
        GameObject[] all =
            UnityEngine.Object.FindObjectsByType<GameObject>(
                FindObjectsSortMode.None);
        foreach (var go in all)
        {
            if (go.name == "StatusText")
            {
                TMP_Text t = go.GetComponent<TMP_Text>();
                if (t != null) return t;
            }
        }
        return null;
    }

    // Tries to find the "retry" button to copy visual style
    // (Image sprite/color) from.
    private static Button FindStyleSourceButton()
    {
        GameObject[] all =
            UnityEngine.Object.FindObjectsByType<GameObject>(
                FindObjectsSortMode.None);
        foreach (var go in all)
        {
            if (go.name == "retry")
            {
                Button b = go.GetComponent<Button>();
                if (b != null) return b;
            }
        }
        return null;
    }

    // ─── Creating the book count text ───────────────────────

    private static TMP_Text GetOrCreateBookCountText(
        Transform parent, TMP_Text styleSource)
    {
        Transform existing = parent.Find("BookCountText");
        if (existing != null)
            return existing.GetComponent<TMP_Text>();

        GameObject textObj = new GameObject(
            "BookCountText", typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(
            textObj, "Create BookCountText");

        RectTransform rt =
            textObj.GetComponent<RectTransform>();
        rt.SetParent(parent, false);
        rt.anchorMin = new Vector2(0.5f, 1f);
        rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        // Placed near the top of the panel, below where a
        // header/status line typically sits. Adjust the Y
        // offset by hand if it overlaps StatusText.
        rt.anchoredPosition = new Vector2(0f, -140f);
        rt.sizeDelta = new Vector2(400f, 60f);

        TextMeshProUGUI tmp =
            textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "Books placed: 0";
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 32f;
        tmp.color = Color.white;

        if (styleSource != null)
        {
            tmp.font = styleSource.font;
            tmp.fontSize = styleSource.fontSize;
            tmp.color = styleSource.color;
        }

        return tmp;
    }

    // ─── Creating the Proceed to Solid button ───────────────

    private static void GetOrCreateProceedButton(
        Transform parent,
        Button styleSourceButton,
        TMP_Text styleSourceText,
        out Button createdButton,
        out TMP_Text createdText)
    {
        Transform existing =
            parent.Find("ProceedToSolidButton");
        if (existing != null)
        {
            createdButton = existing.GetComponent<Button>();
            createdText = existing.GetComponentInChildren<
                TMP_Text>();
            return;
        }

        GameObject buttonObj = new GameObject(
            "ProceedToSolidButton",
            typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(
            buttonObj, "Create ProceedToSolidButton");

        RectTransform rt =
            buttonObj.GetComponent<RectTransform>();
        rt.SetParent(parent, false);
        rt.anchorMin = new Vector2(0.5f, 0f);
        rt.anchorMax = new Vector2(0.5f, 0f);
        rt.pivot = new Vector2(0.5f, 0f);
        // Placed near the bottom-center of the panel.
        // Adjust by hand to match your layout.
        rt.anchoredPosition = new Vector2(0f, 120f);
        rt.sizeDelta = new Vector2(360f, 90f);

        Image img = buttonObj.AddComponent<Image>();
        Button btn = buttonObj.AddComponent<Button>();

        if (styleSourceButton != null)
        {
            Image sourceImg =
                styleSourceButton.GetComponent<Image>();
            if (sourceImg != null)
            {
                img.sprite = sourceImg.sprite;
                img.color = sourceImg.color;
                img.type = sourceImg.type;
            }

            // Copy color tint states (normal/highlighted/
            // pressed) so it visually matches other buttons.
            btn.colors = styleSourceButton.colors;
        }
        else
        {
            img.color = new Color(0.2f, 0.5f, 0.9f, 1f);
        }

        GameObject textObj = new GameObject(
            "ProceedToSolidButtonText",
            typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(
            textObj, "Create ProceedToSolidButtonText");

        RectTransform textRt =
            textObj.GetComponent<RectTransform>();
        textRt.SetParent(buttonObj.transform, false);
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp =
            textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "PROCEED TO SOLID";
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 28f;
        tmp.color = Color.white;

        if (styleSourceText != null)
        {
            tmp.font = styleSourceText.font;
        }

        // Button starts hidden — Chapter2AExperiment already
        // calls SetActive(false) on this in OnUnlocked() and
        // only shows it after the hollow structure collapses.
        buttonObj.SetActive(false);

        createdButton = btn;
        createdText = tmp;
    }

    // ─── Auto-wiring into Chapter2AExperiment ───────────────

    private static int WireIntoExperimentScript(
        TMP_Text bookCountText,
        Button proceedButton,
        TMP_Text proceedButtonText)
    {
        // Chapter2AExperiment is a MonoBehaviour we can't
        // reference by type here without knowing this Editor
        // script sits in the same assembly — it does, since
        // Unity compiles Editor scripts after runtime scripts
        // and Editor code can see them. If this line fails to
        // compile, your Chapter2AExperiment.cs may be in a
        // custom Assembly Definition the Editor folder can't
        // see — in that case, wire the 3 fields by hand.

        Chapter2AExperiment exp =
            UnityEngine.Object.FindFirstObjectByType<
                Chapter2AExperiment>();

        if (exp == null)
        {
            Debug.LogWarning(
                "[Chapter2APanelPopulateTool] Could not find " +
                "a Chapter2AExperiment component in the scene " +
                "— wire bookCountText, proceedToSolidButton, " +
                "and proceedToSolidButtonText fields manually.");
            return 0;
        }

        SerializedObject so = new SerializedObject(exp);
        int wired = 0;

        SerializedProperty bookCountProp =
            so.FindProperty("bookCountText");
        if (bookCountProp != null && bookCountText != null)
        {
            bookCountProp.objectReferenceValue =
                bookCountText;
            wired++;
        }

        SerializedProperty proceedButtonProp =
            so.FindProperty("proceedToSolidButton");
        if (proceedButtonProp != null &&
            proceedButton != null)
        {
            proceedButtonProp.objectReferenceValue =
                proceedButton.gameObject;
            wired++;
        }

        SerializedProperty proceedTextProp =
            so.FindProperty("proceedToSolidButtonText");
        if (proceedTextProp != null &&
            proceedButtonText != null)
        {
            proceedTextProp.objectReferenceValue =
                proceedButtonText;
            wired++;
        }

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(exp);

        // Also wire the button's OnClick() to call
        // OnProceedToSolidPressed(), if it isn't already set.
        if (proceedButton != null)
        {
            UnityEditor.Events.UnityEventTools
                .AddPersistentListener(
                    proceedButton.onClick,
                    exp.OnProceedToSolidPressed);
        }

        return wired;
    }
}