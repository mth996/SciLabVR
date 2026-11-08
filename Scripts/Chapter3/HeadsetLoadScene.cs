using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Attach to the Oculus Quest 3 headset object.
/// When the headset gets close to the player's head (camera), loads the main menu.
/// </summary>
public class HeadsetLoadScene : MonoBehaviour
{
    [Header("Scene to Load")]
    public string sceneToLoad = "Main Menu";

    [Header("Distance Threshold")]
    [Tooltip("How close the headset needs to be to the camera to trigger (in metres)")]
    public float triggerDistance = 0.3f;

    [Header("References")]
    [Tooltip("Drag your Main Camera here, or leave empty to auto-find")]
    public Transform playerHead;

    private bool _triggered = false;

    void Start()
    {
        // Auto find camera if not assigned
        if (playerHead == null)
            playerHead = Camera.main?.transform;
    }

    void Update()
    {
        if (_triggered || playerHead == null) return;

        float dist = Vector3.Distance(transform.position, playerHead.position);

        if (dist <= triggerDistance)
        {
            _triggered = true;
            Debug.Log("[HeadsetLoadScene] Headset near head — loading: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, triggerDistance);
    }
}