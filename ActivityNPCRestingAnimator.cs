using System.Collections;
using UnityEngine;

public class ActivityNPCRestingAnimator : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Animation Name")]
    public string activityAnimationName = "Resting";

    [Header("Duration")]
    public float playDuration = 3f;

    private Coroutine currentRoutine;
    private bool isPlaying = false;

    public bool IsPlaying
    {
        get { return isPlaying; }
    }

    public void PlayActivitySequence()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ActivitySequence());
    }

    private IEnumerator ActivitySequence()
    {
        isPlaying = true;

        if (animator != null && !string.IsNullOrEmpty(activityAnimationName))
            animator.Play(activityAnimationName);

        yield return new WaitForSeconds(playDuration);

        isPlaying = false;
        currentRoutine = null;

        Debug.Log(gameObject.name + " resting sequence done.");
    }
}