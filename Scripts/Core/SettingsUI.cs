using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle muteToggle;

    private const string KEY_VOLUME = "Volume";
    private const string KEY_MUTED = "Muted";

    void OnEnable()
    {
        float savedVolume = PlayerPrefs.GetFloat(KEY_VOLUME, 0.5f);
        bool savedMuted = PlayerPrefs.GetInt(KEY_MUTED, 0) == 1;

        if (volumeSlider != null) volumeSlider.value = savedVolume;
        if (muteToggle != null) muteToggle.isOn = savedMuted;

        ApplyAudio(savedVolume, savedMuted);

        if (volumeSlider != null) volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        if (muteToggle != null) muteToggle.onValueChanged.AddListener(OnMuteToggled);
    }

    void OnDisable()
    {
        if (volumeSlider != null) volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        if (muteToggle != null) muteToggle.onValueChanged.RemoveListener(OnMuteToggled);
    }

    void OnVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(KEY_VOLUME, value);
        PlayerPrefs.Save();
        ApplyAudio(value, muteToggle != null && muteToggle.isOn);
    }

    void OnMuteToggled(bool muted)
    {
        PlayerPrefs.SetInt(KEY_MUTED, muted ? 1 : 0);
        PlayerPrefs.Save();
        ApplyAudio(volumeSlider != null ? volumeSlider.value : 0.5f, muted);
    }

    void ApplyAudio(float volume, bool muted)
    {
        if (AudioManager.Instance != null)
        {
            // Only music volume is controlled by slider
            AudioManager.Instance.SetMusicVolume(muted ? 0f : volume);
            AudioManager.Instance.SetMusicMuted(muted);
            // SFX always full — button sounds unaffected by slider
        }
    }
}