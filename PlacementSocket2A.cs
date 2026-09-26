using UnityEngine;

public enum PartType2A
{
    HollowCylinder,
    SolidCylinder,
    Board
}

public class PlacementSocket2A : MonoBehaviour
{
    [Header("Settings")]
    public PartType2A partType;

    [Header("Visuals")]
    public GameObject highlightObject;
    public GameObject stationaryObject;

    private bool isFilled = false;
    private bool acceptingInput = false;

    public bool IsFilled()
    {
        return isFilled;
    }

    // Called by the experiment coordinator to open/close
    // this socket's "turn" in the placement sequence.
    // Highlight only shows while accepting AND not yet filled.
    public void SetAcceptingInput(bool accepting)
    {
        acceptingInput = accepting;

        if (highlightObject != null)
            highlightObject.SetActive(
                accepting && !isFilled);
    }

    // Called by GrabbablePart2A when it enters
    // this socket's trigger zone while held.
    public bool TryFill(PartType2A incomingType)
    {
        if (isFilled) return false;
        if (!acceptingInput) return false;
        if (incomingType != partType) return false;

        isFilled = true;
        acceptingInput = false;

        if (highlightObject != null)
            highlightObject.SetActive(false);
        if (stationaryObject != null)
            stationaryObject.SetActive(true);

        return true;
    }

    public void ResetSocket()
    {
        isFilled = false;
        acceptingInput = false;

        // Highlight stays OFF until the experiment
        // script explicitly calls SetAcceptingInput(true)
        // when it becomes this socket's turn.
        if (highlightObject != null)
            highlightObject.SetActive(false);
        if (stationaryObject != null)
            stationaryObject.SetActive(false);
    }

    // Kept for compatibility — board sockets in the
    // updated Chapter2AExperiment use SetAcceptingInput
    // instead, but nothing else breaks if this is called.
    public void SetHighlightVisible(bool visible)
    {
        if (isFilled) return;
        if (highlightObject != null)
            highlightObject.SetActive(visible);
    }
}