using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chapter2AExperiment : MonoBehaviour
{
    public static Chapter2AExperiment Instance;

    // ─── Hollow Sockets ───────────────────────────────

    [Header("Hollow — Sockets")]
    public PlacementSocket2A[] hollowCylinderSockets;
    public PlacementSocket2A hollowBoardSocket;

    [Header("Hollow — Grabbable Parts")]
    public GrabbablePart2A[] hollowCylinderParts;
    public GrabbablePart2A hollowBoardPart;

    [Header("Hollow — Structure Physics")]
    [Tooltip("Rigidbodies of the stationary hollow " +
        "cylinders + board — flipped to non-kinematic " +
        "when the collapse threshold is reached.")]
    public Rigidbody[] hollowStructureRigidbodies;

    [Header("Hollow — Book Zone")]
    public BookDropZone2A hollowBookZone;

    // ─── Solid Sockets ────────────────────────────────

    [Header("Solid — Sockets")]
    public PlacementSocket2A[] solidCylinderSockets;
    public PlacementSocket2A solidBoardSocket;

    [Header("Solid — Grabbable Parts")]
    public GrabbablePart2A[] solidCylinderParts;
    public GrabbablePart2A solidBoardPart;

    [Header("Solid — Structure Physics")]
    public Rigidbody[] solidStructureRigidbodies;

    [Header("Solid — Book Zone")]
    public BookDropZone2A solidBookZone;

    // ─── Shared Book Pool ─────────────────────────────

    [Header("Book Pool (shared, reused both phases)")]
    public GrabbableBook2A[] bookPool;

    // ─── Collapse Imbalance ───────────────────────────

    [Header("Hollow — Collapse Imbalance")]
    [Tooltip("Box colliders of the 4 stationary hollow cylinders ONLY " +
        "(not the board). One is randomly shrunk at collapse to force " +
        "a topple instead of a stable stack.")]
    public BoxCollider[] hollowCylinderColliders;

    [Header("Solid — Collapse Imbalance")]
    [Tooltip("Box colliders of the 4 stationary solid cylinders ONLY.")]
    public BoxCollider[] solidCylinderColliders;

    public enum ColliderHeightAxis { X, Y, Z }

    [Header("Settings — Collapse Imbalance")]
    [Range(0.1f, 0.9f)]
    [Tooltip("Fraction of original height kept on the randomly chosen " +
        "cylinder — lower = more dramatic imbalance.")]
    public float collapseHeightKeepFraction = 0.4f;
    [Tooltip("Which local axis of the box collider represents height " +
        "for these cylinders — check each cylinder's Box Collider size " +
        "to see which axis is the long one.")]
    public ColliderHeightAxis heightAxis = ColliderHeightAxis.Y;

    // ─── Settings — Collapse Thresholds ────────────────

    [Header("Settings — Collapse Thresholds")]
    [Tooltip("Number of books that causes the HOLLOW " +
        "structure to collapse. Tune freely.")]
    public int hollowCollapseBookCount = 5;
    [Tooltip("Number of books that causes the SOLID " +
        "structure to collapse. Tune freely.")]
    public int solidCollapseBookCount = 3;

    // ─── UI ───────────────────────────────────────────

    [Header("UI — Status")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI headerLabel;
    public TextMeshProUGUI bookCountText;

    [Header("UI — Proceed to Solid")]
    public GameObject proceedToSolidButton;
    public TextMeshProUGUI proceedToSolidButtonText;

    [Header("UI — Conclusion")]
    public GameObject conclusionPanel;
    public TextMeshProUGUI conclusionTitle;
    public TextMeshProUGUI conclusionText;
    public GameObject proceedButtonEN;
    public GameObject proceedButtonBM;

    [Header("UI Positioning")]
    public Transform canvasTransform;   // drag in "UI Canvas 2B" itself
    public Transform uiAnchor2A;        // empty object positioned near 2A's apparatus

    // ─── State ────────────────────────────────────────

    private enum Phase
    {
        Locked,
        BuildingHollowCylinders,
        BuildingHollowBoard,
        StackingHollowBooks,
        AwaitingProceedToSolid,
        BuildingSolidCylinders,
        BuildingSolidBoard,
        StackingSolidBooks,
        Complete
    }

    private Phase currentPhase = Phase.Locked;

    private int hollowFilledCount = 0;
    private int solidFilledCount = 0;
    private int currentBookCount = 0;

    private int hollowBooksAtCollapse = 0;
    private int solidBooksAtCollapse = 0;

    private bool _isEnglish = true;

    // Cached original transforms for reset
    private Dictionary<GrabbablePart2A, (Vector3, Quaternion)>
        originalPartTransforms =
        new Dictionary<GrabbablePart2A, (Vector3, Quaternion)>();
    private Dictionary<GrabbableBook2A, (Vector3, Quaternion)>
        originalBookTransforms =
        new Dictionary<GrabbableBook2A, (Vector3, Quaternion)>();

    // Cached original collider sizes, for collapse-imbalance reset
    private Dictionary<BoxCollider, Vector3> originalColliderSizes =
        new Dictionary<BoxCollider, Vector3>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (LanguageManager.Instance != null)
            _isEnglish = LanguageManager
                .Instance.IsEnglish();

        SetActive(proceedToSolidButton, false);
        SetActive(conclusionPanel, false);

        if (headerLabel != null)
            headerLabel.text =
                Chapter2BTexts.ExperimentHeader2A();
        if (proceedToSolidButtonText != null)
            proceedToSolidButtonText.text =
                Chapter2BTexts.ProceedToSolidButton2A();

        CacheOriginalTransforms();
    }

    private void CacheOriginalTransforms()
    {
        foreach (var part in AllParts())
        {
            if (part == null) continue;
            originalPartTransforms[part] =
                (part.transform.position,
                 part.transform.rotation);
        }

        foreach (var book in bookPool)
        {
            if (book == null) continue;
            originalBookTransforms[book] =
                (book.transform.position,
                 book.transform.rotation);
        }

        foreach (var c in hollowCylinderColliders)
            if (c != null) originalColliderSizes[c] = c.size;
        foreach (var c in solidCylinderColliders)
            if (c != null) originalColliderSizes[c] = c.size;
    }

    private IEnumerable<GrabbablePart2A> AllParts()
    {
        foreach (var p in hollowCylinderParts) yield return p;
        yield return hollowBoardPart;
        foreach (var p in solidCylinderParts) yield return p;
        yield return solidBoardPart;
    }

    // ─── Start / Reset ────────────────────────────────

    // Called by Chapter2Manager.StartExperiment2A()
    public void OnUnlocked()
    {
        if (canvasTransform != null && uiAnchor2A != null)
        {
            canvasTransform.position = uiAnchor2A.position;
            canvasTransform.rotation = uiAnchor2A.rotation;
        }

        _isEnglish = LanguageManager.Instance != null
            && LanguageManager.Instance.IsEnglish();

        StopAllCoroutines();

        hollowFilledCount = 0;
        solidFilledCount = 0;
        currentBookCount = 0;
        hollowBooksAtCollapse = 0;
        solidBooksAtCollapse = 0;

        SetActive(proceedToSolidButton, false);
        SetActive(conclusionPanel, false);

        SetActive(proceedButtonEN, _isEnglish);
        SetActive(proceedButtonBM, !_isEnglish);

        ResetAllSockets();
        ResetAllParts();
        ResetAllBooks();
        ResetStructurePhysics();
        ResetColliderSizes();

        // Board highlights must stay hidden until all 4
        // cylinders for that phase are placed.
        if (hollowBoardSocket != null)
            hollowBoardSocket.SetHighlightVisible(false);
        if (solidBoardSocket != null)
            solidBoardSocket.SetHighlightVisible(false);

        currentPhase = Phase.BuildingHollowCylinders;

        SetHollowActive(true);
        SetSolidActive(false);

        UpdateBookCountUI();

        if (statusText != null)
            statusText.text =
                Chapter2BTexts.PlaceHollowCylinders2A();

        Debug.Log("Chapter2AExperiment: Started.");
    }

    private void ResetAllSockets()
    {
        foreach (var s in hollowCylinderSockets)
            if (s != null) s.ResetSocket();
        if (hollowBoardSocket != null)
            hollowBoardSocket.ResetSocket();

        foreach (var s in solidCylinderSockets)
            if (s != null) s.ResetSocket();
        if (solidBoardSocket != null)
            solidBoardSocket.ResetSocket();
    }

    private void ResetAllParts()
    {
        foreach (var part in AllParts())
        {
            if (part == null) continue;
            if (originalPartTransforms.TryGetValue(
                part, out var t))
                part.ResetPart(t.Item1, t.Item2);
        }
    }

    private void ResetAllBooks()
    {
        foreach (var book in bookPool)
        {
            if (book == null) continue;
            if (originalBookTransforms.TryGetValue(
                book, out var t))
                book.ResetBook(t.Item1, t.Item2);
        }
    }

    private void ResetStructurePhysics()
    {
        SetKinematic(hollowStructureRigidbodies, true);
        SetKinematic(solidStructureRigidbodies, true);
    }

    private void ResetColliderSizes()
    {
        foreach (var c in hollowCylinderColliders)
            if (c != null && originalColliderSizes.TryGetValue(c, out var s))
                c.size = s;
        foreach (var c in solidCylinderColliders)
            if (c != null && originalColliderSizes.TryGetValue(c, out var s))
                c.size = s;
    }

    private void SetKinematic(
        Rigidbody[] bodies, bool kinematic)
    {
        if (bodies == null) return;
        foreach (var rb in bodies)
        {
            if (rb == null) continue;
            rb.isKinematic = kinematic;
            rb.useGravity = !kinematic;
        }
    }

    private void SetHollowActive(bool active)
    {
        foreach (var p in hollowCylinderParts)
            if (p != null) p.gameObject.SetActive(active);
        if (hollowBoardPart != null)
            hollowBoardPart.gameObject.SetActive(false);

        // Close every socket first...
        foreach (var s in hollowCylinderSockets)
            if (s != null) s.SetAcceptingInput(false);

        // ...then open ONLY the first one in sequence.
        if (active && hollowCylinderSockets.Length > 0
                   && hollowCylinderSockets[0] != null)
            hollowCylinderSockets[0].SetAcceptingInput(true);
    }

    private void SetSolidActive(bool active)
    {
        foreach (var p in solidCylinderParts)
            if (p != null) p.gameObject.SetActive(active);
        if (solidBoardPart != null)
            solidBoardPart.gameObject.SetActive(false);

        foreach (var s in solidCylinderSockets)
            if (s != null) s.SetAcceptingInput(false);

        if (active && solidCylinderSockets.Length > 0
                   && solidCylinderSockets[0] != null)
            solidCylinderSockets[0].SetAcceptingInput(true);
    }

    // ─── Part Placed Callback ─────────────────────────

    public void OnPartPlaced(PartType2A partType)
    {
        if (currentPhase == Phase.BuildingHollowCylinders
            && partType == PartType2A.HollowCylinder)
        {
            hollowFilledCount++;
            if (hollowFilledCount >= hollowCylinderSockets.Length)
                BeginHollowBoardPhase();
            else
                hollowCylinderSockets[hollowFilledCount]
                    .SetAcceptingInput(true);
        }
        else if (currentPhase == Phase.BuildingHollowBoard
                 && partType == PartType2A.Board)
        {
            BeginHollowBookPhase();
        }
        else if (currentPhase == Phase.BuildingSolidCylinders
                 && partType == PartType2A.SolidCylinder)
        {
            solidFilledCount++;
            if (solidFilledCount >= solidCylinderSockets.Length)
                BeginSolidBoardPhase();
            else
                solidCylinderSockets[solidFilledCount]
                    .SetAcceptingInput(true);
        }
        else if (currentPhase == Phase.BuildingSolidBoard
                 && partType == PartType2A.Board)
        {
            BeginSolidBookPhase();
        }
    }

    private void BeginHollowBoardPhase()
    {
        currentPhase = Phase.BuildingHollowBoard;

        if (hollowBoardPart != null)
            hollowBoardPart.gameObject.SetActive(true);
        if (hollowBoardSocket != null)
            hollowBoardSocket.SetAcceptingInput(true);

        if (statusText != null)
            statusText.text =
                Chapter2BTexts.PlaceHollowBoard2A();
    }

    private void BeginHollowBookPhase()
    {
        currentPhase = Phase.StackingHollowBooks;
        currentBookCount = 0;
        UpdateBookCountUI();

        if (statusText != null)
            statusText.text =
                Chapter2BTexts.StackHollowBooks2A();
    }

    private void BeginSolidBoardPhase()
    {
        currentPhase = Phase.BuildingSolidBoard;

        if (solidBoardPart != null)
            solidBoardPart.gameObject.SetActive(true);
        if (solidBoardSocket != null)
            solidBoardSocket.SetAcceptingInput(true);

        if (statusText != null)
            statusText.text =
                Chapter2BTexts.PlaceSolidBoard2A();
    }

    private void BeginSolidBookPhase()
    {
        currentPhase = Phase.StackingSolidBooks;
        currentBookCount = 0;
        UpdateBookCountUI();

        if (statusText != null)
            statusText.text =
                Chapter2BTexts.StackSolidBooks2A();
    }

    // ─── Book Placed Callback ─────────────────────────

    public void OnBookPlaced()
    {
        if (currentPhase != Phase.StackingHollowBooks
            && currentPhase != Phase.StackingSolidBooks)
            return;

        currentBookCount++;
        UpdateBookCountUI();

        bool isHollowPhase =
            currentPhase == Phase.StackingHollowBooks;

        int threshold = isHollowPhase
            ? hollowCollapseBookCount
            : solidCollapseBookCount;

        if (currentBookCount >= threshold)
        {
            if (isHollowPhase)
                TriggerHollowCollapse();
            else
                TriggerSolidCollapse();
        }
    }

    private void UpdateBookCountUI()
    {
        if (bookCountText != null)
            bookCountText.text =
                Chapter2BTexts.BookCountLabel2A(
                    currentBookCount);
    }

    // ─── Collapse ─────────────────────────────────────

    private void InduceCollapseImbalance(BoxCollider[] cylinderColliders)
    {
        if (cylinderColliders == null || cylinderColliders.Length == 0)
            return;

        int index = Random.Range(0, cylinderColliders.Length);
        BoxCollider chosen = cylinderColliders[index];
        if (chosen == null) return;

        Vector3 size = chosen.size;
        switch (heightAxis)
        {
            case ColliderHeightAxis.X: size.x *= collapseHeightKeepFraction; break;
            case ColliderHeightAxis.Y: size.y *= collapseHeightKeepFraction; break;
            case ColliderHeightAxis.Z: size.z *= collapseHeightKeepFraction; break;
        }
        chosen.size = size;
    }

    private void TriggerHollowCollapse()
    {
        hollowBooksAtCollapse = currentBookCount;

        SetKinematic(hollowStructureRigidbodies, false);
        InduceCollapseImbalance(hollowCylinderColliders);

        if (statusText != null)
            statusText.text =
                Chapter2BTexts.HollowCollapsed2A(
                    hollowBooksAtCollapse);

        currentPhase = Phase.AwaitingProceedToSolid;
        SetActive(proceedToSolidButton, true);
    }

    private void TriggerSolidCollapse()
    {
        solidBooksAtCollapse = currentBookCount;

        SetKinematic(solidStructureRigidbodies, false);
        InduceCollapseImbalance(solidCylinderColliders);

        if (statusText != null)
            statusText.text =
                Chapter2BTexts.SolidCollapsed2A(
                    solidBooksAtCollapse);

        SendResultsToNotebook();
        Invoke("ShowConclusion", 2f);
    }

    // ─── Proceed to Solid ─────────────────────────────

    public void OnProceedToSolidPressed()
    {
        SetActive(proceedToSolidButton, false);

        currentPhase = Phase.BuildingSolidCylinders;
        SetSolidActive(true);

        if (statusText != null)
            statusText.text =
                Chapter2BTexts.PlaceSolidCylinders2A();
    }

    // ─── Notebook ─────────────────────────────────────

    private void SendResultsToNotebook()
    {
        bool hollowStronger =
            hollowBooksAtCollapse > solidBooksAtCollapse;

        ExperimentSection section = new ExperimentSection();
        section.experimentTitle =
            Chapter2BTexts.ExperimentTitle2A();
        section.stars = hollowStronger ? 1 : 0;

        section.entries.Add(new ExperimentEntry
        {
            label = Chapter2BTexts.HollowLabel2A(),
            result = hollowBooksAtCollapse + "",
            passed = true
        });

        section.entries.Add(new ExperimentEntry
        {
            label = Chapter2BTexts.SolidLabel2A(),
            result = solidBooksAtCollapse + "",
            passed = true
        });

        section.entries.Add(new ExperimentEntry
        {
            label = hollowStronger
                ? Chapter2BTexts.ComparisonHollowStronger2A()
                : Chapter2BTexts.ComparisonUnexpected2A(),
            result = "",
            passed = hollowStronger
        });

        if (Chapter2Manager.Instance != null)
            Chapter2Manager.Instance
                .SendToNotebook(section, false);
    }

    // ─── Conclusion ───────────────────────────────────

    private void ShowConclusion()
    {
        currentPhase = Phase.Complete;

        SetActive(conclusionPanel, true);

        if (conclusionTitle != null)
            conclusionTitle.text =
                Chapter2BTexts.ConclusionTitle2A();
        if (conclusionText != null)
            conclusionText.text =
                Chapter2BTexts.Conclusion2A(
                    hollowBooksAtCollapse,
                    solidBooksAtCollapse);

        if (statusText != null)
            statusText.text = "";
    }

    public void OnConclude2APressed()
    {
        SetActive(conclusionPanel, false);

        if (Chapter2Manager.Instance != null)
            Chapter2Manager.Instance.On2AComplete();
    }

    private void SetActive(
        GameObject obj, bool active)
    {
        if (obj != null) obj.SetActive(active);
    }
}