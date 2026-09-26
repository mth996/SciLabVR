using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit
    .Interactables;

public enum DropperType
{
    EthanoicAcid,
    Ammonia
}

public class Dropper3D : MonoBehaviour
{
    [Header("Settings")]
    public DropperType dropperType;
    public float dropCooldown = 0.5f;

    [Header("Trigger Zone")]
    [Tooltip("Tag assigned to the beaker's " +
        "trigger collider GameObject. Set this " +
        "tag on the trigger zone in the scene " +
        "instead of dragging an exact reference.")]
    public string triggerTag = "BeakerDropZone";

    [Header("VFX")]
    [Tooltip("Loops while this dropper is " +
        "held inside the trigger zone. " +
        "Assign a different effect per " +
        "dropper (acid vs ammonia).")]
    public ParticleSystem dropVFX;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dropSound;
    [Range(0f, 0.2f)] public float pitchVariation = 0.05f;

    private XRGrabInteractable grabInteractable;
    private bool isEnabled = true;
    private bool isInTrigger = false;
    private float lastDropTime = 0f;
    private bool isHeld = false;

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

        if (dropVFX != null)
            dropVFX.Stop(true,
                ParticleSystemStopBehavior
                    .StopEmittingAndClear);
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
        Debug.Log(dropperType + " dropper grabbed");
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
        StopVFX();
        Debug.Log(dropperType + " dropper released");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isEnabled) return;
        if (!isHeld) return;

        if (other.CompareTag(triggerTag))
        {
            isInTrigger = true;
            PlayVFX();
            AddDrop();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isEnabled) return;
        if (!isHeld) return;
        if (!isInTrigger) return;

        if (other.CompareTag(triggerTag))
        {
            if (Time.time - lastDropTime
                >= dropCooldown)
                AddDrop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            isInTrigger = false;
            StopVFX();
        }
    }

    private void AddDrop()
    {
        if (!isEnabled) return;

        lastDropTime = Time.time;

        PlayDropSound();

        Chapter3DExperiment exp =
            Chapter3Manager.Instance
            .GetComponent<Chapter3DExperiment>();

        if (exp == null)
        {
            Debug.LogWarning(
                "Chapter3DExperiment not found!");
            return;
        }

        if (dropperType ==
            DropperType.EthanoicAcid)
            exp.OnDropAddedToA();
        else
            exp.OnDropAddedToB();

        Debug.Log(dropperType
            + " drop added");
    }

    private void PlayDropSound()
    {
        if (audioSource == null || dropSound == null) return;

        audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        audioSource.PlayOneShot(dropSound);
    }

    private void PlayVFX()
    {
        if (dropVFX != null && !dropVFX.isPlaying)
            dropVFX.Play();
    }

    private void StopVFX()
    {
        if (dropVFX != null && dropVFX.isPlaying)
            dropVFX.Stop();
    }

    public void EnableDropper()
    {
        isEnabled = true;
        if (grabInteractable != null)
            grabInteractable.enabled = true;
    }

    public void DisableDropper()
    {
        isEnabled = false;
        if (grabInteractable != null)
            grabInteractable.enabled = false;
        isInTrigger = false;
        StopVFX();
    }
}