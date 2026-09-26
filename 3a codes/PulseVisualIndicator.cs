using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PulseVisualIndicator : MonoBehaviour
{
    public static PulseVisualIndicator Instance;

    [Header("UI Reference")]
    public Image pulseCircle;

    [Header("Settings")]
    public float flashDuration = 0.1f;
    public Color pulseColor = new Color(1f, 0.2f, 0.2f);
    public Color idleColor = new Color(0.3f, 0.3f, 0.3f);

    private Coroutine pulseRoutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (pulseCircle != null)
            pulseCircle.color = idleColor;
    }

    public void StartVisualPulse(int bpm)
    {
        StopVisualPulse();
        pulseRoutine = StartCoroutine(PulseLoop(bpm));
    }

    public void StopVisualPulse()
    {
        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);

        if (pulseCircle != null)
            pulseCircle.color = idleColor;
    }

    private IEnumerator PulseLoop(int bpm)
    {
        float interval = 60f / bpm;

        while (true)
        {
            // Flash on
            pulseCircle.color = pulseColor;
            pulseCircle.transform.localScale = Vector3.one * 1.3f;

            yield return new WaitForSeconds(flashDuration);

            // Flash off
            pulseCircle.color = idleColor;
            pulseCircle.transform.localScale = Vector3.one * 1f;

            yield return new WaitForSeconds(interval - flashDuration);
        }
    }
}

