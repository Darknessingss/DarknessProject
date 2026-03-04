using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] private AudioSource backgroundMusicSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (backgroundMusicSource == null)
            {
                backgroundMusicSource = GetComponent<AudioSource>();
            }

            if (backgroundMusicSource != null)
            {
                backgroundMusicSource.playOnAwake = true;
                backgroundMusicSource.loop = true;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Play();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadCurrentScene();
        }
    }

    private void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public float GetVolume()
    {
        return backgroundMusicSource != null ? backgroundMusicSource.volume : 1f;
    }

    public void SetVolume(float volume)
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = volume;
        }
    }
}