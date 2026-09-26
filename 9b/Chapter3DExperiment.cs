using System.Collections;
using UnityEngine;
using TMPro;


public class Chapter3DExperiment : MonoBehaviour
{
    // ─── 3D Begin Panels ────────────────────────────────────────────

    [Header("3D Begin Panels")]
    public GameObject begin3DPanelEnglish;
    public GameObject begin3DPanelMalay;

    // ─── 3D Intro Panels ────────────────────────────────────────────

    [Header("3D Intro Panels")]
    public GameObject intro3DPanelEnglish;
    public GameObject intro3DPanelMalay;

    // ─── Beaker A — Ethanoic Acid ─────────────────────────────────────

    [Header("Beaker A — Ethanoic Acid")]
    public GameObject latexLiquidA;
    public GameObject coagulatedLatexA;
    public Transform dropperTriggerA;

    // ─── Beaker B — Ammonia ───────────────────────────────────────────

    [Header("Beaker B — Ammonia Solution")]
    public GameObject latexLiquidB;
    public Transform dropperTriggerB;

    // ─── Droppers ─────────────────────────────────────────────────────

    [Header("Droppers")]
    public Dropper3D acidDropper;
    public Dropper3D ammoniaDropper;

    // ─── Reaction Settings ────────────────────────────────────────────

    [Header("Reaction Settings")]
    public float reactionDuration = 15f;
    public int dropsRequired = 10;

    // ─── UI References ────────────────────────────────────────────────

