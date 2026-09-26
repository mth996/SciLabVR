using UnityEngine;

public class NotebookSpawnPointGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.6f, 0.8f, 0.02f));
        Gizmos.DrawLine(transform.position,
            transform.position + transform.forward * 0.3f);
    }
}