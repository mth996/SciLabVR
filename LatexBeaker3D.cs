using System.Collections;
using UnityEngine;
using TMPro;

public class LatexBeaker3D : MonoBehaviour
{
    [Header("Beaker Settings")]
    public string beakerName = "Beaker";
    public int requiredDrops = 10;

    [Header("Latex Liquid")]
    public Transform latexLiquid;
    public float finalLiquidYScale = 0f;

    [Header("Coagulation Object")]
    public GameObject coagulatedLatexObject;

    [Header("Reaction Timing")]
    public float reactionDuration = 15f;

    [Range(0f, 1f)]
    public float coagulationAppearAt = 0.5f;

    [Header("UI Optional")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI dropCountText;

    private int ch3coohDrops = 0;
    private int nh3Drops = 0;

    private bool reactionComplete = false;
    private bool reactionRunning = false;

    private Vector3 originalLiquidScale;
    private Vector3 originalChunkScale;

    private void Start()
    {
        if (latexLiquid != null)
            originalLiquidScale = latexLiquid.localScale;

        if (coagulatedLatexObject != null)
        {
            originalChunkScale =
                coagulatedLatexObject.transform.localScale;

            coagulatedLatexObject.SetActive(false);
            coagulatedLatexObject.transform.localScale =
                Vector3.zero;
        }

        UpdateDropText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (reactionComplete || reactionRunning)
            return;

        ChemicalDripper3D dripper =
            other.GetComponent<ChemicalDripper3D>();

        if (dripper == null)
            dripper =
                other.GetComponentInParent<ChemicalDripper3D>();

        if (dripper == null)
            return;

        AddDrop(dripper);
    }

    private void AddDrop(ChemicalDripper3D dripper)
    {
        dripper.PlayDropEffect();

        if (dripper.chemicalType ==
            ChemicalType3D.CH3COOH)
        {
            ch3coohDrops += dripper.dropsPerTouch;

            if (statusText != null)
                statusText.text =
                    beakerName + ": CH3COOH added";
        }
        else if (dripper.chemicalType ==
            ChemicalType3D.NH3)
        {
            nh3Drops += dripper.dropsPerTouch;

            if (statusText != null)
                statusText.text =
                    beakerName + ": NH3 added";
        }

        UpdateDropText();
        CheckReaction();
    }

    private void CheckReaction()
    {
        if (ch3coohDrops >= requiredDrops)
        {
            StartCoroutine(CoagulationReaction());
        }
        else if (nh3Drops >= requiredDrops)
        {
            StartNoReaction();
        }
    }

    private IEnumerator CoagulationReaction()
    {
        reactionRunning = true;

        if (statusText != null)
            statusText.text =
                beakerName + ": Coagulation started";

        float timer = 0f;
        bool chunkShown = false;

        while (timer < reactionDuration)
        {
            timer += Time.deltaTime;

            float progress =
                Mathf.Clamp01(timer / reactionDuration);

            if (latexLiquid != null)
            {
                Vector3 scale = originalLiquidScale;

                scale.y = Mathf.Lerp(
                    originalLiquidScale.y,
                    finalLiquidYScale,
                    progress);

                latexLiquid.localScale = scale;
            }

            if (progress >= coagulationAppearAt)
            {
                if (!chunkShown)
                {
                    chunkShown = true;

                    if (coagulatedLatexObject != null)
                    {
                        coagulatedLatexObject.SetActive(true);
                        coagulatedLatexObject.transform.localScale =
                            Vector3.zero;
                    }
                }

                if (coagulatedLatexObject != null)
                {
                    float chunkProgress =
                        Mathf.InverseLerp(
                            coagulationAppearAt,
                            1f,
                            progress);

                    coagulatedLatexObject.transform.localScale =
                        Vector3.Lerp(
                            Vector3.zero,
                            originalChunkScale,
                            chunkProgress);
                }
            }

            yield return null;
        }

        if (latexLiquid != null)
        {
            Vector3 finalScale = originalLiquidScale;
            finalScale.y = finalLiquidYScale;
            latexLiquid.localScale = finalScale;
        }

        if (coagulatedLatexObject != null)
        {
            coagulatedLatexObject.SetActive(true);
            coagulatedLatexObject.transform.localScale =
                originalChunkScale;
        }

        reactionRunning = false;
        reactionComplete = true;

        if (statusText != null)
            statusText.text =
                beakerName + ": Latex coagulated";
    }

    private void StartNoReaction()
    {
        reactionComplete = true;

        if (statusText != null)
            statusText.text =
                beakerName + ": No coagulation";
    }

    private void UpdateDropText()
    {
        if (dropCountText == null)
            return;

        dropCountText.text =
            beakerName +
            "\nCH3COOH: " + ch3coohDrops +
            " / " + requiredDrops +
            "\nNH3: " + nh3Drops +
            " / " + requiredDrops;
    }

    public void ResetBeaker()
    {
        StopAllCoroutines();

        ch3coohDrops = 0;
        nh3Drops = 0;

        reactionComplete = false;
        reactionRunning = false;

        if (latexLiquid != null)
            latexLiquid.localScale =
                originalLiquidScale;

        if (coagulatedLatexObject != null)
        {
            coagulatedLatexObject.SetActive(false);
            coagulatedLatexObject.transform.localScale =
                Vector3.zero;
        }

        UpdateDropText();

        if (statusText != null)
            statusText.text =
                beakerName + ": Ready";
    }
}