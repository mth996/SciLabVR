using UnityEngine;
using UnityEditor;

// ─────────────────────────────────────────────────────────────
// Chapter2UIRestructureTool
//
// One-click Editor tool that restructures the existing
// "UI Canvas 2B" > "BackgroundPanel" hierarchy into three groups:
//
//   BackgroundPanel
//   ├── Shared_2A2B      <- elements BOTH 2A and 2B use
//   ├── Panel_2B_Only    <- elements ONLY 2B uses
//   └── Panel_2A_Only    <- elements ONLY 2A uses (empty, for you
//                           to build 2A's new elements into)
//
// This ONLY reparents existing GameObjects (SetParent), which
// preserves their instance IDs. Any script field already
// pointing at these objects (Chapter2BExperiment, etc.) will
// keep working exactly as before — nothing is destroyed or
// recreated.
//
// HOW TO USE:
// 1. Copy this file into an "Editor" folder anywhere under
//    Assets (e.g. Assets/Editor/Chapter2UIRestructureTool.cs).
//    It MUST be inside a folder literally named "Editor" or
//    Unity will throw a compile error (editor-only scripts
//    can't ship in a build).
// 2. Open your Ch2 scene, select "UI Canvas 2B" in the
//    Hierarchy (the tool searches under whatever you select —
//    or under the whole scene if nothing is selected).
// 3. Menu bar → Tools > SciLab VR > Restructure Chapter 2 UI
// 4. Check the Console for a full report of what moved and
//    what it could NOT find (so you can fix names/hierarchy
//    by hand for anything unusual).
// 5. Save the scene (Ctrl+S) once you're happy with the result.
//
// You can safely run this MULTIPLE times — objects already in
// the right place are skipped, not moved again or duplicated.
// ─────────────────────────────────────────────────────────────

public static class Chapter2UIRestructureTool
{
    // Adjust these three lists if your actual object names
    // differ slightly, or if you want to reclassify something.

    private static readonly string[] SharedNames =
    {
        "Border",
        "HeaderLabel",
        "HeaderDivider",
        "StatusText",
        "Divider2",
        "NotebookButton",
        "quit",
        "retry",
        "retry (1)",
        "ConclusionPanel",
        "CompletePanel_EN",
        "CompletePanel_BM",
        "exit panel",
        "exit panel malay"
    };

    private static readonly string[] Panel2BOnlyNames =
    {
        "DayLabel",
        "InputPanel",
        "PrevDayButton",
        "NextDayButton"
    };

    // 2A currently has no existing UI objects to move — this
    // list is here so you can add names to it later if you
    // build 2A elements somewhere else first and want the tool
    // to relocate them into Panel_2A_Only automatically.
    private static readonly string[] Panel2AOnlyNames =
    {
        // e.g. "BookCountText", "ProceedToSolidButton"
    };

    [MenuItem("Tools/SciLab VR/Restructure Chapter 2 UI")]
    private static void Restructure()
    {
        GameObject backgroundPanel = FindBackgroundPanel();

        if (backgroundPanel == null)
        {
            EditorUtility.DisplayDialog(
                "Chapter 2 UI Restructure",
                "Could not find a GameObject named " +
                "'BackgroundPanel' under 'UI Canvas 2B' " +
                "(or under your current selection).\n\n" +
                "Select 'UI Canvas 2B' in the Hierarchy first, " +
                "then run this again.",
                "OK");
            return;
        }

        Transform bg = backgroundPanel.transform;

        Transform sharedGroup =
            GetOrCreateGroup(bg, "Shared_2A2B");
        Transform panel2BGroup =
            GetOrCreateGroup(bg, "Panel_2B_Only");
        Transform panel2AGroup =
            GetOrCreateGroup(bg, "Panel_2A_Only");

        int movedShared = MoveNamedChildren(
            bg, sharedGroup, SharedNames);
        int moved2B = MoveNamedChildren(
            bg, panel2BGroup, Panel2BOnlyNames);
        int moved2A = MoveNamedChildren(
            bg, panel2AGroup, Panel2AOnlyNames);

        EditorUtility.SetDirty(backgroundPanel);

        Debug.Log(
            "[Chapter2UIRestructureTool] Done.\n" +
            $"  Moved into Shared_2A2B: {movedShared}\n" +
            $"  Moved into Panel_2B_Only: {moved2B}\n" +
            $"  Moved into Panel_2A_Only: {moved2A}\n" +
            "Check above for any '[NOT FOUND]' warnings — " +
            "those objects need to be moved/renamed by hand.");

        EditorUtility.DisplayDialog(
            "Chapter 2 UI Restructure",
            $"Moved {movedShared} shared, {moved2B} " +
            $"2B-only, and {moved2A} 2A-only objects.\n\n" +
            "See the Console for full details, then save " +
            "the scene.",
            "OK");
    }

    private static GameObject FindBackgroundPanel()
    {
        // Prefer searching under the current selection, if any.
        GameObject selected = Selection.activeGameObject;
        if (selected != null)
        {
            Transform found =
                FindDeepChild(selected.transform,
                    "BackgroundPanel");
            if (found != null) return found.gameObject;

            // Selection might already BE BackgroundPanel.
            if (selected.name == "BackgroundPanel")
                return selected;
        }

        // Fall back to searching the whole loaded scene.
        GameObject[] all =
            UnityEngine.Object.FindObjectsByType<GameObject>(
                FindObjectsSortMode.None);
        foreach (var go in all)
        {
            if (go.name == "BackgroundPanel")
                return go;
        }

        return null;
    }

    private static Transform FindDeepChild(
        Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform result = FindDeepChild(child, name);
            if (result != null) return result;
        }
        return null;
    }

    private static Transform GetOrCreateGroup(
        Transform parent, string groupName)
    {
        Transform existing = parent.Find(groupName);
        if (existing != null) return existing;

        GameObject groupObj = new GameObject(
            groupName, typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(
            groupObj, "Create " + groupName);

        RectTransform rt =
            groupObj.GetComponent<RectTransform>();
        rt.SetParent(parent, false);

        // Stretch to fill the parent so children that expect
        // to be positioned relative to BackgroundPanel still
        // look correct after moving one level deeper.
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;

        return rt;
    }

    private static int MoveNamedChildren(
        Transform sourceParent,
        Transform destGroup,
        string[] names)
    {
        int movedCount = 0;

        foreach (string name in names)
        {
            Transform child = sourceParent.Find(name);

            if (child == null)
            {
                Debug.LogWarning(
                    "[Chapter2UIRestructureTool] " +
                    $"[NOT FOUND] '{name}' was not found " +
                    "directly under BackgroundPanel — check " +
                    "spelling/casing or move it manually.");
                continue;
            }

            if (child == destGroup) continue;

            // Already inside the correct destination group?
            if (child.parent == destGroup)
            {
                movedCount++; // count as handled, skip re-move
                continue;
            }

            Undo.SetTransformParent(
                child, destGroup,
                "Restructure Chapter 2 UI");

            // worldPositionStays = true keeps it visually in
            // the same screen position after reparenting.
            child.SetParent(destGroup, true);

            movedCount++;
        }

        return movedCount;
    }
}