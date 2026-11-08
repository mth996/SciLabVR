using UnityEngine;

public enum ChemicalType3D
{
    CH3COOH,
    NH3
}

public class ChemicalDripper3D : MonoBehaviour
{
    [Header("Chemical Settings")]
    public ChemicalType3D chemicalType;

    [Header("Drop Settings")]
    public int dropsPerTouch = 1;

    [Header("Optional Drop VFX")]
    public ParticleSystem dropVFX;

    public void PlayDropEffect()
    {
        if (dropVFX != null)
            dropVFX.Play();
    }
}