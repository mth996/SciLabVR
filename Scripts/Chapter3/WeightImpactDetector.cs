using UnityEngine;

public class WeightImpactDetector : MonoBehaviour
{
    [Header("Which Metal")]
    [SerializeField]
    private HardnessExperimentController
        .MetalType metalType;

    [Header("Impact Detection")]
    [SerializeField]
    private string impactTag =
        "MetalImpact";

    [SerializeField]
    private float minimumImpactSpeed =
        0.5f;

    [Header("Impact Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip impactSound;

    private bool impactRegistered;

    private void OnCollisionEnter(
        Collision collision)
    {
        if (impactRegistered)
            return;

        if (!collision.collider.CompareTag(
                impactTag))
        {
            return;
        }

        float impactSpeed =
            collision.relativeVelocity.magnitude;

        if (impactSpeed < minimumImpactSpeed)
            return;

        impactRegistered = true;

        if (audioSource != null && impactSound != null)
            audioSource.PlayOneShot(impactSound);

        if (HardnessExperimentController.Instance
            != null)
        {
            HardnessExperimentController.Instance
                .RegisterImpact(metalType);
        }

        Debug.Log(
            metalType + " impact detected at speed: " +
            impactSpeed);
    }

    public void ResetImpactDetector()
    {
        impactRegistered = false;
    }
}