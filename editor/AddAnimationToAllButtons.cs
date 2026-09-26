#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/// <summary>
/// Editor tool — put this script in Assets/Editor folder.
/// Use: top menu → Tools → Add Animation To All Buttons
///      top menu → Tools → Add Hover Effect To Chapter Buttons
/// </summary>
public class AddAnimationToAllButtons : EditorWindow
{
    // ── Exact names of your 5 chapter button GameObjects ──
    static readonly string[] chapterButtonNames = new string[]
    {
        "Body Health",
        "Support and growth",
        "Industrial chem",
        "Medicince",
        "Force and motion"
    };

    // ─────────────────────────────────────────────────────
    [MenuItem("Tools/Add Animation To All Buttons")]
    static void AddToAll()
    {
        Button[] allButtons = FindObjectsOfType<Button>(true);
        int count = 0;

        foreach (Button btn in allButtons)
        {
            if (btn.GetComponent<ButtonAnimation>() == null)
            {
                btn.gameObject.AddComponent<ButtonAnimation>();
                count++;
            }
        }

        Debug.Log($"[ButtonAnimation] Added to {count} buttons!");
    }

    // ─────────────────────────────────────────────────────
    [MenuItem("Tools/Add Hover Effect To Chapter Buttons")]
    static void AddHoverToChapterButtons()
    {
        int count = 0;

        foreach (string btnName in chapterButtonNames)
        {
            // Search in ALL loaded scenes (finds both EN and Malay versions)
            GameObject[] allObjects = FindObjectsOfType<GameObject>(true);

            foreach (GameObject go in allObjects)
            {
                if (go.name == btnName)
                {
                    if (go.GetComponent<TextHoverEffect>() == null)
                    {
                        go.AddComponent<TextHoverEffect>();
                        EditorUtility.SetDirty(go);
                        count++;
                        Debug.Log($"[TextHoverEffect] Added to: {go.name} (scene: {go.scene.name})");
                    }
                    else
                    {
                        Debug.Log($"[TextHoverEffect] Already exists on: {go.name} — skipped.");
                    }
                }
            }
        }

        Debug.Log($"[TextHoverEffect] Done! Added to {count} chapter button(s).");
    }
}
#endif