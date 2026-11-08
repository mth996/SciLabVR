using UnityEngine;

public class GroundReset : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if it's an apple
        AppleSlice apple =
            other.GetComponent<AppleSlice>();
        if (apple != null && !apple.isPlaced)
        {
            Debug.Log(other.gameObject.name
                + " hit ground — resetting apple!");
            apple.ResetApple();
            return;
        }

        // Check if it's a nail
        NailItem nail =
            other.GetComponent<NailItem>();
        if (nail != null && !nail.isPlaced)
        {
            Debug.Log(other.gameObject.name
                + " hit ground — resetting nail!");
            nail.SendMessage(
                "ResetNail",
                SendMessageOptions
                .DontRequireReceiver);
        }
    }
}