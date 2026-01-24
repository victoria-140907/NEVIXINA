using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clip Arrays")]
    public AudioClip[] musicList;
    public AudioClip[] sfxList;
    
    [Header("Audio Source References")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;   

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // IMPORTANTE: Reproducir música automáticamente al inicio
            if (musicList.Length > 0)
            {
                PlayMusic(0); // Reproduce la primera música
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(int musicIndex)
    {
        if (musicIndex >= 0 && musicIndex < musicList.Length && musicList[musicIndex] != null)
        {
            if (musicSource != null)
            {
                musicSource.clip = musicList[musicIndex];
                musicSource.Play();
                Debug.Log("Reproduciendo música: " + musicList[musicIndex].name);
            }
        }
    }
    
    public void PlaySFX(int sfxIndex)
    {
        if (sfxIndex >= 0 && sfxIndex < sfxList.Length && sfxList[sfxIndex] != null)
        {
            if (sfxSource != null)
            {
                sfxSource.PlayOneShot(sfxList[sfxIndex]);
            }
        }
    }

    private void Start()
    {
        // Reproducir música según la escena actual
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
        if (currentScene == "MainMenu" || currentScene == "ExplanationScene")
        {
            PlayMusic(0); // Música de menú
        }
        else if (currentScene.Contains("Level") || currentScene == "EndMenu")
        {
            PlayMusic(1); // Música de juego
        }
    
        Debug.Log("Música iniciada en escena: " + currentScene);
    }
}