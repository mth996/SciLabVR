using UnityEngine;

public class ActivityController : MonoBehaviour
{
    // ── CHARACTERS ──
    [Header("Drag 3 Characters Here")]
    public Animator character1;
    public Animator character2;
    public Animator character3;

    // ── HIGHLIGHTS ──
    [Header("Optional: Highlight Objects")]
    public GameObject highlight1;
    public GameObject highlight2;
    public GameObject highlight3;

    // ── UI PANELS ──
    [Header("Character UI Panels")]
    public GameObject uiChar1;
    public GameObject uiChar2;
    public GameObject uiChar3;

    // ── INTERNAL STATE ──
    private Animator activeAnim = null;
    private int selectedChar = 0;
    private bool handRaised = false;

    // ═══════════════════════════════
    void Start()
    {
        SetHighlight(0);
        SetCharacterUI(0);
        Debug.Log("ActivityManager ready — select a character!");
    }

    // ═══════════════════════════════
    // CHARACTER SELECTION
    // ═══════════════════════════════

    public void OnSelectCharacter1()
    {
        SelectCharacter(1, character1);
    }

    public void OnSelectCharacter2()
    {
        SelectCharacter(2, character2);
    }

    public void OnSelectCharacter3()
    {
        SelectCharacter(3, character3);
    }

    void SelectCharacter(int num, Animator anim)
    {
        selectedChar = num;
        activeAnim = anim;
        handRaised = false;

        // Reset hand state when switching character
        activeAnim.SetBool("doRaiseHand", false);

        SetHighlight(num);
        SetCharacterUI(num);

        Debug.Log("Selected Character " + num);
    }

    // ═══════════════════════════════
    // WALK
    // ═══════════════════════════════
    public void OnWalkPressed()
    {
        if (!Ready()) return;

        handRaised = false;
        activeAnim.SetBool("doRaiseHand", false);

        activeAnim.ResetTrigger("doRun");
        activeAnim.SetTrigger("doWalk");

        Debug.Log("Character " + selectedChar + " → Walking");
    }

    // ═══════════════════════════════
    // RUN
    // ═══════════════════════════════
    public void OnRunPressed()
    {
        if (!Ready()) return;

        handRaised = false;
        activeAnim.SetBool("doRaiseHand", false);

        activeAnim.ResetTrigger("doWalk");
        activeAnim.SetTrigger("doRun");

        Debug.Log("Character " + selectedChar + " → Running");
    }

    // ═══════════════════════════════
    // REST (BACK TO IDLE)
    // ═══════════════════════════════
    public void OnRestPressed()
    {
        if (!Ready()) return;

        handRaised = false;

        activeAnim.ResetTrigger("doWalk");
        activeAnim.ResetTrigger("doRun");
        activeAnim.SetBool("doRaiseHand", false);

        Debug.Log("Character " + selectedChar + " → Resting (Idle)");
    }

    // ═══════════════════════════════
    // RAISE HAND (TOGGLE)
    // ═══════════════════════════════
    public void OnRaiseHandPressed()
    {
        if (!Ready()) return;

        handRaised = !handRaised;

        activeAnim.ResetTrigger("doWalk");
        activeAnim.ResetTrigger("doRun");

        activeAnim.SetBool("doRaiseHand", handRaised);

        Debug.Log("Character " + selectedChar +
            (handRaised ? " → Hand UP" : " → Hand DOWN"));
    }

    // ═══════════════════════════════
    // HELPERS
    // ═══════════════════════════════

    bool Ready()
    {
        if (activeAnim == null)
        {
            Debug.LogWarning("Select a character first!");
            return false;
        }
        return true;
    }

    void SetHighlight(int num)
    {
        if (highlight1 != null) highlight1.SetActive(num == 1);
        if (highlight2 != null) highlight2.SetActive(num == 2);
        if (highlight3 != null) highlight3.SetActive(num == 3);
    }

    void SetCharacterUI(int num)
    {
        if (uiChar1 != null) uiChar1.SetActive(num == 1);
        if (uiChar2 != null) uiChar2.SetActive(num == 2);
        if (uiChar3 != null) uiChar3.SetActive(num == 3);
    }
}