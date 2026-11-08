using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;

public enum NailType
{
    Iron,
    Copper
}

public class NailItem : MonoBehaviour
{
    [Header("Nail Settings")]
    public NailType nailType;

    [Header("Rust Shader")]
    public Renderer nailRenderer;
    public string rustAmountProperty =
        "_RustAmount";

    [Header("State")]
    public bool isPlaced = false;

    [Header("Label Settings")]
    public float labelOffsetY = 0.05f;
    public float labelFontSize = 1f;
    public Color labelColor = Color.white;

    [Header("Reset Settings")]
    [Tooltip("If nail falls below this Y " +
        "position it will reset")]
    public float fallThresholdY = -1f;
    [Tooltip("Delay in seconds before " +
        "resetting after falling")]
    public float resetDelay = 1f;

    [Header("Home Transform")]
    [Tooltip("Assign the parent this nail " +
        "lives under in 9b (e.g. nails group). " +
        "Set homeLocalPosition and " +
        "homeLocalEulerAngles to match its " +
        "starting local position/rotation.")]
    public Transform homeParent;
    public Vector3 homeLocalPosition;
    public Vector3 homeLocalEulerAngles;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private Material nailMaterial;
    private GameObject labelObject;
    private TextMeshPro labelText;

    private bool isGrabbed = false;
    private bool isResetting = false;

    private void Awake()
    {
        grabInteractable =
            GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        if (nailRenderer != null)
            nailMaterial = nailRenderer.material;

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered
                .AddListener(OnGrabbed);
            grabInteractable.selectExited
                .AddListener(OnReleased);
        }

        CreateLabel();
    }

    // ─── Create Label ─────────────────────────

    private void CreateLabel()
    {
        labelObject = new GameObject(
            "NailLabel");
        labelObject.transform.SetParent(
            transform);
        labelObject.transform.localPosition =
            new Vector3(0, labelOffsetY, 0);
        labelObject.transform.localRotation =
            Quaternion.identity;
        labelObject.transform.localScale =
            Vector3.one;

        labelText = labelObject
            .AddComponent<TextMeshPro>();
        labelText.fontSize = labelFontSize;
        labelText.alignment =
            TextAlignmentOptions.Center;
        labelText.color = labelColor;
        labelText.outlineWidth = 0.2f;
        labelText.outlineColor = Color.black;

        RectTransform rect =
            labelObject.GetComponent
            <RectTransform>();
        if (rect != null)
            rect.sizeDelta =
                new Vector2(2f, 0.5f);

        labelObject.SetActive(false);
    }

    // ─── Get Label Text ───────────────────────

    private string GetNailName()
    {
        bool isEnglish = true;
        if (LanguageManager.Instance != null)
            isEnglish = LanguageManager
                .Instance.IsEnglish();

        switch (nailType)
        {
            case NailType.Iron:
                return isEnglish
                    ? "Iron Nail"
                    : "Paku Besi";
            case NailType.Copper:
                return isEnglish
                    ? "Copper Nail"
                    : "Paku Tembaga";
            default:
                return isEnglish
                    ? "Nail"
                    : "Paku";
        }
    }

    // ─── Grab / Release ───────────────────────

    private void OnGrabbed(
        SelectEnterEventArgs args)
    {
        isGrabbed = true;
        isResetting = false;
        CancelInvoke("ResetNail");

        transform.SetParent(null);

        if (rb != null)
            rb.isKinematic = false;

        if (labelObject != null
            && labelText != null)
        {
            labelText.text = GetNailName();
            labelObject.SetActive(true);
        }
    }

    private void OnReleased(
        SelectExitEventArgs args)
    {
        isGrabbed = false;

        if (labelObject != null)
            labelObject.SetActive(false);
    }

    // ─── Reset Logic ──────────────────────────

    private void Update()
    {
        if (isGrabbed || isPlaced
            || isResetting)
            return;

        if (transform.position.y
            < fallThresholdY)
        {
            isResetting = true;
            Debug.Log(nailType
                + " nail fell — resetting in "
                + resetDelay + " seconds.");
            Invoke("ResetNail", resetDelay);
        }

        if (labelObject != null
            && labelObject.activeSelf)
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                labelObject.transform.LookAt(
                    labelObject.transform
                        .position
                    + cam.transform.rotation
                    * Vector3.forward,
                    cam.transform.rotation
                    * Vector3.up);
            }
        }
    }

    private void ResetNail()
    {
        if (labelObject != null)
            labelObject.SetActive(false);

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        ReturnToHome();

        if (rb != null)
            rb.isKinematic = false;

        isResetting = false;
        isPlaced = false;

        if (grabInteractable != null)
            grabInteractable.enabled = true;

        Debug.Log(nailType + " nail reset.");
    }

    // ─── Returns nail to home transform ───────

    private void ReturnToHome()
    {
        if (homeParent != null)
        {
            transform.SetParent(homeParent);
            transform.localPosition =
                homeLocalPosition;
            transform.localEulerAngles =
                homeLocalEulerAngles;
        }
        else
        {
            // Fallback — shouldn't happen if
            // homeParent is assigned in Inspector
            Debug.LogWarning(nailType
                + ": homeParent not assigned!");
        }
    }

    // ─── Place in Tube ────────────────────────

    public void PlaceInTube(
        TestTube tube, Transform snapPoint)
    {
        if (isPlaced) return;
        isPlaced = true;

        if (labelObject != null)
            labelObject.SetActive(false);

        if (grabInteractable != null)
        {
            grabInteractable.interactionManager
                .CancelInteractableSelection(
                    (IXRSelectInteractable)
                    grabInteractable);
            grabInteractable.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (snapPoint != null)
        {
            transform.SetParent(snapPoint);
            transform.localPosition =
                Vector3.zero;
            transform.localRotation =
                Quaternion.identity;
        }
        else
        {
            transform.SetParent(tube.transform);
            transform.localPosition =
                new Vector3(0, -0.05f, 0);
            transform.localRotation =
                Quaternion.identity;
        }

        Debug.Log(nailType
            + " nail snapped into tube "
            + tube.tubeLabel);
    }

    public void SetRustAmount(float amount)
    {
        if (nailMaterial == null) return;

        if (nailMaterial.HasProperty(
            rustAmountProperty))
            nailMaterial.SetFloat(
                rustAmountProperty, amount);
    }

    public void AllowPickup()
    {
        if (grabInteractable != null)
            grabInteractable.enabled = true;

        isPlaced = false;
    }

    // ─── Full Reset for Retry ─────────────────

    public void ResetForRetry()
    {
        CancelInvoke("ResetNail");

        if (labelObject != null)
            labelObject.SetActive(false);

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        ReturnToHome();

        if (rb != null)
            rb.isKinematic = false;

        isResetting = false;
        isPlaced = false;
        isGrabbed = false;
        SetRustAmount(0f);

        if (grabInteractable != null)
            grabInteractable.enabled = true;

        Debug.Log(nailType
            + " nail fully reset for retry.");
    }
}