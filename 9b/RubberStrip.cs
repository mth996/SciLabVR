using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit
    .Interactables;

public enum RubberType
{
    Natural,
    Vulcanised
}

public class RubberStrip : MonoBehaviour
{
    [Header("Strip Settings")]
    public RubberType rubberType;

    [Header("Snap Points")]
    public Transform startSnapPoint;
    public Transform tubeSnapPoint;

    [Header("Size Settings")]
    public float originalLengthCm = 5f;

    [Header("State")]
    public bool isPlaced = false;
    public bool isInTube = false;

    [HideInInspector]
    public float currentLengthCm = 5f;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    // Recorded once at Start()
    // Never overwritten
    private Vector3 recordedOriginalScale;
    private bool scaleRecorded = false;

    // Original parent recorded at Start()
    // so SnapBackToStart never unparents
    private Transform originalParent;

    private void Awake()
    {
        grabInteractable =
            GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Record original parent (e.g. 9c)
        // so we can restore it on snap back
        originalParent = transform.parent;

        // Record exact scale as set in scene
        // Only once — never overwritten
        if (!scaleRecorded)
        {
            recordedOriginalScale =
                transform.localScale;
            scaleRecorded = true;
        }

        currentLengthCm = originalLengthCm;

        // Kinematic at start —
        // nothing can push it around
        SetKinematic(true);

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered
                .AddListener(OnGrabbed);
            grabInteractable.selectExited
                .AddListener(OnReleased);
        }

        Debug.Log(rubberType
            + " original scale recorded: "
            + recordedOriginalScale
            + " | parent: "
            + (originalParent != null
                ? originalParent.name
                : "none"));
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered
                .RemoveListener(OnGrabbed);
            grabInteractable.selectExited
                .RemoveListener(OnReleased);
        }
    }

    // ─── Grab Events ──────────────────────────────────────────────────────

    private void OnGrabbed(
        SelectEnterEventArgs args)
    {
        if (isPlaced) return;
        SetKinematic(false);
        Debug.Log(rubberType + " grabbed.");
    }

    private void OnReleased(
        SelectExitEventArgs args)
    {
        if (!isPlaced && !isInTube)
            SnapToStart();
    }

    // ─── Placement ────────────────────────────────────────────────────────

    public void PlaceInTube(BoilingTube3C tube)
    {
        if (isPlaced) return;
        isPlaced = true;
        isInTube = true;

        DisableGrab();

        if (tubeSnapPoint != null)
        {
            transform.SetParent(tubeSnapPoint);
            transform.localPosition =
                Vector3.zero;
            transform.localRotation =
                Quaternion.identity;
        }

        Debug.Log(rubberType
            + " placed in tube.");
    }

    // ─── Snap Back ────────────────────────────────────────────────────────

    public void SnapBackToStart()
    {
        isInTube = false;
        isPlaced = false;

        // Restore original parent (9c) so the
        // strip stays hidden when 9c is inactive
        transform.SetParent(originalParent);

        SnapToStart();
        EnableGrab();

        Debug.Log(rubberType
            + " snapped back to start, "
            + "parent restored to: "
            + (originalParent != null
                ? originalParent.name
                : "none"));
    }

    private void SnapToStart()
    {
        SetKinematic(true);

        if (startSnapPoint != null)
        {
            transform.position =
                startSnapPoint.position;
            // Always restore Y=90 rotation
            transform.rotation =
                startSnapPoint.rotation;
        }
    }

    // ─── Deformation ──────────────────────────────────────────────────────

    // Called every Update during heating
    // Simply lerps Z from original
    // down to (original * (1 - shrinkPercent))
    // X and Y never change
    public void DeformByPercent(
        float shrinkPercent, float progress)
    {
        if (!scaleRecorded) return;

        float currentZ = Mathf.Lerp(
            recordedOriginalScale.z,
            recordedOriginalScale.z
                * (1f - shrinkPercent),
            progress);

        transform.localScale = new Vector3(
            recordedOriginalScale.x,
            recordedOriginalScale.y,
            currentZ);
    }

    // Called once at end of timer
    // Locks in exact final Z value
    public void SetFinalDeformation(
        float shrinkPercent)
    {
        if (!scaleRecorded) return;

        float finalZ =
            recordedOriginalScale.z
            * (1f - shrinkPercent);

        transform.localScale = new Vector3(
            recordedOriginalScale.x,
            recordedOriginalScale.y,
            finalZ);

        Debug.Log(rubberType
            + " Z: "
            + recordedOriginalScale.z
            .ToString("F4")
            + " → "
            + finalZ.ToString("F4"));
    }

    // ─── Reset ────────────────────────────────────────────────────────────

    public void ResetStrip()
    {
        if (!scaleRecorded) return;

        // Restore exact original scale
        transform.localScale =
            recordedOriginalScale;

        currentLengthCm = originalLengthCm;
        isPlaced = false;
        isInTube = false;

        // Restore original parent (9c)
        transform.SetParent(originalParent);

        SnapToStart();
        EnableGrab();

        Debug.Log(rubberType
            + " reset. Scale restored: "
            + recordedOriginalScale);
    }

    // ─── Grab Control ─────────────────────────────────────────────────────

    public void DisableGrab()
    {
        if (grabInteractable != null)
        {
            grabInteractable.interactionManager
                .CancelInteractableSelection(
                    (IXRSelectInteractable)
                    grabInteractable);
            grabInteractable.enabled = false;
        }
        SetKinematic(true);
    }

    public void EnableGrab()
    {
        if (grabInteractable != null)
            grabInteractable.enabled = true;
        SetKinematic(true);
    }

    private void SetKinematic(bool kinematic)
    {
        if (rb == null) return;
        rb.isKinematic = kinematic;
        if (kinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}