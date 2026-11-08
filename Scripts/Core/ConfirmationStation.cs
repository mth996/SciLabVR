using UnityEngine;
using UnityEngine.UI;

public class ConfirmationStation : MonoBehaviour
{
    [Header("English UI References")]
    public GameObject confirmButtonEnglish;
    public Image colorSwatchImageEnglish;

    [Header("Malay UI References")]
    public GameObject confirmButtonMalay;
    public Image colorSwatchImageMalay;

    [Header("References")]
    public BeakerSolution beaker;

    private bool isConfirmed = false;
    private bool isEnglish = true;

    private void Start()
    {
        if (LanguageManager.Instance != null)
            isEnglish = LanguageManager
                .Instance.IsEnglish();
        else
            isEnglish = true;

        HideAllUI();

        if (beaker == null)
            Debug.LogError(
                "Beaker is NULL on "
                + gameObject.name);
    }

    private void HideAllUI()
    {
        if (confirmButtonEnglish != null)
            confirmButtonEnglish.SetActive(false);
        if (confirmButtonMalay != null)
            confirmButtonMalay.SetActive(false);
        if (colorSwatchImageEnglish != null)
            colorSwatchImageEnglish
                .gameObject.SetActive(false);
        if (colorSwatchImageMalay != null)
            colorSwatchImageMalay
                .gameObject.SetActive(false);
    }

    // ─── Reset ────────────────────────────────────────

    public void ResetStation()
    {
        isConfirmed = false;
        HideAllUI();
        Debug.Log(gameObject.name + " reset!");
    }

    // ─── Oxidation Complete ───────────────────────────

    public void OnOxidationComplete()
    {
        Debug.Log("OnOxidationComplete called on "
            + gameObject.name);

        if (isConfirmed) return;

        if (beaker == null)
        {
            Debug.LogError(
                "Beaker is NULL on "
                + gameObject.name);
            return;
        }

        Image activeSwatch = isEnglish
            ? colorSwatchImageEnglish
            : colorSwatchImageMalay;
        GameObject activeButton = isEnglish
            ? confirmButtonEnglish
            : confirmButtonMalay;

        if (activeSwatch != null)
        {
            activeSwatch.gameObject.SetActive(true);
            activeSwatch.color =
                beaker.GetTargetAppleColor();
        }

        if (activeButton != null)
            activeButton.SetActive(true);
    }

    // ─── Confirm Pressed ──────────────────────────────

    public void OnConfirmPressed()
    {
        Debug.Log("OnConfirmPressed called on "
            + gameObject.name);

        if (beaker == null)
        {
            Debug.LogError(
                "Beaker is NULL — cannot confirm!");
            return;
        }

        if (Chapter4Manager.Instance == null)
        {
            Debug.LogError(
                "Chapter4Manager.Instance is NULL!");
            return;
        }

        if (isConfirmed)
        {
            Debug.Log("Already confirmed — skipping");
            return;
        }

        isConfirmed = true;

        GameObject activeButton = isEnglish
            ? confirmButtonEnglish
            : confirmButtonMalay;
        if (activeButton != null)
            activeButton.SetActive(false);

        Chapter4Manager.Instance
            .OnAppleConfirmed(beaker);
    }
}