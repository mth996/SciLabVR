using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit
    .Interactables;

public class GrabbablePart2A : MonoBehaviour
{
    [Header("Settings")]
    public PartType2A partType;

    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;
    private bool isUsed = false;

    private void Awake()
    {
        grabInteractable =
            GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered
                .AddListener(OnGrabbed);
            grabInteractable.selectExited
                .AddListener(OnReleased);
        }
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

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isHeld || isUsed) return;

        PlacementSocket2A socket =
            other.GetComponent<PlacementSocket2A>();

        if (socket == null) return;

        if (socket.TryFill(partType))
        {
            isUsed = true;
            gameObject.SetActive(false);

            if (Chapter2AExperiment.Instance != null)
                Chapter2AExperiment.Instance
                    .OnPartPlaced(partType);
        }
    }

    public void ResetPart(
        Vector3 originalPosition,
        Quaternion originalRotation)
    {
        isUsed = false;
        isHeld = false;

        transform.SetPositionAndRotation(
            originalPosition, originalRotation);

        gameObject.SetActive(true);

        if (grabInteractable != null)
            grabInteractable.enabled = true;
    }
}