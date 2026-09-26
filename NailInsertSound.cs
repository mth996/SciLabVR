using UnityEngine;

/// <summary>
/// Attach this to the same GameObject as your TestTube script (e.g. tube "q").
/// Plays a glass clink sound when a nail enters the tube trigger collider.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class NailInsertSound : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip nailHitsGlassClip;
    [Range(0f, 1f)] public float volume = 1f;

    [Header("Nail Tag (must match your nail GameObjects)")]
    public string nailTag = "Nail";

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip        = nailHitsGlassClip;
        audioSource.loop        = false;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;   // 3D positional in VR
        audioSource.volume      = volume;
        audioSource.minDistance = 0.1f;
        audioSource.maxDistance = 2f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(nailTag)) return;
        if (nailHitsGlassClip == null) return;

        // Stop if somehow already playing, then play fresh
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.PlayOneShot(nailHitsGlassClip, volume);
    }
}
