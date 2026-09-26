using UnityEngine;

public enum TubeType3C
{
    TubeA_Natural,
    TubeB_Vulcanised
}

public class BoilingTube3C : MonoBehaviour
{
    [Header("Settings")]
    public TubeType3C tubeType;
    public string tubeLabel = "A";

    [Header("Snap Point")]
    public Transform stripSnapPoint;

    [Header("State")]
    public bool hasStrip = false;
    public RubberStrip stripInside;

    private void OnTriggerEnter(Collider other)
    {
        if (hasStrip) return;

        RubberStrip strip =
            other.GetComponent<RubberStrip>();

        if (strip == null) return;
        if (strip.isPlaced) return;

        // Check correct strip type
        // for correct tube
        if (tubeType ==
            TubeType3C.TubeA_Natural
            && strip.rubberType !=
            RubberType.Natural)
        {
            if (Chapter3CExperiment.Instance
                != null)
                Chapter3CExperiment.Instance
                    .OnWrongStrip(tubeLabel);
            return;
        }

        if (tubeType ==
            TubeType3C.TubeB_Vulcanised
            && strip.rubberType !=
            RubberType.Vulcanised)
        {
            if (Chapter3CExperiment.Instance
                != null)
                Chapter3CExperiment.Instance
                    .OnWrongStrip(tubeLabel);
            return;
        }

        hasStrip = true;
        stripInside = strip;
        strip.tubeSnapPoint = stripSnapPoint;
        strip.PlaceInTube(this);

        if (Chapter3CExperiment.Instance != null)
            Chapter3CExperiment.Instance
                .OnStripPlaced(this, strip);
    }

    public void Reset()
    {
        hasStrip = false;
        stripInside = null;
    }
}