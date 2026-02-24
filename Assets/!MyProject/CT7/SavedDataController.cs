using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SavedDataController : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;

    private string savePath;
    private const string fileName = "SoundData.txt";
    private float lastSavedVolume;

    private void Start()
    {
        string folderPath = Path.Combine(Application.dataPath, "!MyProject", "CT7");
        savePath = Path.Combine(folderPath, fileName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        if (File.Exists(savePath))
        {
            LoadVolume();
        }
        else
        {
            SaveVolume();
        }

        if (audioSource != null)
        {
            lastSavedVolume = audioSource.volume;
        }
    }

    private void Update()
    {
        if (audioSource != null && audioSource.volume != lastSavedVolume)
        {
            SaveVolume();
            lastSavedVolume = audioSource.volume;
        }
    }

    private void SaveVolume()
    {
        if (audioSource == null) return;

        SoundData data = new SoundData(audioSource.volume);
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(savePath, json);
    }

    private void LoadVolume()
    {
        if (audioSource == null) return;

        string json = File.ReadAllText(savePath);
        SoundData data = JsonConvert.DeserializeObject<SoundData>(json);

        audioSource.volume = data.volume;
        Debug.Log($"Громкость загружена: {data.volume}");
    }

    private void OnDestroy()
    {
        if (audioSource != null)
        {
            SaveVolume();
        }
    }
}