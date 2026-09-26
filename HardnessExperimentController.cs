using UnityEngine;

public class HardnessExperimentController : MonoBehaviour
{
    public static HardnessExperimentController Instance;

    public enum MetalType
    {
        Copper,
        Bronze
    }

    [Header("Copper Weight")]
    [SerializeField] private Rigidbody copperWeight;
    [SerializeField] private Transform copperResetPoint;

    [Header("Bronze Weight")]
    [SerializeField] private Rigidbody bronzeWeight;
    [SerializeField] private Transform bronzeResetPoint;

    [Header("Copper Objects")]
    [SerializeField] private GameObject copperNormal;
    [SerializeField] private GameObject copperDented;

    [Header("Bronze Objects")]
    [SerializeField] private GameObject bronzeNormal;
    [SerializeField] private GameObject bronzeDented;

    [Header("Metal Parents")]
    [Tooltip("Both are active at the same time now " +
        "— side by side, not toggled.")]
    [SerializeField] private GameObject copperSetup;
    [SerializeField] private GameObject bronzeSetup;

    [Header("Impact Detectors")]
    [SerializeField] private WeightImpactDetector copperDetector;
    [SerializeField] private WeightImpactDetector bronzeDetector;

    private bool weightsReleased;
    private bool copperCompleted;
    private bool bronzeCompleted;
    private bool experimentComplete;

    private Vector3 originalCopperPosition;
    private Quaternion originalCopperRotation;
    private Vector3 originalBronzePosition;
    private Quaternion originalBronzeRotation;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (copperWeight != null)
        {
            originalCopperPosition =
                copperWeight.transform.position;
            originalCopperRotation =
                copperWeight.transform.rotation;
        }

        if (bronzeWeight != null)
        {
            originalBronzePosition =
                bronzeWeight.transform.position;
            originalBronzeRotation =
                bronzeWeight.transform.rotation;
        }

        ResetCompleteExperiment();
    }

    // Connect the single Release button to this.
    public void ReleaseBothWeights()
    {
        if (experimentComplete)
        {
            Debug.Log(
                "Experiment is already complete.");
            return;
        }

        if (weightsReleased)
        {
            Debug.Log(
                "Weights have already been released.");
            return;
        }

        weightsReleased = true;

        ReleaseWeight(copperWeight);
        ReleaseWeight(bronzeWeight);

        Debug.Log(
            "Both weights released simultaneously.");
    }

    private void ReleaseWeight(Rigidbody weight)
    {
        if (weight == null)
        {
            Debug.LogWarning(
                "A falling weight Rigidbody is missing.");
            return;
        }

        weight.isKinematic = false;
        weight.useGravity = true;
        weight.WakeUp();
    }

    // Called by WeightImpactDetector when either
    // ball hits its strip.
    public void RegisterImpact(MetalType type)
    {
        if (!weightsReleased)
            return;

        if (type == MetalType.Copper)
            CompleteCopperImpact();
        else
            CompleteBronzeImpact();

        StopWeightAfterImpact(
            type == MetalType.Copper
                ? copperWeight
                : bronzeWeight);

        CheckBothComplete();
    }

    private void CompleteCopperImpact()
    {
        if (copperCompleted)
            return;

        copperCompleted = true;

        SetActive(copperNormal, false);
        SetActive(copperDented, true);

        Debug.Log(
            "Copper impact complete. " +
            "Dented copper activated.");
    }

    private void CompleteBronzeImpact()
    {
        if (bronzeCompleted)
            return;

        bronzeCompleted = true;

        SetActive(bronzeNormal, false);
        SetActive(bronzeDented, true);

        Debug.Log(
            "Bronze impact complete. " +
            "Dented bronze activated.");
    }

    private void CheckBothComplete()
    {
        if (!copperCompleted || !bronzeCompleted)
            return;

        experimentComplete = true;

        Debug.Log(
            "Both copper and bronze impacts " +
            "complete. Experiment finished.");
    }

    private void StopWeightAfterImpact(
        Rigidbody weight)
    {
        if (weight == null)
            return;

        weight.linearVelocity = Vector3.zero;
        weight.angularVelocity = Vector3.zero;
    }

    public void ResetCompleteExperiment()
    {
        StopAllCoroutines();

        copperCompleted = false;
        bronzeCompleted = false;
        experimentComplete = false;
        weightsReleased = false;

        if (copperDetector != null)
            copperDetector.ResetImpactDetector();
        if (bronzeDetector != null)
            bronzeDetector.ResetImpactDetector();

        SetActive(copperNormal, true);
        SetActive(copperDented, false);

        SetActive(bronzeNormal, true);
        SetActive(bronzeDented, false);

        // Both setups active side by side now
        SetActive(copperSetup, true);
        SetActive(bronzeSetup, true);

        ResetWeight(copperWeight,
            copperResetPoint,
            originalCopperPosition,
            originalCopperRotation);

        ResetWeight(bronzeWeight,
            bronzeResetPoint,
            originalBronzePosition,
            originalBronzeRotation);

        Debug.Log(
            "Hardness experiment reset.");
    }

    private void ResetWeight(
        Rigidbody weight,
        Transform resetPoint,
        Vector3 originalPosition,
        Quaternion originalRotation)
    {
        if (weight == null)
            return;

        weight.useGravity = false;
        weight.isKinematic = true;
        weight.linearVelocity = Vector3.zero;
        weight.angularVelocity = Vector3.zero;

        if (resetPoint != null)
        {
            weight.transform.SetPositionAndRotation(
                resetPoint.position,
                resetPoint.rotation);
        }
        else
        {
            weight.transform.SetPositionAndRotation(
                originalPosition,
                originalRotation);
        }
    }

    public bool IsCopperComplete()
    {
        return copperCompleted;
    }

    public bool IsBronzeComplete()
    {
        return bronzeCompleted;
    }

    public bool IsExperimentComplete()
    {
        return experimentComplete;
    }

    private void SetActive(
        GameObject target,
        bool active)
    {
        if (target != null)
            target.SetActive(active);
    }
}