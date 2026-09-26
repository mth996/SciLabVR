using UnityEngine;

public class ActivityNPCAnimator : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Movement")]
    public float moveSpeed = 1.5f;

    [Header("Animator")]
    public Animator animator;
    public string activityAnimationName = "Walking";

    private bool movingToB = false;
    private bool movingToA = false;
    private bool isPlaying = false;

    public bool IsPlaying => isPlaying;
    public bool IsMovingToB => movingToB;
    public bool IsMovingToA => movingToA;

    public void PlayActivitySequence()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("PointA or PointB missing on " + gameObject.name);
            return;
        }

        isPlaying = true;
        movingToB = true;
        movingToA = false;

        transform.position = pointA.position;
        FaceTarget(pointB.position);

        if (animator != null && !string.IsNullOrEmpty(activityAnimationName))
            animator.Play(activityAnimationName);
    }

    private void Update()
    {
        if (movingToB)
        {
            MoveTowards(pointB.position);
        }
        else if (movingToA)
        {
            MoveTowards(pointA.position);
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 fixedTarget = target;
        fixedTarget.y = transform.position.y;

        transform.position = Vector3.MoveTowards(
            transform.position,
            fixedTarget,
            moveSpeed * Time.deltaTime
        );
    }

    public void ReachedTurnPoint()
    {
        if (!isPlaying || !movingToB) return;

        movingToB = false;
        movingToA = true;

        FaceTarget(pointA.position);

        Debug.Log(gameObject.name + " reached Point B, turning back to A.");
    }

    public void ReachedEndPoint()
    {
        if (!isPlaying || !movingToA) return;

        movingToA = false;
        isPlaying = false;

        Debug.Log(gameObject.name + " reached Point A, sequence complete.");
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f;

        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }
}