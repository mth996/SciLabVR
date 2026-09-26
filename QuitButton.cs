using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour
{
    [Tooltip("Name of the scene to load (must be added to Build Settings)")]
    public string sceneToLoad;

    // Call this from any button's OnClick()
    public void QuitApp()
    {
        Debug.Log($"[QuitButton] Loading scene: {sceneToLoad}");

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("[QuitButton] No scene name assigned!");
        }
    }
}