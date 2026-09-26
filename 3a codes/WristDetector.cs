using UnityEngine;
using UnityEngine.Events;

public class WristDetector : MonoBehaviour
{
    [Header("Settings")]
    public PulseTarget pulseTarget;

    [Header("Hold Settings")]
    [Tooltip("Seconds player must hold hand in zone before detecting")]
    public float holdDuration = 0.8f;

    [Header("Events")]
    public UnityEvent onHandEnter;
    public UnityEvent onHandExit;

    private bool handInZone = false;
    private bool alreadyMeasured = false;
    private float holdTimer = 0f;
    private bool sessionActivated = false;
    private bool sessionStarted = false;

    private void Update()
    {
        if (!handInZone) return;
        if (alreadyMeasured) return;
        if (sessionActivated) return;

        holdTimer += Time.deltaTime;

        if (PulseSessionManager.Instance != null)
        {
            PulseSessionManager.Instance.SetHoldProgress(
                holdTimer / holdDuration
            );
        }

        if (holdTimer >= holdDuration)
        {
            sessionActivated = true;
            holdTimer = 0f;

            if (Chapter1Manager.Instance != null)
            {
                Chapter1Manager.Instance.MoveTimerToTarget(pulseTarget);
                Chapter1Manager.Instance.ShowTimerPanel();
            }

            if (PulseSessionManager.Instance != null)
            {
                PulseSessionManager.Instance.SetActiveTarget(pulseTarget);
            }

            if (pulseTarget != null)
            {
                Debug.Log("Detected NPC: " + pulseTarget.GetSubjectName());
            }

            onHandEnter.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerHand"))
            return;

        if (alreadyMeasured)
            return;

        handInZone = true;
        holdTimer = 0f;

        if (!sessionActivated)
        {
            if (PulseSessionManager.Instance != null && pulseTarget != null)
            {
                PulseSessionManager.Instance.ShowHoldPrompt(
                    pulseTarget.GetSubjectName()
                );
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("PlayerHand"))
            return;

        handInZone = false;
        holdTimer = 0f;

        if (PulseSessionManager.Instance != null)
        {
            PulseSessionManager.Instance.SetHoldProgress(0f);
        }

        if (sessionStarted)
        {
            if (PulseSessionManager.Instance != null)
            {
                PulseSessionManager.Instance.OnHandRemovedDuringSession();
            }

            return;
        }

        if (sessionActivated)
        {
            sessionActivated = false;

            if (PulseSessionManager.Instance != null)
            {
                PulseSessionManager.Instance.ClearActiveTarget();
            }

            onHandExit.Invoke();
            return;
        }

        if (PulseSessionManager.Instance != null)
        {
            PulseSessionManager.Instance.ClearActiveTarget();
        }

        onHandExit.Invoke();
    }

    public void OnSessionStarted()
    {
        sessionStarted = true;
    }

    public void ResetForRetry()
    {
        alreadyMeasured = false;
        sessionActivated = false;
        sessionStarted = false;
        holdTimer = 0f;
        handInZone = false;
    }

    public void MarkMeasured()
    {
        alreadyMeasured = true;
        sessionActivated = false;
        sessionStarted = false;
        handInZone = false;
        holdTimer = 0f;
    }
}