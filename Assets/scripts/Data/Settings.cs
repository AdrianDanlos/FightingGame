using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Audio")]
    public Toggle musicToggle;
    public AudioSource music;

    void Start()
    {
        musicToggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(musicToggle);
        });
    }
    void ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            music.volume = 0f;
        }
        else if (!change.isOn)
        {
            music.volume = 0.15f;
        }
    }
}
