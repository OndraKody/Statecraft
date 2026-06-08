using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        // Nacti ulozenou hodnotu nebo pouzij vychozi 0.5
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);

        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // Nastav pocatecni hlasitost
        AudioListener.volume = volumeSlider.value;
    }

    private void OnVolumeChanged(float value)
    {
        // AudioListener.volume ovlada celkovou hlasitost hry globalne
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }
}