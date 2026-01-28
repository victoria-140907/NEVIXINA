/*using UnityEngine;
using UnityEngine.SceneManagement

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

            // SUSCRIBIRSE a cambios de escena
            SceneManager.sceneLoaded += OnSceneLoaded;

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
}*/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clip Arrays")]
    public AudioClip[] musicList; // 0 = Música menús, 1 = Música juego
    public AudioClip[] sfxList;
    
    [Header("Audio Source References")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;   
    
    [Header("Fade Settings")]
    public float fadeDuration = 1.0f;
    
    private int currentMusicIndex = -1;
    private Coroutine fadeCoroutine;
    private string currentSceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            currentSceneName = SceneManager.GetActiveScene().name;
            
            // Iniciar música para la escena actual
            PlayMusicForScene(currentSceneName);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("AudioManager: Escena cargada - " + scene.name);
        
        // Solo cambiar música si la escena es diferente
        if (currentSceneName != scene.name)
        {
            currentSceneName = scene.name;
            PlayMusicForScene(scene.name);
        }
    }
    
    private void PlayMusicForScene(string sceneName)
    {
        int musicIndex = -1;
        
        // Determinar qué música tocar según la escena
        if (sceneName == "MainMenu" || sceneName == "ExplanationScene" || 
            sceneName == "EndMenu" || sceneName == "PauseMenu" || 
            sceneName == "OptionsMenu" || sceneName.Contains("Menu"))
        {
            musicIndex = 0; // Música relajante para menús
        }
        else if (sceneName.Contains("Level") || sceneName.Contains("Game") || 
                 sceneName.Contains("Play") || sceneName.Contains("Battle"))
        {
            musicIndex = 1; // Música de acción para juego
        }
        
        // Solo reproducir si el índice es válido y es diferente a la música actual
        if (musicIndex != -1 && musicIndex != currentMusicIndex)
        {
            PlayMusicWithFade(musicIndex);
        }
        else if (musicIndex == -1)
        {
            Debug.LogWarning("No se pudo determinar la música para la escena: " + sceneName);
        }
    }
    
    public void PlayMusic(int musicIndex)
    {
        if (musicIndex >= 0 && musicIndex < musicList.Length && musicList[musicIndex] != null)
        {
            if (musicSource != null && currentMusicIndex != musicIndex)
            {
                musicSource.clip = musicList[musicIndex];
                musicSource.Play();
                currentMusicIndex = musicIndex;
                Debug.Log("Reproduciendo música: " + musicList[musicIndex].name + " (Índice: " + musicIndex + ")");
            }
        }
        else
        {
            Debug.LogWarning("Índice de música inválido: " + musicIndex);
        }
    }
    
    public void PlayMusicWithFade(int musicIndex)
    {
        if (musicIndex >= 0 && musicIndex < musicList.Length && musicList[musicIndex] != null)
        {
            if (musicSource != null && currentMusicIndex != musicIndex)
            {
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }
                fadeCoroutine = StartCoroutine(FadeToNewMusic(musicList[musicIndex], musicIndex));
            }
        }
    }
    
    private IEnumerator FadeToNewMusic(AudioClip newClip, int newIndex)
    {
        if (musicSource.isPlaying && currentMusicIndex != -1)
        {
            // Fade out de la música actual
            float startVolume = musicSource.volume;
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                if (musicSource != null)
                    musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }
            
            if (musicSource != null)
            {
                musicSource.Stop();
                musicSource.volume = startVolume;
            }
        }
        
        // Cambiar a la nueva música
        if (musicSource != null)
        {
            musicSource.clip = newClip;
            musicSource.Play();
            currentMusicIndex = newIndex;
            
            // Fade in de la nueva música
            float originalVolume = musicSource.volume;
            musicSource.volume = 0;
            
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                if (musicSource != null)
                    musicSource.volume = Mathf.Lerp(0, originalVolume, t / fadeDuration);
                yield return null;
            }
            
            Debug.Log("Transición completada a: " + newClip.name + " (Índice: " + newIndex + ")");
        }
        
        fadeCoroutine = null;
    }
    
    // Método público para forzar cambio de música (útil para botones de menú)
    public void ForceChangeToMenuMusic()
    {
        PlayMusicWithFade(0);
    }
    
    public void ForceChangeToGameMusic()
    {
        PlayMusicWithFade(1);
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
        else
        {
            Debug.LogWarning("Índice de SFX inválido: " + sfxIndex);
        }
    }
    
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            currentMusicIndex = -1;
        }
    }
    
    public void ResumeMusic()
    {
        if (musicSource != null && musicSource.clip != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = Mathf.Clamp01(volume);
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }
    }
    
    public void ToggleMusic()
    {
        if (musicSource != null)
        {
            musicSource.mute = !musicSource.mute;
        }
    }
    
    public void ToggleSFX()
    {
        if (sfxSource != null)
        {
            sfxSource.mute = !sfxSource.mute;
        }
    }
    
    // Método para llamar desde botones de UI
    public void ReturnToMainMenuMusic()
    {
        PlayMusicWithFade(0);
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}