    [Header("UI")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI dropCountAText;
    public TextMeshProUGUI dropCountBText;
    public GameObject conclusionPanel3D;
    public TextMeshProUGUI conclusionTitle3D;
    public TextMeshProUGUI conclusionText3D;
    public GameObject proceedButton3DEN;
    public GameObject proceedButton3DBM;
    public GameObject confirmButton3D;
    public TextMeshProUGUI confirmButtonText3D;

    // ─── State ────────────────────────────────────────────────────────

    private int dropCountA = 0;
    private int dropCountB = 0;
    private bool reactionAStarted = false;
    private bool reactionAComplete = false;
    private bool reactionBComplete = false;
    private bool experimentComplete = false;
    private bool _isEnglish = true;

    private Vector3 liquidAOriginalScale;
    private Vector3 liquidBOriginalScale;

    // ─── Lifecycle ────────────────────────────────────────────────────

    private void Start()
    {
        if (LanguageManager.Instance != null)
            _isEnglish = LanguageManager
                .Instance.IsEnglish();

        HideAll3DPanels();
    }

    // ─── Begin / Intro Flow ───────────────────────────────────────────

    // Called by Chapter3Manager.Trigger3DBegin()
    public void OnUnlocked()
    {
        HideAll3DPanels();

        if (_isEnglish)
            SetActive(begin3DPanelEnglish, true);
        else
            SetActive(begin3DPanelMalay, true);

        Debug.Log("3D Begin panel shown.");
    }

    public void On3DBeginPressed()
    {
        HideAll3DPanels();

        if (_isEnglish)
            SetActive(intro3DPanelEnglish, true);
        else
            SetActive(intro3DPanelMalay, true);

        Debug.Log("3D Intro panel shown.");
    }

    public void On3DIntroNextPressed()
    {
        HideAll3DPanels();
        Chapter3Manager.Instance
            .StartExperiment3D();
        Debug.Log(
            "3D intro done. Manager activating 3D.");
    }

    private void HideAll3DPanels()
    {
        SetActive(begin3DPanelEnglish, false);
        SetActive(begin3DPanelMalay, false);
        SetActive(intro3DPanelEnglish, false);
        SetActive(intro3DPanelMalay, false);
    }

    // ─── Init (actual experiment setup) ───────────────────────────────

    // Called by Chapter3Manager.StartExperiment3D()
    public void StartExperiment()
    {
        _isEnglish = LanguageManager.Instance != null
            && LanguageManager.Instance.IsEnglish();

        // Store original scales
        if (latexLiquidA != null)
            liquidAOriginalScale =
                latexLiquidA.transform.localScale;
        if (latexLiquidB != null)
            liquidBOriginalScale =
                latexLiquidB.transform.localScale;

        // Hide coagulated latex at start
        if (coagulatedLatexA != null)
            coagulatedLatexA.transform.localScale =
                Vector3.zero;

        // Reset drop counts
        dropCountA = 0;
        dropCountB = 0;
        reactionAStarted = false;
        reactionAComplete = false;
        reactionBComplete = false;
        experimentComplete = false;

        // Hide conclusion panel
        if (conclusionPanel3D != null)
            conclusionPanel3D.SetActive(false);
        if (confirmButton3D != null)
            confirmButton3D.SetActive(false);

        // Set UI texts
        RefreshUI();

        if (statusText != null)
            statusText.text =
                Chapter3Texts.PlaceDropper3D();

        // Enable droppers
        if (acidDropper != null)
            acidDropper.EnableDropper();
        if (ammoniaDropper != null)
            ammoniaDropper.EnableDropper();

        // Set language buttons
        if (proceedButton3DEN != null)
            proceedButton3DEN.SetActive(_isEnglish);
        if (proceedButton3DBM != null)
            proceedButton3DBM.SetActive(!_isEnglish);

        Debug.Log("Chapter3DExperiment: Started.");
    }

    private void RefreshUI()
    {
        if (dropCountAText != null)
            dropCountAText.text =
                Chapter3Texts.DropsAdded3D(
                    Chapter3Texts.EthanoicAcid3D(),
                    dropCountA);

        if (dropCountBText != null)
            dropCountBText.text =
                Chapter3Texts.DropsAdded3D(
                    Chapter3Texts.AmmoniaSolution3D(),
                    dropCountB);

        if (confirmButtonText3D != null)
            confirmButtonText3D.text =
                Chapter3Texts.ConfirmButton3D();

        if (conclusionTitle3D != null)
            conclusionTitle3D.text =
                Chapter3Texts.ConclusionTitle3D();
    }

    // ─── Drop Detection ───────────────────────────────────────────────

    // Called by Dropper3D when placed in trigger A
    public void OnDropAddedToA()
    {
        if (reactionAStarted || reactionAComplete)
            return;

        dropCountA++;
        Debug.Log("Drop added to A: " + dropCountA);

        if (statusText != null)
            statusText.text =
                Chapter3Texts.DropsAdded3D(
                    Chapter3Texts.EthanoicAcid3D(),
                    dropCountA);

        if (dropCountAText != null)
            dropCountAText.text =
                Chapter3Texts.DropsAdded3D(
                    Chapter3Texts.EthanoicAcid3D(),
                    dropCountA);

        if (dropCountA >= dropsRequired)
        {
            reactionAStarted = true;
            if (acidDropper != null)
                acidDropper.DisableDropper();
            StartCoroutine(ReactionA());
        }
    }

    // Called by Dropper3D when placed in trigger B
    public void OnDropAddedToB()
    {
        if (reactionBComplete) return;

        dropCountB++;
        Debug.Log("Drop added to B: " + dropCountB);

        if (statusText != null)
            statusText.text =
                Chapter3Texts.DropsAdded3D(
                    Chapter3Texts
                    .AmmoniaSolution3D(),
                    dropCountB);

        if (dropCountBText != null)
            dropCountBText.text =
                Chapter3Texts.DropsAdded3D(
                    Chapter3Texts
                    .AmmoniaSolution3D(),
                    dropCountB);

        if (dropCountB >= dropsRequired)
        {
            if (ammoniaDropper != null)
                ammoniaDropper.DisableDropper();
            OnAmmoniaComplete();
        }
    }

    // ─── Reaction A — Ethanoic Acid ───────────────────────────────────

    private IEnumerator ReactionA()
    {
        if (statusText != null)
            statusText.text =
                Chapter3Texts.ReactionStarting3D();

        float elapsed = 0f;

        while (elapsed < reactionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / reactionDuration;

            // Liquid shrinks Y scale to zero
            if (latexLiquidA != null)
            {
                Vector3 s = liquidAOriginalScale;
                s.y = Mathf.Lerp(
                    liquidAOriginalScale.y, 0f, t);
                latexLiquidA.transform
                    .localScale = s;
            }

            // Coagulated chunk grows from 50%
            if (t >= 0.5f && coagulatedLatexA != null)
            {
                float chunkT =
                    (t - 0.5f) / 0.5f;
                coagulatedLatexA.transform
                    .localScale = Vector3.Lerp(
                        Vector3.zero,
                        Vector3.one,
                        chunkT);
            }

            yield return null;
        }

        // Reaction complete
        if (latexLiquidA != null)
        {
            Vector3 s = liquidAOriginalScale;
            s.y = 0f;
            latexLiquidA.transform.localScale = s;
        }

        if (coagulatedLatexA != null)
            coagulatedLatexA.transform
                .localScale = Vector3.one;

        reactionAComplete = true;
        Debug.Log("Reaction A complete");

        if (statusText != null)
            statusText.text =
                Chapter3Texts.ReactionComplete3D();

        CheckBothComplete();
    }

    // ─── Reaction B — Ammonia ─────────────────────────────────────────

    private void OnAmmoniaComplete()
    {
        reactionBComplete = true;
        Debug.Log("Ammonia B complete — no reaction");

        // Latex B stays unchanged
        if (latexLiquidB != null)
            latexLiquidB.transform.localScale =
                liquidBOriginalScale;

        if (statusText != null)
            statusText.text =
                Chapter3Texts.NoReaction3D();

        CheckBothComplete();
    }

    // ─── Both Complete ────────────────────────────────────────────────

    private void CheckBothComplete()
    {
        if (!reactionAComplete
            || !reactionBComplete) return;

        experimentComplete = true;

        if (statusText != null)
            statusText.text =
                Chapter3Texts.BothComplete3D();

        // Show confirm button
        if (confirmButton3D != null)
            confirmButton3D.SetActive(true);

        Debug.Log("3D: Both complete");
    }

    // ─── Confirm Button ───────────────────────────────────────────────

    public void OnConfirmPressed()
    {
        if (!experimentComplete) return;

        if (confirmButton3D != null)
            confirmButton3D.SetActive(false);

        // Send to notebook
        ExperimentSection section =
            new ExperimentSection();
        section.experimentTitle =
            Chapter3Texts.ExperimentTitle3D();
        section.stars = 1;

        section.entries.Add(new ExperimentEntry
        {
            label = Chapter3Texts.EthanoicAcid3D(),
            result = Chapter3Texts.AcidResult3D(),
            passed = true
        });

        section.entries.Add(new ExperimentEntry
        {
            label =
                Chapter3Texts.AmmoniaSolution3D(),
            result = Chapter3Texts.AmmoniaResult3D(),
            passed = true
        });

        Chapter3Manager.Instance.SendNotebook3D(
            section);

        // Show conclusion
        ShowConclusion();
    }

    private void ShowConclusion()
    {
        if (conclusionPanel3D != null)
            conclusionPanel3D.SetActive(true);

        if (conclusionText3D != null)
            conclusionText3D.text =
                Chapter3Texts.Conclusion3D();

        if (conclusionTitle3D != null)
            conclusionTitle3D.text =
                Chapter3Texts.ConclusionTitle3D();

        if (statusText != null)
            statusText.text = "";

        // Re-apply language-correct button visibility right when the panel actually shows
        if (proceedButton3DEN != null)
            proceedButton3DEN.SetActive(_isEnglish);
        if (proceedButton3DBM != null)
            proceedButton3DBM.SetActive(!_isEnglish);
    }

    // ─── Conclude Button ──────────────────────────────────────────────

    // Wire to Proceed/Next button
    // inside conclusion panel
    public void OnConclude3DPressed()
    {
        if (conclusionPanel3D != null)
            conclusionPanel3D.SetActive(false);

        Chapter3Manager.Instance.On3DComplete();
    }

    // ─── Reset ────────────────────────────────────────────────────────

    public void ResetExperiment()
    {
        StopAllCoroutines();

        dropCountA = 0;
        dropCountB = 0;
        reactionAStarted = false;
        reactionAComplete = false;
        reactionBComplete = false;
        experimentComplete = false;

        // Reset liquid scales
        if (latexLiquidA != null)
            latexLiquidA.transform.localScale =
                liquidAOriginalScale;
        if (latexLiquidB != null)
            latexLiquidB.transform.localScale =
                liquidBOriginalScale;

        // Hide coagulated chunk
        if (coagulatedLatexA != null)
            coagulatedLatexA.transform.localScale =
                Vector3.zero;

        // Re-enable droppers
        if (acidDropper != null)
            acidDropper.EnableDropper();
        if (ammoniaDropper != null)
            ammoniaDropper.EnableDropper();

        // Hide conclusion and confirm
        if (conclusionPanel3D != null)
            conclusionPanel3D.SetActive(false);
        if (confirmButton3D != null)
            confirmButton3D.SetActive(false);

        RefreshUI();

        if (statusText != null)
            statusText.text =
                Chapter3Texts.PlaceDropper3D();

        Debug.Log("3D: Reset complete");
    }

    private void SetActive(
        GameObject obj, bool active)
    {
        if (obj != null) obj.SetActive(active);
    }
}