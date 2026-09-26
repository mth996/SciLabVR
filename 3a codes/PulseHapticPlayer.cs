using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class PulseHapticPlayer : MonoBehaviour
{
    public static PulseHapticPlayer Instance;

    private Coroutine hapticRoutine;

    private void Awake()
    {
        Instance = this;
    }

    public void StartPulse(int bpm)
    {
        StopPulse();
        hapticRoutine = StartCoroutine(PulseLoop(bpm));
    }

    public void StopPulse()
    {
        if (hapticRoutine != null)
            StopCoroutine(hapticRoutine);
    }

    private IEnumerator PulseLoop(int bpm)
    {
        float interval = 60f / bpm;

        while (true)
        {
            SendHaptic(0.5f, 0.1f); // amplitude, duration
            yield return new WaitForSeconds(interval);
        }
    }
    
    private void SendHaptic(float amplitude, float duration) 
    {
        var leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        leftHand.SendHapticImpulse(0, amplitude, duration);
    }
}