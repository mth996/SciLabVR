using UnityEngine;

public class ExperimentRetryPanel : MonoBehaviour
{
    [Header("Panels")]
    public GameObject retryPanel;
    public GameObject experimentSelectionPanel;

    // ─── Hide retry panel ─────────────────────

    public void HideRetryPanel()
    {
        if (retryPanel != null)
            retryPanel.SetActive(false);
    }

    // ─── Show experiment selection ────────────

    public void OnChapterSelectPressed()
    {
        if (retryPanel != null)
            retryPanel.SetActive(false);

        if (experimentSelectionPanel != null)
            experimentSelectionPanel
                .SetActive(true);
    }
}