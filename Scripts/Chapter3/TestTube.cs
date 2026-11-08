using UnityEngine;

public class TestTube : MonoBehaviour
{
    [Header("Settings")]
    public string tubeLabel = "P";
    public NailType acceptedNailType;

    [Header("Snap Point")]
    public Transform nailSnapPoint;

    [Header("State")]
    public bool hasNail = false;
    public NailItem nailInside;

    private void OnTriggerEnter(Collider other)
    {
        if (hasNail) return;

        NailItem nail = other.GetComponent<NailItem>();

        if (nail == null) return;
        if (nail.isPlaced) return;
        if (nail.nailType != acceptedNailType)
        {
            Debug.Log("Wrong nail type for tube " + tubeLabel);
            return;
        }

        hasNail = true;
        nailInside = nail;
        nail.PlaceInTube(this, nailSnapPoint);

        Chapter3Manager.Instance.OnNailPlaced(this, nail);
    }
}