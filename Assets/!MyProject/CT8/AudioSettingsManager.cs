using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Audio References")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string masterVolumeParameter = "MasterVolume";

    [Header("UI References")]
    [SerializeField] private Slider volumeSlider;

    private const string VOLUME_PREF_KEY = "MasterVolume";

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        InitializeVolume();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindVolumeSlider();

        if (volumeSlider != null)
        {
            LoadVolumeSettings();
        }
    }

    private void FindVolumeSlider()
    {
        Slider[] sliders = FindObjectsOfType<Slider>();
        foreach (Slider slider in sliders)
        {
            if (slider.CompareTag("VolumeSlider") || slider.name.Contains("Volume"))
            {
                volumeSlider = slider;
                break;
            }
        }

        if (volumeSlider == null && sliders.Length > 0)
        {
            volumeSlider = sliders[0];
        }

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(SetMasterVolume);
            volumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }
    }

    private void InitializeVolume()
    {
        FindVolumeSlider();
        LoadVolumeSettings();
    }

    private void LoadVolumeSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_PREF_KEY, 0.75f);

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }

        SetMasterVolume(savedVolume);
    }

    public void SetMasterVolume(float sliderValue)
    {
        if (audioMixer == null)
        {
            return;
        }

        float volumeInDb;

        if (sliderValue <= 0.001f)
        {
            volumeInDb = -80f;
        }
        else
        {
            volumeInDb = Mathf.Log10(sliderValue) * 20;
            volumeInDb = Mathf.Clamp(volumeInDb, -80f, 0f);
        }

        audioMixer.SetFloat(masterVolumeParameter, volumeInDb);

        PlayerPrefs.SetFloat(VOLUME_PREF_KEY, sliderValue);
        PlayerPrefs.Save();
    }

    public float GetCurrentVolume()
    {
        return PlayerPrefs.GetFloat(VOLUME_PREF_KEY, 0.75f);
    }
}