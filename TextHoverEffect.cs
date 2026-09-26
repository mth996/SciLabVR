using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Attach to a chapter button GameObject.
/// Text is always visible. Underline appears on hover only.
/// Targets the LAST TMP child (the chapter name label).
/// </summary>
public class TextHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("The TMP text label to affect. Leave empty to auto-find last TMP child.")]
    public TextMeshProUGUI label;

    void Start()
    {
        // Auto-find: get ALL TMP children,
        // pick the LAST one (chapter name label)
        if (label == null)
        {
            TextMeshProUGUI[] all =
                GetComponentsInChildren<TextMeshProUGUI>(true);
            if (all.Length > 0)
                label = all[all.Length - 1];
        }

        if (label != null)
        {
            // Always visible, no underline at start
            label.enabled = true;
            label.fontStyle = FontStyles.Normal;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (label == null) return;
        label.fontStyle = FontStyles.Underline;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (label == null) return;
        label.fontStyle = FontStyles.Normal;
    }
}