using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DripSound : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dripSound;
    [Range(0f, 0.2f)]
    public float pitchVariation = 0.05f;

    [Header("Drop Counting")]
    public int maxDrops = 10;

    [Header("State")]
    [Tooltip("Set true only while the dropper is being held/squeezed above the beaker")]
    public bool isDripping = false;

    private ParticleSystem ps;
    private int dropCount = 0;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // TEMP TEST ONLY — press T in Play mode to test audio path directly, bypassing collision
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Manual test: trying to play sound");
            if (audioSource != null && dripSound != null)
            {
                audioSource.PlayOneShot(dripSound);
                Debug.Log("PlayOneShot called");
            }
            else
            {
                Debug.Log("Missing reference — audioSource: " + (audioSource != null) + ", dripSound: " + (dripSound != null));
            }
        }
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle collided with: " + other.name); // TEMP — confirms collision detection works

        if (!isDripping) return;
        if (dropCount >= maxDrops) return;

        dropCount++;

        if (audioSource != null && dripSound != null)
        {
            audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
            audioSource.PlayOneShot(dripSound);
        }

        Debug.Log($"Drop {dropCount}/{maxDrops}");

        if (dropCount >= maxDrops)
        {
            OnMaxDropsReached();
        }
    }

    // ── Called by your dropper/interaction script ──

    public void StartDripping()
    {
        isDripping = true;
    }

    public void StopDripping()
    {
        isDripping = false;
    }

    private void OnMaxDropsReached()
    {
        Debug.Log("Reached max drops — stopping emission.");

        isDripping = false;

        var emission = ps.emission;
        emission.enabled = false;
    }

    public void ResetDropCount()
    {
        dropCount = 0;

        var emission = ps.emission;
        emission.enabled = true;
    }
}