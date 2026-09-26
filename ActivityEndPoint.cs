using UnityEngine;

public class ActivityEndPoint : MonoBehaviour
{
    public ActivityNPCSwapManager swapManager;

    private void OnTriggerEnter(Collider other)
    {
        ActivityNPCAnimator npc = other.GetComponent<ActivityNPCAnimator>();

        if (npc != null && npc.IsMovingToA)
        {
            npc.ReachedEndPoint();

            if (swapManager != null)
                swapManager.OnAnimatedSequenceFinished();

            // Optional: disable A trigger after finish
            gameObject.SetActive(false);
        }
    }
}