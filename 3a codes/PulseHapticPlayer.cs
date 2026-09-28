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

        if (bpm <= 0)
        {
            Debug.LogWarning($"{nameof(PulseHapticPlayer)} requires a BPM greater than zero.", this);
            return;
        }

        hapticRoutine = StartCoroutine(PulseLoop(bpm));
    }

    public void StopPulse()
    {
        if (hapticRoutine == null)
            return;

        StopCoroutine(hapticRoutine);
        hapticRoutine = null;
    }

    private IEnumerator PulseLoop(int bpm)
    {
        float interval = 60f / bpm;

        while (true)
        {
            SendHaptic(0.5f, 0.1f);
            yield return new WaitForSeconds(interval);
        }
    }

    private void SendHaptic(float amplitude, float duration)
    {
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (leftHand.isValid)
            leftHand.SendHapticImpulse(0, amplitude, duration);
    }
}
