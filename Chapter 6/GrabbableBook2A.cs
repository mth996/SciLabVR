using UnityEngine;

public class GrabbableBook2A : MonoBehaviour
{
    private bool counted = false;
    private Rigidbody bookRigidbody;

    private void Awake()
    {
        bookRigidbody = GetComponent<Rigidbody>();
    }

    public bool IsCounted()
    {
        return counted;
    }

    public void MarkCounted()
    {
        counted = true;
    }

    public void ResetBook(
        Vector3 originalPosition,
        Quaternion originalRotation)
    {
        counted = false;

        if (bookRigidbody != null)
        {
            bookRigidbody.linearVelocity =
                Vector3.zero;
            bookRigidbody.angularVelocity =
                Vector3.zero;
        }

        transform.SetPositionAndRotation(
            originalPosition, originalRotation);

        gameObject.SetActive(true);
    }
}