/*using UnityEditor;
using UnityEngine;

public class CreateChapter3DUI
{
    [MenuItem("Tools/Create Chapter 3D UI Panel")]
    public static void CreatePanel3D()
    {
        GameObject uiCanvas =
            GameObject.Find("UI Canvas");

        if (uiCanvas == null)
        {
            Debug.LogError("UI Canvas not found.");
            return;
        }

        Transform panel3B =
            FindChildRecursive(
                uiCanvas.transform,
                "Panel3B");

        if (panel3B == null)
        {
            Debug.LogError(
                "Panel3B not found anywhere under UI Canvas.");
            return;
        }

        Transform parent = panel3B.parent;

        Transform oldPanel3D =
            FindChildRecursive(
                uiCanvas.transform,
                "Panel3D");

        if (oldPanel3D != null)
        {
            bool replace =
                EditorUtility.DisplayDialog(
                    "Panel3D already exists",
                    "Panel3D already exists. Replace it?",
                    "Replace",
                    "Cancel");

            if (!replace)
                return;

            Object.DestroyImmediate(
                oldPanel3D.gameObject);
        }

        // ─── Clone Panel3B as the base ─────────────

        GameObject panel3D =
            Object.Instantiate(
                panel3B.gameObject,
                parent);

        panel3D.name = "Panel3D";

        panel3D.transform.SetSiblingIndex(
            panel3B.GetSiblingIndex() + 1);

        // ─── Remove pieces 3D does not need ────────

        RemoveChild(panel3D.transform, "TubePLabel");
        RemoveChild(panel3D.transform, "TubeQLabel");
        RemoveChild(panel3D.transform, "InputPanel");

        // ─── Rename pieces 3D repurposes ───────────

        RenameChild(panel3D.transform,
            "IronRustText", "DropCountAText");

        RenameChild(panel3D.transform,
            "CopperRustText", "DropCountBText");

        RenameChild(panel3D.transform,
            "CompletePanel_EN", "ProceedButton3D_EN");

        RenameChild(panel3D.transform,
            "CompletePanel_BM", "ProceedButton3D_BM");

        panel3D.SetActive(false);

        EditorUtility.SetDirty(uiCanvas);
        AssetDatabase.SaveAssets();

        Debug.Log(
            "Panel3D created by cloning Panel3B. " +
            "Remember to manually: (1) retitle " +
            "HeaderLabel text, (2) clear default " +
            "text on DropCountAText/DropCountBText, " +
            "(3) check contents of ConclusionPanel " +
            "and ProceedButton3D_EN/BM, " +
            "(4) rewire ConfirmButton OnClick to " +
            "Chapter3DExperiment.OnConfirmPressed(), " +
            "(5) rewire both ProceedButton3D " +
            "OnClick to " +
            "Chapter3DExperiment.OnConclude3DPressed().");
    }

    private static Transform FindChildRecursive(
        Transform parent,
        string targetName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == targetName)
                return child;

            Transform result =
                FindChildRecursive(child, targetName);

            if (result != null)
                return result;
        }

        return null;
    }

    private static void RemoveChild(
        Transform root, string targetName)
    {
        Transform target =
            FindChildRecursive(root, targetName);

        if (target != null)
            Object.DestroyImmediate(
                target.gameObject);
        else
            Debug.LogWarning(
                targetName +
                " not found under clone — skipped.");
    }

    private static void RenameChild(
        Transform root,
        string targetName,
        string newName)
    {
        Transform target =
            FindChildRecursive(root, targetName);

        if (target != null)
            target.name = newName;
        else
            Debug.LogWarning(
                targetName +
                " not found under clone — skipped.");
    }
}*/